﻿// <copyright file="JsonParser.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Files.Code;

/// <summary>
/// A JSON implementation of <see cref="IParser"/>.
/// </summary>
public class JsonParser : IParser
{
    private readonly JsonSerializerSettings _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonParser"/> class.
    /// </summary>
    /// <param name="questionConverter">Json question converter instance.</param>
    public JsonParser(JsonConverter<IQuestion> questionConverter)
    {
        _settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new[] { questionConverter },
        };
    }

    /// <inheritdoc/>
    public TModel Parse<TModel>(string contents)
        where TModel : class
    {
        if (contents is null)
            throw new ArgumentNullException(nameof(contents));

        return JsonConvert.DeserializeObject<TModel>(contents, _settings) ?? throw new InvalidOperationException(string.Format(Resources.JsonParser_FailedToParse, contents));
    }

    /// <inheritdoc/>
    public string Serialize<TModel>(TModel model)
        where TModel : class => JsonConvert.SerializeObject(model, _settings);
}