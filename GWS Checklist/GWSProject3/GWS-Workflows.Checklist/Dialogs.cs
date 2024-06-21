// <copyright file="Dialogs.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist;

/// <summary>
/// Dialogs enumeration.
/// </summary>
public enum Dialogs
{
    /// <summary>
    /// Start state, for beginning of execution and get the checklist to be loaded.
    /// </summary>
    Start,

    /// <summary>
    /// Perform the loaded checklist.
    /// </summary>
    DoChecklist,

    /// <summary>
    /// Current checklist completed.
    /// </summary>
    Finished,
}
