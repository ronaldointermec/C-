using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GuidedWork.AppBase.Services.DataService;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Continuous;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Moq;
using System.Globalization;
using System.IO.Abstractions;
using System.Text;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Tests.Continuous;
public class BehaviorTests
{
    [Fact]
    public async Task GetWorkOrderAsync_NullObject()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceId = "9999999999";

        var fileMock = new Mock<IFile>();

        var pathMock = new Mock<IPath>();
        pathMock.Setup(x => x.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns("TestingCombinedPath");

        var directoryMock = new Mock<IDirectory>();
        directoryMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>())).Returns(Array.Empty<string>());

        var fileSystemMock = new Mock<IFileSystem>();
        fileSystemMock.SetupGet(x => x.File).Returns(fileMock.Object);
        fileSystemMock.SetupGet(x => x.Directory).Returns(directoryMock.Object);

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock.Object, new ParserFactory()) { CallBase = true };

        var behavior = behaviorMock.Object;

        var res = await behavior.GetWorkOrderAsync(operatorName, deviceId);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Batch|Behavior|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrder"))));

        res.Should().BeNull();
    }


    [Fact]
    public async Task GetWorkOrderAsync()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceId = "9999999999";
        var code = "001";

        var fileMock = new Mock<IFile>();
        fileMock.Setup(x => x.ReadAllTextAsync(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult("TestingFileData"));

        var directoryMock = new Mock<IDirectory>();
        directoryMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>())).Returns(new string[] { @$"RequestsPath\{code}_{operatorName}.json" });

        var pathMock = new Mock<IPath>();
        pathMock.Setup(x => x.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns("TestingCombinedPath");

        var fileSystemMock = new Mock<IFileSystem>();
        fileSystemMock.SetupGet(x => x.File).Returns(fileMock.Object);
        fileSystemMock.SetupGet(x => x.Path).Returns(pathMock.Object);
        fileSystemMock.SetupGet(x => x.Directory).Returns(directoryMock.Object);

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        var order = new PickingLine(code) { Aisle = "TestAisle", ProductName = "TestProduct", StockCounting = StockCountingMode.No };

        parserMock.Setup(x => x.Parse<IGetWorkOrderItem>(It.IsAny<string>())).Returns(order);

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock.Object, parserFactoryMock.Object) { CallBase = true };

        var behavior = behaviorMock.Object;

        behavior.Initialize();

        var res = await behavior.GetWorkOrderAsync(operatorName, deviceId);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Batch|Behavior|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrder"))));

        res.Should().NotBeNull();
        res.Should().Be(order);
    }

    [Theory]
    [ClassData(typeof(BehaviorTestsData))]
    public async Task SetWorkorderAsync(ISetWorkOrderItem orderResult, string logMessage, Type type)
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceId = "9999999999";

        var directoryMock = new Mock<IDirectory>();
        directoryMock.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>())).Returns(Array.Empty<string>());

        var pathMock = new Mock<IPath>();
        pathMock.Setup(x => x.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns("TestingCombinedPath");

        var fileMock = new Mock<IFile>();
        fileMock.Setup(x => x.WriteAllTextAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult("TestingFileData"));
        fileMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

        var fileSystemMock = new Mock<IFileSystem>();
        fileSystemMock.SetupGet(x => x.File).Returns(fileMock.Object);
        fileSystemMock.SetupGet(x => x.Path).Returns(pathMock.Object);
        fileSystemMock.SetupGet(x => x.Directory).Returns(directoryMock.Object);


        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();

        parserMock.Setup(x => x.Serialize(orderResult)).Returns($"Testing {logMessage}");

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock.Object, parserFactoryMock.Object) { CallBase = true };

        var behavior = behaviorMock.Object;

        behavior.Initialize();

        var res = await behavior.SetWorkOrderAsync(operatorName, deviceId, orderResult);
        res.Should().Be(null);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Batch|Behavior|SetWorkOrderAsync|user|9999999999|") && x.EndsWith($"-> SetWorkOrder - Testing {logMessage}"))));
        fileMock.Verify(x => x.WriteAllTextAsync(It.IsAny<string>(), It.IsAny<string>(), Encoding.UTF8, CancellationToken.None));
    }
}
