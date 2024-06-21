// <copyright file="YamlParser.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Files.Code;

/// <summary>
/// A YAML implementation of <see cref="IParser"/>.
/// </summary>
public class YamlParser : IParser
{
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;
    private readonly IDictionary<string, Type> _types = new Dictionary<string, Type>()
    {
        { "message", typeof(Message) },
        { "ask", typeof(Ask) },
        { "number", typeof(IntegerValue) },
        { "decimal", typeof(FloatValue) },
        { "string", typeof(StringValue) },
        { "date", typeof(Date) },
        { "time", typeof(Time) },
        { "choice", typeof(Select) },
        { "multiple_choice", typeof(SelectMultiple) },

    };

    /// <summary>
    /// Initializes a new instance of the <see cref="YamlParser"/> class.
    /// </summary>
    public YamlParser()
    {
        foreach (var t in AdditionalTypes)
        {
            if (_types.ContainsKey(t.Key))
                _types[t.Key] = t.Value;
            else
                _types.Add(t.Key, t.Value);
        }

        DeserializerBuilder deserializerBuilder = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance);

        SerializerBuilder serializerBuilder = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .WithTypeConverter(new UriYamlTypeConverter())
        .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull);

        foreach (var t in _types)
        {
            deserializerBuilder.WithTagMapping($"tag:yaml.org,2002:{t.Key}", t.Value);
            serializerBuilder.WithTagMapping($"tag:yaml.org,2002:{t.Key}", t.Value);
        }

        _deserializer = deserializerBuilder.Build();
        _serializer = serializerBuilder.Build();
    }

    /// <summary>
    /// Gets additional types to be considered for deserialization.
    /// </summary>
    public virtual IDictionary<string, Type> AdditionalTypes { get; } = new Dictionary<string, Type>();

    /// <inheritdoc/>
    public TModel Parse<TModel>(string contents)
        where TModel : class => _deserializer.Deserialize<TModel>(contents);

    /// <inheritdoc/>
    public string Serialize<TModel>(TModel model)
        where TModel : class => _serializer.Serialize(model);

    internal sealed class UriYamlTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(Uri);

        public object ReadYaml(YamlDotNet.Core.IParser parser, Type type)
        {
            if (parser.Current is null)
                return null!;

            var value = ((Scalar)parser.Current).Value;
            parser.MoveNext();
            return new Uri(value);
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            if (value is null)
            {
                emitter.Emit(new Scalar(null, null, null!, ScalarStyle.Any, true, false));
                return;
            }

            var uri = (Uri)value;
            emitter.Emit(new Scalar(null, null, uri?.ToString()!, ScalarStyle.Any, true, false));
        }
    }
}