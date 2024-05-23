// <copyright file="PrintLabelsBatch.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// PrintLabels request object.
/// </summary>
public class PrintLabelsBatch
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrintLabelsBatch"/> class.
    /// </summary>
    /// <param name="code">Work order identifier.</param>
    public PrintLabelsBatch(string code) => Code = code;

    /// <summary>
    /// Gets work order identifier.
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// Gets the number of labels to be printed.
    /// </summary>
    public int Count { get; init; }
}
