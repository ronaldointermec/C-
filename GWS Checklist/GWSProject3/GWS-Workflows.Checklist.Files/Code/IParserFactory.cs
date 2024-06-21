// <copyright file="IParserFactory.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Files.Code;

/// <summary>
/// Base interface for Parser Factory.
/// </summary>
public interface IParserFactory
{
    /// <summary>
    /// Gets a new parser based on the File format.
    /// </summary>
    /// <param name="fileFormat">File format.</param>
    /// <returns>A new <see cref="IParser"/> instance.</returns>
    IParser GetParser(FileFormat? fileFormat);
}