// <copyright file="ParserFactory.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;

/// <summary>
/// Base implementation for Parser Factory.
/// </summary>
public class ParserFactory : IParserFactory
{
    /// <inheritdoc/>
    public IParser GetParser(FileFormat format)
    {
        return format switch
        {
            FileFormat.Json => new JsonParser(new PickingJsonConverter()),
            FileFormat.Yaml => new YamlParser(),
            _ => throw new NotImplementedException(),
        };
    }
}
