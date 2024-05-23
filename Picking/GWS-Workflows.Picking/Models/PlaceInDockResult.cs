// <copyright file="PlaceInDockResult.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Place In Dock work result.
/// </summary>
public class PlaceInDockResult : SetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceInDockResult"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    public PlaceInDockResult(string code)
        : base(code)
    {
    }

    /// <summary>
    /// Gets the dock where the container has been placed.
    /// </summary>
    public string? Dock { get; init; }
}