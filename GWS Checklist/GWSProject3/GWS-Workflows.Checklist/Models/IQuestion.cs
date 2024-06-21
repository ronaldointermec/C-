// <copyright file="IQuestion.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// Interface for a question.
/// </summary>
public interface IQuestion
{
    /// <summary>
    /// Gets unique code for the question within the checklist.
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Gets the message of the question to be asked to the operator.
    /// </summary>
    string Prompt { get; }

    /// <summary>
    /// Gets additional information to be given to the operator when asking for help.
    /// </summary>
    string AdditionalInformation { get; }

    /// <summary>
    /// Gets a value indicating whether if question can be skipped.
    /// </summary>
    bool SkipAllowed { get; }

    /// <summary>
    /// Gets or sets operator who answered the question.
    /// </summary>
    string? Operator { get; set; }

    /// <summary>
    /// Gets or sets timestamp of when the operator entered into this question.
    /// </summary>
    DateTime? StartTime { get; set; }

    /// <summary>
    /// Gets or sets timestamp of when the operator leaved the question by giving an answer (if required).
    /// </summary>
    DateTime? EndTime { get; set; }

    /// <summary>
    /// Method for build instructions set for the question.
    /// </summary>
    /// <param name="device">Device identifier.</param>
    /// <param name="instr">Current instruction set.</param>
    /// <param name="id">Checklist identifier.</param>
    /// <param name="previousCode">Previous question code.</param>
    void BuildDialog(IDevice device, InstructionSet instr, string id, string? previousCode);
}

/// <summary>
/// Extended question with a <see cref="Answer"/> property.
/// </summary>
/// <typeparam name="TAnswer">Type of the answer (value type).</typeparam>
public interface IQuestionWithValueTypeAnswer<TAnswer> : IQuestion
    where TAnswer : struct
{
    /// <summary>
    /// Gets or sets answer given by the operator.
    /// </summary>
    public TAnswer? Answer { get; set; }
}

/// <summary>
/// Extended question with a <see cref="Answer"/> property.
/// </summary>
/// <typeparam name="TAnswer">Type of the answer (reference type).</typeparam>
public interface IQuestionWithReferenceTypeAnswer<TAnswer> : IQuestion
    where TAnswer : class
{
    /// <summary>
    /// Gets or sets answer given by the operator.
    /// </summary>
    public TAnswer? Answer { get; set; }
}