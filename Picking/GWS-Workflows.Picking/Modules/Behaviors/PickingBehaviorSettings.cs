// <copyright file="PickingBehaviorSettings.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.SDK;
using System.Collections.Generic;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;

/// <summary>
/// Settings class for configuration.
/// </summary>
public class PickingBehaviorSettings : ConnectorBehaviorSettingsBase, IPickingBehaviorSettings
{
    /// <summary>
    /// Key value used to define break options.
    /// </summary>
    protected const string BreakOptionsKey = "MenuOptions:TakeABreak";

    /// <summary>
    /// Key value used to define whether server logs are enabled.
    /// </summary>
    protected const string LogServerKey = "Log:Server";

    /// <summary>
    /// Initializes a new instance of the <see cref="PickingBehaviorSettings"/> class.
    /// </summary>
    /// <param name="configService"><inheritdoc/></param>
    public PickingBehaviorSettings(IConfigService configService)
        : base(configService)
    {
    }

    /// <inheritdoc/>
    public bool ServerLogEnabled
    {
        get => GetConfigValue<bool>(LogServerKey);
        set => SaveConfig(LogServerKey, value);
    }

    /// <inheritdoc/>
    public string? BreakOptions
    {
        get => GetConfigValue<string>(BreakOptionsKey);
        set => SaveConfig(BreakOptionsKey, value);
    }

    /// <inheritdoc/>
    protected override Dictionary<string, string> DefaultValues { get; } = new()
    {
        { ServerKey, string.Empty },
        { LogDeviceKey, "true" },
        { LogServerKey, "true" },
        { BreakOptionsKey, "WC|Break" },
    };
}