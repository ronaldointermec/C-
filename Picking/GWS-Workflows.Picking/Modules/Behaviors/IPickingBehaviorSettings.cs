// <copyright file="IPickingBehaviorSettings.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.SDK.Interfaces;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors
{
    /// <summary>
    /// Interface for common Picking settings.
    /// </summary>
    public interface IPickingBehaviorSettings : IConnectorBehaviorSettings
    {
        /// <summary>
        /// Gets a value indicating whether if the server log is enabled.
        /// </summary>
        bool ServerLogEnabled { get; }

        /// <summary>
        /// Gets information of the break options.
        /// </summary>
        string? BreakOptions { get; }
    }
}