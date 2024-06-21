// <copyright file="Message.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// Message to be spoken to the operator.
/// </summary>
public class Message : Question
{
    /// <inheritdoc/>
    public override sealed string Type => "message";

    /// <summary>
    /// Gets a value indicating whether if question can be answered before <see cref="Question.Prompt"/> has been answered.
    /// </summary>
    public bool Priority { get; init; }

    /// <summary>
    /// Gets a value indicating whether if operator has to confirm to proceed with the next question.
    /// </summary>
    public bool ReadyToContinue { get; init; }

    /// <summary>
    /// Gets a value with an image to be displayed when the question is showed.
    /// </summary>
    public Uri? Image { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(IDevice device, InstructionSet instr, string id, string? previousCode)
    {
        var prompt = ReadyToContinue ? device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Message_ToContinueSayVCONFIRM), Prompt) : Prompt;
        var skipCommand = SkipAllowed ? $"{Labels.End}_{Code}" : string.Empty;
        var undoCommand = !string.IsNullOrEmpty(previousCode) ? $"{Labels.Undo}_{previousCode}" : string.Empty;

        instr.SetCommands(Labels.LeaveJob, skipCommand, undoCommand);
        instr.Say(prompt, ReadyToContinue, Priority, AdditionalInformation, imageUrl: Image?.AbsoluteUri);
    }
}
