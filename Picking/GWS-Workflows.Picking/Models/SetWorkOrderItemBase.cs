// <copyright file="SetWorkOrderItemBase.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Base implementation class for Set Work Order.
/// </summary>
public class SetWorkOrderItemBase : ISetWorkOrderItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetWorkOrderItemBase"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    public SetWorkOrderItemBase(string code) => Code = code;

    /// <inheritdoc/>
    public string Code { get; init; }

    /// <inheritdoc/>
    public string Status { get; init; } = null!;

    /// <inheritdoc/>
    public DateTime Started { get; init; }

    /// <inheritdoc/>
    public DateTime Finished { get; init; }
}
