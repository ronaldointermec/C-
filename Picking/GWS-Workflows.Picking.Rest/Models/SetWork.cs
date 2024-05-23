// <copyright file="SetWork.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Models;

/// <summary>
/// Result for REST request performed in SetWorkOrderAsync requests. Depending of request, only related parameters will be used.
/// </summary>
/// <param name="Code">Work order code.</param>
/// <param name="OperatorName">Operator name.</param>
/// <param name="Device">Device identifier.</param>
/// <param name="Started">Timestamp for order started time.</param>
/// <param name="Finished">Timestamp for order finished time.</param>
/// <param name="Status">Work order completion status.</param>
/// <param name="Picked">Quantity picked.</param>
/// <param name="ServingCode">Serving coe.</param>
/// <param name="Weight">Weight.</param>
/// <param name="Dock">Dock placed. </param>
/// <param name="Stock">Stock remaining in location.</param>
/// <param name="ProductCD">Product validation code.</param>
/// <param name="BreakageQty">Quantity declared as breakage.</param>
/// <param name="HasPackage">If has package.</param>
/// <param name="Batch">Batch number.</param>
/// <param name="LabelsToPrint">Number of labels to print (only continuous mode).</param>
/// <param name="Printer">Printer chosen (only continuous mode).</param>
/// <param name="ReadLabels">Read labels (only continuous mode).</param>
public record SetWork(string Code, string OperatorName, string Device, DateTime Started, DateTime Finished, string Status,
    int? Picked, string? ServingCode, decimal? Weight, string? Dock, int? Stock, string? ProductCD, int? BreakageQty, bool? HasPackage, string? Batch, int? LabelsToPrint, int? Printer, IEnumerable<string>? ReadLabels);
