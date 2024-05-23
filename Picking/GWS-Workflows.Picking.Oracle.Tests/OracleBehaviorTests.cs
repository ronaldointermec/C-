using Common.Logging;
using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Moq;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;
using Resources = Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle.Properties.Resources;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle.Tests;

public sealed class OracleBehaviorTests : IDisposable
{

    private readonly Mock<IDbConnection> _connMock;
    private readonly Mock<IDbCommand> _cmdMock;

    public OracleBehaviorTests()
    {
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        var logMock = new Mock<ILog>();
        var loggerMock = new Mock<ILoggerFactoryAdapter>();
        loggerMock.Setup(x => x.GetLogger(It.IsAny<Type>())).Returns(logMock.Object);

        TinyIoC.TinyIoCContainer.Current.Register(loggerMock.Object);

        _connMock = new Mock<IDbConnection>();
        _cmdMock = new Mock<IDbCommand>();
    }

    public void Dispose()
    {
        TinyIoC.TinyIoCContainer.Current.Unregister<ILoggerFactoryAdapter>();
    }

    [Fact]
    public void Initialize_ServerNullOrEmpty_ThrowsException()
    {
        /* Arrange */
        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService(string.Empty);
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), new Mock<IServerLog>().Object, new DbDataReaderParser()) { CallBase = true };

        /* Act */
        Action act = () => behaviorMock.Object.Initialize();

        /* Arrange */
        act.Should().Throw<InvalidOperationException>().WithMessage(Resources.ServerEmpty);
    }

    [Fact]
    public void Initialize_OK()
    {
        /* Arrange */
        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorSettings = new PickingBehaviorSettings(configServiceMock.Object);

        var behaviorMock = new Mock<OracleBehavior>(behaviorSettings, new Mock<IServerLog>().Object, new DbDataReaderParser()) { CallBase = true };

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.Open());

        /* Act */
        Action act = () => behaviorMock.Object.Initialize();

        /* Assert */
        act.Should().NotThrow();
    }

    [Fact]
    public void Initialize_WithWrongServer_ContinuesAndLogAnError()
    {
        /* Arrange */
        var logMock = new Mock<IServerLog>();
        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        /* Act */
        Action act = () => behaviorMock.Object.Initialize();

        /* Assert */
        act.Should().NotThrow();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|Initialize| The database was not accessible during initialization (System.ArgumentException):"))));
    }

    [Fact]
    public void ConnectToServerAsync_WithException_ReturnsNotAllowed()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");
        deviceMock.Setup(x => x.Language).Returns("en");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        _connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.ConnectAsync(operatorName, deviceMock.Object).AsTask().Result;

        /* Assert */
        res.Should().NotBeNull();
        res.Allowed.Should().BeFalse();
        res.Password.Should().BeNull();
        res.Message.Should().Be(DialogResources.Error);

        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "operator" && x.Value.Equals(operatorName))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "device" && x.Value.Equals(deviceMock.Object.DeviceID))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "language" && x.Value.Equals(deviceMock.Object.Language))));

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorStart - language: 'en'"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.Contains("<- Exception (Oracle.ManagedDataAccess.Client.OracleException):"))));
    }

    [Fact]
    public void ConnectToServerAsync_WithoutRows_ReturnsNotAllowed()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");
        deviceMock.Setup(x => x.Language).Returns("en");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var sqlDataReaderMock = new Mock<DbDataReader>();
        _cmdMock.Setup(x => x.ExecuteReader()).Returns(sqlDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.ConnectAsync(operatorName, deviceMock.Object).AsTask().Result;

        /* Assert */
        res.Should().NotBeNull();
        res.Allowed.Should().BeFalse();
        res.Password.Should().BeNull();
        res.Message.Should().Be(DialogResources.Error_ConnectMissingServerResponse);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorStart - language: 'en'"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<- Error accessing: no response received from the server"))));
    }

    [Fact]
    public void ConnectToServerAsync_ReturnsNotAllowedWithMessage()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");
        deviceMock.Setup(x => x.Language).Returns("en");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());
        oracleDataReaderMock.Setup(x => x["Allowed"]).Returns(0);
        oracleDataReaderMock.Setup(x => x["Message"]).Returns("Not Allowed");

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.ConnectAsync(operatorName, deviceMock.Object).AsTask().Result;

        /* Assert */
        res.Should().NotBeNull();
        res.Allowed.Should().BeFalse();
        res.Password.Should().BeNullOrEmpty();
        res.Message.Should().Be("Not Allowed");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorStart - language: 'en'"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<- Allowed: 'False' - Password: 'XXX' - Message: 'Not Allowed'"))));
    }

    [Fact]
    public void ConnectToServerAsync_ReturnsNotAllowedWithoutMessage()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");
        deviceMock.Setup(x => x.Language).Returns("en");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());
        oracleDataReaderMock.Setup(x => x["Allowed"]).Returns(0);
        oracleDataReaderMock.Setup(x => x["Message"]).Returns(DBNull.Value);

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.ConnectAsync(operatorName, deviceMock.Object).AsTask().Result;

        /* Assert */
        res.Should().NotBeNull();
        res.Allowed.Should().BeFalse();
        res.Password.Should().BeNullOrEmpty();
        res.Message.Should().Be(DialogResources.Error_UnknownReason);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorStart - language: 'en'"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<- Allowed: 'False' - Password: 'XXX' - Message: ''"))));
    }

    [Fact]
    public void ConnectToServerAsync_ReturnsAllowedWithAMessageAndPassword()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");
        deviceMock.Setup(x => x.Language).Returns("en");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());
        oracleDataReaderMock.Setup(x => x["Allowed"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["Password"]).Returns("1234");
        oracleDataReaderMock.Setup(x => x["Message"]).Returns("Welcome");

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.ConnectAsync(operatorName, deviceMock.Object).AsTask().Result;

        /* Assert */
        res.Should().NotBeNull();
        res.Allowed.Should().BeTrue();
        res.Password.Should().Be("1234");
        res.Message.Should().Be("Welcome");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorStart - language: 'en'"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<- Allowed: 'True' - Password: 'XXX' - Message: 'Welcome'"))));
    }

    [Fact]
    public void ConnectToServerAsync_ReturnsAllowedWithoutMessageAndPassword()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");
        deviceMock.Setup(x => x.Language).Returns("en");

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());
        oracleDataReaderMock.Setup(x => x["Allowed"]).Returns(1);
        oracleDataReaderMock.Setup(x => x["Password"]).Returns(DBNull.Value);
        oracleDataReaderMock.Setup(x => x["Message"]).Returns(DBNull.Value);

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.ConnectAsync(operatorName, deviceMock.Object).AsTask().Result;

        /* Assert */
        res.Should().NotBeNull();
        res.Allowed.Should().BeTrue();
        res.Password.Should().BeNull();
        res.Message.Should().BeNull();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorStart - language: 'en'"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<- Allowed: 'True' - Password: 'XXX' - Message: ''"))));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Disconnect_WithException_ThrowException(bool force)
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService("Server=");
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        _connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        Action act = () => behavior.DisconnectAsync(operatorName, deviceID, force).AsTask();

        /* Assert */
        act.Should().Throw<InvalidOperationException>();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorExit"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.Contains("<- Exception (Oracle.ManagedDataAccess.Client.OracleException):"))));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Disconnect_WithoutRows_ThrowException(bool force)
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService("Server=");
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        behavior.Invoking(async x => await x.DisconnectAsync(operatorName, deviceID, force)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[DisconnectAsync] {Resources.Error_ServerResponse}");

        /* Assert */

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorExit"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("<- Error disconnecting: no response received from the server"))));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Disconnect_ReturnsAllowed(bool force)
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());
        oracleDataReaderMock.Setup(x => x["Allowed"]).Returns(1);

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.DisconnectAsync(operatorName, deviceID, force).AsTask().Result;

        /* Assert */
        res.Should().NotBeNull();
        res.Allowed.Should().BeTrue();

        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "operator" && x.Value.Equals(operatorName))));

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorExit"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("<- Allowed: 'True' - Message: ''"))));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Disconnect_ReturnsNotAllowedWithMessage(bool force)
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());
        oracleDataReaderMock.Setup(x => x["Allowed"]).Returns(0);
        oracleDataReaderMock.Setup(x => x["Message"]).Returns("Not allowed");

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.DisconnectAsync(operatorName, deviceID, force).AsTask().Result;

        /* Assert */
        res.Should().NotBeNull();
        res.Allowed.Should().Be(force);
        res.Message.Should().Be("Not allowed");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorExit"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("<- Allowed: 'False' - Message: 'Not allowed'"))));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Disconnect_ReturnsNotAllowedWithoutMessage(bool force)
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());
        oracleDataReaderMock.Setup(x => x["Allowed"]).Returns(0);
        oracleDataReaderMock.Setup(x => x["Message"]).Returns(DBNull.Value);

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.DisconnectAsync(operatorName, deviceID, force).AsTask().Result;

        /* Assert */
        res.Should().NotBeNull();
        res.Allowed.Should().Be(force);
        res.Message.Should().BeNull();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> GetOperatorExit"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("<- Allowed: 'False' - Message: ''"))));
    }

    [Fact]
    public void RegisterOperatorStart_WithException_ThrowException()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        _connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        Action act = () => behavior.RegisterOperatorStartAsync(operatorName, deviceID);

        /* Assert */
        act.Should().Throw<InvalidOperationException>();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> SetOperatorStart"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.Contains("<- Exception (Oracle.ManagedDataAccess.Client.OracleException):"))));
    }

    [Fact]
    public void RegisterOperatorStart_WithoutRows_ThrowException()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        Action act = () => behavior.RegisterOperatorStartAsync(operatorName, deviceID);

        /* Assert */
        act.Should().Throw<InvalidOperationException>().WithMessage($"[RegisterOperatorStartAsync] {string.Format(Resources.Error_ServerResponse)}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> SetOperatorStart"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("<- Error registering operator start: no response received from the server"))));
    }

    [Fact]
    public void RegisterOperatorStart_ReturnsNullMessage()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());
        oracleDataReaderMock.Setup(x => x["Message"]).Returns(DBNull.Value);

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.RegisterOperatorStartAsync(operatorName, deviceID).Result;

        /* Assert */
        res.Should().BeNull();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> SetOperatorStart"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("<- Message: ''"))));
    }

    [Fact]
    public void RegisterOperatorStart_Success()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var oracleDataReaderMock = new Mock<DbDataReader>();

        oracleDataReaderMock.Setup(x => x.HasRows).Returns(true);
        oracleDataReaderMock.Setup(x => x.Read());
        oracleDataReaderMock.Setup(x => x["Message"]).Returns("Welcome!");

        _cmdMock.Setup(x => x.ExecuteReader()).Returns(oracleDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        var res = behavior.RegisterOperatorStartAsync(operatorName, deviceID).Result;

        /* Assert */
        res.Should().Be("Welcome!");

        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "operator" && x.Value.Equals(operatorName))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "device" && x.Value.Equals(deviceID))));

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> SetOperatorStart"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("<- Message: 'Welcome!'"))));
    }

    [Fact]
    public void BeginBreak_WithException_ThrowException()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";
        var res = new BeginBreak("A123456")
        {
            Reason = 1,
        };

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        _connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        Action act = () => behavior.BeginBreakAsync(operatorName, deviceID, res).GetAwaiter();

        /* Assert */
        act.Should().Throw<InvalidOperationException>();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("-> BeginBreak - {\"Code\":\"A123456\",\"Reason\":1}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|BeginBreakAsync|user|9999999999|") && x.Contains("<- Exception (Oracle.ManagedDataAccess.Client.OracleException):"))));
    }

    [Fact]
    public void BeginBreak_Success()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";
        var res = new BeginBreak("A123456")
        {
            Reason = 1,
        };

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        Action act = () => behavior.BeginBreakAsync(operatorName, deviceID, res);

        /* Assert */
        act.Should().NotThrow();

        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "operator" && x.Value.Equals(operatorName))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "code" && x.Value.Equals("A123456"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "reason" && x.Value.Equals((short)1))));

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("-> BeginBreak - {\"Code\":\"A123456\",\"Reason\":1}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|BeginBreakAsync|user|9999999999|") && x.Contains("<- Task completed"))));
    }

    [Fact]
    public void EndBreak_WithException_ThrowException()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };


        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        _connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        Action act = () => behavior.EndBreakAsync(operatorName, deviceID);

        /* Assert */
        act.Should().Throw<InvalidOperationException>();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("-> EndBreak"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|EndBreakAsync|user|9999999999|") && x.Contains("<- Exception (Oracle.ManagedDataAccess.Client.OracleException):"))));
    }

    [Fact]
    public void EndBreak_Success()
    {
        /* Arrange */
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";
        var deviceID = "9999999999";

        var logMock = new Mock<IServerLog>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<OracleBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, new DbDataReaderParser()) { CallBase = true };

        _connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        _cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(_connMock.Object);
        _connMock.Setup(x => x.CreateCommand()).Returns(_cmdMock.Object);

        var behavior = behaviorMock.Object;

        /* Act */
        Action act = () => behavior.EndBreakAsync(operatorName, deviceID);

        /* Assert */
        act.Should().NotThrow();

        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "operator" && x.Value.Equals(operatorName))));

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("-> EndBreak"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Oracle|Behavior|EndBreakAsync|user|9999999999|") && x.Contains("<- Task completed"))));
    }
}