// <copyright file="Operator.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// Model representing an Operator.
/// </summary>
public class Operator
{
    /// <summary>
    /// Gets name of the operator.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets password to be spoken by the operator to sign on (optional).
    /// </summary>
    public string? Password { get; init; }
}
