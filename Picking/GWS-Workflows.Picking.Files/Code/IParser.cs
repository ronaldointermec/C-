// <copyright file="IParser.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;

/// <summary>
/// Utility interface for parsing information in different formats.
/// </summary>
public interface IParser
{
    /// <summary>
    /// Parse into an object.
    /// </summary>
    /// <param name="contents">A string containing serialized object to be parsed.</param>
    /// <typeparam name="TModel">Type of the object to create by parsing the string.</typeparam>
    /// <returns>An object representing the contents string.</returns>
    TModel Parse<TModel>(string contents)
        where TModel : class;

    /// <summary>
    /// Serialize an object to a string.
    /// </summary>
    /// <param name="model">Object to be serialized.</param>
    /// <typeparam name="TModel">Type of the object to be serialized.</typeparam>
    /// <returns>A string representing the serialized object.</returns>
    string Serialize<TModel>(TModel model)
        where TModel : class;
}