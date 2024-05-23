// <copyright file="ConnectModel.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Models;

/// <summary>
/// Result for REST request performed in <see cref="Modules.Behaviors.IPickingBehavior.ConnectAsync(string, SDK.Interfaces.IDevice)"/>.
/// </summary>
/// <param name="Name">Operator name.</param>
/// <param name="Allowed">Allowed to connect.</param>
/// <param name="Pwd">Optional password.</param>
/// <param name="Message">Message spoken on sign on.</param>
public record ConnectModel(string Name, bool Allowed, string? Pwd, string? Message);