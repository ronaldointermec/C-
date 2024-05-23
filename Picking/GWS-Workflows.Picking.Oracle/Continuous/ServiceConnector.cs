// <copyright file="ServiceConnector.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>
#if NETFRAMEWORK
using Honeywell.GWS.Connector.SDK.Service;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle.Continuous;

/// <summary>
/// An implementation for ServiceConnector for GWS-Workflows - Picking (ORACLE Continuous) to be used with GWS Connector Service.
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