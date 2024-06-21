// <copyright file="ParserFactory.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Files.Code;

/// <summary>
/// Base implementation for Parser Factory.
/// </summary>
public class ParserFactory : IParserFactory
{
    /// <inheritdoc/>
    public IParser GetParser(FileFormat? fileFormat)
    {
        return fileFormat switch
        {
            FileFormat.Json => new JsonParser(new QuestionJsonConverter()),
            FileFormat.Yaml => new YamlParser(),
            _ => throw new NotImplementedException(),
        };
    }
}