﻿// <copyright file="PickingJsonConverter.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;

/// <summary>
/// JSON Converter to create the right type, based on the "type" property of the JSON parsed.
/// </summary>
public class PickingJsonConverter : JsonConverter
{
    /// <inheritdoc/>
    public override bool CanWrite => false;

    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        return typeof(IGetWorkOrderItem).IsAssignableFrom(objectType);
    }

    /// <inheritdoc/>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
            return null;

        var item = JObject.Load(reader);
        if (item is null)
            return null;

        var itemType = item["type"]?.Value<string>();

        return itemType switch
        {
            nameof(AskQuestion) => item.ToObject<AskQuestion>(),
            nameof(BeginPickingOrder) => item.ToObject<BeginPickingOrder>(),
            nameof(BeginBreak) => item.ToObject<BeginBreak>(),
            nameof(EmptyWork) => item.ToObject<EmptyWork>(),
            nameof(PickingLine) => item.ToObject<PickingLine>(),
            nameof(PlaceInDock) => item.ToObject<PlaceInDock>(),
            nameof(PrintLabels) => item.ToObject<PrintLabels>(),
            nameof(ValidatePrinting) => item.ToObject<ValidatePrinting>(),

            _ => null,
        };
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => throw new NotImplementedException();
}
