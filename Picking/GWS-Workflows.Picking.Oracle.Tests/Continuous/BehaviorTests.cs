using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle.Continuous;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Moq;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle.Tests.Continuous;

public class BehaviorTests
{

    private readonly Mock<IDbConnection> _connMock;
    private readonly Mock<IDbCommand> _cmdMock;

    public BehaviorTests()
    {
        _connMock = new Mock<IDbConnection>();
        _cmdMock = new Mock<IDbCommand>();
    }

    [Fact]
    public void GetWorkOrder_WithException_ThrowException()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        _connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        Action act = () => behavior.GetWorkOrderAsync(operatorName, deviceID);

        /* Assert */
        act.Should().Throw<InvalidOperationException>();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrder"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|GetWorkOrderAsync|user|9999999999|") && x.Contains("<- Exception (Oracle.ManagedDataAccess.Client.OracleException):"))));
    }

    [Fact]
    public void GetWorkOrder_ReturnsNull()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        var sqlDataReaderMock = new Mock<DbDataReader>();
        _cmdMock.Setup(x => x.ExecuteReader()).Returns(sqlDataReaderMock.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.GetWorkOrderAsync(operatorName, deviceID).GetAwaiter().GetResult();

        /* Assert */
        res.Should().BeNull();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrder"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- No work order received"))));
    }

    [Fact]
    public void GetWorkOrder_InvalidType_ReturnsNull()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        var oracleDataReaderMock = new Mock<DbDataReader>();
        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());

        // Setup Begin Picking Line
        oracleDataReaderMock.Setup(x => x["Type"]).Returns("Invalid");

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.GetWorkOrderAsync(operatorName, deviceID).GetAwaiter().GetResult();

        /* Assert */
        res.Should().BeNull();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrder"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Work Order: 'null'"))));
    }

    [Fact]
    public void GetWorkOrder_Success()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        var oracleDataReaderMock = new Mock<DbDataReader>();
        var data = new BeginPickingOrder("001", "Picking order 001")
        {
            OrderNumber = "12345",
            Customer = "Customer",
            ContainersCount = 1,
            ContainerType = "Pallet"
        };

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());

        // Setup Begin Picking Line
        oracleDataReaderMock.Setup(x => x[It.IsAny<string>()]).Returns(DBNull.Value);
        oracleDataReaderMock.Setup(x => x["Type"]).Returns("BeginPickingOrder");
        oracleDataReaderMock.SetupSequence(x => x["Code"]).Returns("001").Returns("001");
        oracleDataReaderMock.Setup(x => x["Message"]).Returns("Picking order 001");
        oracleDataReaderMock.Setup(x => x["ContainerType"]).Returns("Pallet");
        oracleDataReaderMock.Setup(x => x["ContainersCount"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["Customer"]).Returns("Customer");
        oracleDataReaderMock.Setup(x => x["OrderNumber"]).Returns("12345");

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.GetWorkOrderAsync(operatorName, deviceID).GetAwaiter().GetResult();

        /* Assert */
        res.Should().BeEquivalentTo(data);

        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "operator" && x.Value.Equals(operatorName))));

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrder"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Work Order: '{\"OrderNumber\":\"12345\",\"Customer\":\"Customer\",\"ContainersCount\":1,\"ContainerType\":\"Pallet\",\"Image\":null,\"Code\":\"001\",\"Message\":\"Picking order 001\"}'"))));
    }

    [Fact]
    public void SetWorkOrder_WithException_ThrowException()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        _connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        var res1 = new BeginPickingOrderResult("001");
        var res2 = new PickingLineResult("002");
        var res3 = new PlaceInDockResult("003");
        var res4 = new AskQuestionResult("004");

        /* Act */
        Action act1 = () => behavior.SetWorkOrderAsync(operatorName, deviceID, res1);
        Action act2 = () => behavior.SetWorkOrderAsync(operatorName, deviceID, res2);
        Action act3 = () => behavior.SetWorkOrderAsync(operatorName, deviceID, res3);
        Action act4 = () => behavior.SetWorkOrderAsync(operatorName, deviceID, res4);

        /* Assert */
        act1.Should().Throw<InvalidOperationException>();
        act2.Should().Throw<InvalidOperationException>();
        act3.Should().Throw<InvalidOperationException>();
        act4.Should().Throw<InvalidOperationException>();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> SetWorkOrder"))), Times.Exactly(4));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|SetWorkOrderAsync|user|9999999999|") && x.Contains("<- Exception (Oracle.ManagedDataAccess.Client.OracleException):"))), Times.Exactly(4));
    }

    [Fact]
    public void SetWorkOrderAsync_EmptyResponseContent_ReturnsNull()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        var oracleDataReaderMock = new Mock<DbDataReader>();
        oracleDataReaderMock.Setup(x => x.HasRows).Returns(false);

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res1 = behavior.SetWorkOrderAsync(operatorName, deviceID, new BeginPickingOrderResult("001")).GetAwaiter().GetResult();
        var res2 = behavior.SetWorkOrderAsync(operatorName, deviceID, new PickingLineResult("002")).GetAwaiter().GetResult();
        var res3 = behavior.SetWorkOrderAsync(operatorName, deviceID, new PlaceInDockResult("003")).GetAwaiter().GetResult();
        var res4 = behavior.SetWorkOrderAsync(operatorName, deviceID, new AskQuestionResult("004")).GetAwaiter().GetResult();
        var res5 = behavior.SetWorkOrderAsync(operatorName, deviceID, new PrintLabelsResult("005")).GetAwaiter().GetResult();
        var res6 = behavior.SetWorkOrderAsync(operatorName, deviceID, new ValidatePrintingResult("006")).GetAwaiter().GetResult();

        /* Assert */
        res1.Should().BeNull();
        res2.Should().BeNull();
        res3.Should().BeNull();
        res4.Should().BeNull();
        res5.Should().BeNull();
        res6.Should().BeNull();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> SetWorkOrder"))), Times.Exactly(6));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|SetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- No work order received"))), Times.Exactly(6));
    }

    [Fact]
    public void SetWorkOrder_Success()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();

        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x[It.IsAny<string>()]).Returns(DBNull.Value);
        oracleDataReaderMock.SetupSequence(x => x["Type"]).Returns("PickingLine").Returns("PlaceInDock").Returns("AskQuestion").Returns("PrintLabels").Returns("PrintLabels").Returns("ValidatePrinting").Returns("BeginPickingOrder");
        oracleDataReaderMock.SetupSequence(x => x["Code"]).Returns("002").Returns("002").Returns("003").Returns("003").Returns("004").Returns("004").Returns("005").Returns("005").Returns("005").Returns("005").Returns("006").Returns("006").Returns("001").Returns("001").Returns(DBNull.Value);
        oracleDataReaderMock.SetupSequence(x => x["Message"]).Returns(DBNull.Value).Returns(DBNull.Value).Returns(DBNull.Value).Returns(DBNull.Value).Returns(DBNull.Value).Returns(DBNull.Value).Returns(DBNull.Value).Returns("Empty work message");

        // Setup Begin Picking Line
        oracleDataReaderMock.Setup(x => x["ContainerType"]).Returns("Pallet");
        oracleDataReaderMock.Setup(x => x["ContainersCount"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["Customer"]).Returns("Customer");
        oracleDataReaderMock.Setup(x => x["OrderNumber"]).Returns("12345");

        // Setup Picking Line
        oracleDataReaderMock.Setup(x => x["Customer"]).Returns("Customer");
        oracleDataReaderMock.Setup(x => x["Aisle"]).Returns("Aisle");
        oracleDataReaderMock.Setup(x => x["Slot"]).Returns("Slot");
        oracleDataReaderMock.Setup(x => x["Position"]).Returns("Position");
        oracleDataReaderMock.Setup(x => x["CD"]).Returns("CD");
        oracleDataReaderMock.Setup(x => x["ProductName"]).Returns("ProductName");
        oracleDataReaderMock.Setup(x => x["ProductNumber"]).Returns("ProductNumber");
        oracleDataReaderMock.Setup(x => x["UpcNumber"]).Returns("UpcNumber");
        oracleDataReaderMock.Setup(x => x["OriginalServingCode"]).Returns("OriginalServingCode");
        oracleDataReaderMock.Setup(x => x["OriginalServingPrompt"]).Returns("OriginalServingPrompt");
        oracleDataReaderMock.Setup(x => x["OriginalServingQuantity"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["OriginalServingUpperTolerance"]).Returns(2.5);
        oracleDataReaderMock.Setup(x => x["OriginalServingLowerTolerance"]).Returns(1.5);
        oracleDataReaderMock.Setup(x => x["OriginalMaxQuantityAllowedPerPick"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["OriginalUnitsFormat"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["AlternativeServingCode"]).Returns("AlternativeServingCode");
        oracleDataReaderMock.Setup(x => x["AlternativeServingPrompt"]).Returns("AlternativeServingPrompt");
        oracleDataReaderMock.Setup(x => x["AlternativeServingQuantity"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["AlternativeServingUpperTolerance"]).Returns(2.5);
        oracleDataReaderMock.Setup(x => x["AlternativeServingLowerTolerance"]).Returns(1.5);
        oracleDataReaderMock.Setup(x => x["AlternativeMaxQuantityAllowedPerPick"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["AlternativeUnitsFormat"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["UnitsQuantity"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["UnitsUpperTolerance"]).Returns(1.5);
        oracleDataReaderMock.Setup(x => x["UnitsLowerTolerance"]).Returns(2.5);
        oracleDataReaderMock.Setup(x => x["UnitsMaxQuantityAllowedPerPick"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["AskWeight"]).Returns(true);
        oracleDataReaderMock.Setup(x => x["WeightMin"]).Returns(1.5);
        oracleDataReaderMock.Setup(x => x["WeightMax"]).Returns(2.5);
        oracleDataReaderMock.Setup(x => x["HowMuchMore"]).Returns(10);
        oracleDataReaderMock.Setup(x => x["StockCounting"]).Returns("1");
        oracleDataReaderMock.Setup(x => x["SpeakProductName"]).Returns(false);
        oracleDataReaderMock.Setup(x => x["AskBatch"]).Returns(true);
        oracleDataReaderMock.Setup(x => x["AskPackage"]).Returns(false);
        oracleDataReaderMock.Setup(x => x["Batches"]).Returns("123|456");
        oracleDataReaderMock.Setup(x => x["CanSkipAisle"]).Returns(false);
        oracleDataReaderMock.Setup(x => x["HelpMsg"]).Returns("HelpMessage");
        oracleDataReaderMock.Setup(x => x["CountdownPick"]).Returns(false);
        oracleDataReaderMock.Setup(x => x["ProductCDs"]).Returns("1234|567");

        // Setup Place In Dock
        oracleDataReaderMock.Setup(x => x["Dock"]).Returns("Dock");
        oracleDataReaderMock.Setup(x => x["CD"]).Returns("CD");

        // Setup PrintLabels
        oracleDataReaderMock.SetupSequence(x => x["DefaultPrinter"]).Returns(1).Returns(1).Returns(DBNull.Value).Returns(DBNull.Value);
        oracleDataReaderMock.SetupSequence(x => x["Copies"]).Returns(1).Returns(1).Returns(DBNull.Value).Returns(DBNull.Value);
        oracleDataReaderMock.SetupSequence(x => x["Printers"]).Returns("1|2|3").Returns("1|2|3").Returns(DBNull.Value).Returns(DBNull.Value);

        // Setup Validate Printing
        oracleDataReaderMock.Setup(x => x["ValidationCodes"]).Returns("1|2|3|4|5");
        oracleDataReaderMock.Setup(x => x["VoiceLength"]).Returns(1);

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        var started = new DateTime(2022, 1, 1, 0, 0, 0);
        var finished = new DateTime(2022, 1, 1, 0, 0, 1);

        var res1 = new BeginPickingOrderResult("001")
        {
            Started = started,
            Finished = finished,
            Status = "OK",
        };
        var res2 = new PickingLineResult("002")
        {
            Started = started,
            Finished = finished,
            Status = "OK",
            Picked = 1,
            ServingCode = "UNITS",
            Weight = 2.5M,
            Dock = "DOCK",
            Stock = 1,
            ProductCD = "ProductCD",
            Breakage = 1,
            Batch = "batch",
        };
        var res3 = new PlaceInDockResult("003")
        {
            Started = started,
            Finished = finished,
            Status = "OK",
            Dock = "DOCK1"
        };
        var res4 = new AskQuestionResult("004")
        {
            Started = started,
            Finished = finished,
            Status = "OK",
        };
        var res5 = new PrintLabelsResult("005")
        {
            Started = started,
            Finished = finished,
            Status = "OK",
            LabelsToPrint = 1,
            Printer = 1
        };
        var res6 = new ValidatePrintingResult("006")
        {
            Started = started,
            Finished = finished,
            Status = "OK",
            ReadLabels = new List<string> { "123", "456" },
        };

        /* Act */
        var pickingLine = behavior.SetWorkOrderAsync(operatorName, deviceID, res1).GetAwaiter().GetResult();
        var placeInDock = behavior.SetWorkOrderAsync(operatorName, deviceID, res2).GetAwaiter().GetResult();
        var askQuestion = behavior.SetWorkOrderAsync(operatorName, deviceID, res3).GetAwaiter().GetResult();
        var printLabels = behavior.SetWorkOrderAsync(operatorName, deviceID, res4).GetAwaiter().GetResult();
        var printLabelsE = behavior.SetWorkOrderAsync(operatorName, deviceID, res4).GetAwaiter().GetResult();
        var validatePrinting = behavior.SetWorkOrderAsync(operatorName, deviceID, res5).GetAwaiter().GetResult();
        var beginPickingOrder = behavior.SetWorkOrderAsync(operatorName, deviceID, res6).GetAwaiter().GetResult();
        var emptyWork = behavior.SetWorkOrderAsync(operatorName, deviceID, res1).GetAwaiter().GetResult();

        /* Assert */
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Operator" && x.Value.Equals(operatorName) && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Device" && x.Value.Equals(deviceID) && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Code" && x.Value.Equals("001") && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Started" && x.Value.Equals(started) && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Finished" && x.Value.Equals(finished) && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Status" && x.Value.Equals("OK") && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Picked" && x.Value.Equals(1) && x.OracleDbType == OracleDbType.Int32)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "ServingCode" && x.Value.Equals("UNITS") && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Weight" && x.Value.Equals(2.5M) && x.OracleDbType == OracleDbType.Decimal)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Dock" && x.Value.Equals("DOCK") && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Dock" && x.Value.Equals("DOCK1") && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Stock" && x.Value.Equals(1) && x.OracleDbType == OracleDbType.Int32)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "ProductCD" && x.Value.Equals("ProductCD") && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Breakage" && x.Value.Equals(1) && x.OracleDbType == OracleDbType.Int32)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Batch" && x.Value.Equals("batch") && x.OracleDbType == OracleDbType.Varchar2)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "LabelsToPrint" && x.Value.Equals(1) && x.OracleDbType == OracleDbType.Int32)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "Printer" && x.Value.Equals(1) && x.OracleDbType == OracleDbType.Int32)));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "ReadLabels" && x.Value.Equals("123|456") && x.OracleDbType == OracleDbType.Varchar2)));



        pickingLine.Should().BeEquivalentTo(new PickingLine("002", string.Empty)
        {
            Customer = "Customer",
            Aisle = "Aisle",
            Slot = "Slot",
            Position = "Position",
            CD = "CD",
            ProductName = "ProductName",
            ProductNumber = "ProductNumber",
            UpcNumber = "UpcNumber",
            OriginalServingCode = "OriginalServingCode",
            OriginalServingPrompt = "OriginalServingPrompt",
            OriginalServingQuantity = 1,
            OriginalServingUpperTolerance = 2.5m,
            OriginalServingLowerTolerance = 1.5m,
            OriginalMaxQuantityAllowedPerPick = 1,
            OriginalUnitsFormat = 1,
            AlternativeServingCode = "AlternativeServingCode",
            AlternativeServingPrompt = "AlternativeServingPrompt",
            AlternativeServingQuantity = 1,
            AlternativeServingUpperTolerance = 2.5m,
            AlternativeServingLowerTolerance = 1.5m,
            AlternativeMaxQuantityAllowedPerPick = 1,
            AlternativeUnitsFormat = 1,
            UnitsQuantity = 1,
            UnitsUpperTolerance = 1.5m,
            UnitsLowerTolerance = 2.5m,
            UnitsMaxQuantityAllowedPerPick = 1,
            AskWeight = true,
            WeightMin = 1.5m,
            WeightMax = 2.5m,
            HowMuchMore = 10,
            StockCounting = StockCountingMode.PartialPicked,
            SpeakProductName = false,
            AskBatch = true,
            Batches = new string[] { "123", "456" },
            CanSkipAisle = false,
            HelpMsg = "HelpMessage",
            CountdownPick = false,
            ProductCDs = new string[] { "1234", "567" },
        });

        placeInDock.Should().BeEquivalentTo(new PlaceInDock("003", string.Empty)
        {
            Dock = "Dock",
            CD = "CD"
        });

        askQuestion.Should().BeEquivalentTo(new AskQuestion("004", string.Empty));

        printLabels.Should().BeEquivalentTo(new PrintLabels("005", string.Empty)
        {
            DefaultPrinter = 1,
            Copies = 1,
            Printers = new int[] { 1, 2, 3 },
        });

        printLabelsE.Should().BeEquivalentTo(new PrintLabels("005", string.Empty)
        {
            DefaultPrinter = null,
            Copies = null,
            Printers = Array.Empty<int>(),
        });

        validatePrinting.Should().BeEquivalentTo(new ValidatePrinting("006", string.Empty)
        {
            ValidationCodes = new string[] { "1", "2", "3", "4", "5" },
            VoiceLength = 1,
        });

        beginPickingOrder.Should().BeEquivalentTo(new BeginPickingOrder("001", string.Empty)
        {
            ContainerType = "Pallet",
            ContainersCount = 1,
            Customer = "Customer",
            OrderNumber = "12345",
        });

        emptyWork.Should().BeEquivalentTo(new EmptyWork("Empty work message"));

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> SetWorkOrder"))), Times.Exactly(8));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Continuous|Behavior|SetWorkOrderAsync|user|9999999999|") && x.Contains("<- Work Order:"))), Times.Exactly(8));
    }
}
