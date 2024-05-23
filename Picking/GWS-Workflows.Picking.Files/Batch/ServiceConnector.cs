// <copyright file="ServiceConnector.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>
#if NETFRAMEWORK
using Honeywell.GWS.Connector.SDK.Service;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Batch;

/// <summary>
/// Base implementation for File Task Connector.
/// </summary>
public class ServiceConnector : ConnectorBase<Behavior>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceConnector"/> class.
    /// </summary>
    /// <param name="connectorBehavior">Base connector behavior.</param>
    public ServiceConnector(Behavior connectorBehavior)
        : base(connectorBehavior)
    {
    }
}
#endif