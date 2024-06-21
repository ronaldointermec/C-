// <copyright file="IConnectorBehavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules;

/// <summary>
/// Base interface for Checklist connector behavior.
/// </summary>
public interface IConnectorBehavior : SDK.Interfaces.IConnectorBehavior
{
    /// <summary>
    /// Retrieves an operator.
    /// </summary>
    /// <param name="operator">Operator code.</param>
    /// <returns>An Operator or null if not found.</returns>
    Operator? GetOperator(string @operator);

    /// <summary>
    /// Retrieves the checklist to be processed.
    /// </summary>
    /// <param name="operator">Operator requesting the checklist.</param>
    /// <param name="id">Checklist identifier.</param>
    /// <returns>A Checklist or null if not found.</returns>
    /// <remarks>Only questions unanswered are included in the response.</remarks>
    Models.Checklist? GetChecklist(string @operator, string id);

    /// <summary>
    /// Submit checklist results.
    /// </summary>
    /// <param name="operator">Operator submitting the checklist results.</param>
    /// <param name="id">Checklist identifier.</param>
    /// <param name="code">Code of the question answered.</param>
    /// <param name="answer">Answer value spoken by the operator, or null if question was skipped or undone.</param>
    /// <param name="startTime">Timestamp when the operator entered the question.</param>
    /// <param name="endTime">Timestamp when the operator leaved the question providing an answer, or null if question was skipped or undone.</param>
    /// <remarks>Questions may or not be answered, and it's answer can also be erased by passing null.</remarks>
    public void SubmitChecklistResult(string @operator, string id, string code, string? answer, DateTime startTime, DateTime? endTime);

    /// <summary>
    /// Marks a checklist as completed, only if has no skipped questions.
    /// </summary>
    /// <param name="operator">Operator completing the checklist.</param>
    /// <param name="id">Checklist identifier.</param>
    /// <param name="force">Force to complete the checklist even if there are skipped questions.</param>
    /// <returns>A bool indicating if the checklist has been completed or not.</returns>
    public bool CompleteChecklist(string @operator, string id, bool force);
}