// <copyright file="Workflow.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking;

/// <summary>
/// Base implementation for Picking Workflow.
/// </summary>
[Description("GWS-Workflows - Picking")]
public class Workflow : WorkflowBase, IWorkflowWithData, IWorkflowWithMenus
{
    /* COMMANDS:
     *  1. Exception
     *  2. Dock
     *  3. Take a break
     *  4. Skip slot
     *  5. Printer
     *  6. How much more
     *  7. Location
     *  8. Description
     *  9. Cancel
     *  10. Breakage
     *  11. Store number
     *  12. Change unit of measure
     *  13. Units
     *  14. Skip aisle
     *  15. Product number
     *  16. Upc number
    */

    private readonly IPickingBehavior _behavior;

    /// <summary>
    /// Initializes a new instance of the <see cref="Workflow"/> class.
    /// </summary>
    /// <param name="behavior">Behavior used by the Workflow.</param>
    public Workflow(IPickingBehavior behavior)
    {
        _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
    }

    /// <inheritdoc/>
    public override async ValueTask<SignOn> ConnectAsync(IDevice device)
    {
        var signOn = new SignOn();

        var connectResult = await _behavior.ConnectAsync(device.Operator, device);

        if (connectResult.Allowed)
        {
            if (string.IsNullOrEmpty(connectResult.Password))
                signOn.NoPassword(connectResult.Message ?? string.Empty);
            else
                signOn.WithPassword(connectResult.Message ?? string.Empty, connectResult.Password ?? string.Empty);
        }
        else
        {
            signOn.NotAllowed(connectResult.Message ?? string.Empty);
        }

        return signOn;
    }

    /// <inheritdoc/>
    public override ValueTask<VariableSet> GetVariableSetAsync(Dictionary<string, string> data, IDevice device)
    {
        var vars = new VariableSet();
        vars.Add(Vars.DLG, "00", true);
        vars.Add(Vars.RESPONSETYPE);
        vars.Add(Vars.MENU);
        vars.Add(Vars.BATCH);
        vars.Add(Vars.CODE);
        vars.Add(Vars.STATUS);
        vars.Add(Vars.START_TIME);
        vars.Add(Vars.END_TIME);
        vars.Add(Vars.PICKED);
        vars.Add(Vars.SERVING);
        vars.Add(Vars.WEIGHT);
        vars.Add(Vars.STOCK);
        vars.Add(Vars.DOCK);
        vars.Add(Vars.WHEREAMI);
        vars.Add(Vars.PRODUCT_DESCRIPTION);
        vars.Add(Vars.PRODUCT_NUMBER);
        vars.Add(Vars.UPC_NUMBER);
        vars.Add(Vars.PROMPT);
        vars.Add(Vars.QTY_REQUESTED);
        vars.Add(Vars.QTY_REQUESTED_ORIG);
        vars.Add(Vars.MAX_QTY_ALLOWED_PER_PICK);
        vars.Add(Vars.TOTAL_WEIGHT);
        vars.Add(Vars.TOTAL_PICKED);
        vars.Add(Vars.QTY_UPPER);
        vars.Add(Vars.QTY_LOWER);
        vars.Add(Vars.DUMMY);
        vars.Add(Vars.CURRENT_AISLE);
        vars.Add(Vars.CURRENT_POSITION);
        vars.Add(Vars.LABELS);
        vars.Add(Vars.LABELS_COUNT);
        vars.Add(Vars.SKIPPING_AISLE);
        vars.Add(Vars.PRODUCT_CD);
        vars.Add(Vars.BREAKAGE);
        vars.Add(Vars.CUSTOMER);
        vars.Add(Vars.PRINTER);
        vars.Add(Vars.BREAK_REASON);

        for (int i = 1; i <= Constants.MAX_N_LABELS; i++)
        {
            vars.Add($"LABELS_{i}_LABEL");
            vars.Add($"LABELS_{i}_CODE");
            vars.Add($"LABELS_{i}_VALIDATED");
        }

        for (int i = 1; i <= Constants.MAX_N_PRINTERS; i++)
        {
            vars.Add($"PRINTER_{i}_CODE");
        }

        return new ValueTask<VariableSet>(vars);
    }

