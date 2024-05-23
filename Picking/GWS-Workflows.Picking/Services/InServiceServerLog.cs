// <copyright file="InServiceServerLog.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>
#if NETFRAMEWORK
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Services;

/// <summary>
/// Implementation for storing Server communications related messages into a file.
/// </summary>
public class InServiceServerLog : IServerLog
{
    private static readonly object _serverLogLock = new();
    private static readonly string _programDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    /// <inheritdoc/>
    public void WriteLog(string msg)
    {
        Task.Run(() =>
        {
            lock (_serverLogLock)
            {
                var logDir = Path.Combine(_programDir, string.Format(@"Logs\{0:yyyy}\{0:MM}\{0:dd}", DateTime.Now));
                Directory.CreateDirectory(logDir);
                File.AppendAllText(Path.Combine(logDir, string.Format("ServerLog-{0:yyyy-MM-dd}.txt", DateTime.Now)), $"[{DateTime.Now}] {msg}{Environment.NewLine}");
            }
        });
    }
}
#endif