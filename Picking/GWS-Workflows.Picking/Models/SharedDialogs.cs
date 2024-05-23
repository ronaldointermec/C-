// <copyright file="SharedDialogs.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Extension class to provide common dialogs implementations.
/// </summary>
public static class SharedDialogs
{
    /// <summary>
    /// Dialog implementation for break work.
    /// </summary>
    /// <param name="i">The dialog object that is being build.</param>
    /// <param name="askForReason">Ask for a break reason.</param>
    /// <param name="device">Device object.</param>
    public static void Break(InstructionSet i, bool askForReason, IDevice device)
    {
        if (askForReason)
        {
            i.GetMenu(Vars.BREAK_REASON, Menus.BREAK, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Break_ConfirmReason)), confirmationPrompt: "?");
            i.SetSendHostFlag(Vars.BREAK_REASON, true);
        }

        i.SetSendHostFlag(Vars.RESPONSETYPE, true);
        i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.BeginBreak);
        i.GetVariablesOdr();

        if (askForReason)
            i.SetSendHostFlag(Vars.BREAK_REASON, false);

        i.SetCommands();
        i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Break_Resume)), true);
        i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.EndBreak);
        i.GetVariablesOdr();
    }
}