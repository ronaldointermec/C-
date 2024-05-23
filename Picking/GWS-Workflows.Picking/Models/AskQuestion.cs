// <copyright file="AskQuestion.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Implementation class for Ask Question work.
/// </summary>
public class AskQuestion : GetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AskQuestion"/> class.
    /// </summary>
    public AskQuestion()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AskQuestion"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    /// <param name="message">Optional message.</param>
    public AskQuestion(string code, string? message)
        : base(code, message)
    {
    }

    /// <summary>
    /// Gets an image uri to be displayed.
    /// </summary>
    public virtual Uri? Image { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(InstructionSet i, IPickingBehavior behavior, IDevice device)
    {
        i.SetCommands();
        i.AssignStr(Vars.STATUS, string.Empty);
        i.AssignNum(Vars.START_TIME, "#time");

        i.Ask(Vars.DUMMY, Message, priorityPrompt: true, imageUrl: Image?.AbsoluteUri);

        i.DoIf(Vars.DUMMY, false, Operation.EQ, ifb =>
        {
            i.AssignStr(Vars.STATUS, "Cancelled");
            ifb.DoElse();
            i.AssignStr(Vars.STATUS, "OK");
        });

        i.AssignNum(Vars.END_TIME, "#time");
        i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.AskQuestionResult);

        i.SetSendHostFlag(true, Vars.STATUS, Vars.START_TIME, Vars.END_TIME);
        if (behavior is IPickingBatchBehavior)
        {
            i.GetVariablesOdr();
            i.SetSendHostFlag(false, Vars.STATUS, Vars.START_TIME, Vars.END_TIME);
        }
        else
        {
            i.SetSendHostFlag(true, Vars.RESPONSETYPE);
        }
    }
}
