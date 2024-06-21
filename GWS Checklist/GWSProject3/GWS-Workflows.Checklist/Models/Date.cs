// <copyright file="Date.cs" company="Honeywell | Safety and Productivity Solutions">
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
/// Question to be answered with multiple prompts which conforms a Date.
/// </summary>
public class Date : QuestionWithValueTypeAnswer<DateTime>
{
    private readonly Regex _dateFormatRegex = new(@"d{2}|M{2}|y{4}|y{2}", RegexOptions.Compiled);

    /// <inheritdoc/>
    public override sealed string Type => "date";

    /// <summary>
    /// Gets a value indicating whether confirmation is enabled.
    /// </summary>
    public bool ConfirmationEnabled { get; init; }

    /// <summary>
    /// Gets format of the date.
    /// </summary>
    /// <remarks>It's a subset of C# standard date format strings.</remarks>
    /// <example>'ddMMyyyy' or 'yyyyMM'.</example>
    public string Format { get; init; } = null!;

    /// <summary>
    /// Gets a value with an image to be displayed when the question is showed.
    /// </summary>
    public Uri? Image { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(IDevice device, InstructionSet instr, string id, string? previousCode)
    {
        if (!Format.Any(x => new[] { 'd', 'M', 'y' }.Contains(x)))
            throw new InvalidOperationException(string.Format(Resources.Date_InvalidFormat, id, Code, Format, DialogResources.Date_InvalidCharacters));

        var formatMatches = _dateFormatRegex.Matches(Format);
        if (formatMatches.Count == 0)
            throw new InvalidOperationException(string.Format(Resources.Date_InvalidFormat, id, Code, Format, DialogResources.Date_NoMatches));

        var skipCommand = SkipAllowed ? $"{Labels.End}_{Code}" : string.Empty;
        var undoCommand = !string.IsNullOrEmpty(previousCode) ? $"{Labels.Undo}_{previousCode}" : string.Empty;
        var dayAsked = false;
        var monthAsked = false;
        var yearAsked = false;

        instr.SetCommands(Labels.LeaveJob, skipCommand, undoCommand);

        instr.Label($"{Labels.Ask}_{Code}");
        instr.Say(Prompt, priorityPrompt: true);

        instr.AssignStr(Vars.AUX, string.Empty);
        instr.AssignStr(Vars.YEAR, string.Empty);
        instr.AssignStr(Vars.MONTH, string.Empty);
        instr.AssignStr(Vars.DAY, string.Empty);

        foreach (Match match in formatMatches)
        {
            switch (match.Value)
            {
                case "yyyy":
                    AskYearLong(instr);
                    break;
                case "yy":
                    AskYearShort(instr);
                    break;
                case "MM":
                    AskMonth(instr);
                    break;
                case "dd":
                    AskDay(instr);
                    break;
            }
        }

        if (monthAsked && yearAsked)
        {
            instr.DoIf(Vars.MONTH, 2, Operation.EQ, _ =>
                instr.DoIf(Vars.DAY, 28, Operation.GT, _ => instr.GoTo($"{Labels.Wrong}_{Code}")));

            instr.DoIf(Vars.MONTH, 4, Operation.EQ, _ =>
                instr.DoIf(Vars.DAY, 30, Operation.GT, _ => instr.GoTo($"{Labels.Wrong}_{Code}")));

            instr.DoIf(Vars.MONTH, 6, Operation.EQ, _ =>
                instr.DoIf(Vars.DAY, 30, Operation.GT, _ => instr.GoTo($"{Labels.Wrong}_{Code}")));

            instr.DoIf(Vars.MONTH, 9, Operation.EQ, _ =>
                instr.DoIf(Vars.DAY, 30, Operation.GT, _ => instr.GoTo($"{Labels.Wrong}_{Code}")));

            instr.DoIf(Vars.MONTH, 11, Operation.EQ, _ =>
                instr.DoIf(Vars.DAY, 30, Operation.GT, _ => instr.GoTo($"{Labels.Wrong}_{Code}")));
        }

        instr.GoTo($"{Labels.Ok}_{Code}");

        instr.Label($"{Labels.Wrong}_{Code}");
        instr.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Date_InvalidDate)), priorityPrompt: true);
        instr.GoTo($"{Labels.Ask}_{Code}");

        instr.Label($"{Labels.Ok}_{Code}");

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

        void AskYearLong(InstructionSet instr)
        {
            if (yearAsked)
                throw new InvalidOperationException(string.Format(Resources.Date_InvalidFormat, id, Code, Format, DialogResources.Date_YearRepeated));

            yearAsked = true;
            instr.GetDigits(Vars.YEAR, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Date_Year)), helpMsg: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.FourDigits)), minLength: "4", maxLength: "4", minRange: "1900", maxRange: "2100", imageUrl: Image?.AbsoluteUri);
            instr.Concat(Vars.ANSWER, Vars.YEAR);

            instr.Concat(Vars.AUX, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Date_Year)));
            instr.Concat(Vars.AUX, Vars.YEAR);
            instr.Concat(Vars.AUX, ",");
        }

        void AskYearShort(InstructionSet instr)
        {
            if (yearAsked)
                throw new InvalidOperationException(string.Format(Resources.Date_InvalidFormat, id, Code, Format, DialogResources.Date_YearRepeated));

            yearAsked = true;
            instr.GetDigits(Vars.YEAR, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Date_Year)), helpMsg: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.TwoDigits)), minLength: "2", maxLength: "2", imageUrl: Image?.AbsoluteUri);
            instr.Concat(Vars.ANSWER, Vars.YEAR);

            instr.Concat(Vars.AUX, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Date_Year)));
            instr.Concat(Vars.AUX, Vars.YEAR);
            instr.Concat(Vars.AUX, ",");
        }

        void AskMonth(InstructionSet instr)
        {
            if (monthAsked)
                throw new InvalidOperationException(string.Format(Resources.Date_InvalidFormat, id, Code, Format, DialogResources.Date_MonthRepeated));

            monthAsked = true;
            instr.GetDigits(Vars.MONTH, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Date_Month)), helpMsg: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.TwoDigits)), minLength: "2", maxLength: "2", minRange: "1", maxRange: "12", imageUrl: Image?.AbsoluteUri);
            instr.Concat(Vars.ANSWER, Vars.MONTH);

            instr.Concat(Vars.AUX, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Date_Month)));
            instr.Concat(Vars.AUX, Vars.MONTH);
            instr.Concat(Vars.AUX, ",");
        }

        void AskDay(InstructionSet instr)
        {
            if (dayAsked)
                throw new InvalidOperationException(string.Format(Resources.Date_InvalidFormat, id, Code, Format, DialogResources.Date_DayRepeated));

            dayAsked = true;
            instr.GetDigits(Vars.DAY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Date_Day)), helpMsg: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.TwoDigits)), minLength: "2", maxLength: "2", minRange: "1", maxRange: "31", imageUrl: Image?.AbsoluteUri);
            instr.Concat(Vars.ANSWER, Vars.DAY);

            instr.Concat(Vars.AUX, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Date_Day)));
            instr.Concat(Vars.AUX, Vars.DAY);
            instr.Concat(Vars.AUX, ",");
        }
    }
}
