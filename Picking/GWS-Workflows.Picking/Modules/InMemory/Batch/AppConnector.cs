// <copyright file="AppConnector.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if !NETFRAMEWORK
using Honeywell.Firebird.Module;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.SDK.App;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.InMemory.Batch;

/// <summary>
/// An implementation for ServiceConnector for GWS-Workflows - Picking (Memory Batch) to be used with GWS Connector App.
/// </summary>
public class AppConnector : ConnectorBase<Behavior, PickingBehaviorSettings, Workflow>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppConnector"/> class.
    /// </summary>
    /// <param name="context"><inheritdoc/></param>
    public AppConnector(IAppBaseModuleContext context)
        : base(context)
    {
    }

    /// <inheritdoc/>
    public override string ConnectorName => "Memory Batch";
}
#endif