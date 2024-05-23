// <copyright file="AppConnector.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if !NETFRAMEWORK
using Honeywell.Firebird.Module;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Newtonsoft.Json;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Continuous;

/// <summary>
/// Base implementation for AppConnector to be used with GWS Connector App. All implementations of Picking (REST Continuous) connectors implementations must inherit from this base class.
/// </summary>
/// <typeparam name="TConnectorBehavior">Type of ConnectorBehavior.</typeparam>
/// <typeparam name="TConnectorBehaviorSettings">Type of ConnectorBehaviorSettings.</typeparam>
/// <typeparam name="TWorkflow">Type of Workflow.</typeparam>
public abstract class AppConnector<TConnectorBehavior, TConnectorBehaviorSettings, TWorkflow> : AppConnectorBase<TConnectorBehavior, TConnectorBehaviorSettings, TWorkflow>
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
    public override string ConnectorName => "REST Continuous";

    /// <inheritdoc/>
    public override void RegisterServices()
    {
        base.RegisterServices();

        Context.Container.Register<JsonConverter, GetWorkOrderConverter>();
    }
}

/// <summary>
/// An implementation for AppConnector for GWS-Workflows - Picking (REST Continuous) to be used with GWS Connector App.
/// </summary>
public class AppConnector : AppConnector<Behavior, PickingBehaviorSettings, Workflow>
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