// <copyright file="Checklist.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// A Checklist loaded to be processed by voice.
/// </summary>
public class Checklist
{
    /// <summary>
    /// Gets a collection of questions which conforms the checklist.
    /// </summary>
    public IEnumerable<IQuestion> Questions { get; init; } = null!;

    /// <summary>
    /// Gets or sets timestamp of checklist completion.
    /// </summary>
    public DateTime? Completed { get; set; } = null!;

    /// <summary>
    /// Gets or sets the operator who completed the checklist.
    /// </summary>
    public string? CompletedBy { get; set; } = null!;
}
