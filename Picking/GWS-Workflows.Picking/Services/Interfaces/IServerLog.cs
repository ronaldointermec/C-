// <copyright file="IServerLog.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;

/// <summary>
/// Represents the interface for server logger.
/// </summary>
public interface IServerLog
{
    /// <summary>
    /// Writes a log mesage.
    /// </summary>
    /// <param name="msg">String of the log message.</param>
    public void WriteLog(string msg);
}
