// <copyright file="PickingLineResult.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Picking Line work result.
/// </summary>
public class PickingLineResult : SetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PickingLineResult"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    public PickingLineResult(string code)
        : base(code)
    {
    }

    /// <summary>
    /// Gets the quantity picked by operator.
    /// </summary>
    public int? Picked { get; init; }

    /// <summary>
    /// Gets the picking line serving code prepared by operator.
    /// </summary>
    public string? ServingCode { get; init; }

    /// <summary>
    /// Gets the weight informed by operator when is required.
    /// </summary>
    public decimal? Weight { get; init; }

    /// <summary>
    /// Gets the stock quantity informed by operator when is required.
    /// </summary>
    public int? Stock { get; init; }

    /// <summary>
    /// Gets the dock informed by operator when is required.
    /// </summary>
    public string? Dock { get; init; }

    /// <summary>
    /// Gets the check digit product informed by operator when is required.
    /// </summary>
    public string? ProductCD { get; init; }

    /// <summary>
    /// Gets the breakage quantity informed by operator when is required.
    /// </summary>
    public int? Breakage { get; init; }

    /// <summary>
    /// Gets the batch informed by operator when is required.
    /// </summary>
    public string? Batch { get; init; }
}
