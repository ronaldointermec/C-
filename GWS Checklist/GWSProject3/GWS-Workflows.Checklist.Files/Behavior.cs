// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

#if NETSTANDARD2_1
using Honeywell.GuidedWork.AppBase.Services.DataService;
#endif
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Files.Code;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Files;

/// <summary>
/// An implementation of <see cref="ConnectorBehavior"></see> based on file storage.
/// </summary>
public class Behavior : ConnectorBehavior<BehaviorSettings>
{
#if NETSTANDARD2_1
    private readonly ICustomDataPath _customDataPath = null!;
#endif

    private string _server = null!;
    private IParser _parser = null!;
    private Encoding _encoding = null!;
    private string _fileExtension = null!;
    private string _checkPath = null!;
    private string _operatorPath = null!;

#if NETSTANDARD2_1
    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class.
    /// This constructor is used by GWS App Connector.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="customDataPath">CustomDataPath service.</param>
    /// <param name="fileSystem">FileSystem instance.</param>
    /// <param name="parserFactory">ParserFactory instance.</param>
    public Behavior(BehaviorSettings settings, ICustomDataPath customDataPath, IFileSystem fileSystem, IParserFactory parserFactory)
        : base(settings)
    {
        _customDataPath = customDataPath;
        FileSystem = fileSystem;
        ParserFactory = parserFactory;

    }
#else

    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class.
    /// This constructor is used by GWS Connector Service.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    public Behavior(BehaviorSettings settings)
        : base(settings)
    {
        FileSystem = new FileSystem();
    }
#endif

    /// <summary>
    /// Gets FileSystem object.
    /// </summary>
    protected IFileSystem FileSystem { get; init; }
    /// <summary>
    /// Gets parser factory object.
    /// </summary>
#if NETFRAMEWORK
    protected virtual IParserFactory ParserFactory { get; } = new ParserFactory();
#else
    protected IParserFactory ParserFactory { get; }
#endif

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        if (string.IsNullOrEmpty(Settings.Server))
            throw new InvalidOperationException(Resources.ServerEmpty);

        _encoding = !string.IsNullOrEmpty(Settings.Encoding) ? Encoding.GetEncoding(Settings.Encoding) : Encoding.UTF8;
        _server = Settings.Server;

#if NETSTANDARD2_1
        _server = FileSystem.Path.Combine(_customDataPath.Path, _server);
#endif
        _checkPath = FileSystem.Path.Combine(_server, "Checklists");
        _operatorPath = FileSystem.Path.Combine(_server, "Operators");

        FileSystem.Directory.CreateDirectory(_operatorPath);
        FileSystem.Directory.CreateDirectory(_checkPath);

        if (!Settings.FileFormat.HasValue)
            throw new InvalidOperationException(Resources.FileFormatEmpty);

        _parser = ParserFactory.GetParser(Settings.FileFormat);

        switch (Settings.FileFormat)
        {
            case FileFormat.Json:
                _fileExtension = ".json";
                break;
            case FileFormat.Yaml:
                _fileExtension = ".yaml";
                break;
            default:
                throw new NotImplementedException();
        }
    }

    /// <inheritdoc/>
    public override Operator? GetOperator(string @operator)
    {
        string contents;

        try
        {
            var fileName = FileSystem.Path.Combine(_operatorPath, @operator + _fileExtension);

            if (!FileSystem.File.Exists(fileName))
                return null;

            contents = FileSystem.File.ReadAllText(fileName, _encoding);
        }
        catch (FileNotFoundException)
        {
            return null;
        }

        return _parser.Parse<Operator>(contents);
    }

    /// <inheritdoc/>
    public override Models.Checklist? RetrieveChecklist(string @operator, string id)
    {
        string contents;

        var fileName = FileSystem.Path.Combine(_checkPath, id + _fileExtension);

        if (!FileSystem.File.Exists(fileName))
            return null;

        contents = FileSystem.File.ReadAllText(fileName, _encoding);

        return _parser.Parse<Models.Checklist>(contents);
    }

    /// <inheritdoc/>
    public override void UpdateChecklist(string @operator, string id, Models.Checklist checklist) => FileSystem.File.WriteAllText(Path.Combine(_server, "Checklists", id + _fileExtension), _parser.Serialize(checklist), _encoding);
}