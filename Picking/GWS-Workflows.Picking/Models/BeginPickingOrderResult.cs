// <copyright file="BeginPickingOrderResult.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Begin Picking Order work result.
/// </summary>
public class BeginPickingOrderResult : SetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BeginPickingOrderResult"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    public BeginPickingOrderResult(string code)
        : base(code)
    {
    }
}
