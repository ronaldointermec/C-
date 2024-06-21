// <copyright file="ConnectorBehavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules;

/// <summary>
/// A base implementation for <see cref="ConnectorBehavior{TConnectorBehaviorSettings}"/>.
/// </summary>
/// <remarks>Includes common functionality for working with Checklists and expose methods for concrete implementations.</remarks>
/// <typeparam name="TConnectorBehaviorSettings">Type of Settings used.</typeparam>
public abstract class ConnectorBehavior<TConnectorBehaviorSettings> : ConnectorBehaviorBase<TConnectorBehaviorSettings>, IConnectorBehavior
    where TConnectorBehaviorSettings : ConnectorBehaviorSettingsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectorBehavior{TConnectorBehaviorSettings}"/> class.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    protected ConnectorBehavior(TConnectorBehaviorSettings settings)
        : base(settings)
    {
    }

    /// <inheritdoc/>
    public abstract Operator? GetOperator(string @operator);

    /// <inheritdoc/>
    public Models.Checklist? GetChecklist(string @operator, string id)
    {
        var checklist = RetrieveChecklist(@operator, id);
        if (checklist is null)
            return null;

        if (checklist.Questions.GroupBy(x => x.Code).Any(x => x.Count() > 1))
            throw new InvalidOperationException(string.Format(Resources.Behavior_RepeatedQuestions, id));

        return checklist;
    }

    /// <inheritdoc/>
    public void SubmitChecklistResult(string @operator, string id, string code, string? answer, DateTime startTime, DateTime? endTime)
    {
        var checklist = RetrieveChecklist(@operator, id) ?? throw new InvalidOperationException(string.Format(Resources.Behavior_NotFound, id));

        var question = checklist.Questions.SingleOrDefault(x => x.Code == code) ?? throw new InvalidOperationException(string.Format(Resources.Behavior_QuestionNotFound, id, code));

        question.Operator = @operator;
        question.StartTime = startTime;
        question.EndTime = endTime;

        var prop = question.GetType().GetProperty("Answer");

        if (prop is not null)
        {
            try
            {
                SetValue(prop, question, answer);
            }
            catch (Exception ex)
            {
                Log(string.Format(Resources.Behavior_ErrorSettingAnswer, id, code, answer, ex.GetType(), ex.Message), LogLevel.Error);
            }
        }

        UpdateChecklist(@operator, id, checklist);
    }

    /// <inheritdoc/>
    public bool CompleteChecklist(string @operator, string id, bool force)
    {
        var checklist = RetrieveChecklist(@operator, id) ?? throw new InvalidOperationException(string.Format(Resources.Behavior_NotFound, id));

        var skipped = checklist.Questions.Any(x => !x.EndTime.HasValue);
        if (skipped && !force)
            return false;

        checklist.Completed = DateTime.Now;
        checklist.CompletedBy = @operator;

        UpdateChecklist(@operator, id, checklist);

        return true;
    }

    /// <summary>
    /// Retrieve a Checklist from storage.
    /// </summary>
    /// <param name="operator">Operator retrieving the checklist.</param>
    /// <param name="id">Checklist identifier to be retrieved.</param>
    /// <returns>A checklist loaded from storage.</returns>
    public abstract Models.Checklist? RetrieveChecklist(string @operator, string id);

    /// <summary>
    /// Updates a Checklist to the storage.
    /// </summary>
    /// <param name="operator">Operator performing the operation.</param>
    /// <param name="id">Checklist identifier to be updated.</param>
    /// <param name="checklist">The checklist to be updated.</param>
    public abstract void UpdateChecklist(string @operator, string id, Models.Checklist checklist);

    private static DateTime ParseDateAnswer(string answer, string format) => DateTime.ParseExact(answer.Replace(" ", string.Empty), format, CultureInfo.InvariantCulture);

    private static TimeSpan ParseTimeAnswer(string answer, string format) => TimeSpan.ParseExact(answer.Replace(" ", string.Empty), format, CultureInfo.InvariantCulture);

    private static IEnumerable<short> ParseSelectMultipleAnswer(string answer) => answer.TrimStart().Split(' ').Select(x => Convert.ToInt16(x));

    private void SetValue(PropertyInfo prop, IQuestion question, string? answer)
    {
        if (answer is null)
        {
            prop.SetValue(question, null);
        }
        else
        {
            if (question is Date dateQuestion)
            {
                dateQuestion.Answer = ParseDateAnswer(answer, dateQuestion.Format);
            }
            else if (question is Time timeQuestion)
            {
                timeQuestion.Answer = ParseTimeAnswer(answer, timeQuestion.Format);
            }
            else if (question is SelectMultiple selectMultipleQuestion)
            {
                selectMultipleQuestion.Answer = ParseSelectMultipleAnswer(answer);
            }
            else
            {
                prop.SetValue(question, Convert.ChangeType(answer, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
            }
        }
    }
}

/// <summary>
/// A simple implementation for <see cref="ConnectorBehavior{TConnectorBehaviorSettings}"/> using <see cref="ConnectorBehaviorSettingsBase"/>.
/// </summary>
public abstract class ConnectorBehavior : ConnectorBehavior<ConnectorBehaviorSettingsBase>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectorBehavior"/> class.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    protected ConnectorBehavior(ConnectorBehaviorSettingsBase settings)
        : base(settings)
    {
    }
}