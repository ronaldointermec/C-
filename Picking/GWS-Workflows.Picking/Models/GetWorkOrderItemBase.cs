// <copyright file="GetWorkOrderItemBase.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Abstract base class for <see cref="IGetWorkOrderItem"/>.
/// </summary>
public abstract class GetWorkOrderItemBase : IGetWorkOrderItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetWorkOrderItemBase"/> class.
    /// </summary>
    protected GetWorkOrderItemBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetWorkOrderItemBase"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    /// <param name="message">Optional message.</param>
    protected GetWorkOrderItemBase(string code, string? message)
    {
        Code = code;
        Message = message;
    }

    /// <inheritdoc/>
    public string Code { get; init; } = null!;

    /// <inheritdoc/>
    public string? Message { get; init; }

    /// <inheritdoc/>
    public abstract void BuildDialog(InstructionSet i, IPickingBehavior behavior, IDevice device);
}
