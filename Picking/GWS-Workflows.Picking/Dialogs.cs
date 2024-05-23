// <copyright file="Dialogs.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking;

/// <summary>
/// Enum class to keep track of dialog types.
/// Based on a State Machine, dialogs will move from one state to another.
/// </summary>
public enum Dialogs
{
    /// <summary>
    /// Starting execution point. Execution points to <see cref="Work"/> state after running this one.
    /// </summary>
    Start,

    /// <summary>
    /// Work. Main dialog as the execution will remain in this state for the rest of the session.
    /// </summary>
    Work,
}
