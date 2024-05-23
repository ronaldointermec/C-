// <copyright file="IGetWorkOrderItem.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;

/// <summary>
/// Interface base for representing a work order item.
/// </summary>
public interface IGetWorkOrderItem : IWorkOrderItem
{
    /// <summary>
    /// Gets the optional message.
    /// </summary>
    string? Message { get; }

    /// <summary>
    /// Builds the dialog.
    /// </summary>
    /// <param name="i">The dialog object that is being build.</param>
    /// <param name="behavior">Picking behavior.</param>
    /// <param name="device">Device object.</param>
    void BuildDialog(InstructionSet i, IPickingBehavior behavior, IDevice device);
}