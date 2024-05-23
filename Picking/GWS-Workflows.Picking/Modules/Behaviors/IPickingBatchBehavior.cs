// <copyright file="IPickingBatchBehavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;

/// <summary>
/// Base generic interface for Picking Batch Behavior.
/// </summary>
public interface IPickingBatchBehavior
{
    /// <summary>
    /// Gets a list of assigned work orders.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device serial number provided by device request.</param>
    /// <returns>A list of work order items.</returns>
    Task<IEnumerable<IGetWorkOrderItem>> GetWorkOrdersAsync(string operatorName, string device);

    /// <summary>
    /// Sets the work result.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device serial number provided by device request.</param>
    /// <param name="res">Work Order result object.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetWorkOrderAsync(string operatorName, string device, ISetWorkOrderItem res);

    /// <summary>
    /// Calls print labels function.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device serial number provided by device request.</param>
    /// <param name="res">PrintLabels request object.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PrintLabelsBatchAsync(string operatorName, string device, PrintLabelsBatch res);
}