// <copyright file="FilesBehavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if !NETFRAMEWORK
using Honeywell.GuidedWork.AppBase.Services.DataService;
#endif

using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
#if !NETFRAMEWORK
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
#endif
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files;

/// <summary>
/// Base class for File implementation.
/// </summary>
public abstract class FilesBehavior : PickingBehaviorBase<FilesBehaviorSettings>
{
    private const string LogHeader = "Files|Behavior";

#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="FilesBehavior"/> class. This constructor is used by GWS Connector Service.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    protected FilesBehavior(FilesBehaviorSettings settings)
        : base(settings)
    {
        FileSystem = new FileSystem();
    }
#else
    private readonly ICustomDataPath _customDataPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilesBehavior"/> class. This constructor is used by GWS App.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="serverLog">ServerLog service instance.</param>
    /// <param name="customDataPath">CustomDataPath service instance.</param>
    /// <param name="fileSystem">FileSystem instance.</param>
    /// <param name="parserFactory">Factory used to build the parser.</param>
    protected FilesBehavior(FilesBehaviorSettings settings, IServerLog serverLog, ICustomDataPath customDataPath, IFileSystem fileSystem, IParserFactory parserFactory)
        : base(settings, serverLog)
    {
        _customDataPath = customDataPath;
        FileSystem = fileSystem;
        ParserFactory = parserFactory;
    }
#endif

    /// <summary>
    /// Gets Parser object.
    /// </summary>
    internal IParser Parser { get; private set; } = null!;

    /// <summary>
    /// Gets parser factory object.
    /// </summary>
#if NETFRAMEWORK
    protected virtual IParserFactory ParserFactory { get; } = new ParserFactory();
#else
    protected IParserFactory ParserFactory { get; }
#endif

    /// <summary>
    /// Gets file extension.
    /// </summary>
    protected string FileExtension { get; private set; } = null!;

    /// <summary>
    /// Gets operators file path.
    /// </summary>
    protected string OperatorsPath { get; private set; } = null!;

    /// <summary>
    /// Gets file path to get work order requests.
    /// </summary>
    protected string RequestsPath { get; private set; } = null!;

    /// <summary>
    /// Gets file path to set work order results.
    /// </summary>
    protected string ResultsPath { get; private set; } = null!;

    /// <summary>
    /// Gets FileSystem object.
    /// </summary>
    protected virtual IFileSystem FileSystem { get; init; }

    /// <summary>
    /// Gets File encoding.
    /// </summary>
    protected virtual Encoding FileEncoding { get; private set; } = null!;

    /// <summary>
    /// Gets or sets a list of Semaphore objects to prevent concurrent writing on operators file.
    /// </summary>
    private Dictionary<string, SemaphoreSlim> OperatorSemaphoreDictionary { get; set; } = new Dictionary<string, SemaphoreSlim>();

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        if (string.IsNullOrEmpty(Settings.Server))
            throw new InvalidOperationException(Resources.ServerEmpty);

        if (string.IsNullOrEmpty(Settings.FileFormat))
            throw new InvalidOperationException(Resources.FileFormatEmpty);

        if (!Enum.TryParse<FileFormat>(Settings.FileFormat, out var fileFormatEnum))
            throw new InvalidOperationException(Resources.FileFormatNotValid);

        FileEncoding = !string.IsNullOrEmpty(Settings.FileEncoding) ? Encoding.GetEncoding(Settings.FileEncoding) : Encoding.UTF8;

        Parser = ParserFactory.GetParser(fileFormatEnum);

        switch (fileFormatEnum)
        {
            case FileFormat.Json:
                FileExtension = ".json";
                break;
            case FileFormat.Yaml:
                FileExtension = ".yaml";
                break;
            default:
                throw new NotImplementedException();
        }

        var server = Settings.Server;

#if !NETFRAMEWORK
        server = FileSystem.Path.Combine(_customDataPath.Path, server);
#endif

        OperatorsPath = FileSystem.Path.Combine(server, "operators");
        RequestsPath = FileSystem.Path.Combine(server, "requests");
        ResultsPath = FileSystem.Path.Combine(server, "results");

