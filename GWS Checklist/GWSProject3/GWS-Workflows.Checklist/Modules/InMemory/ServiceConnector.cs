// <copyright file="ServiceConnector.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if NET472
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules.InMemory;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.GWS.Connector.SDK.Interfaces.Service;
using Honeywell.GWS.Connector.SDK.Service;
using System.Collections.Generic;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules.InMemory;

/// <summary>
/// Base implementation for Memory Task Connector.
/// </summary>
public class ServiceConnector : ConnectorBase<Behavior>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceConnector"/> class.
    /// </summary>
    /// <param name="connectorBehavior">ConnectorBehavior instance.</param>
    public ServiceConnector(Behavior connectorBehavior)
        : base(connectorBehavior)
    {
    }
}
#endif