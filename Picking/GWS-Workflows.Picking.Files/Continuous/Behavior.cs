// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if !NETFRAMEWORK
using Honeywell.GuidedWork.AppBase.Services.DataService;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;
#endif

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
#if !NETFRAMEWORK
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using System.IO.Abstractions;
#endif
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Continuous;

/// <summary>
/// Base implementation for Files ConnectorBehavior.
/// </summary>
public class Behavior : FilesBehavior, IPickingContinuousBehavior
{
    private const string LogHeader = "Files|Batch|Behavior";

#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class. This constructor is used by GWS Connector Service.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    public Behavior(FilesBehaviorSettings settings)
        : base(settings)
    {
    }
#else

    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class. This constructor is used by GWS App.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="serverLog">ServerLog service instance.</param>
    /// <param name="customDataPath">CustomDataPath service instance.</param>
    /// <param name="fileSystem">FileSystem service instance.</param>
    /// <param name="parserFactory">Factory used to build the parser.</param>
    public Behavior(FilesBehaviorSettings settings, IServerLog serverLog, ICustomDataPath customDataPath, IFileSystem fileSystem, IParserFactory parserFactory)
        : base(settings, serverLog, customDataPath, fileSystem, parserFactory)
    {
    }
#endif

    /// <inheritdoc/>
#if NETFRAMEWORK
    public Task<IGetWorkOrderItem?> GetWorkOrderAsync(string operatorName, string device)
#else
    public async Task<IGetWorkOrderItem?> GetWorkOrderAsync(string operatorName, string device)
#endif
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| -> GetWorkOrder");

        var workOrderFilePattern = $"*_{operatorName}{FileExtension}";

        var fileNames = FileSystem.Directory.GetFiles(RequestsPath, workOrderFilePattern);
        if (!fileNames.Any())
#if NETFRAMEWORK
            return Task.FromResult<IGetWorkOrderItem?>(null);
#else
            return null;
#endif

        var workOrderFile = fileNames.FirstOrDefault();
#if NETFRAMEWORK
        var order = Parser.Parse<IGetWorkOrderItem>(FileSystem.File.ReadAllText(workOrderFile, FileEncoding));
        FileSystem.File.Delete(workOrderFile);

        return Task.FromResult<IGetWorkOrderItem?>(order);
#else
        var order = Parser.Parse<IGetWorkOrderItem>(await FileSystem.File.ReadAllTextAsync(workOrderFile, FileEncoding));
        FileSystem.File.Delete(workOrderFile);

        return order;
#endif
    }

    /// <inheritdoc/>
#if NETFRAMEWORK
    public virtual Task<IGetWorkOrderItem?> SetWorkOrderAsync(string operatorName, string device, ISetWorkOrderItem res)
#else
    public virtual async Task<IGetWorkOrderItem?> SetWorkOrderAsync(string operatorName, string device, ISetWorkOrderItem res)
#endif
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| -> SetWorkOrder - {Parser.Serialize(res)}");

        var resultFileName = $"{res.Code}_{operatorName}{FileExtension}";

        var resultFile = FileSystem.Path.Combine(ResultsPath, resultFileName.ToString());

#if NETFRAMEWORK
        FileSystem.File.WriteAllText(resultFile, Parser.Serialize(res), FileEncoding);
        return GetWorkOrderAsync(operatorName, device);
#else
        await FileSystem.File.WriteAllTextAsync(resultFile, Parser.Serialize(res), FileEncoding);
        return await GetWorkOrderAsync(operatorName, device);
#endif
    }
}
