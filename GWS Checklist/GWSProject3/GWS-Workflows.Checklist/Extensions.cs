// <copyright file="Extensions.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.VIO.SDK;
using System;
using System.Collections.Generic;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist;

/// <summary>
/// Helper methods for dialog generation.
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Extend <see cref="InstructionSet"/> to add an AssignNum which receives a <see cref="Dialogs"/> enumeration value.
    /// </summary>
    /// <param name="i">Current instruction set instance.</param>
    /// <param name="variableName">Variable to be updated.</param>
    /// <param name="dialog">Dialog enumeration value.</param>
    internal static void AssignNum(this InstructionSet i, string variableName, Dialogs dialog) => i.AssignNum(variableName, (int)dialog);

    /// <summary>
    /// Extend <see cref="CommandRequest"/> to add an AssignNum which receives a <see cref="Dialogs"/> enumeration value.
    /// </summary>
    /// <param name="cmd">Current command request response.</param>
    /// <param name="variableName">Variable to be updated.</param>
    /// <param name="dialog">Dialog enumeration value.</param>
    internal static void AssignNum(this CommandRequest cmd, string variableName, Dialogs dialog) => cmd.AssignNum(variableName, (int)dialog);

    /// <summary>
    /// Tries to retrieve a value from a dictionary and cast it to the <typeparamref name="TValueAs"/> type.
    /// </summary>
    /// <typeparam name="TKey">Type of the <see cref="IDictionary{TKey, TValue}"/> key.</typeparam>
    /// <typeparam name="TValue">Type of the <see cref="IDictionary{TKey, TValue}"/> value.</typeparam>
    /// <typeparam name="TValueAs">Type of the data to be casted to.</typeparam>
    /// <param name="dictionary">Current dictionary.</param>
    /// <param name="key">Key to be retrieved.</param>
    /// <param name="valueAs">Parameter to get the value.</param>
    /// <returns>If value could be returned and casted.</returns>
    internal static bool TryGetValue<TKey, TValue, TValueAs>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValueAs? valueAs)
    {
        if (dictionary.TryGetValue(key, out TValue value))
        {
            var t = typeof(TValueAs);
            t = Nullable.GetUnderlyingType(t) ?? t;

            if (value != null)
            {
                valueAs = (TValueAs)Convert.ChangeType(value, t);
                return true;
            }
        }

        valueAs = default;
        return false;
    }
}