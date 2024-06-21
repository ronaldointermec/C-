// <copyright file="Ask.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// Question to be answered as 'Yes' or 'No'.
/// </summary>
public class Ask : QuestionWithValueTypeAnswer<bool>
{
    /// <inheritdoc/>
    public override sealed string Type => "ask";

    /// <summary>
    /// Gets a value indicating whether if question can be answered before <see cref="Question.Prompt"/> has been answered.
    /// </summary>
    public bool Priority { get; init; }

    /// <summary>
    /// Gets a value with an image to be displayed when the question is showed.
    /// </summary>
    public Uri? Image { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(IDevice device, InstructionSet instr, string id, string? previousCode)
    {
        var skipCommand = SkipAllowed ? $"{Labels.End}_{Code}" : string.Empty;
        var undoCommand = !string.IsNullOrEmpty(previousCode) ? $"{Labels.Undo}_{previousCode}" : string.Empty;

        instr.SetCommands(Labels.LeaveJob, skipCommand, undoCommand);
        instr.Ask(Vars.ANSWER, Prompt, "true", "false", AdditionalInformation, priorityPrompt: Priority, imageUrl: Image?.AbsoluteUri);
        instr.DoIf(Vars.ANSWER, "true", Operation.EQ, CompareType.Str, ifb =>
        {
            instr.Say($"VYES");
            ifb.DoElse();
            instr.Say($"VNO");
        });
    }
}
