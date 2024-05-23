// <copyright file="PickingBehaviorBase.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;

/// <summary>
/// Base generic implementation for Picking ConnectorBehavior for extending Settings.
/// </summary>
/// <typeparam name="TBehaviorSettings">Settings type.</typeparam>
public abstract class PickingBehaviorBase<TBehaviorSettings> : ConnectorBehaviorBase<TBehaviorSettings>, IPickingBehavior
    where TBehaviorSettings : PickingBehaviorSettings
{
#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="PickingBehaviorBase{TBehaviorSettings}"/> class.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    protected PickingBehaviorBase(TBehaviorSettings settings)
        : base(settings)
    {
        ServerLog = new InServiceServerLog();
    }
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="PickingBehaviorBase{TBehaviorSettings}"/> class.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="serverLog">ServerLog instance.</param>
    protected PickingBehaviorBase(TBehaviorSettings settings, IServerLog serverLog)
        : base(settings)
    {
        ServerLog = serverLog;
    }

    /// <inheritdoc/>
    IPickingBehaviorSettings IConnectorBehavior<IPickingBehaviorSettings>.Settings => Settings;

    /// <summary>
    /// Gets the object to log server information.
    /// </summary>
    protected IServerLog ServerLog { get; }

    /// <inheritdoc/>
    public abstract Task BeginBreakAsync(string operatorName, string device, BeginBreak res);

    /// <inheritdoc/>
    public abstract ValueTask<ConnectResult> ConnectAsync(string operatorName, IDevice device);

    /// <inheritdoc/>
    public abstract ValueTask<DisconnectResult> DisconnectAsync(string operatorName, string device, bool force);

    /// <inheritdoc/>
    public abstract Task EndBreakAsync(string operatorName, string device);

    /// <inheritdoc/>
    public abstract Task<string?> RegisterOperatorStartAsync(string operatorName, string device);
}

/// <summary>
/// Base implementation for Picking ConnectorBehavior.
/// </summary>
public abstract class PickingBehaviorBase : PickingBehaviorBase<PickingBehaviorSettings>
{
#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="PickingBehaviorBase"/> class.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    protected PickingBehaviorBase(PickingBehaviorSettings settings)
        : base(settings)
    {
    }
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="PickingBehaviorBase"/> class.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="serverLog">ServerLog instance.</param>
    protected PickingBehaviorBase(PickingBehaviorSettings settings, IServerLog serverLog)
        : base(settings, serverLog)
    {
    }
}
