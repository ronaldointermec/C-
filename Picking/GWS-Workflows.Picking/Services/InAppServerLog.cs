// <copyright file="InAppServerLog.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if !NETFRAMEWORK
using Common.Logging;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Services;

/// <summary>
/// Implementation for storing Server communications related messages into App log system.
/// </summary>
public class InAppServerLog : IServerLog
{
    private readonly ILog _log = TinyIoC.TinyIoCContainer.Current.Resolve<ILoggerFactoryAdapter>().GetLogger("ServerLog");

    /// <inheritdoc/>
    public void WriteLog(string msg)
    {
        _log.Info(msg);
    }
}
#endif