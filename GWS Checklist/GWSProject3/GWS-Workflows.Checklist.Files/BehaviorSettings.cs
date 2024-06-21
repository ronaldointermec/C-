// <copyright file="BehaviorSettings.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.SDK;
using System;
using System.Collections.Generic;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Files;

/// <summary>
/// Settings class for configuration.
/// </summary>
public class BehaviorSettings : ConnectorBehaviorSettingsBase
{
    private const string FileFormatKey = "File:Format";
    private const string EncodingKey = "File:Encoding";

    /// <summary>
    /// Initializes a new instance of the <see cref="BehaviorSettings"/> class.
    /// </summary>
    /// <param name="configService"><inheritdoc/></param>
    public BehaviorSettings(IConfigService configService)
        : base(configService)
    {
    }

    /// <inheritdoc/>
    protected override Dictionary<string, string> DefaultValues => new()
    {
        { ServerKey, string.Empty },
        { LogDeviceKey, "true" },
        { FileFormatKey, "Yaml" },
        { EncodingKey, "utf-8" },
    };

    /// <summary>
    /// Gets or sets files encoding.
    /// </summary>
    public string Encoding
    {
        get => GetConfigValue<string>(EncodingKey);
        set => SaveConfig(EncodingKey, value);
    }

    /// <summary>
    /// Gets or sets the information of the rest options.
    /// </summary>
    public FileFormat? FileFormat
    {
        get => (FileFormat)Enum.Parse(typeof(FileFormat), GetConfig(FileFormatKey).Value);
        set => SaveConfig(FileFormatKey, value);
    }
}
