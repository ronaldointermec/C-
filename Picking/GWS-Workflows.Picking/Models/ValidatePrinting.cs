// <copyright file="ValidatePrinting.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Implementation class for Validate Printing work (only for Continuous behavior).
/// </summary>
public class ValidatePrinting : GetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatePrinting"/> class.
    /// </summary>
    public ValidatePrinting()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatePrinting"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    /// <param name="message">Optional message.</param>
    public ValidatePrinting(string code, string? message = null)
        : base(code, message)
    {
    }

    /// <summary>
    /// Gets the list of codes used to validate labels.
    /// </summary>
    public virtual IEnumerable<string>? ValidationCodes { get; init; }

    /// <summary>
    /// Gets the number of digits required to validate labels.
    /// </summary>
    public virtual int? VoiceLength { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while asking for printing confirmation.
    /// </summary>
    public virtual Uri? ConfirmImage { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while validating labels.
    /// </summary>
    public virtual Uri? ValidateLabelsImage { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(InstructionSet i, IPickingBehavior behavior, IDevice device)
    {
        if (behavior is IPickingBatchBehavior)
        {
            throw new InvalidOperationException(device.Translate(Resources.ResourceManager, Resources.Error_IncorrectNonContinuousBehavior, nameof(ValidatePrinting)));
        }

        if (!string.IsNullOrEmpty(Message))
        {
            i.SetCommands();
            i.Say(Message, true, true);
        }

        i.SetCommands(command01: $"{Labels.EXCEPTION}_{Code}");

        i.AssignNum(Vars.START_TIME, "#time");

        i.Ask(Vars.DUMMY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_ConfirmResult)), priorityPrompt: true, imageUrl: ConfirmImage?.AbsoluteUri);
        i.DoIf(Vars.DUMMY, false, Operation.EQ, _ =>
        {
            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Retrying)));
            i.AssignStr(Vars.STATUS, "Retry");

            i.GoTo($"{Labels.END}_{Code}");
        });

        i.SetCommands();
        i.AssignStr(Vars.STATUS, "OK");

        if (ValidationCodes?.Any() ?? false)
        {
            i.AssignNum(Vars.LABELS_COUNT, ValidationCodes.Count());
            for (int x = 1; x <= Constants.MAX_N_LABELS; x++)
            {
                if (x <= ValidationCodes.Count())
                {
                    i.SetSendHostFlag($"LABELS_{x}_LABEL", false);

                    var element = ValidationCodes.ElementAt(x - 1);

                    if (VoiceLength.HasValue)
                    {
                        var pos = element.Length - VoiceLength.Value;
                        i.AssignStr($"LABELS_{x}_CODE", element.Substring(pos));
                    }
                    else
                    {
                        i.AssignStr($"LABELS_{x}_CODE", element);
                    }

                    i.AssignStr($"LABELS_{x}_LABEL", element);

                    i.AssignNum($"LABELS_{x}_VALIDATED", 0);
                }
            }

            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.ValidatePrinting_Confirm)));

            i.SetCommands(command09: $"{Labels.END}_{Code}");

            i.Label($"{Labels.VALIDATE}");
            i.DoIf(Vars.LABELS_COUNT, 0, Operation.EQ, _ =>
            {
                i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.ValidatePrinting_ValidationCompleted)));
                i.GoTo($"{Labels.END}_{Code}");
            });

            i.Concat(Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.ValidatePrinting_Pendings)), Vars.LABELS_COUNT);

            i.GetDigits(Vars.DUMMY, $"${Vars.PROMPT}", minLength: VoiceLength != 0 ? VoiceLength.ToString() : "1", maxLength: VoiceLength != 0 ? VoiceLength.ToString() : "20", barcodeEnabled: true, imageUrl: ValidateLabelsImage?.AbsoluteUri);

            for (int x = 1; x <= ValidationCodes.Count(); x++)
            {
                i.DoIf(Vars.DUMMY, $"LABELS_{x}_CODE", Operation.EQ, CompareType.Str, _ =>
                i.DoIf($"LABELS_{x}_VALIDATED", 0, Operation.EQ, _ =>
                {
                    i.AssignNum($"LABELS_{x}_VALIDATED", 1);
                    i.Decrement(Vars.LABELS_COUNT);
                    i.SetSendHostFlag($"LABELS_{x}_LABEL", true);
                    i.GoTo($"{Labels.VALIDATE}");
                }));
            }

            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error_Incorrect)));
            i.GoTo($"{Labels.VALIDATE}");
        }
        else
        {
            i.GoTo($"{Labels.END}_{Code}");
        }

        // Exception
        i.Label($"{Labels.EXCEPTION}_{Code}");
        i.AssignStr(Vars.STATUS, "Cancelled");

        // End
        i.Label($"{Labels.END}_{Code}");
        i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.ValidatePrintingResult);
        i.SetSendHostFlag(true, Vars.RESPONSETYPE, Vars.STATUS, Vars.START_TIME, Vars.END_TIME);
        i.AssignNum(Vars.END_TIME, "#time");
    }
}
