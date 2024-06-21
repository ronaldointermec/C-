﻿// <copyright file="AppConnector.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if !NETFRAMEWORK
using Honeywell.Firebird.Module;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.App;
using Honeywell.GWS.Connector.SDK.Interfaces;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules.InMemory;

/// <summary>
/// An implementation for ServiceConnector for GWS-Workflows - Checklist (InMemory) to be used with GWS Connector App.
/// </summary>
/// <typeparam name="TConnectorBehavior">Type of ConnectorBehavior.</typeparam>
/// <typeparam name="TConnectorBehaviorSettings">Type of ConnectorBehaviorSettings.</typeparam>
/// <typeparam name="TWorkflow">Type of Workflow.</typeparam>
public abstract class AppConnector<TConnectorBehavior, TConnectorBehaviorSettings, TWorkflow> : ConnectorBase<TConnectorBehavior, TConnectorBehaviorSettings, TWorkflow>
    where TConnectorBehavior : class, IConnectorBehavior<TConnectorBehaviorSettings>
    where TConnectorBehaviorSettings : class, IConnectorBehaviorSettings
    where TWorkflow : class, IWorkflow
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppConnector{TConnectorBehavior, TConnectorBehaviorSettings, TWorkflow}"/> class.
    /// </summary>
    /// <param name="context"><inheritdoc/></param>
    protected AppConnector(IAppBaseModuleContext context)
        : base(context)
    {
    }

    /// <inheritdoc/>
    public override string ConnectorName => "InMemory";
}

/// <summary>
/// An implementation for AppConnector for GWS-Workflows - Checklist (InMemory) to be used with GWS Connector App.
/// </summary>
public class AppConnector : AppConnector<Behavior, ConnectorBehaviorSettingsBase, Workflow>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppConnector"/> class.
    /// </summary>
    /// <param name="context"><inheritdoc/></param>
    public AppConnector(IAppBaseModuleContext context)
        : base(context)
    {
    }
}
#endif