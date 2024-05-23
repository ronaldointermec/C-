// <copyright file="BeginPickingOrder.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;
using System.Linq;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Implementation class for Begin Picking Order work.
/// </summary>
public class BeginPickingOrder : GetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BeginPickingOrder"/> class.
    /// </summary>
    public BeginPickingOrder()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BeginPickingOrder"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    /// <param name="message">Optional message.</param>
    public BeginPickingOrder(string code, string? message)
        : base(code, message)
    {
    }

    /// <summary>
    /// Gets the order number.
    /// </summary>
    public virtual string OrderNumber { get; init; } = null!;

    /// <summary>
    /// Gets the order customer.
    /// </summary>
    public virtual string? Customer { get; init; }

    /// <summary>
    /// Gets the number of containers to prepare.
    /// </summary>
    public virtual int? ContainersCount { get; init; }

    /// <summary>
    /// Gets the container type.
    /// </summary>
    public virtual string? ContainerType { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed.
    /// </summary>
    public virtual Uri? Image { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(InstructionSet i, IPickingBehavior behavior, IDevice device)
    {
        i.AssignStr(Vars.CURRENT_AISLE, string.Empty);
        i.AssignStr(Vars.CURRENT_POSITION, string.Empty);
        if (!string.IsNullOrEmpty(Customer))
            i.AssignStr(Vars.CUSTOMER, Customer);

        i.AssignNum(Vars.START_TIME, "#time");

        i.Label($"{Labels.START}_{Code}");
        {
            i.SetCommands(command03: $"{Labels.BREAK}_{Code}");

            i.AssignStr(Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.BeginPickingOrder_Order), OrderNumber));

            if (!string.IsNullOrEmpty(Customer))
                i.Concat(Vars.PROMPT, $", {Customer}");

            if (!string.IsNullOrEmpty(ContainerType))
                i.Concat(Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.BeginPickingOrder_ContainerType), ContainerType));

            if (ContainersCount.HasValue)
                i.Concat(Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.BeginPickingOrder_ContainersCount), ContainersCount));

            i.Say($"${Vars.PROMPT}", true, true, imageUrl: Image?.AbsoluteUri);

            if (!string.IsNullOrEmpty(Message))
                i.Say(Message, true, true);

            i.GoTo($"{Labels.END}_{Code}");
        }

        i.Label($"{Labels.BREAK}_{Code}");
        {
            // If the user says 'cancel' it returns at the beginning
            i.SetCommands(command09: $"{Labels.START}_{Code}");
            SharedDialogs.Break(i, behavior.Settings.BreakOptions?.Any() ?? false, device);

            i.GoTo($"{Labels.START}_{Code}");
        }

        i.Label($"{Labels.END}_{Code}");
        {
            i.AssignNum(Vars.END_TIME, "#time");
            i.AssignStr(Vars.STATUS, "OK");
            i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.BeginPickingResult);

            i.SetSendHostFlag(true, Vars.STATUS, Vars.START_TIME, Vars.END_TIME);
            if (behavior is IPickingBatchBehavior)
            {
                i.GetVariablesOdr();
                i.SetSendHostFlag(false, Vars.STATUS, Vars.START_TIME, Vars.END_TIME);
            }
            else
            {
                i.SetSendHostFlag(true, Vars.RESPONSETYPE);
            }
        }
    }
}
