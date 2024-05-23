using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GuidedWork.AppBase.Services.DataService;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Moq;
using System.Globalization;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Tests;

public class FilesBehaviorTests
{
    [Fact]
    public void Initialize_ServerNullOrEmpty_ThrowsException()
    {
        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService(server: string.Empty);
        var behaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), new Mock<IServerLog>().Object, new Mock<ICustomDataPath>().Object, new FileSystem(), new Mock<IParserFactory>().Object) { CallBase = true };

        behaviorMock.Invoking(x => x.Object.Initialize()).Should().Throw<InvalidOperationException>().WithMessage(Resources.ServerEmpty);
    }

    [Fact]
    public void Initialize_FileFormatNullOrEmpty_ThrowsException()
    {
        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService(fileFormat: string.Empty);
        var behaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), new Mock<IServerLog>().Object, new Mock<ICustomDataPath>().Object, new FileSystem(), new Mock<IParserFactory>().Object) { CallBase = true };

        behaviorMock.Invoking(x => x.Object.Initialize()).Should().Throw<InvalidOperationException>().WithMessage(Resources.FileFormatEmpty);
    }

    [Fact]
    public void Initialize_FileFormatNotValid_ThrowsException()
    {
        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService(fileFormat: "NotValidFormat");
        var behaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), new Mock<IServerLog>().Object, new Mock<ICustomDataPath>().Object, new FileSystem(), new Mock<IParserFactory>().Object) { CallBase = true };

        behaviorMock.Invoking(x => x.Object.Initialize()).Should().Throw<InvalidOperationException>().WithMessage(Resources.FileFormatNotValid);
    }

    [Fact]
    public void Initialize_Path()
    {
        var serverPath = "testingServer";
        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var fileSystemMock = new MockFileSystem();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService(server: serverPath);
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), new Mock<IServerLog>().Object, customPathMock.Object, fileSystemMock, new Mock<IParserFactory>().Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var operatorPath = filesBehavior.GetType().GetProperty("OperatorsPath", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(filesBehavior) as string;
        var requestsPath = filesBehavior.GetType().GetProperty("RequestsPath", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(filesBehavior) as string;
        var resultsPath = filesBehavior.GetType().GetProperty("ResultsPath", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(filesBehavior) as string;

        operatorPath.Should().Be(fileSystemMock.Path.Combine(customPathMock.Object.Path, serverPath, "operators"));
        requestsPath.Should().Be(fileSystemMock.Path.Combine(customPathMock.Object.Path, serverPath, "requests"));
        resultsPath.Should().Be(fileSystemMock.Path.Combine(customPathMock.Object.Path, serverPath, "results"));

        fileSystemMock.Directory.Exists(fileSystemMock.Path.Combine(customPathMock.Object.Path, serverPath, "operators")).Should().BeTrue();
        fileSystemMock.Directory.Exists(fileSystemMock.Path.Combine(customPathMock.Object.Path, serverPath, "requests")).Should().BeTrue();
        fileSystemMock.Directory.Exists(fileSystemMock.Path.Combine(customPathMock.Object.Path, serverPath, "results")).Should().BeTrue();
    }

    [Fact]
    public void Initialize_WithJsonFormat()
    {
        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var fileSystemMock = new MockFileSystem();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), new Mock<IServerLog>().Object, customPathMock.Object, fileSystemMock, new ParserFactory()) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();
        filesBehavior.Parser.Should().NotBeNull();
        filesBehavior.Parser.GetType().Should().Be(typeof(JsonParser));

        var operatorPath = filesBehavior.GetType().GetProperty("FileExtension", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(filesBehavior) as string;
        operatorPath.Should().Be(".json");
    }

    [Fact]
    public void Initialize_WithYamlFormat()
    {
        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var fileSystemMock = new MockFileSystem();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService(fileFormat: "Yaml");
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), new Mock<IServerLog>().Object, customPathMock.Object, fileSystemMock, new ParserFactory()) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();
        filesBehavior.Parser.Should().NotBeNull();
        filesBehavior.Parser.GetType().Should().Be(typeof(YamlParser));

        var operatorPath = filesBehavior.GetType().GetProperty("FileExtension", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(filesBehavior) as string;
        operatorPath.Should().Be(".yaml");
    }

    [Fact]
    public async Task ConnectAsync_ReturnsNotAllowed_MissingFIle()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var behavior = filesBehaviorMock.Object;
        behavior.Initialize();

        var res = await behavior.ConnectAsync(operatorName, deviceMock.Object);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> Connect"))));

        res.Should().NotBeNull();
        res.Allowed.Should().Be(false);
        res.Message.Should().Be(deviceMock.Object.Translate(Resources.ResourceManager, nameof(Resources.Error_MissingFile)));
    }

    [Fact]
    public async Task ConnectAsync_ReturnsNotAllowed_InvalidType()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Throws(new InvalidOperationException("Invalid type 'testType'"));

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.ConnectAsync(operatorName, deviceMock.Object);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> Connect"))));

        res.Should().NotBeNull();
        res.Allowed.Should().Be(false);
        res.Message.Should().Be("Error accessing: Invalid type 'testType'");
    }

    [Fact]
    public async Task ConnectAsync_ReturnsAllowed()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        var operatorData = new Operator
        {
            Name = "TestingOperatorName",
            StartTime = DateTime.Now,
        };
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Returns(operatorData);

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.ConnectAsync(operatorName, deviceMock.Object);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> Connect"))));

        res.Should().NotBeNull();
        res.Allowed.Should().Be(true);
        res.Message.Should().BeNull();
    }

    [Fact]
    public async Task DisconnectAsync_ReturnsNotAllowed_MissingFile()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem();

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, new Mock<IParserFactory>().Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.DisconnectAsync(operatorName, "9999999999", false);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> Disconnect"))));

        res.Should().NotBeNull();
        res.Allowed.Should().Be(false);
        res.Message.Should().Be(Resources.Error_MissingFile);
    }

    [Fact]
    public async Task DisconnectAsync_ReturnsAllowed_MissingFile_Force()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem();

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, new Mock<IParserFactory>().Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.DisconnectAsync(operatorName, "9999999999", true);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> Disconnect"))));

        res.Should().NotBeNull();
        res.Allowed.Should().Be(true);
        res.Message.Should().Be(Resources.Error_MissingFile);
    }

    [Fact]
    public async Task DisconnectAsync_ReturnsNotAllowed_InvalidType()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Throws(new InvalidOperationException("Invalid type 'testType'"));

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.DisconnectAsync(operatorName, "9999999999", false);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> Disconnect"))));

        res.Should().NotBeNull();
        res.Allowed.Should().Be(false);
        res.Message.Should().Be("Error accessing: Invalid type 'testType'");
    }

    [Fact]
    public async Task DisconnectAsync_ReturnsNotAllowed_InvalidType_Force()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");


        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Throws(new InvalidOperationException("Invalid type 'testType'"));

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.DisconnectAsync(operatorName, "9999999999", true);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> Disconnect"))));

        res.Should().NotBeNull();
        res.Allowed.Should().Be(true);
        res.Message.Should().Be("Error accessing: Invalid type 'testType'");
    }

    [Fact]
    public async Task DisconnectAsync_ReturnsAllowed()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        var operatorData = new Operator
        {
            Name = "TestingOperatorName",
            StartTime = DateTime.Now,
        };
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Returns(operatorData);

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object) { Server = "TestServer", ServerLogEnabled = true, FileFormat = "Json" }, logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.DisconnectAsync(operatorName, "9999999999", true);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> Disconnect"))));

        res.Should().NotBeNull();
        res.Allowed.Should().Be(true);
        res.Message.Should().BeNull();
    }

    [Fact]
    public async Task RegisterOperatorStartAsync_MissingFile()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem();

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, new Mock<IParserFactory>().Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.RegisterOperatorStartAsync(operatorName, "9999999999");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> RegisterOperatorStart"))));

        res.Should().Be(Resources.Error_MissingFile);
    }

    [Fact]
    public async Task RegisterOperatorStartAsync_InvalidType()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Throws(new InvalidOperationException("Invalid type 'testType'"));

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.RegisterOperatorStartAsync(operatorName, "9999999999");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> RegisterOperatorStart"))));

        res.Should().Be("Error accessing: Invalid type 'testType'");
    }

    [Fact]
    public async Task RegisterOperatorStartAsync_Ok()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        var operatorData = new Operator
        {
            Name = "TestingOperatorName",
            StartTime = DateTime.Now,
        };
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Returns(operatorData);

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = await filesBehavior.RegisterOperatorStartAsync(operatorName, "9999999999");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> RegisterOperatorStart"))));

        res.Should().BeNull();
    }

    [Fact]
    public async Task BeginBreakAsync_MissingFile()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem();

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, new Mock<IParserFactory>().Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = new BeginBreak("TestingCode");
        await filesBehavior.Invoking(x => x.BeginBreakAsync(operatorName, "9999999999", res)).Should().ThrowAsync<InvalidOperationException>(Resources.Error_MissingFile);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("-> BeginBreak"))));
    }

    [Fact]
    public async Task BeginBreakAsync_InvalidType()
    {
        var server = "TestServer";
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath",server,"operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();

        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Throws(new InvalidOperationException("Invalid type 'testType'"));

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = new BeginBreak("TestingCode");
        await filesBehavior.Invoking(x => x.BeginBreakAsync(operatorName, "9999999999", res)).Should().ThrowAsync<InvalidOperationException>().WithMessage("Error accessing: Invalid type 'testType'");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("-> BeginBreak"))));
    }

    [Fact]
    public async Task BeginBreakAsync_Ok()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        var operatorData = new Operator
        {
            Name = "TestingOperatorName",
            StartTime = DateTime.Now,
        };

        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Returns(operatorData);

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        var res = new BeginBreak("TestingCode");
        await filesBehavior.Invoking(x => x.BeginBreakAsync(operatorName, "9999999999", res)).Should().NotThrowAsync();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("-> BeginBreak"))));
    }

    [Fact]
    public async Task EndBreakAsync_MissingFile()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileMock = new Mock<IFile>();
        fileMock.Setup(x => x.Exists(It.IsAny<string?>())).Returns(false);

        var pathMock = new Mock<IPath>();
        pathMock.Setup(x => x.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns("TestingCombinedPath");

        var fileSystemMock = new Mock<IFileSystem>();
        fileSystemMock.SetupGet(x => x.File).Returns(fileMock.Object);
        fileSystemMock.SetupGet(x => x.Path).Returns(pathMock.Object);

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock.Object, new Mock<IParserFactory>().Object) { CallBase = true };

        var behavior = behaviorMock.Object;

        await behavior.Invoking(x => x.EndBreakAsync(operatorName, "9999999999")).Should().ThrowAsync<InvalidOperationException>(Resources.Error_MissingFile);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("-> EndBreak"))));
    }

    [Fact]
    public async Task EndBreakAsync_InvalidType()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Throws(new InvalidOperationException("Invalid type 'testType'"));

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        await filesBehavior.Invoking(x => x.EndBreakAsync(operatorName, "9999999999")).Should().ThrowAsync<InvalidOperationException>("Error accessing: Invalid type 'testType'");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("-> EndBreak"))));
    }

    [Fact]
    public async Task EndBreakAsync_Ok()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        var operatorData = new Operator
        {
            Name = "TestingOperatorName",
            StartTime = DateTime.Now,
        };
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Returns(operatorData);

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var filesBehaviorMock = new Mock<FilesBehavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var filesBehavior = filesBehaviorMock.Object;
        filesBehavior.Initialize();

        await filesBehavior.Invoking(x => x.EndBreakAsync(operatorName, "9999999999")).Should().NotThrowAsync();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("-> EndBreak"))));
    }
}
