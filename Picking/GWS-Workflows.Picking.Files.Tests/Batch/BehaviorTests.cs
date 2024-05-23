using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GuidedWork.AppBase.Services.DataService;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Batch;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Moq;
using System.Globalization;
using System.IO.Abstractions.TestingHelpers;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Tests.Batch;

public class BehaviorTests
{
    [Fact]
    public async Task GetWorkorderAsync_NullObject()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceId = "9999999999";

        var fileSystemMock = new MockFileSystem();

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, new ParserFactory()) { CallBase = true };

        var behavior = behaviorMock.Object;
        behavior.Initialize();

        var res = await behavior.GetWorkOrdersAsync(operatorName, deviceId);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Batch|Behavior|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrders"))));

        res.Should().BeEmpty();
    }

    [Fact]
    public async Task GetWorkorderAsync()
    {
        var serverPath = "TestServer";
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceId = "9999999999";
        var code = "001";

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath", serverPath, "requests", $"{code}_{operatorName}.json"), new MockFileData("TestingData") },
            { Path.Combine("CustomPath", serverPath,"operators","user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        var orderList = new List<IGetWorkOrderItem>
        {
            new PickingLine(code) { Aisle = "TestAisle", ProductName = "TestProduct", StockCounting = StockCountingMode.No },
            new PlaceInDock(code) {},
        };
        parserMock.Setup(x => x.Parse<List<IGetWorkOrderItem>>(It.IsAny<string>())).Returns(orderList);

        var operatorData = new Operator
        {
            Name = "TestingOperatorName",
            StartTime = DateTime.Now,
        };
        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Returns(operatorData);

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var behavior = behaviorMock.Object;
        behavior.Initialize();

        var res = await behavior.GetWorkOrdersAsync(operatorName, deviceId);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Batch|Behavior|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrders"))));

        res.Should().Equal(orderList);
    }

    [Theory]
    [ClassData(typeof(BehaviorTestsData))]
    public async Task SetWorkorderAsync_FileDoesNotExist(ISetWorkOrderItem orderResult, Type type)
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceId = "9999999999";

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath", "TestServer", "operators", "user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        parserMock.Setup(x => x.Serialize(It.IsAny<List<object>>())).Returns("Testing AskQuestionResult");
        parserMock.Setup(x => x.Serialize(orderResult)).Returns($"Testing {type}");

        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Returns(new Operator { WorkOrderFileName = "TestingFileName" });

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var behavior = behaviorMock.Object;
        behavior.Initialize();

        await behavior.Invoking(x => x.SetWorkOrderAsync(operatorName, deviceId, orderResult)).Should().NotThrowAsync();
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Batch|Behavior|SetWorkOrderAsync|user|9999999999|") && x.EndsWith($"-> SetWorkOrder - Testing {type}"))));
    }

    [Theory]
    [ClassData(typeof(BehaviorTestsData))]
    public async Task SetWorkorderAsync_Null(ISetWorkOrderItem orderResult, Type type)
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceId = "9999999999";

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","results","TestingWorkOrderFileName.json"), new MockFileData("TestingData") },
            { Path.Combine("CustomPath", "TestServer", "operators", "user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        parserMock.Setup(x => x.Parse<List<object>>(It.IsAny<string>())).Returns<List<object>>(null);
        parserMock.Setup(x => x.Serialize(orderResult)).Returns($"Testing {type}");

        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Returns(new Operator { WorkOrderFileName = "TestingWorkOrderFileName" });

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };


        var behavior = behaviorMock.Object;
        behavior.Initialize();

        await behavior.Invoking(x => x.SetWorkOrderAsync(operatorName, deviceId, orderResult)).Should().NotThrowAsync();
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Batch|Behavior|SetWorkOrderAsync|user|9999999999|") && x.EndsWith($"-> SetWorkOrder - Testing {type}"))));

        parserMock.Verify(x => x.Parse<List<object>>(It.IsAny<string>()));
    }

    [Theory]
    [ClassData(typeof(BehaviorTestsData))]
    public async Task SetWorkorderAsync_FileExists(ISetWorkOrderItem orderResult, Type type)
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceId = "9999999999";

        var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { Path.Combine("CustomPath","TestServer","results","TestingWorkOrderFileName.json"), new MockFileData("TestingData") },
            { Path.Combine("CustomPath", "TestServer", "operators", "user.json"), new MockFileData("TestingData") },
        });

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        parserMock.Setup(x => x.Parse<List<object>>(It.IsAny<string>())).Returns(new List<object>());
        parserMock.Setup(x => x.Serialize(orderResult)).Returns($"Testing {type}");

        parserMock.Setup(x => x.Parse<Operator>(It.IsAny<string>())).Returns(new Operator { WorkOrderFileName = "TestingWorkOrderFileName" });

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var behavior = behaviorMock.Object;
        behavior.Initialize();

        await behavior.Invoking(x => x.SetWorkOrderAsync(operatorName, deviceId, orderResult)).Should().NotThrowAsync();
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Batch|Behavior|SetWorkOrderAsync|user|9999999999|") && x.EndsWith($"-> SetWorkOrder - Testing {type}"))));

        parserMock.Verify(x => x.Parse<List<object>>(It.IsAny<string>()));
    }

    [Fact]
    public async Task PrintLabelsBatchAsync()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceId = "9999999999";
        var code = "001";

        var fileSystemMock = new MockFileSystem();

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var parserFactoryMock = new Mock<IParserFactory>();

        var parserMock = new Mock<IParser>();
        var orderResult = new PrintLabelsBatch(code)
        {
            Count = 2,
        };
        parserMock.Setup(x => x.Serialize(It.IsAny<PrintLabelsBatchRequest>())).Returns("Testing PrintLabelsBatch");
        parserMock.Setup(x => x.Serialize(It.IsAny<PrintLabelsBatch>())).Returns("Testing PrintLabelsBatch");

        parserFactoryMock.Setup(x => x.GetParser(It.IsAny<FileFormat>())).Returns(parserMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new FilesBehaviorSettings(configServiceMock.Object), logMock.Object, customPathMock.Object, fileSystemMock, parserFactoryMock.Object) { CallBase = true };

        var behavior = behaviorMock.Object;
        behavior.Initialize();

        await behavior.Invoking(x => x.PrintLabelsBatchAsync(operatorName, deviceId, orderResult)).Should().NotThrowAsync();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Files|Batch|Behavior|PrintLabelsBatchAsync|user|9999999999|") && x.EndsWith($"-> PrintLabels - Testing PrintLabelsBatch"))));
        fileSystemMock.File.ReadAllText(fileSystemMock.Path.Combine("CustomPath", "TestServer", "requests", "001_print.json")).Should().Be("Testing PrintLabelsBatch");
    }
}
