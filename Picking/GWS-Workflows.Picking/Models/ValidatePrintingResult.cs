// <copyright file="ValidatePrintingResult.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Validate Printing work result.
/// </summary>
public class ValidatePrintingResult : SetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatePrintingResult"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    public ValidatePrintingResult(string code)
        : base(code)
    {
    }

    /// <summary>
    /// Gets the labels read by the operator.
    /// </summary>
    public IEnumerable<string> ReadLabels { get; init; } = null!;
}
