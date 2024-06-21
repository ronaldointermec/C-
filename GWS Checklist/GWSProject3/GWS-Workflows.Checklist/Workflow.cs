// <copyright file="Workflow.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using IConnectorBehavior = Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules.IConnectorBehavior;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist;

/// <summary>
/// Workflow implementation.
/// </summary>
[Description("GWS-Workflows - Checklist")]
public class Workflow : WorkflowBase, IWorkflowWithData, IWorkflowWithMenus
{
    /* Comandos / Commands
     * 01. Abandonar trabajo / Leave job
     * 02. Saltar / Skip
     * 03. Deshacer / Undo
     * 04. Finalizar / No more
     */

    private readonly IConnectorBehavior _behavior;

    /// <summary>
    /// Initializes a new instance of the <see cref="Workflow"/> class.
    /// </summary>
    /// <param name="behavior">Instance of ConnectorBehavior used.</param>
    public Workflow(IConnectorBehavior behavior)
    {
        _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
    }

    /// <inheritdoc/>
    public override ValueTask<SignOn> ConnectAsync(IDevice device)
    {
        var @operator = _behavior.GetOperator(device.Operator);
        var signon = new SignOn();

        if (@operator is null)
        {
            _behavior.Log(string.Format(Resources.ServerConnection_OperatorNotFound, device, device.Operator), LogLevel.Error);
            signon.NotAllowed(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Workflow_OperatorNotFound), device.Operator));
        }
        else
        {
            if (string.IsNullOrEmpty(@operator.Password))
            {
                signon.NoPassword(device.Translate(SDK.Properties.DialogResources.ResourceManager, nameof(SDK.Properties.DialogResources.SignOn_Welcome)));
            }
            else
            {
                signon.WithPassword(
                    device.Translate(SDK.Properties.DialogResources.ResourceManager, nameof(SDK.Properties.DialogResources.SignOn_Welcome)),
                    @operator.Password,
                    device.Translate(SDK.Properties.DialogResources.ResourceManager, nameof(SDK.Properties.DialogResources.SignOn_PasswordPrompt)),
                    device.Translate(SDK.Properties.DialogResources.ResourceManager, nameof(SDK.Properties.DialogResources.SignOn_PasswordPromptHelp), @operator.Name),
                    device.Translate(SDK.Properties.DialogResources.ResourceManager, nameof(SDK.Properties.DialogResources.SignOn_WrongPassword)));
            }
        }

        return new ValueTask<SignOn>(signon);
    }

    /// <inheritdoc/>
    public override ValueTask<VariableSet> GetVariableSetAsync(Dictionary<string, string> data, IDevice device)
    {
        var vars = new VariableSet();

        vars.Add(Vars.DLG, "00", true);
        vars.Add(Vars.PROMPT);
        vars.Add(Vars.AUX);
        vars.Add(Vars.DUMMY);
        vars.Add(Vars.COMPLETE, "1");

        vars.Add(Vars.ID);
        vars.Add(Vars.CODE);
        vars.Add(Vars.ANSWER);
        vars.Add(Vars.UNDO);

        vars.Add(Vars.START_TIME);
        vars.Add(Vars.END_TIME);

        vars.Add(Vars.YEAR);
        vars.Add(Vars.MONTH);
        vars.Add(Vars.DAY);
        vars.Add(Vars.HOUR);
        vars.Add(Vars.MINUTE);
        vars.Add(Vars.SECOND);

        return new ValueTask<VariableSet>(vars);
    }

    /// <inheritdoc/>
    public override ValueTask BuildInstructionsAsync(InstructionSet instr, Dictionary<string, string> data, IDevice device)
    {
        if (!data.ContainsKey(Vars.DLG))
            throw new InvalidOperationException(DialogResources.MissingDeviceState);

        if (!Enum.TryParse(data[Vars.DLG], out Dialogs dialog))
            throw new InvalidOperationException(string.Format(DialogResources.UnknownState, data[Vars.DLG]));

        switch (dialog)
        {
            case Dialogs.Start:
                instr.SetCommandsConfirm(true, true, true);
                instr.SetCommands();
                instr.GetString(Vars.ID, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Workflow_Job)), barcodeEnabled: true);
                instr.SetSendHostFlag(Vars.ID, true);

                instr.AssignNum(Vars.DLG, Dialogs.DoChecklist);
                break;

            case Dialogs.DoChecklist:
                DoChecklist(instr, data, device);
                break;

            case Dialogs.Finished:
                Finished(instr, data, device);
                break;
        }

        return default;
    }

    /// <inheritdoc/>
    public ValueTask ProcessDataAsync(Dictionary<string, string> data, IDevice device)
    {
        if (!data.TryGetValue(Vars.ID, out string id))
            throw new ArgumentException(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));

        if (!data.TryGetValue(Vars.CODE, out string code))
            throw new ArgumentException(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));

        if (!data.TryGetValue(Vars.START_TIME, out int startTime))
            throw new ArgumentException(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));

        data.TryGetValue(Vars.ANSWER, out string? answer);

        data.TryGetValue(Vars.END_TIME, out int? endTime);

        _behavior.SubmitChecklistResult(device.Operator, id, code, answer, new DateTime(2009, 1, 1).AddSeconds(startTime), endTime.HasValue ? new DateTime(2009, 1, 1).AddSeconds(endTime.Value) : null);
        return default;
    }

    /// <inheritdoc/>
    public ValueTask<MenuOptions> GetMenuOptionsAsync(IDevice device, string variableValue)
    {
        var menuOptions = new MenuOptions();

        if (device.Bag["MenuOptions"] is not Dictionary<string, IEnumerable<SelectOption>> options)
            throw new InvalidOperationException(string.Format(Resources.Workflow_SelectOptionsNotFound, device));

        foreach (var o in options.SelectMany(x => x.Value, (k, v) => new { k.Key, v.Code, v.Description }))
            menuOptions.AddOption(o.Key, o.Code, o.Description);

        return new ValueTask<MenuOptions>(menuOptions);
    }

    /// <inheritdoc/>
    public override ValueTask<SignOff> DisconnectAsync(bool force, IDevice device)
    {
        if (force)
            return base.DisconnectAsync(force, device);

        if (!device.Data.ContainsKey(Vars.DLG) || !Enum.TryParse<Dialogs>(device.Data[Vars.DLG], out var dlg))
            return base.DisconnectAsync(force, device);

        if (dlg == Dialogs.DoChecklist)
            return base.DisconnectAsync(false, device);

        return base.DisconnectAsync(force, device);
    }

    private void DoChecklist(InstructionSet instr, Dictionary<string, string> data, IDevice device)
    {
        if (!data.TryGetValue(Vars.ID, out var id))
            throw new ArgumentException(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));

        Models.Checklist? checklist;
        try
        {
            checklist = _behavior.GetChecklist(device.Operator, id);
        }
        catch (InvalidOperationException ex)
        {
            _behavior.Log(ex.Message, LogLevel.Error);
            instr.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Workflow_ErrorRetrievingJob)), true, true);
            instr.AssignNum(Vars.DLG, Dialogs.Start);
            return;
        }

        if (checklist is null)
        {
            _behavior.Log(string.Format(Resources.Workflow_JobNotFound, id), LogLevel.Error);

            instr.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Workflow_JobNotFound_Voice), id), true, true);
            instr.AssignNum(Vars.DLG, Dialogs.Start);
            return;
        }

        var menuOptions = new Dictionary<string, IEnumerable<SelectOption>>();

        foreach (var option in checklist.Questions.OfType<Select>().ToDictionary(x => x.Code, x => x.Options))
            menuOptions.Add(option.Key, option.Value);

        foreach (var option in checklist.Questions.OfType<SelectMultiple>().ToDictionary(x => x.Code, x => x.Options))
            menuOptions.Add(option.Key, option.Value);

        if (menuOptions.Any())
        {
            device.Bag["MenuOptions"] = menuOptions;
            instr.LoadMenuOptions(Vars.ID);
        }
        else
        {
            device.Bag.Remove("MenuOptions");
        }

        instr.SetSendHostFlag(true, Vars.CODE, Vars.ANSWER, Vars.START_TIME, Vars.END_TIME);
        var linkedList = new LinkedList<IQuestion>(checklist.Questions.Where(x => !x.EndTime.HasValue));
        foreach (var question in linkedList)
        {
            instr.Label($"{Labels.Begin}_{question.Code}");
            instr.AssignStr(Vars.CODE, question.Code);
            instr.AssignStr(Vars.ANSWER, string.Empty);
            instr.AssignNum(Vars.START_TIME, "#time");
            instr.AssignStr(Vars.END_TIME, string.Empty);
            instr.DoIf(Vars.UNDO, true, Operation.EQ, _ =>
            {
                instr.GetVariablesOdr();
                instr.AssignNum(Vars.UNDO, false);
            });

            question.BuildDialog(device, instr, id, GetPreviousCode(question));

            instr.AssignNum(Vars.END_TIME, "#time");
            instr.GoTo($"{Labels.End}_{question.Code}");

            instr.Label($"{Labels.Undo}_{question.Code}");
            instr.AssignNum(Vars.UNDO, true);
            instr.GoTo($"{Labels.Begin}_{question.Code}");

            instr.Label($"{Labels.End}_{question.Code}");
            instr.GetVariablesOdr();
        }

        instr.AssignNum(Vars.DLG, Dialogs.Finished);
        instr.GoTo(Labels.End);

        instr.Label(Labels.LeaveJob);
        instr.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Workflow_LeavingJob)));
        instr.AssignNum(Vars.DLG, Dialogs.Start);

        instr.Label(Labels.End);
        instr.SetSendHostFlag(false, Vars.CODE, Vars.ANSWER, Vars.START_TIME, Vars.END_TIME);

        string? GetPreviousCode(IQuestion question)
        {
            string? res = null;

            var prev = linkedList.Find(question).Previous;
            while (prev is not null && res is null)
            {
                if (prev.Value is Message msg && !msg.ReadyToContinue)
                {
                    prev = prev.Previous;
                    continue;
                }

                res = prev.Value.Code;
            }

            return res;
        }
    }

    private void Finished(InstructionSet instr, Dictionary<string, string> data, IDevice device)
    {
        if (!data.TryGetValue(Vars.ID, out var id))
            throw new ArgumentException(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));

        var forceComplete = data.ContainsKey(Vars.COMPLETE);

        var completed = _behavior.CompleteChecklist(device.Operator, id, forceComplete);
        if (completed)
        {
            instr.SetCommands();
            instr.AssignNum(Vars.COMPLETE, false);
            instr.SetSendHostFlag(false, Vars.ID, Vars.COMPLETE);
            instr.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Workflow_JobFinished)), true, true);
            instr.AssignNum(Vars.DLG, Dialogs.Start);
        }
        else
        {
            instr.SetCommands(command04: Labels.Complete);
            instr.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Workflow_JobFinishedWithSkippedQuestions)), true, true);
            instr.AssignNum(Vars.DLG, Dialogs.Start);
            instr.GoTo(Labels.End);

            instr.Label(Labels.Complete);
            instr.SetSendHostFlag(Vars.COMPLETE, true);

            instr.Label(Labels.End);
        }
    }
}