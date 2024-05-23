// <copyright file="BeginBreak.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// BeginBreak request object.
/// </summary>
public class BeginBreak
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BeginBreak"/> class.
    /// </summary>
    /// <param name="code">Work order identifier.</param>
    public BeginBreak(string code) => Code = code;

    /// <summary>
    /// Gets work order identifier.
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// Gets or sets break reason.
    /// </summary>
    public short? Reason { get; set; }
}
