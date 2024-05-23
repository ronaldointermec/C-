// <copyright file="IPickingBehavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;

/// <summary>
/// Base interface for Picking Behavior.
/// </summary>
public interface IPickingBehavior : IConnectorBehavior<IPickingBehaviorSettings>
{
    /// <summary>
    /// Device establish connection with the Server and retrieves required information for handling the signOn request.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device object.</param>
    /// <returns>Result of the connection attempt.</returns>
    ValueTask<ConnectResult> ConnectAsync(string operatorName, IDevice device);

    /// <summary>
    /// Device try to disconnect from the Server and retrieves if it is allowed or not.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device serial number provided by device request.</param>
    /// <param name="force">Indicates if the operation must be forced.</param>
    /// <returns>If operation is allowed to signoff.</returns>
    ValueTask<DisconnectResult> DisconnectAsync(string operatorName, string device, bool force);

    /// <summary>
    /// Operator initiates a break.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device serial number provided by device request.</param>
    /// <param name="res">Begin break request object.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task BeginBreakAsync(string operatorName, string device, BeginBreak res);

    /// <summary>
    /// Operator ends a break.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device serial number provided by device request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EndBreakAsync(string operatorName, string device);

    /// <summary>
    /// Register the start of the operator's work.
    /// </summary>
    /// <param name="operatorName">Operator identifier provided by device request.</param>
    /// <param name="device">Device serial number provided by device request.</param>
    /// <returns>Initial message for operator.</returns>
    Task<string?> RegisterOperatorStartAsync(string operatorName, string device);
}

/// <summary>
/// Result of the Sign On attempt operation.
/// </summary>
/// <param name="Allowed">Allowed to sign on.</param>
/// <param name="Password">Password to be spoken by the operator.</param>
/// <param name="Message">Message to be spoken to the operator.</param>
public record ConnectResult(bool Allowed, string? Password, string? Message);

/// <summary>
/// Result of the Sign Off attempt operation.
/// </summary>
/// <param name="Allowed">Allowed to sign off.</param>
/// <param name="Message">Message to be spoken to the operator.</param>
public record DisconnectResult(bool Allowed, string? Message);