    /// <inheritdoc/>
    public override async ValueTask BuildInstructionsAsync(InstructionSet instr, Dictionary<string, string> data, IDevice device)
    {
        if (_behavior is IPickingContinuousBehavior && data.ContainsKey(Vars.RESPONSETYPE))
        {
            try
            {
                var res = await ProcessDataWorkAsync(data, device);

                if (Enum.TryParse(data[Vars.RESPONSETYPE], out ResponseTypes type))
                {
                    DisableContinuousVariables(instr, type);
                }

                if (res != null)
                {
                    instr.AssignStr(Vars.CODE, res.Code);

                    switch (res)
                    {
                        case IGetWorkOrderItem w:
                            w.BuildDialog(instr, _behavior, device);
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                _behavior.HandleDialogException(ex, instr, device.Culture);
                return;
            }
        }

        if (!data.ContainsKey(Vars.DLG))
            throw new ArgumentException(Resources.Error_MissingDeviceState);

        if (!Enum.TryParse(data[Vars.DLG], out Dialogs dialog))
            throw new ArgumentException(string.Format(Resources.Error_UnknownDialog, data[Vars.DLG]));

        switch (dialog)
        {
            case Dialogs.Start:
                await StartAsync(instr, device);
                break;
            case Dialogs.Work:
                await WorkAsync(instr, device);
                break;
        }
    }

    /// <inheritdoc/>
    public ValueTask ProcessDataAsync(Dictionary<string, string> data, IDevice device) => new(ProcessDataWorkAsync(data, device));

    /// <inheritdoc/>
    public ValueTask<MenuOptions> GetMenuOptionsAsync(IDevice device, string variableValue)
    {
        var menu = new MenuOptions();

        /* Menu options for zero picked in a partial quantity mode (used in PickingLine class):
         *  1. Empty
         *  2. Breakage
         *  3. Complete
         *  4. Dock
         *  5. Continue
         *  6. Cancel
        */

        var lowQuantityOptions = new string[6] { device.Translate(DialogResources.ResourceManager, nameof(DialogResources.LowerQuantityMenu_Empty)), device.Translate(DialogResources.ResourceManager, nameof(DialogResources.LowerQuantityMenu_Breakage)), device.Translate(DialogResources.ResourceManager, nameof(DialogResources.LowerQuantityMenu_Complete)), device.Translate(DialogResources.ResourceManager, nameof(DialogResources.LowerQuantityMenu_Dock)), device.Translate(DialogResources.ResourceManager, nameof(DialogResources.LowerQuantityMenu_Continue)), device.Translate(DialogResources.ResourceManager, nameof(DialogResources.LowerQuantityMenu_Cancel)) };

        for (short i = 0; i < lowQuantityOptions.Count(); i++)
        {
            menu.AddOption(Menus.LOWER_QUANTITY, (short)(i + 1), lowQuantityOptions[i]);
        }

        var breakOptions = _behavior?.Settings?.BreakOptions?.Split('|');

        if (breakOptions?.Any() ?? false)
        {
            for (short i = 0; i < breakOptions.Count(); i++)
            {
                menu.AddOption(Menus.BREAK, (short)(i + 1), breakOptions.ElementAt(i));
            }
        }

        return new ValueTask<MenuOptions>(menu);
    }

    /// <inheritdoc/>
    public override async ValueTask<SignOff> DisconnectAsync(bool force, IDevice device)
    {
        var signOff = new SignOff();

        var disconnectResult = await _behavior.DisconnectAsync(device.Operator, device.DeviceID, force);

        if (disconnectResult.Allowed)
            signOff.Allowed(disconnectResult.Message ?? string.Empty);
        else
            signOff.NotAllowed(disconnectResult.Message ?? string.Empty);

        return signOff;
    }

    /// <summary>
    /// Process the VIO request from a getInstructions or getVariablesODR.
    /// </summary>
    /// <param name="data">Active variables included in the request.</param>
    /// <param name="device">Device that performed the request.</param>
    /// <returns>Next work order to perform by operator.</returns>
    protected virtual async Task<IWorkOrderItem?> ProcessDataWorkAsync(Dictionary<string, string> data, IDevice device)
    {
        if (!data.TryGetValue(Vars.RESPONSETYPE, out string responseTypeStr))
        {
            _behavior.Log(string.Format(Resources.Error_MissingData, Vars.RESPONSETYPE), LogLevel.Error);
            return null;
        }

        if (!Enum.TryParse(responseTypeStr, out ResponseTypes type))
        {
            _behavior.Log(string.Format(Resources.Error_InvalidaData, Vars.RESPONSETYPE, responseTypeStr), LogLevel.Error);
            return null;
        }

        if (!data.TryGetValue(Vars.CODE, out string code))
        {
            _behavior.Log(string.Format(Resources.Error_MissingData, Vars.CODE), LogLevel.Error);
            return null;
        }

        switch (type)
        {
            case ResponseTypes.PrintLabelsBatchResult:
                {
                    if (!data.TryGetValue(Vars.LABELS, out int labelsCount))
                    {
                        _behavior.Log(string.Format(Resources.Error_MissingData, Vars.LABELS), LogLevel.Error);
                        return null;
                    }

                    if (_behavior is IPickingContinuousBehavior)
                    {
                        _behavior.Log(string.Format(Resources.Error_IncorrectContinuousBehavior, "PrintLabelsBatch"), LogLevel.Error);
                        return null;
                    }

                    if (_behavior is IPickingBatchBehavior batchBehavior)
                    {
                        await batchBehavior.PrintLabelsBatchAsync(device.Operator, device.DeviceID, new PrintLabelsBatch(code) { Count = labelsCount });
                    }

                    return null;
                }

            case ResponseTypes.BeginBreak:
                {
                    await _behavior.BeginBreakAsync(
                        device.Operator,
                        device.DeviceID,
                        new BeginBreak(code)
                        {
                            Reason = data.ContainsKey(Vars.BREAK_REASON) ? Convert.ToInt16(data[Vars.BREAK_REASON]) : null,
                        });
                    return null;
                }

            case ResponseTypes.EndBreak:
                {
                    await _behavior.EndBreakAsync(device.Operator, device.DeviceID);

                    return null;
                }

            default:
                return await ProcessSetWorkOrderAsync(data, device, type, code);
        }
    }

    private async Task<IWorkOrderItem?> ProcessSetWorkOrderAsync(Dictionary<string, string> data, IDevice device, ResponseTypes type, string code)
    {
        IWorkOrderItem? response = null;

        if (!data.TryGetValue(Vars.START_TIME, out string startTime))
        {
            _behavior.Log(string.Format(Resources.Error_MissingData, Vars.START_TIME), LogLevel.Error);
            return null;
        }

        if (!data.TryGetValue(Vars.END_TIME, out string endTime))
        {
            _behavior.Log(string.Format(Resources.Error_MissingData, Vars.END_TIME), LogLevel.Error);
            return null;
        }

        if (!data.TryGetValue(Vars.STATUS, out string status))
        {
            _behavior.Log(string.Format(Resources.Error_MissingData, Vars.STATUS), LogLevel.Error);
            return null;
        }

        var startTimeDateTime = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(startTime));
        var endTimeDateTime = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(endTime));

        switch (type)
        {
            case ResponseTypes.AskQuestionResult:
                {
                    var result = new AskQuestionResult(code)
                    {
                        Status = status,
                        Started = startTimeDateTime,
                        Finished = endTimeDateTime,
                    };

                    switch (_behavior)
                    {
                        case IPickingBatchBehavior batchBehavior:
                            await batchBehavior.SetWorkOrderAsync(device.Operator, device.DeviceID, result);
                            break;
                        case IPickingContinuousBehavior continuousBehavior:
                            response = await continuousBehavior.SetWorkOrderAsync(device.Operator, device.DeviceID, result);
                            break;
                    }

                    return response;
                }

            case ResponseTypes.BeginPickingResult:
                {
                    var result = new BeginPickingOrderResult(code)
                    {
                        Status = status,
                        Started = startTimeDateTime,
                        Finished = endTimeDateTime,
                    };

                    switch (_behavior)
                    {
                        case IPickingBatchBehavior batchBehavior:
                            await batchBehavior.SetWorkOrderAsync(device.Operator, device.DeviceID, result);
                            break;
                        case IPickingContinuousBehavior continuousBehavior:
                            response = await continuousBehavior.SetWorkOrderAsync(device.Operator, device.DeviceID, result);
                            break;
                    }

                    return response;
                }

            case ResponseTypes.PickingLineResult:
                {
                    var result = new PickingLineResult(code)
                    {
                        Status = status,
                        Started = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(startTime)),
                        Finished = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(endTime)),
                        Picked = data.ContainsKey(Vars.TOTAL_PICKED) ? Convert.ToInt32(data[Vars.TOTAL_PICKED]) : null,
                        ServingCode = data.ContainsKey(Vars.SERVING) ? data[Vars.SERVING] : null,
                        Weight = data.ContainsKey(Vars.TOTAL_WEIGHT) ? Convert.ToDecimal(data[Vars.TOTAL_WEIGHT]) : null,
                        Stock = data.ContainsKey(Vars.STOCK) ? Convert.ToInt32(data[Vars.STOCK]) : null,
                        Dock = data.ContainsKey(Vars.DOCK) ? data[Vars.DOCK] : null,
                        ProductCD = data.ContainsKey(Vars.PRODUCT_CD) ? data[Vars.PRODUCT_CD] : null,
                        Breakage = data.ContainsKey(Vars.BREAKAGE) ? Convert.ToInt32(data[Vars.BREAKAGE]) : null,
                        Batch = data.ContainsKey(Vars.BATCH) ? data[Vars.BATCH] : null,
                    };

                    switch (_behavior)
                    {
                        case IPickingBatchBehavior batchBehavior:
                            await batchBehavior.SetWorkOrderAsync(device.Operator, device.DeviceID, result);
                            break;
                        case IPickingContinuousBehavior continuousBehavior:
                            response = await continuousBehavior.SetWorkOrderAsync(device.Operator, device.DeviceID, result);
                            break;
                    }

                    return response;
                }

