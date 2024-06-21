// <copyright file="Vars.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist;

/// <summary>
/// Variables Helper class.
/// </summary>
public static class Vars
{
    /// <summary>
    /// Current state.
    /// </summary>
    internal static readonly string DLG = "DLG";

    /// <summary>
    /// Current prompt.
    /// </summary>
    internal static readonly string PROMPT = "PROMPT";

    /// <summary>
    /// Auxiliar variable to hold intermediate results.
    /// </summary>
    internal static readonly string AUX = "AUX";

    /// <summary>
    /// Dummy variable to hold intermediate results.
    /// </summary>
    internal static readonly string DUMMY = "DUMMY";

    /// <summary>
    /// Flag variable to mark as completed.
    /// </summary>
    internal static readonly string COMPLETE = "COMPLETE";

    /// <summary>
    /// Current checklist identifier.
    /// </summary>
    internal static readonly string ID = "ID";

    /// <summary>
    /// Question code within current checklist.
    /// </summary>
    internal static readonly string CODE = "CODE";

    /// <summary>
    /// Answer collected for this question.
    /// </summary>
    internal static readonly string ANSWER = "ANSWER";

    /// <summary>
    /// Flag variable to erase the answer from the current question.
    /// </summary>
    internal static readonly string UNDO = "UNDO";

    /// <summary>
    /// Start time for this question.
    /// </summary>
    internal static readonly string START_TIME = "START_TIME";

    /// <summary>
    /// End time for this question.
    /// </summary>
    internal static readonly string END_TIME = "END_TIME";

    /// <summary>
    /// Intermediate variable to hold the year part in a Date answer.
    /// </summary>
    internal static readonly string YEAR = "YEAR";

    /// <summary>
    /// Intermediate variable to hold the month part in a Date answer.
    /// </summary>
    internal static readonly string MONTH = "MONTH";

    /// <summary>
    /// Intermediate variable to hold the day in part a Date answer.
    /// </summary>
    internal static readonly string DAY = "DAY";

    /// <summary>
    /// Intermediate variable to hold the hour in part a Time answer.
    /// </summary>
    internal static readonly string HOUR = "HOUR";

    /// <summary>
    /// Intermediate variable to hold the minute part in a Time answer.
    /// </summary>
    internal static readonly string MINUTE = "MINUTE";

    /// <summary>
    /// Intermediate variable to hold the second in part a Time answer.
    /// </summary>
    internal static readonly string SECOND = "SECOND";
}
