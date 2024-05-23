// <copyright file="PlaceInDock.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Implementation class for Place In Dock work.
/// </summary>
public class PlaceInDock : GetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceInDock"/> class.
    /// </summary>
    public PlaceInDock()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceInDock"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    /// <param name="message">Optional message.</param>
    public PlaceInDock(string code, string? message = null)
        : base(code, message)
    {
    }

    /// <summary>
    /// Gets the dock where to deposit the container.
    /// </summary>
    public virtual string? Dock { get; init; }

    /// <summary>
    /// Gets the check digit to validate dock.
    /// </summary>
    public virtual string? CD { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed.
    /// </summary>
    public virtual Uri? Image { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(InstructionSet i, IPickingBehavior behavior, IDevice device)
    {
        i.Label($"{Labels.DOCK}_{Code}");

        if (!string.IsNullOrEmpty(Message))
        {
            i.SetCommands();
            i.Say(Message, true, true);
        }

        i.AssignStr(Vars.STATUS, string.Empty);
        i.AssignNum(Vars.START_TIME, "#time");

        i.SetCommands(command01: $"{Labels.DOCK_EXCEPTION}_{Code}");

        i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PlaceInDock_PlaceInDock)));

        if (behavior is IPickingBatchBehavior)
        {
            i.Label($"{Labels.PRINT_LABELS}_{Code}");
            i.GetDigits(Vars.LABELS, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Labels)), maxLength: "2");

            i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.PrintLabelsBatchResult);
            i.SetSendHostFlag(true, Vars.LABELS);
            i.GetVariablesOdr();
            i.SetSendHostFlag(false, Vars.LABELS);

            i.Ask(Vars.DUMMY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_ConfirmResult)), priorityPrompt: true);
            i.DoIf(Vars.DUMMY, false, Operation.EQ, _ =>
            {
                i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Retrying)));
                i.GoTo($"{Labels.PRINT_LABELS}_{Code}");
            });
        }

        if (!string.IsNullOrEmpty(Dock))
        {
            if (!string.IsNullOrEmpty(CD))
                i.GetDigits(Vars.DUMMY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PlaceInDock_Dock), Dock), mustEqual: CD, wrongPrompt: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error_Incorrect)), imageUrl: Image?.AbsoluteUri, barcodeEnabled: true);
            else
                i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PlaceInDock_Dock), Dock), imageUrl: Image?.AbsoluteUri);

            i.AssignStr(Vars.DOCK, Dock);
        }
        else
        {
            i.GetDigits(Vars.DOCK, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PlaceInDock_Destination)), imageUrl: Image?.AbsoluteUri);
        }

        i.SetSendHostFlag(true, Vars.DOCK);
        i.AssignStr(Vars.STATUS, "OK");

        i.GoTo($"{Labels.DOCK_END}_{Code}");

        // Exception
        i.Label($"{Labels.DOCK_EXCEPTION}_{Code}");
        i.AssignStr(Vars.STATUS, "Cancelled");

        // End
        i.Label($"{Labels.DOCK_END}_{Code}");
        {
            i.AssignNum(Vars.END_TIME, "#time");
            i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.PlaceInDockResult);

            i.SetSendHostFlag(true, Vars.STATUS, Vars.DOCK, Vars.START_TIME, Vars.END_TIME);

            if (behavior is IPickingBatchBehavior)
            {
                i.GetVariablesOdr();
                i.SetSendHostFlag(false, Vars.STATUS, Vars.DOCK, Vars.START_TIME, Vars.END_TIME);
            }
            else
            {
                i.SetSendHostFlag(true, Vars.RESPONSETYPE);
            }

            i.AssignStr(Vars.CURRENT_AISLE, string.Empty);
        }
    }
}
