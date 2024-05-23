// <copyright file="RegisterOperatorModel.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Models;

/// <summary>
/// Result for REST request performed in <see cref="Modules.Behaviors.IPickingBehavior.RegisterOperatorStartAsync(string, string)"/>.
/// </summary>
/// <param name="Message">Message to be spoken to the operator.</param>
public record RegisterOperatorModel(string Message);
