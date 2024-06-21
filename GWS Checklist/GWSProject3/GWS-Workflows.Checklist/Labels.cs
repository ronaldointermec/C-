// <copyright file="Labels.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist;

/// <summary>
/// Labels Helper class.
/// </summary>
public static class Labels
{
    /// <summary>
    /// Beginning of a question.
    /// </summary>
    internal static readonly string Begin = "LABEL_BEGIN";

    /// <summary>
    /// End of a question.
    /// </summary>
    internal static readonly string End = "LABEL_END";

    /// <summary>
    /// Leave current checklist.
    /// </summary>
    internal static readonly string LeaveJob = "LABEL_LEAVEJOB";

    /// <summary>
    /// Wrong answer.
    /// </summary>
    internal static readonly string Wrong = "LABEL_WRONG";

    /// <summary>
    /// Ask question prompt.
    /// </summary>
    internal static readonly string Ask = "LABEL_ASK";

    /// <summary>
    /// Answer is correct or completed.
    /// </summary>
    internal static readonly string Ok = "LABEL_OK";

    /// <summary>
    /// Undo current answer.
    /// </summary>
    internal static readonly string Undo = "LABEL_UNDO";

    /// <summary>
    /// Complete current process.
    /// </summary>
    internal static readonly string Complete = "LABEL_COMPLETE";
}
