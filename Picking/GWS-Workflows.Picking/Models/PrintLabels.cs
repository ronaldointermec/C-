// <copyright file="PrintLabels.cs" company="Honeywell | Safety and Productivity Solutions">
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
/// Implementation class for Print Labels work (only for Continuous behavior).
/// </summary>
public class PrintLabels : GetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrintLabels"/> class.
    /// </summary>
    public PrintLabels()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PrintLabels"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    /// <param name="message">Optional message.</param>
    public PrintLabels(string code, string? message = null)
        : base(code, message)
    {
    }

    /// <summary>
    /// Gets the list of available printers.
    /// </summary>
    public virtual IEnumerable<int>? Printers { get; init; }

    /// <summary>
    /// Gets the default printer.
    /// </summary>
    public virtual int? DefaultPrinter { get; init; }

    /// <summary>
    /// Gets the number of copies to print.
    /// </summary>
    public virtual int? Copies { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while asking/informing for number of copies.
    /// </summary>
    public virtual Uri? CopiesImage { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while asking/informing for printer.
    /// </summary>
    public virtual Uri? PrinterImage { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(InstructionSet i, IPickingBehavior behavior, IDevice device)
    {
        if (behavior is IPickingBatchBehavior)
        {
            throw new InvalidOperationException(device.Translate(Resources.ResourceManager, Resources.Error_IncorrectNonContinuousBehavior, nameof(PrintLabels)));
        }

        if (!string.IsNullOrEmpty(Message))
        {
            i.SetCommands();
            i.Say(Message, true, true);
        }

        i.AssignStr(Vars.STATUS, string.Empty);
        i.AssignNum(Vars.START_TIME, "#time");

        if (DefaultPrinter.HasValue)
            i.AssignNum(Vars.PRINTER, DefaultPrinter.Value);
        else
            i.AssignStr(Vars.PRINTER, string.Empty);

        i.Label($"{Labels.SELECT_LABELS}_{Code}");
        {
            i.SetCommands(command05: $"{Labels.SELECT_PRINTER}_{Code}", command09: $"{Labels.CANCEL}_{Code}");

            i.DoIf(Vars.PRINTER, string.Empty, Operation.EQ, CompareType.Str, ifPrinter =>
            {
                i.Concat(Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Printer)), device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Unknown)));
                ifPrinter.DoElse();
                i.Concat(Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Printer)), Vars.PRINTER);
            });

            if (Copies.HasValue)
            {
                i.Concat(Vars.PROMPT, Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Confirm)));

                i.Say($"${Vars.PROMPT}", requireReady: true, imageUrl: CopiesImage?.AbsoluteUri);

                i.AssignNum(Vars.LABELS, Copies.Value);
            }
            else
            {
                i.Concat(Vars.PROMPT, Vars.PROMPT, $", {device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Labels))}");

                i.GetDigits(Vars.LABELS, $"${Vars.PROMPT}", maxLength: "2", confirmationPrompt: "?", imageUrl: CopiesImage?.AbsoluteUri);
            }

            i.SetSendHostFlag(true, Vars.RESPONSETYPE, Vars.STATUS, Vars.START_TIME, Vars.END_TIME, Vars.LABELS, Vars.PRINTER);

            i.AssignStr(Vars.STATUS, "OK");

            i.GoTo($"{Labels.END}_{Code}");
        }

        // Select printer
        i.Label($"{Labels.SELECT_PRINTER}_{Code}");
        {
            i.SetCommands(command09: $"{Labels.SELECT_LABELS}_{Code}");

            if (Printers?.Any() ?? false)
            {
                for (int x = 1; x <= Printers.Count(); x++)
                {
                    i.AssignNum($"PRINTER_{x}_CODE", Printers.ElementAt(x - 1));
                }

                i.Label($"{Labels.PRINTER}_{Code}");

                i.GetDigits(Vars.PRINTER, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_ConfirmPrinter)), maxLength: "2", imageUrl: PrinterImage?.AbsoluteUri);

                for (int x = 1; x <= Printers.Count(); x++)
                {
                    i.DoIf(Vars.PRINTER, $"PRINTER_{x}_CODE", Operation.EQ, CompareType.Num, _ =>
                    {
                        i.GoTo($"{Labels.SELECT_LABELS}_{Code}");
                    });
                }

                i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_UnknownPrinter)));
                i.GoTo($"{Labels.PRINTER}_{Code}");
            }
            else
            {
                i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_PrintersNotAvailable)));
                i.GoTo($"{Labels.SELECT_LABELS}_{Code}");
            }
        }

        // Cancel
        i.Label($"{Labels.CANCEL}_{Code}");
        i.AssignStr(Vars.STATUS, "Cancelled");

        // End
        i.Label($"{Labels.END}_{Code}");
        i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.PrintLabelsResult);
        i.AssignNum(Vars.END_TIME, "#time");
    }
}
