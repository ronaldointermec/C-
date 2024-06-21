// <copyright file="SelectMultiple.cs" company="Honeywell | Safety and Productivity Solutions">
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
/// Question to be answered with one or more options within offered <see cref="Options"/>.
/// </summary>
public class SelectMultiple : QuestionWithReferenceTypeAnswer<IEnumerable<short>>
{
    /// <inheritdoc/>
    public override sealed string Type => "multiple_choice";

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

        instr.SetCommands(Labels.LeaveJob, skipCommand, undoCommand, $"{Labels.Complete}_{Code}");
        instr.AssignStr(Vars.PROMPT, Prompt);
        instr.AssignStr(Vars.AUX, string.Empty);

        instr.Label($"{Labels.Ask}_{Code}");
        instr.GetMenu(Vars.DUMMY, Code, $"${Vars.PROMPT}", AdditionalInformation, wrongPrompt: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Select_Wrong)));

        var f = Options.First();
        instr.DoIf(Vars.DUMMY, f.Code, Operation.EQ, ifb =>
        {
            instr.Say(f.Description, priorityPrompt: true);
            instr.Concat(Vars.AUX, $", {f.Description}");

            foreach (var o in Options.Skip(1))
            {
                ifb.DoElseIf(Vars.DUMMY, o.Code, Operation.EQ);
                instr.Say(o.Description, priorityPrompt: true);
                instr.Concat(Vars.AUX, $", {o.Description}");
            }
        });

        instr.Concat(Vars.ANSWER, Vars.DUMMY);
        instr.AssignStr(Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.SelectMultiple_Next)));
        instr.GoTo($"{Labels.Ask}_{Code}");

        instr.Label($"{Labels.Complete}_{Code}");

        if (ConfirmationEnabled)
        {
            instr.Concat(Vars.AUX, "?");
            instr.Ask(Vars.DUMMY, $"${Vars.AUX}");
            instr.DoIf(Vars.DUMMY, false, Operation.EQ, _ =>
            {
                instr.AssignStr(Vars.PROMPT, Prompt);
                instr.AssignStr(Vars.ANSWER, string.Empty);
                instr.AssignStr(Vars.AUX, string.Empty);

                instr.GoTo($"{Labels.Ask}_{Code}");
            });
        }
        else
        {
            instr.Say($"${Vars.AUX}");
        }
    }
}