        FileSystem.Directory.CreateDirectory(OperatorsPath);
        FileSystem.Directory.CreateDirectory(RequestsPath);
        FileSystem.Directory.CreateDirectory(ResultsPath);
    }

    /// <inheritdoc/>
    public override async ValueTask<ConnectResult> ConnectAsync(string operatorName, IDevice device)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| -> Connect");

        var fileName = FileSystem.Path.Combine(OperatorsPath, $"{operatorName}{FileExtension}");
        if (!FileSystem.File.Exists(fileName))
            return new ConnectResult(false, null, device.Translate(Resources.ResourceManager, nameof(Resources.Error_MissingFile)));

        var operatorFileSemaphore = GetOperatorFileSemaphore(operatorName);

        await operatorFileSemaphore.WaitAsync();
        using var fs = FileSystem.File.Open(fileName, FileMode.Open);
        try
        {
            using var stReader = new StreamReader(fs, FileEncoding);
            var fileInfo = await stReader.ReadToEndAsync();

            var operatorData = Parser.Parse<Operator>(fileInfo);
            operatorData.StartTime = DateTime.Now;
            operatorData.EndTime = null;
            operatorData.BreakStartTime = null;
            operatorData.BreakEndTime = null;

            var stringData = Parser.Serialize(operatorData);

            using var stWriter = new StreamWriter(fs, FileEncoding);
            fs.SetLength(0);
            await stWriter.WriteAsync(stringData);
        }
        catch (InvalidOperationException ex)
        {
            return new ConnectResult(false, null, device.Translate(Resources.ResourceManager, nameof(Resources.Error_ParseFail), ex.Message));
        }
        finally
        {
            fs.Close();
            operatorFileSemaphore.Release();
        }

        return new ConnectResult(true, null, null);
    }

    /// <inheritdoc/>
    public override async ValueTask<DisconnectResult> DisconnectAsync(string operatorName, string device, bool force)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| -> Disconnect");

        var fileName = FileSystem.Path.Combine(OperatorsPath, $"{operatorName}{FileExtension}");

        if (!FileSystem.File.Exists(fileName))
            return new DisconnectResult(force, Resources.Error_MissingFile);

        var operatorFileSemaphore = GetOperatorFileSemaphore(operatorName);

        await operatorFileSemaphore.WaitAsync();
        using var fs = FileSystem.File.Open(fileName, FileMode.Open);
        try
        {
            using var stReader = new StreamReader(fs, FileEncoding);
            var fileInfo = await stReader.ReadToEndAsync();

            var operatorData = Parser.Parse<Operator>(fileInfo.ToString());
            operatorData.EndTime = DateTime.Now;

            var stringData = Parser.Serialize(operatorData);

            using var stWriter = new StreamWriter(fs, FileEncoding);
            fs.SetLength(0);
            await stWriter.WriteAsync(stringData);
        }
        catch (InvalidOperationException ex)
        {
            return new DisconnectResult(force, string.Format(Resources.Error_ParseFail, ex.Message));
        }
        finally
        {
            fs.Close();
            operatorFileSemaphore.Release();
        }

        return new DisconnectResult(true, null);
    }

    /// <inheritdoc/>
    public override async Task<string?> RegisterOperatorStartAsync(string operatorName, string device)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| -> RegisterOperatorStart");

        var fileName = FileSystem.Path.Combine(OperatorsPath, $"{operatorName}{FileExtension}");

        if (!FileSystem.File.Exists(fileName))
            return Resources.Error_MissingFile;

        var operatorFileSemaphore = GetOperatorFileSemaphore(operatorName);

        await operatorFileSemaphore.WaitAsync();
        using var fs = FileSystem.File.Open(fileName, FileMode.Open);
        try
        {
            using var stReader = new StreamReader(fs, FileEncoding);
            var fileInfo = await stReader.ReadToEndAsync();

            var operatorData = Parser.Parse<Operator>(fileInfo.ToString());
            operatorData.StartTime = DateTime.Now;

            var stringData = Parser.Serialize(operatorData);

            using var stWriter = new StreamWriter(fs, FileEncoding);
            fs.SetLength(0);
            await stWriter.WriteAsync(stringData);
        }
        catch (InvalidOperationException ex)
        {
            return string.Format(Resources.Error_ParseFail, ex.Message);
        }
        finally
        {
            fs.Close();
            operatorFileSemaphore.Release();
        }

        return null;
    }

    /// <inheritdoc/>
    public override async Task BeginBreakAsync(string operatorName, string device, BeginBreak res)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(BeginBreakAsync)}|{operatorName}|{device}| -> BeginBreak");

        var fileName = FileSystem.Path.Combine(OperatorsPath, $"{operatorName}{FileExtension}");

        if (!FileSystem.File.Exists(fileName))
            throw new InvalidOperationException(Resources.Error_MissingFile);

        var operatorFileSemaphore = GetOperatorFileSemaphore(operatorName);

        await operatorFileSemaphore.WaitAsync();
        using var fs = FileSystem.File.Open(fileName, FileMode.Open);
        try
        {
            using var stReader = new StreamReader(fs, FileEncoding);
            var fileInfo = await stReader.ReadToEndAsync();

            var operatorData = Parser.Parse<Operator>(fileInfo.ToString());
            operatorData.BreakStartTime = DateTime.Now;
            operatorData.BreakEndTime = null;
            operatorData.Code = res.Code;
            operatorData.Reason = res.Reason;

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

    /// <inheritdoc/>
    public override async Task EndBreakAsync(string operatorName, string device)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(EndBreakAsync)}|{operatorName}|{device}| -> EndBreak");

        var fileName = FileSystem.Path.Combine(OperatorsPath, $"{operatorName}{FileExtension}");

        if (!FileSystem.File.Exists(fileName))
            throw new InvalidOperationException(Resources.Error_MissingFile);

        var operatorFileSemaphore = GetOperatorFileSemaphore(operatorName);

        await operatorFileSemaphore.WaitAsync();
        using var fs = FileSystem.File.Open(fileName, FileMode.Open);
        try
        {
            using var stReader = new StreamReader(fs, FileEncoding);
            var fileInfo = await stReader.ReadToEndAsync();

            var operatorData = Parser.Parse<Operator>(fileInfo.ToString());
            operatorData.BreakEndTime = DateTime.Now;

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
    /// Gets file semaphore for provided operator.
    /// </summary>
    /// <param name="operatorName">Operator name.</param>
    /// <returns>Operator File semaphore.</returns>
    protected SemaphoreSlim GetOperatorFileSemaphore(string operatorName)
    {
        if (!OperatorSemaphoreDictionary.TryGetValue(operatorName, out SemaphoreSlim operatorFileSemaphore))
        {
            operatorFileSemaphore = new SemaphoreSlim(1, 1);
            OperatorSemaphoreDictionary.Add(operatorName, operatorFileSemaphore);
        }

        return operatorFileSemaphore;
    }
}
