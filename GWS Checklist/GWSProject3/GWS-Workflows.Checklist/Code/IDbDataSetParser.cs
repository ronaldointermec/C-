// <copyright file="IDbDataSetParser.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using System.Collections.Generic;
using System.Data;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;

/// <summary>
/// Utility class to extract the data from the database dataset object.
/// </summary>
public interface IDbDataSetParser
{
    /// <summary>
    /// Extract array of questions from database dataset object.
    /// </summary>
    /// <param name="ds">DataSet object.</param>
    /// <returns>Question list to answer.</returns>
    IEnumerable<IQuestion> Parse(DataSet ds);
}
