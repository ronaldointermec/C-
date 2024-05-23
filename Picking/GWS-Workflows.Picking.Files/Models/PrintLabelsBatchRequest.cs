// <copyright file="PrintLabelsBatchRequest.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Models;

/// <summary>
/// PrintLabels file request object.
/// </summary>
public class PrintLabelsBatchRequest
{
    /// <summary>
    /// Gets or sets current operator name.
    /// </summary>
    public string OperatorName { get; set; } = null!;

    /// <summary>
    /// Gets or sets work order identifier.
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Gets or sets the number of labels to be printed.
    /// </summary>
    public int Count { get; set; }
}
