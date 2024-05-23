// <copyright file="FilesBehaviorSettings.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using System.Collections.Generic;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files;

/// <summary>
/// Settings class for configuration.
/// </summary>
public class FilesBehaviorSettings : PickingBehaviorSettings
{
    private const string FileFormatKey = "File:Format";
    private const string FileEncodingKey = "File:Encoding";

    /// <summary>
    /// Initializes a new instance of the <see cref="FilesBehaviorSettings"/> class.
    /// </summary>
    /// <param name="configService"><inheritdoc/></param>
    public FilesBehaviorSettings(IConfigService configService)
        : base(configService)
    {
    }

    /// <summary>
    /// Gets or sets the information of the file format.
    /// </summary>
    public string FileFormat
    {
        get => GetConfigValue<string>(FileFormatKey);
        set => SaveConfig(FileFormatKey, value);
    }

    /// <summary>
    /// Gets or sets the information of the file encoding.
    /// </summary>
    public string FileEncoding
    {
        get => GetConfigValue<string>(FileEncodingKey);
        set => SaveConfig(FileEncodingKey, value);
    }

    /// <inheritdoc/>
    protected override Dictionary<string, string> DefaultValues { get; } = new()
    {
        { ServerKey, string.Empty },
        { LogDeviceKey, "true" },
        { LogServerKey, "true" },
        { BreakOptionsKey, "WC|Break" },
        { FileFormatKey, "Json" },
        { FileEncodingKey, "utf-8" },
    };
}
