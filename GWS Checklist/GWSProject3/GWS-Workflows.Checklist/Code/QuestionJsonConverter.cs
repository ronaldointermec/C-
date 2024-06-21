// <copyright file="QuestionJsonConverter.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;

/// <summary>
/// JSON Converter to create the right <see cref="IQuestion"/> implementation type, based on the "type" property of the JSON parsed.
/// </summary>
public class QuestionJsonConverter : JsonConverter<IQuestion>
{
    /// <inheritdoc/>
    public override bool CanWrite => false;

    /// <inheritdoc/>
    public override IQuestion? ReadJson(JsonReader reader, Type objectType, IQuestion? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var json = JObject.Load(reader);
        if (json is null)
            return null;

        var type = json["type"]?.ToObject<string>();

        return type switch
        {
            "message" => json.ToObject<Message>(),
            "ask" => json.ToObject<Ask>(),
            "number" => json.ToObject<IntegerValue>(),
            "decimal" => json.ToObject<FloatValue>(),
            "string" => json.ToObject<StringValue>(),
            "date" => json.ToObject<Date>(),
            "time" => json.ToObject<Time>(),
            "choice" => json.ToObject<Select>(),
            "multiple_choice" => json.ToObject<SelectMultiple>(),
            _ => throw new InvalidOperationException(string.Format(Properties.Resources.JsonParser_InvalidType, type)),
        };
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, IQuestion? value, JsonSerializer serializer) => throw new NotImplementedException();
}
