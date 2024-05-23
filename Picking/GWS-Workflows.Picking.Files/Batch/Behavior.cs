// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if !NETFRAMEWORK
using Honeywell.GuidedWork.AppBase.Services.DataService;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;
#endif
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
#if !NETFRAMEWORK
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
#endif
using System;
using System.Collections.Generic;
using System.IO;
#if !NETFRAMEWORK
using System.IO.Abstractions;
#endif
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Batch;

/// <summary>
/// Base implementation for Files ConnectorBehavior.
/// </summary>
public class Behavior : FilesBehavior, IPickingBatchBehavior
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
    public async Task<IEnumerable<IGetWorkOrderItem>> GetWorkOrdersAsync(string operatorName, string device)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrdersAsync)}|{operatorName}|{device}| -> GetWorkOrders");

        var workOrderFilePath = $"*_{operatorName}{FileExtension}";

        var fileNames = FileSystem.Directory.GetFiles(RequestsPath, workOrderFilePath.ToString());
        if (!fileNames.Any())
            return Enumerable.Empty<IGetWorkOrderItem>();

        var workOrderFile = fileNames.FirstOrDefault();

        await UpdateOperatorWorkOrderFileName(operatorName, Path.GetFileNameWithoutExtension(workOrderFile));

#if NETFRAMEWORK
        var orders = Parser.Parse<List<IGetWorkOrderItem>>(FileSystem.File.ReadAllText(workOrderFile, FileEncoding));
#else
        var orders = Parser.Parse<List<IGetWorkOrderItem>>(await FileSystem.File.ReadAllTextAsync(workOrderFile, FileEncoding));
#endif

        FileSystem.File.Delete(workOrderFile);

        return orders;
    }

    /// <inheritdoc/>
    public virtual async Task SetWorkOrderAsync(string operatorName, string device, ISetWorkOrderItem res)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| -> SetWorkOrder - {Parser.Serialize(res)}");

        var fileName = await GetOperatorWorkOrderFileName(operatorName);

        var resultFile = FileSystem.Path.Combine(ResultsPath, fileName);

        if (!FileSystem.File.Exists(resultFile))
        {
#if NETFRAMEWORK
            FileSystem.File.WriteAllText(resultFile, Parser.Serialize(new List<object> { res }), FileEncoding);
#else
            await FileSystem.File.WriteAllTextAsync(resultFile, Parser.Serialize(new List<object> { res }), FileEncoding);
#endif
            return;
        }

        using var fs = FileSystem.File.Open(resultFile, FileMode.Open);

        using var stReader = new StreamReader(fs, FileEncoding);
        var fileInfo = await stReader.ReadToEndAsync();

        var workData = Parser.Parse<List<object>>(fileInfo.ToString());
        if (workData is null)
        {
#if NETFRAMEWORK
            FileSystem.File.WriteAllText(resultFile, Parser.Serialize(new List<object> { res }), FileEncoding);
#else
            await FileSystem.File.WriteAllTextAsync(resultFile, Parser.Serialize(new List<object> { res }), FileEncoding);
#endif
            return;
        }

        workData.Add(res);

        var stringData = Parser.Serialize(workData);

        using var stWriter = new StreamWriter(fs, FileEncoding);
        fs.Seek(0, SeekOrigin.Begin);
        await stWriter.WriteAsync(stringData);
    }

    /// <inheritdoc/>
#if NETFRAMEWORK
    public Task PrintLabelsBatchAsync(string operatorName, string device, PrintLabelsBatch res)
#else
    public async Task PrintLabelsBatchAsync(string operatorName, string device, PrintLabelsBatch res)
#endif
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(PrintLabelsBatchAsync)}|{operatorName}|{device}| -> PrintLabels - {Parser.Serialize(res)}");

        var printLabelsBatchRequest = new PrintLabelsBatchRequest
        {
            OperatorName = operatorName,
            Code = res.Code,
            Count = res.Count,
        };

        var fileName = $"{res.Code}_print{FileExtension}";

#if NETFRAMEWORK
        FileSystem.File.WriteAllText(FileSystem.Path.Combine(RequestsPath, fileName), Parser.Serialize(printLabelsBatchRequest), FileEncoding);
        return Task.CompletedTask;
#else
        await FileSystem.File.WriteAllTextAsync(FileSystem.Path.Combine(RequestsPath, fileName), Parser.Serialize(printLabelsBatchRequest), FileEncoding);
#endif
    }

    /// <summary>
    /// Updates the current work order file name for the provided operator.
    /// </summary>
    /// <param name="operatorName">Operator name.</param>
    /// <param name="woFileName">Work Order file name.</param>
    /// <returns>Completed task.</returns>
    protected async Task UpdateOperatorWorkOrderFileName(string operatorName, string woFileName)
    {
        var operatorFileName = FileSystem.Path.Combine(OperatorsPath, $"{operatorName}{FileExtension}");

        if (!FileSystem.File.Exists(operatorFileName))
            throw new InvalidOperationException(Resources.Error_MissingFile);

        var operatorFileSemaphore = GetOperatorFileSemaphore(operatorName);

        await operatorFileSemaphore.WaitAsync();
        using var fs = FileSystem.File.Open(operatorFileName, FileMode.Open);
        try
        {
            using var stReader = new StreamReader(fs, FileEncoding);
            var fileInfo = await stReader.ReadToEndAsync();

            var operatorData = Parser.Parse<Operator>(fileInfo.ToString());
            operatorData.WorkOrderFileName = woFileName;

            var stringData = Parser.Serialize(operatorData);

            using var stWriter = new StreamWriter(fs, FileEncoding);
            fs.SetLength(0);

            await stWriter.WriteAsync(stringData);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(string.Format(Resources.Error_ParseFail, ex.Message));
        }
        finally
        {
            fs.Close();
            operatorFileSemaphore.Release();
        }
    }

    /// <summary>
    /// Gets current work order file name for the operator.
    /// </summary>
    /// <param name="operatorName">Operator name.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
#if NETFRAMEWORK
    protected Task<string> GetOperatorWorkOrderFileName(string operatorName)
#else
    protected async Task<string> GetOperatorWorkOrderFileName(string operatorName)
#endif
    {
        var operatorFileName = FileSystem.Path.Combine(OperatorsPath, $"{operatorName}{FileExtension}");

        if (!FileSystem.File.Exists(operatorFileName))
            throw new InvalidOperationException(Resources.Error_MissingFile);

#if NETFRAMEWORK
        var fileInfo = FileSystem.File.ReadAllText(operatorFileName, FileEncoding);
#else
        var fileInfo = await FileSystem.File.ReadAllTextAsync(operatorFileName, FileEncoding);
#endif

        try
        {
            var operatorData = Parser.Parse<Operator>(fileInfo.ToString());
            var fileName = $"{operatorData.WorkOrderFileName}{FileExtension}";

#if NETFRAMEWORK
            return Task.FromResult(fileName);
#else
            return fileName;
#endif
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(string.Format(Resources.Error_ParseFail, ex.Message));
        }
    }
}
