// <copyright file="Operator.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Models;

/// <summary>
/// Model representing an Operator.
/// </summary>
public class Operator
{
    /// <summary>
    /// Gets or sets name of the operator.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets password to be spoken by the operator to sign on (optional).
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the name of the file used for work order result registry.
    /// </summary>
    public string? WorkOrderFileName { get; set; }

    /// <summary>
    /// Gets or sets operators last session start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets operators last session end time.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets operators last break start time.
    /// </summary>
    public DateTime? BreakStartTime { get; set; }

    /// <summary>
    /// Gets or sets operators last break end time.
    /// </summary>
    public DateTime? BreakEndTime { get; set; }

    /// <summary>
    /// Gets or sets work order identifier.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets break reason.
    /// </summary>
    public short? Reason { get; set; }
}
