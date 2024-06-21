// <copyright file="Select.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System.Collections.Generic;
using System.Linq;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// Question to be answered with an option within offered <see cref="Options"/>.
/// </summary>
public class Select : QuestionWithValueTypeAnswer<short>
{
    /// <inheritdoc/>
    public override sealed string Type => "choice";

    /// <summary>
    /// Gets a value indicating whether confirmation is enabled.
    /// </summary>
    public bool ConfirmationEnabled { get; init; }

    /// <summary>
    /// Gets available options to be presented to the operator.
    /// </summary>
    public IEnumerable<SelectOption> Options { get; init; } = null!;

    /// <inheritdoc/>
    public override void BuildDialog(IDevice device, InstructionSet instr, string id, string? previousCode)
    {
        var skipCommand = SkipAllowed ? $"{Labels.End}_{Code}" : string.Empty;
        var undoCommand = !string.IsNullOrEmpty(previousCode) ? $"{Labels.Undo}_{previousCode}" : string.Empty;
        var confirmationPrompt = ConfirmationEnabled ? "?" : string.Empty;

        instr.SetCommands(Labels.LeaveJob, skipCommand, undoCommand);
        instr.GetMenu(Vars.ANSWER, Code, Prompt, AdditionalInformation, confirmationPrompt, wrongPrompt: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Select_Wrong)));

        if (!ConfirmationEnabled)
        {
            var f = Options.First();
            instr.DoIf(Vars.ANSWER, f.Code, Operation.EQ, ifb =>
            {
                instr.Say(f.Description, priorityPrompt: true);
                foreach (var o in Options.Skip(1))
                {
                    ifb.DoElseIf(Vars.ANSWER, o.Code, Operation.EQ);
                    instr.Say(o.Description, priorityPrompt: true);
                }
            });
        }
    }
}