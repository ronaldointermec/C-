// <copyright file="AppConnectorBase.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if !NETFRAMEWORK
using Honeywell.Firebird.Module;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.SDK.App;
using Honeywell.GWS.Connector.SDK.Interfaces;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;

/// <summary>
/// Abstract class for all AppConnector implementations.
/// </summary>
/// <typeparam name="TConnectorBehavior">Type of ConnectorBehavior.</typeparam>
/// <typeparam name="TConnectorBehaviorSettings">Type of ConnectorBehavior settings.</typeparam>
/// <typeparam name="TWorkflow">Type of Workflow.</typeparam>
public abstract class AppConnectorBase<TConnectorBehavior, TConnectorBehaviorSettings, TWorkflow> : ConnectorBase<TConnectorBehavior, TConnectorBehaviorSettings, TWorkflow>
    where TConnectorBehavior : class, IConnectorBehavior<TConnectorBehaviorSettings>
    where TConnectorBehaviorSettings : class, IConnectorBehaviorSettings
    where TWorkflow : class, IWorkflow
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppConnectorBase{TConnectorBehavior, TConnectorBehaviorSettings, TWorkflow}"/> class.
    /// </summary>
    /// <param name="context">Context instance.</param>
    protected AppConnectorBase(IAppBaseModuleContext context)
        : base(context)
    {
    }

    /// <inheritdoc/>
    public override void RegisterServices()
    {
        base.RegisterServices();

        Context.Container.Register<IServerLog, InAppServerLog>();
    }
}
#endif