// <copyright file="IDbDataReaderParser.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using System.Data;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Code;

/// <summary>
/// Utility class to extract the data from the database data reader object.
/// </summary>
public interface IDbDataReaderParser
{
    /// <summary>
    /// Extract work order item from database reader object.
    /// </summary>
    /// <param name="reader">Database reader object.</param>
    /// <returns>Work order to perform by operator.</returns>
    IGetWorkOrderItem? Parse(IDataReader reader);
}