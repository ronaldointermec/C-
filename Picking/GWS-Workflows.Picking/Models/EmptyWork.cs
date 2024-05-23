// <copyright file="EmptyWork.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Implementation class for Empty work.
/// </summary>
public class EmptyWork : GetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyWork"/> class.
    /// </summary>
    public EmptyWork()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyWork"/> class.
    /// </summary>
    /// <param name="message">Additional message.</param>
    public EmptyWork(string message)
        : base(null!, message)
    {
    }

    /// <inheritdoc/>
    public override void BuildDialog(InstructionSet i, IPickingBehavior behavior, IDevice device)
    {
        if (!string.IsNullOrEmpty(Message))
        {
            i.SetCommands();
            i.Say(Message, true, true);
        }
    }
}
