using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.SqlServer.Batch;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.SqlServer.Tests.Batch;

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
    public void GetWorkOrders_WithException_ThrowException()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(SqlException))
                as SqlException;

        _connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        Action act = () => behavior.GetWorkOrdersAsync(operatorName, deviceID);

        /* Assert */
        act.Should().Throw<InvalidOperationException>();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrder"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|GetWorkOrdersAsync|user|9999999999|") && x.Contains("<- Exception (Microsoft.Data.SqlClient.SqlException):"))));
    }

    [Fact]
    public void GetWorkOrders_ReturnsEmptyList()
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
        var res = behavior.GetWorkOrdersAsync(operatorName, deviceID).GetAwaiter().GetResult();

        /* Assert */
        res.Should().BeEmpty();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrder"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("<- Orders: '[]'"))));
    }

    [Fact]
    public void GetWorkOrders_Success()
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
        var data = new IGetWorkOrderItem[] { new BeginPickingOrder("001", "Picking order 001"), new PickingLine("001-01", string.Empty), new PlaceInDock("001-End", string.Empty) };

        sqlDataReaderMock.Setup(x => x.HasRows).Returns(true);
        sqlDataReaderMock.SetupSequence(x => x.Read()).Returns(true).Returns(true).Returns(true).Returns(false);

        sqlDataReaderMock.Setup(x => x[It.IsAny<string>()]).Returns(DBNull.Value);
        sqlDataReaderMock.SetupSequence(x => x["Type"]).Returns("BeginPickingOrder").Returns("PickingLine").Returns("PlaceInDock");
        sqlDataReaderMock.SetupSequence(x => x["Code"]).Returns("001").Returns("001").Returns("001-01").Returns("001-01").Returns("001-End").Returns("001-End");
        sqlDataReaderMock.SetupSequence(x => x["Message"]).Returns("Picking order 001").Returns(DBNull.Value);

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(sqlDataReaderMock.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.GetWorkOrdersAsync(operatorName, deviceID).GetAwaiter().GetResult();

        /* Assert */
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "operator" && x.Value.Equals(operatorName))));

        res.Should().BeEquivalentTo(data);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("-> GetWorkOrder"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|GetWorkOrdersAsync|user|9999999999|") && x.Contains("<- Orders:"))));
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

        var ex = FormatterServices.GetUninitializedObject(typeof(SqlException))
                as SqlException;

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

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> SetWorkOrder"))), Times.Exactly(4));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|SetWorkOrderAsync|user|9999999999|") && x.Contains("<- Exception (Microsoft.Data.SqlClient.SqlException):"))), Times.Exactly(4));
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

        /* Act */
        Action act1 = () => behavior.SetWorkOrderAsync(operatorName, deviceID, res1);
        Action act2 = () => behavior.SetWorkOrderAsync(operatorName, deviceID, res2);
        Action act3 = () => behavior.SetWorkOrderAsync(operatorName, deviceID, res3);
        Action act4 = () => behavior.SetWorkOrderAsync(operatorName, deviceID, res4);

        /* Assert */
        act1.Should().NotThrow();
        act2.Should().NotThrow();
        act3.Should().NotThrow();
        act4.Should().NotThrow();

        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Operator" && x.Value.Equals(operatorName))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Device" && x.Value.Equals(deviceID))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Code" && x.Value.Equals("001"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Started" && x.Value.Equals(started))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Finished" && x.Value.Equals(finished))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Status" && x.Value.Equals("OK"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Picked" && x.Value.Equals(1))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "ServingCode" && x.Value.Equals("UNITS"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Weight" && x.Value.Equals(2.5M))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Dock" && x.Value.Equals("DOCK"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Dock" && x.Value.Equals("DOCK1"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Stock" && x.Value.Equals(1))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "ProductCD" && x.Value.Equals("ProductCD"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Breakage" && x.Value.Equals(1))));
        dataParameterCollection.Verify(x => x.Add(It.Is<SqlParameter>(x => x.ParameterName == "Batch" && x.Value.Equals("batch"))));

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> SetWorkOrder"))), Times.Exactly(4));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|SetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Task completed"))), Times.Exactly(4));
    }

    [Fact]
    public void PrintLabels_WithException_ThrowException()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(SqlException))
                as SqlException;

        _connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        var res = new PrintLabelsBatch("001");

        /* Act */
        Action act = () => behavior.PrintLabelsBatchAsync(operatorName, deviceID, res);

        /* Assert */
        act.Should().Throw<InvalidOperationException>();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|PrintLabelsBatchAsync|user|9999999999|") && x.Contains("-> PrintLabels"))), Times.Exactly(1));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|PrintLabelsBatchAsync|user|9999999999|") && x.Contains("<- Exception (Microsoft.Data.SqlClient.SqlException):"))), Times.Exactly(1));
    }

    [Fact]
    public void PrintLabels_Success()
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

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        var res = new PrintLabelsBatch("001");

        /* Act */
        Action act1 = () => behavior.PrintLabelsBatchAsync(operatorName, deviceID, res);

        /* Assert */
        act1.Should().NotThrow();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|PrintLabelsBatchAsync|user|9999999999|") && x.Contains("-> PrintLabels"))), Times.Exactly(1));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("SqlServer|Batch|Behavior|PrintLabelsBatchAsync|user|9999999999|") && x.EndsWith("<- Task completed"))), Times.Exactly(1));
    }
}
