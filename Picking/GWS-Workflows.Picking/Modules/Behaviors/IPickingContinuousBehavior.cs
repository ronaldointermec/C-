// <copyright file="IPickingContinuousBehavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;

/// <summary>
/// Base generic interface for Picking Continuous Behavior.
/// </summary>
public interface IPickingContinuousBehavior
{
    /// <summary>
    /// Gets assigned work order.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device serial number provided by device request.</param>
    /// <returns>The work order item.</returns>
    Task<IGetWorkOrderItem?> GetWorkOrderAsync(string operatorName, string device);

    /// <summary>
    /// Sets the work result.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device serial number provided by device request.</param>
    /// <param name="res">Work order result object.</param>
    /// <returns>Next work to perform by operator.</returns>
    Task<IGetWorkOrderItem?> SetWorkOrderAsync(string operatorName, string device, ISetWorkOrderItem res);
}