// <copyright file="PrintLabelsResult.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Print Labels work result.
/// </summary>
public class PrintLabelsResult : SetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrintLabelsResult"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    public PrintLabelsResult(string code)
        : base(code)
    {
    }

    /// <summary>
    /// Gets the number of copies to print.
    /// </summary>
    public int? LabelsToPrint { get; init; }

    /// <summary>
    /// Gets the printer used to print the labels.
    /// </summary>
    public int? Printer { get; init; }
}
