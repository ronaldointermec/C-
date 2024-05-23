// <copyright file="IWorkOrderItem.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;

/// <summary>
/// Base interface for work order item.
/// </summary>
public interface IWorkOrderItem
{
    /// <summary>
    /// Gets work identifier.
    /// </summary>
    string Code { get; }
}