// <copyright file="Time.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// Question to be answered with multiple prompts which conforms a Time.
/// </summary>
public class Time : QuestionWithValueTypeAnswer<TimeSpan>
{
    private readonly Regex _timeFormatRegex = new(@"h{2}|m{2}|s{2}", RegexOptions.Compiled);

    /// <inheritdoc/>
    public override sealed string Type => "time";

    /// <summary>
    /// Gets a value indicating whether confirmation is enabled.
    /// </summary>
    public bool ConfirmationEnabled { get; init; }

    /// <summary>
    /// Gets format of the time.
    /// </summary>
    /// <remarks>It's a subset of C# standard time format strings.</remarks>
    /// <example>'hhmmss' or 'mmss'.</example>
    public string Format { get; init; } = null!;

    /// <summary>
    /// Gets a value with an image to be displayed when the question is showed.
    /// </summary>
    public Uri? Image { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(IDevice device, InstructionSet instr, string id, string? previousCode)
    {
        if (!Format.Any(x => new[] { 'h', 'm', 's' }.Contains(x)))
            throw new InvalidOperationException(string.Format(Resources.Time_InvalidFormat, id, Code, Format, DialogResources.Time_InvalidCharacters));

        var formatMatches = _timeFormatRegex.Matches(Format);
        if (formatMatches.Count == 0)
            throw new InvalidOperationException(string.Format(Resources.Time_InvalidFormat, id, Code, Format, DialogResources.Time_NoMatches));

        var skipCommand = SkipAllowed ? $"{Labels.End}_{Code}" : string.Empty;
        var undoCommand = !string.IsNullOrEmpty(previousCode) ? $"{Labels.Undo}_{previousCode}" : string.Empty;
        var hourAsked = false;
        var minuteAsked = false;
        var secondAsked = false;

        instr.SetCommands(Labels.LeaveJob, skipCommand, undoCommand);

        instr.Label($"{Labels.Ask}_{Code}");
        instr.Say(Prompt, priorityPrompt: true);

        instr.AssignStr(Vars.AUX, string.Empty);
        instr.AssignStr(Vars.HOUR, string.Empty);
        instr.AssignStr(Vars.MINUTE, string.Empty);
        instr.AssignStr(Vars.SECOND, string.Empty);

        foreach (Match match in formatMatches.Cast<Match>())
        {
            switch (match.Value)
            {
                case "hh":
                    AskHour(instr);
                    break;
                case "mm":
                    AskMinute(instr);
                    break;
                case "ss":
                    AskSecond(instr);
                    break;
            }
        }

        if (ConfirmationEnabled)
        {
            instr.Concat(Vars.AUX, "?");
            instr.Ask(Vars.DUMMY, $"${Vars.AUX}");
            instr.DoIf(Vars.DUMMY, false, Operation.EQ, _ =>
            {
                instr.AssignStr(Vars.ANSWER, string.Empty);
                instr.GoTo($"{Labels.Ask}_{Code}");
            });
        }
        else
        {
            instr.Say($"${Vars.AUX}");
        }

        void AskHour(InstructionSet instr)
        {
            if (hourAsked)
                throw new InvalidOperationException(string.Format(Resources.Time_InvalidFormat, id, Code, Format, DialogResources.Time_HourRepeated));

            hourAsked = true;
            instr.GetDigits(Vars.HOUR, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Time_Hour)), helpMsg: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.TwoDigits)), minLength: "2", maxLength: "2", minRange: "0", maxRange: "23", imageUrl: Image?.AbsoluteUri);
            instr.Concat(Vars.ANSWER, Vars.HOUR);

            instr.Concat(Vars.AUX, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Time_Hour)));
            instr.Concat(Vars.AUX, Vars.HOUR);
            instr.Concat(Vars.AUX, ",");
        }

        void AskMinute(InstructionSet instr)
        {
            if (minuteAsked)
                throw new InvalidOperationException(string.Format(Resources.Time_InvalidFormat, id, Code, Format, DialogResources.Time_MinuteRepeated));

            minuteAsked = true;
            instr.GetDigits(Vars.MINUTE, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Time_Minute)), helpMsg: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.TwoDigits)), minLength: "2", maxLength: "2", minRange: "0", maxRange: "59", imageUrl: Image?.AbsoluteUri);
            instr.Concat(Vars.ANSWER, Vars.MINUTE);

            instr.Concat(Vars.AUX, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Time_Minute)));
            instr.Concat(Vars.AUX, Vars.MINUTE);
            instr.Concat(Vars.AUX, ",");
        }

        void AskSecond(InstructionSet instr)
        {
            if (secondAsked)
                throw new InvalidOperationException(string.Format(Resources.Time_InvalidFormat, id, Code, Format, DialogResources.Time_SecondRepeated));

            secondAsked = true;
            instr.GetDigits(Vars.SECOND, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Time_Second)), helpMsg: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.TwoDigits)), minLength: "2", maxLength: "2", minRange: "0", maxRange: "59", imageUrl: Image?.AbsoluteUri);
            instr.Concat(Vars.ANSWER, Vars.SECOND);

            instr.Concat(Vars.AUX, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Time_Second)));
            instr.Concat(Vars.AUX, Vars.SECOND);
            instr.Concat(Vars.AUX, ",");
        }
    }
}
