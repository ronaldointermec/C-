// <copyright file="SelectOption.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// Select Option used in <see cref="Select"/> and <see cref="SelectMultiple"/> questions.
/// </summary>
/// <param name="Code">Numeric code to be spoken by the operator for direct selection.</param>
/// <param name="Description">Description spoken to the operator.</param>
public record struct SelectOption(short Code, string Description);