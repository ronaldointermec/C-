// <copyright file="ISetWorkOrderItem.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;

/// <summary>
/// Interface for set work order item.
/// </summary>
public interface ISetWorkOrderItem : IWorkOrderItem
{
    /// <summary>
    /// Gets work order result status.
    /// </summary>
    string Status { get; }

    /// <summary>
    /// Gets the work order start datetime.
    /// </summary>
    DateTime Started { get; }

    /// <summary>
    /// Gets the work order end datetime.
    /// </summary>
    DateTime Finished { get; }
}