            case ResponseTypes.PrintLabelsResult:
                {
                    switch (_behavior)
                    {
                        case IPickingBatchBehavior:
                            _behavior.Log(string.Format(Resources.Error_IncorrectNonContinuousBehavior, "PrintLabelsResult"), LogLevel.Error);
                            return null;
                        case IPickingContinuousBehavior continuousBehavior:
                            response = await continuousBehavior.SetWorkOrderAsync(
                                      device.Operator,
                                      device.DeviceID,
                                      new PrintLabelsResult(code)
                                      {
                                          Status = status,
                                          Started = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(startTime)),
                                          Finished = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(endTime)),
                                          LabelsToPrint = data.ContainsKey(Vars.LABELS) ? Convert.ToInt32(data[Vars.LABELS]) : null,
                                          Printer = data.ContainsKey(Vars.PRINTER) ? Convert.ToInt32(data[Vars.PRINTER]) : null,
                                      });
                            break;
                    }

                    return response;
                }

            case ResponseTypes.ValidatePrintingResult:
                {
                    switch (_behavior)
                    {
                        case IPickingBatchBehavior:
                            _behavior.Log(string.Format(Resources.Error_IncorrectNonContinuousBehavior, "ValidatePrintingResult"), LogLevel.Error);
                            return null;
                        case IPickingContinuousBehavior continuousBehavior:
                            var readLabels = new List<string>();
                            for (int x = 1; x <= Constants.MAX_N_LABELS; x++)
                            {
                                if (data.ContainsKey($"LABELS_{x}_LABEL"))
                                    readLabels.Add(data[$"LABELS_{x}_LABEL"]);
                            }

                            response = await continuousBehavior.SetWorkOrderAsync(
                                     device.Operator,
                                     device.DeviceID,
                                     new ValidatePrintingResult(code)
                                     {
                                         Status = status,
                                         Started = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(startTime)),
                                         Finished = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(endTime)),
                                         ReadLabels = readLabels,
                                     });
                            break;
                    }

                    return response;
                }

            case ResponseTypes.PlaceInDockResult:
                {
                    var result = new PlaceInDockResult(code)
                    {
                        Status = status,
                        Started = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(startTime)),
                        Finished = new DateTime(2009, 1, 1).AddSeconds(Convert.ToInt32(endTime)),
                        Dock = data.ContainsKey(Vars.DOCK) ? data[Vars.DOCK] : null,
                    };

                    switch (_behavior)
                    {
                        case IPickingBatchBehavior batchBehavior:
                            await batchBehavior.SetWorkOrderAsync(device.Operator, device.DeviceID, result);
                            break;
                        case IPickingContinuousBehavior continuousBehavior:
                            response = await continuousBehavior.SetWorkOrderAsync(device.Operator, device.DeviceID, result);
                            break;
                    }

                    return response;
                }

            default:
                return null;
        }
    }

    private void DisableContinuousVariables(InstructionSet i, ResponseTypes type)
    {
        i.SetSendHostFlag(Vars.RESPONSETYPE, false);

        switch (type)
        {
            case ResponseTypes.BeginPickingResult:
            case ResponseTypes.AskQuestionResult:
                i.SetSendHostFlag(false, Vars.STATUS, Vars.START_TIME, Vars.END_TIME);
                break;
            case ResponseTypes.PickingLineResult:
                i.SetSendHostFlag(false, Vars.STATUS, Vars.START_TIME, Vars.END_TIME, Vars.PICKED, Vars.SERVING, Vars.WEIGHT, Vars.STOCK, Vars.PRODUCT_CD, Vars.BREAKAGE, Vars.BATCH, Vars.TOTAL_WEIGHT, Vars.TOTAL_PICKED);
                break;
            case ResponseTypes.PlaceInDockResult:
                i.SetSendHostFlag(false, Vars.STATUS, Vars.START_TIME, Vars.END_TIME, Vars.DOCK);
                break;
            case ResponseTypes.PrintLabelsResult:
                i.SetSendHostFlag(false, Vars.STATUS, Vars.START_TIME, Vars.END_TIME, Vars.LABELS, Vars.PRINTER);
                break;
            case ResponseTypes.ValidatePrintingResult:
                for (int x = 1; x < Constants.MAX_N_LABELS; x++)
                    i.SetSendHostFlag($"LABELS_{x}_LABEL", false);
                break;
            case ResponseTypes.BeginBreak:
                i.SetSendHostFlag(false, Vars.BREAK_REASON);
                break;
        }
    }

    private async ValueTask StartAsync(InstructionSet i, IDevice device)
    {
        if (_behavior is IPickingContinuousBehavior)
            i.SetSendHostFlag(Vars.CODE, true);

        i.LoadMenuOptions(Vars.MENU);
        var res = await _behavior.RegisterOperatorStartAsync(device.Operator, device.DeviceID);
        if (!string.IsNullOrEmpty(res))
            i.Say(res);

        i.SetCommandsConfirm(command01: true, command02: true, command03: true, command04: true, command09: true, command10: true);
        i.AssignNum(Vars.DLG, Dialogs.Work);

        await WorkAsync(i, device);
    }

    private async ValueTask WorkAsync(InstructionSet i, IDevice device)
    {
        try
        {
            switch (_behavior)
            {
                case IPickingBatchBehavior batchBehavior:
                    {
                        var res = await batchBehavior.GetWorkOrdersAsync(device.Operator, device.DeviceID);

                        if (res == null || !res.Any())
                        {
                            i.SetCommands();

                            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.GetWork_NotWork)), true);
                            return;
                        }

                        i.SetSendHostFlag(true, Vars.CODE, Vars.RESPONSETYPE);

                        foreach (var w in res)
                        {
                            i.AssignStr(Vars.CODE, w.Code);
                            w.BuildDialog(i, _behavior, device);
                        }

                        i.SetSendHostFlag(false, Vars.CODE, Vars.RESPONSETYPE);
                    }

                    break;
                case IPickingContinuousBehavior continuousBehavior:
                    {
                        var res = await continuousBehavior.GetWorkOrderAsync(device.Operator, device.DeviceID);

                        if (res == null)
                        {
                            i.SetCommands();

                            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.GetWork_NotWork)), true);
                            return;
                        }

                        i.AssignStr(Vars.CODE, res.Code);
                        res.BuildDialog(i, _behavior, device);
                    }

                    break;
            }
        }
        catch (Exception ex)
        {
            _behavior.HandleDialogException(ex, i, device.Culture);
        }
    }
}