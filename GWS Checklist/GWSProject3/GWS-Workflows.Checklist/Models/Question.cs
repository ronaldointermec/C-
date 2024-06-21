// <copyright file="Question.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Newtonsoft.Json;
using System;
using YamlDotNet.Serialization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

/// <summary>
/// A base implementation of <see cref="IQuestion"/> interface. Used mainly for serialization/deserialization purposes.
/// </summary>
public abstract class Question : IQuestion
{
    /// <summary>
    /// Gets type of the question (used for serialization).
    /// </summary>
    [JsonProperty(Order = -11)]
    [YamlIgnore]
    public abstract string Type { get; }

    /// <inheritdoc/>
    [JsonProperty(Order = -10, NullValueHandling = NullValueHandling.Include)]
    [YamlMember(Order = -10, DefaultValuesHandling = DefaultValuesHandling.Preserve)]
    public string Code { get; init; } = null!;

    /// <inheritdoc/>
    [JsonProperty(Order = -9, NullValueHandling = NullValueHandling.Include)]
    [YamlMember(Order = -9, DefaultValuesHandling = DefaultValuesHandling.Preserve)]
    public string Prompt { get; init; } = null!;

    /// <inheritdoc/>
    [JsonProperty(Order = -8, NullValueHandling = NullValueHandling.Include)]
    [YamlMember(Order = -8, DefaultValuesHandling = DefaultValuesHandling.Preserve)]
    public string AdditionalInformation { get; init; } = null!;

    /// <inheritdoc/>
    [JsonProperty(Order = -7)]
    [YamlMember(Order = -7)]
    public bool SkipAllowed { get; init; }

    /// <inheritdoc/>
    [JsonProperty(Order = 100)]
    [YamlMember(Order = 100)]
    public string? Operator { get; set; }

    /// <inheritdoc/>
    [JsonProperty(Order = 102)]
    [YamlMember(Order = 102)]
    public DateTime? StartTime { get; set; }

    /// <inheritdoc/>
    [JsonProperty(Order = 103)]
    [YamlMember(Order = 103)]
    public DateTime? EndTime { get; set; }

    /// <inheritdoc/>
    public abstract void BuildDialog(IDevice device, InstructionSet instr, string id, string? previousCode);
}

/// <summary>
/// A base implementation of <see cref="IQuestionWithValueTypeAnswer{TAnswer}"/> interface. Used mainly for serialization/deserialization purposes.
/// </summary>
/// <typeparam name="TAnswer"><inheritdoc/></typeparam>
public abstract class QuestionWithValueTypeAnswer<TAnswer> : Question, IQuestionWithValueTypeAnswer<TAnswer>
    where TAnswer : struct
{
    /// <inheritdoc/>
    [JsonProperty(Order = 101)]
    [YamlMember(Order = 101)]
    public TAnswer? Answer { get; set; }
}

/// <summary>
/// A base implementation of <see cref="IQuestionWithReferenceTypeAnswer{TAnswer}"/> interface. Used mainly for serialization/deserialization purposes.
/// </summary>
/// <typeparam name="TAnswer"><inheritdoc/></typeparam>
public abstract class QuestionWithReferenceTypeAnswer<TAnswer> : Question, IQuestionWithReferenceTypeAnswer<TAnswer>
    where TAnswer : class
{
    /// <inheritdoc/>
    [JsonProperty(Order = 101)]
    [YamlMember(Order = -101)]
    public TAnswer? Answer { get; set; }
}