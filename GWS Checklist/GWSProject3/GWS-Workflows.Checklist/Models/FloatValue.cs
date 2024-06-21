// <copyright file="FloatValue.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// Question to be answered with a decimal value.
/// </summary>
public class FloatValue : QuestionWithValueTypeAnswer<float>
{
    /// <inheritdoc/>
    public override sealed string Type => "decimal";

    /// <summary>
    /// Gets a value indicating whether confirmation is enabled.
    /// </summary>
    public bool ConfirmationEnabled { get; init; }

    /// <summary>
    /// Gets min length for the answer.
    /// </summary>
    public int? MinLength { get; init; }

    /// <summary>
    /// Gets max length for the answer.
    /// </summary>
    public int? MaxLength { get; init; }

    /// <summary>
    /// Gets min value for the answer.
    /// </summary>
    public float? MinValue { get; init; }

    /// <summary>
    /// Gets max value for the answer.
    /// </summary>
    public float? MaxValue { get; init; }

    /// <summary>
    /// Gets a value indicating whether if barcode scanner is enabled.
    /// </summary>
    public bool ScannerEnabled { get; init; }

    /// <summary>
    /// Gets a value with an image to be displayed when the question is showed.
    /// </summary>
    public Uri? Image { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(IDevice device, InstructionSet instr, string id, string? previousCode)
    {
        var skipCommand = SkipAllowed ? $"{Labels.End}_{Code}" : string.Empty;
        var undoCommand = !string.IsNullOrEmpty(previousCode) ? $"{Labels.Undo}_{previousCode}" : string.Empty;
        var confirmationPrompt = ConfirmationEnabled ? "?" : string.Empty;
        var minLength = MinLength?.ToString() ?? "1";
        var maxLength = MaxLength?.ToString() ?? "20";
        var minRange = MinValue?.ToString() ?? string.Empty;
        var maxRange = MaxValue.ToString() ?? string.Empty;

        instr.SetCommands(Labels.LeaveJob, skipCommand, undoCommand);
        instr.GetFloat(Vars.ANSWER, Prompt, AdditionalInformation, confirmationPrompt: confirmationPrompt, minLength: minLength, maxLength: maxLength, minRange: minRange, maxRange: maxRange, barcodeEnabled: ScannerEnabled, imageUrl: Image?.AbsoluteUri);

        if (!ConfirmationEnabled)
            instr.Say($"${Vars.ANSWER}");
    }
}
