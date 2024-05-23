// <copyright file="AskQuestionResult.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Ask Question work result.
/// </summary>
public class AskQuestionResult : SetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AskQuestionResult"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    public AskQuestionResult(string code)
        : base(code)
    {
    }
}
