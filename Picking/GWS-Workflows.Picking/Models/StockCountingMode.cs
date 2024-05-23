// <copyright file="StockCountingMode.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Enumeration for stock counting modes.
/// </summary>
public enum StockCountingMode
{
    /// <summary>
    /// Do not count stock.
    /// </summary>
    No,

    /// <summary>
    /// Count stock only if the quantity picked is partial.
    /// </summary>
    PartialPicked,

    /// <summary>
    /// Count stock.
    /// </summary>
    Always,
}