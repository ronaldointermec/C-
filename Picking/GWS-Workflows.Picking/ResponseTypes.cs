// <copyright file="ResponseTypes.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking;

/// <summary>
/// Enum class to keep track of response types.
/// </summary>
public enum ResponseTypes
{
    /// <summary>
    /// Saves result from BeginPickingOrder work.
    /// </summary>
    BeginPickingResult,

    /// <summary>
    /// Saves result from PickingLine work.
    /// </summary>
    PickingLineResult,

    /// <summary>
    /// Saves result from PrintLabels work.
    /// </summary>
    PrintLabelsResult,

    /// <summary>
    /// Saves result from ValidatePrinting work.
    /// </summary>
    ValidatePrintingResult,

    /// <summary>
    /// Saves result from PlaceInDock work.
    /// </summary>
    PlaceInDockResult,

    /// <summary>
    /// Sends BeginBreak request.
    /// </summary>
    BeginBreak,

    /// <summary>
    /// Sends EndBreak request.
    /// </summary>
    EndBreak,

    /// <summary>
    /// Saves result from PrintLabels batch work.
    /// </summary>
    PrintLabelsBatchResult,

    /// <summary>
    /// Saves result from AskQuestion work.
    /// </summary>
    AskQuestionResult,
}
