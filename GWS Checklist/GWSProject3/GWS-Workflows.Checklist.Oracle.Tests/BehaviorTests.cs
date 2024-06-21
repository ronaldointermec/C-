using Common.Logging;
using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK;
using Moq;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Oracle.Tests;

public class BehaviorTests
{
    public BehaviorTests()
    {
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        var logMock = new Mock<ILog>();
        var loggerMock = new Mock<ILoggerFactoryAdapter>();
        loggerMock.Setup(x => x.GetLogger(It.IsAny<Type>())).Returns(logMock.Object);
    }
    private static Mock<IConfigService> GetConfigService()
    {
        var serverKey = "Server";
        var logDeviceKey = "Log:Device";

        var serverConfigParamMock = GetConfigParam(serverKey);
        var logDeviceKeyConfigParamMock = GetConfigParam(logDeviceKey);

        var configServiceMock = new Mock<IConfigService>();
        configServiceMock.Setup(x => x.GetAllConfigs(It.IsAny<IConfigCategory>())).Returns(Enumerable.Empty<IConfigParam>().ToList());
        configServiceMock.Setup(x => x.GetOrCreateConfig(serverKey, It.IsAny<IConfigCategory>())).Returns(serverConfigParamMock.Object);
        configServiceMock.Setup(x => x.GetOrCreateConfig(logDeviceKey, It.IsAny<IConfigCategory>())).Returns(logDeviceKeyConfigParamMock.Object);
        return configServiceMock;
    }

    private static Mock<IConfigParam> GetConfigParam(string key)
    {
        var configParamMock = new Mock<IConfigParam>();

        configParamMock.SetupGet(x => x.Key).Returns(key);

        return configParamMock;
    }

    [Fact]
    public void Initialize_ServerNullOrEmpty_ThrowsException()
    {
        Mock<IConfigService> configServiceMock = GetConfigService();

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object), new DbDataSetParser()) { CallBase = true };

        Action act = () => behaviorMock.Object.Initialize();

        act.Should().Throw<InvalidOperationException>().WithMessage(Resources.ServerEmpty);
    }

    [Fact]
    public void Initialize_OK()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        var behaviorSettings = new ConnectorBehaviorSettingsBase(configServiceMock.Object)
        {
            Server = "Data"
        };

        var behaviorMock = new Mock<Behavior>(behaviorSettings, new DbDataSetParser());

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.Open());

        Action act = () => behaviorMock.Object.Initialize();

        act.Should().NotThrow();
    }

    [Fact]
    public void Initialize_ConnectionThrowsException()
    {
        Mock<IConfigService> configServiceMock = GetConfigService();

        var ex = new Exception("Testing Exception");
        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Data" }, new DbDataSetParser());
        behaviorMock.Setup(x => x.CreateConnection()).Throws(ex);

        Action act = () => behaviorMock.Object.Initialize();

        act.Should().NotThrow();
    }

    [Fact]
    public void GetOperator_WithoutRows_ReturnsNull()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        connMock.Setup(x => x.Open());

        var sqlDataReaderMock = new Mock<DbDataReader>();
        cmdMock.Setup(x => x.ExecuteReader()).Returns(sqlDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.GetOperator(oper);

        res.Should().BeNull();
    }

    [Fact]
    public void GetOperator_WithException_ReturnsNull()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.GetOperator(oper);

        res.Should().BeNull();
    }

    [Fact]
    public void GetOperator_WithRows_ReturnsOperator()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        connMock.Setup(x => x.Open());

        var sqlDataReaderMock = new Mock<DbDataReader>();
        sqlDataReaderMock.Setup(x => x.HasRows).Returns(true);
        sqlDataReaderMock.Setup(x => x.Read());
        sqlDataReaderMock.Setup(x => x.GetString(0)).Returns(oper);
        sqlDataReaderMock.Setup(x => x["password"]).Returns(DBNull.Value);

        cmdMock.Setup(x => x.ExecuteReader()).Returns(sqlDataReaderMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.GetOperator(oper);

        res.Should().NotBeNull();
        if (res != null)
        {
            res.Name.Should().Be(oper);
            res.Password.Should().Be(null);
        }
    }

    [Fact]
    public void RetrieveChecklist_WithException_ReturnsNull()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.RetrieveChecklist(oper, "TestChecklist");

        res.Should().BeNull();
    }

    [Fact]
    public void RetrieveChecklist()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        connMock.Setup(x => x.Open());

        var questionsDataTable = GetQuestionsDataTable();
        var selectOptionsDataTable = GetSelectOptionsDataTable();

        var sqlDataAdapterMock = new Mock<DbDataAdapter>();
        sqlDataAdapterMock.Setup(x => x.Fill(It.IsAny<DataSet>())).Callback((DataSet ds) =>
        {
            ds.Tables.Add(questionsDataTable);
            ds.Tables.Add(selectOptionsDataTable);
        });

        behaviorMock.Setup(x => x.CreateDataAdapter()).Returns(sqlDataAdapterMock.Object);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.RetrieveChecklist(oper, "TestChecklist");

        res.Should().NotBeNull();
        if (res != null)
        {
            res.Questions.Count().Should().Be(9);
            Assert.Collection(res.Questions,
                q1 => { var m = Assert.IsType<Message>(q1); m.Image.Should().NotBeNull(); },
                q2 => { var a = Assert.IsType<Ask>(q2); a.Image.Should().NotBeNull(); },
                q3 => { var i = Assert.IsType<IntegerValue>(q3); i.Image.Should().NotBeNull(); },
                q4 => { var f = Assert.IsType<FloatValue>(q4); f.Image.Should().NotBeNull(); },
                q5 => { var s = Assert.IsType<StringValue>(q5); s.Image.Should().NotBeNull(); },
                q6 => { var d = Assert.IsType<Date>(q6); d.Image.Should().NotBeNull(); },
                q7 => { var t = Assert.IsType<Time>(q7); t.Image.Should().NotBeNull(); },
                q8 => { Assert.IsType<Select>(q8); },
                q9 => { Assert.IsType<SelectMultiple>(q9); }
                );
        }
    }

    [Fact]
    public void UpdateChecklist_Complete_ThrowsException()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var checklist = new Models.Checklist
        {
            Completed = new DateTime(2022, 10, 24, 10, 0, 0),
            CompletedBy = oper
        };

        behavior.Invoking(x => x.UpdateChecklist(oper, "1", checklist)).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void UpdateChecklist_Complete_Ok()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var checklist = new Models.Checklist
        {
            Completed = new DateTime(2022, 10, 24, 10, 0, 0),
            CompletedBy = oper
        };

        behavior.Invoking(x => x.UpdateChecklist(oper, "1", checklist)).Should().NotThrow();
    }

    [Fact]
    public void UpdateChecklist_Complete_CheckCommands_Ok()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();

        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);

        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var checklist = new Models.Checklist
        {
            Completed = new DateTime(2022, 10, 24, 10, 0, 0),
            CompletedBy = oper
        };

        behavior.UpdateChecklist(oper, "1", checklist);
        cmdMock.VerifySet(x => x.CommandType = CommandType.StoredProcedure);
        cmdMock.VerifySet(x => x.CommandText = "CompleteChecklist");
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "operator" && x.Value.Equals(oper))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "checklistId" && x.Value.Equals("1"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "completedAt" && x.Value.Equals(new DateTime(2022, 10, 24, 10, 0, 0)))));
    }

    [Fact]
    public void UpdateChecklist_Update_ThrowsException()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        var ex = FormatterServices.GetUninitializedObject(typeof(OracleException))
                as OracleException;

        connMock.Setup(x => x.Open()).Throws(ex);

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var questions = new List<IQuestion>()
        {
            new Message { Code = "001", EndTime = new DateTime(2022, 10, 24, 10, 0, 0) }
        };
        var checklist = new Models.Checklist
        {
            Questions = questions
        };

        behavior.Invoking(x => x.UpdateChecklist(oper, "1", checklist)).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void UpdateChecklist_Update_Ok()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var questions = new List<IQuestion>()
        {
            new Message { Code = "001", StartTime = new DateTime(2022, 10, 24, 10, 0, 0), EndTime = new DateTime(2022, 10, 24, 10, 0, 0) },
            new Ask { Code = "001", StartTime = new DateTime(2022, 10, 24, 10, 0, 0), EndTime = new DateTime(2022, 10, 24, 10, 0, 0) , Answer = true },
            new Date { Code = "001", StartTime = new DateTime(2022, 10, 24, 10, 0, 0), EndTime = new DateTime(2022, 10, 24, 10, 0, 0) , Answer = new DateTime(2022, 10, 24) },
        };
        var checklist = new Models.Checklist
        {
            Questions = questions
        };

        behavior.Invoking(x => x.UpdateChecklist(oper, "1", checklist)).Should().NotThrow();
    }

    [Fact]
    public void UpdateChecklist_Update_CheckCommands_Ok()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();

        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);

        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var questions = new List<IQuestion>()
        {
            new Message { Code = "001", StartTime = new DateTime(2022, 10, 24, 10, 0, 0), EndTime = new DateTime(2022, 10, 24, 10, 0, 0) },
            new Ask { Code = "001", StartTime = new DateTime(2022, 10, 24, 10, 0, 0), EndTime = new DateTime(2022, 10, 24, 10, 0, 0) , Answer = true },
            new Date { Code = "001", StartTime = new DateTime(2022, 10, 24, 10, 0, 0), EndTime = new DateTime(2022, 10, 24, 10, 0, 0) , Answer = new DateTime(2022, 10, 24) },
        };
        var checklist = new Models.Checklist
        {
            Questions = questions
        };

        behavior.UpdateChecklist(oper, "1", checklist);
        cmdMock.VerifySet(x => x.CommandType = CommandType.StoredProcedure);
        cmdMock.VerifySet(x => x.CommandText = "UpdateChecklist");
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "operator" && x.Value.Equals(oper))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "checklistId" && x.Value.Equals("1"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "code" && x.Value.Equals("001"))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "started" && x.Value.Equals(new DateTime(2022, 10, 24, 10, 0, 0)))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "ended" && x.Value.Equals(new DateTime(2022, 10, 24, 10, 0, 0)))));
        dataParameterCollection.Verify(x => x.Add(It.Is<OracleParameter>(x => x.ParameterName == "answer" && x.Value.Equals(new DateTime(2022, 10, 24).ToString()))));
    }

    [Fact]
    public void UpdateChecklist_UpdateNoEndTime_Ok()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var questions = new List<IQuestion>()
        {
            new Message { Code = "001", StartTime = new DateTime(2022, 10, 24, 10, 0, 0) },
            new Ask { Code = "001", StartTime = new DateTime(2022, 10, 24, 10, 0, 0), Answer = true },
            new Date { Code = "001", StartTime = new DateTime(2022, 10, 24, 10, 0, 0), Answer = new DateTime(2022, 10, 24) },
        };
        var checklist = new Models.Checklist
        {
            Questions = questions
        };

        behavior.Invoking(x => x.UpdateChecklist(oper, "1", checklist)).Should().NotThrow();
    }

    [Fact]
    public void UpdateChecklist_UpdateNoStartTime_Fail()
    {
        var connMock = new Mock<IDbConnection>();
        var cmdMock = new Mock<IDbCommand>();

        Mock<IConfigService> configServiceMock = GetConfigService();

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var oper = "user";

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "Server=localhost" }, new DbDataSetParser()) { CallBase = true };

        connMock.Setup(x => x.Open());

        var dataParameterCollection = new Mock<DbParameterCollection>();
        cmdMock.SetupGet(x => x.Parameters).Returns(dataParameterCollection.Object);

        behaviorMock.Setup(x => x.CreateConnection()).Returns(connMock.Object);
        connMock.Setup(x => x.CreateCommand()).Returns(cmdMock.Object);

        var behavior = behaviorMock.Object;

        var questions = new List<IQuestion>()
        {
            new Message { Code = "001", EndTime = new DateTime(2022, 10, 24, 10, 0, 0) },
            new Ask { Code = "001", EndTime = new DateTime(2022, 10, 24, 10, 0, 0) , Answer = true },
            new Date { Code = "001", EndTime = new DateTime(2022, 10, 24, 10, 0, 0) , Answer = new DateTime(2022, 10, 24) },
        };
        var checklist = new Models.Checklist
        {
            Questions = questions
        };

        behavior.Invoking(x => x.UpdateChecklist(oper, "1", checklist)).Should().Throw<InvalidOperationException>();
    }

    private static DataTable GetQuestionsDataTable()
    {
        var dt = new DataTable("Questions");
        dt.Clear();
        dt.Columns.Add("Type", typeof(string));
        dt.Columns.Add("ChecklistId", typeof(string));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Prompt", typeof(string));
        dt.Columns.Add("SkipAllowed", typeof(bool));
        dt.Columns.Add("ReadyToContinue", typeof(bool));
        dt.Columns.Add("Priority", typeof(bool));
        dt.Columns.Add("AdditionalInformation", typeof(string));
        dt.Columns.Add("ConfirmationEnabled", typeof(bool));
        dt.Columns.Add("ScannerEnabled", typeof(bool));
        dt.Columns.Add("MinLength", typeof(int));
        dt.Columns.Add("MaxLength", typeof(int));
        dt.Columns.Add("MinValue", typeof(string));
        dt.Columns.Add("MaxValue", typeof(string));
        dt.Columns.Add("Format", typeof(string));
        dt.Columns.Add("Image", typeof(string));
        dt.Columns.Add("Operator", typeof(string));
        dt.Columns.Add("Answer", typeof(string));
        dt.Columns.Add("StartTime", typeof(DateTime));
        dt.Columns.Add("EndTime", typeof(DateTime));

        var dr1 = dt.NewRow();
        dr1["Type"] = "message";
        dr1["ChecklistId"] = "1";
        dr1["Code"] = "001";
        dr1["Prompt"] = "message";
        dr1["Image"] = "https://picsum.photos/200";

        dt.Rows.Add(dr1);

        var dr2 = dt.NewRow();
        dr2["Type"] = "ask";
        dr2["ChecklistId"] = "1";
        dr2["Code"] = "002";
        dr2["Prompt"] = "ask";
        dr2["Image"] = "https://picsum.photos/200";
        dt.Rows.Add(dr2);

        var dr3 = dt.NewRow();
        dr3["Type"] = "number";
        dr3["ChecklistId"] = "1";
        dr3["Code"] = "003";
        dr3["Prompt"] = "number";
        dr3["Image"] = "https://picsum.photos/200";
        dt.Rows.Add(dr3);

        var dr4 = dt.NewRow();
        dr4["Type"] = "decimal";
        dr4["ChecklistId"] = "1";
        dr4["Code"] = "004";
        dr4["Prompt"] = "decimal";
        dr4["Image"] = "https://picsum.photos/200";
        dt.Rows.Add(dr4);

        var dr5 = dt.NewRow();
        dr5["Type"] = "string";
        dr5["ChecklistId"] = "1";
        dr5["Code"] = "005";
        dr5["Prompt"] = "string";
        dr5["Image"] = "https://picsum.photos/200";
        dt.Rows.Add(dr5);

        var dr6 = dt.NewRow();
        dr6["Type"] = "date";
        dr6["ChecklistId"] = "1";
        dr6["Code"] = "006";
        dr6["Prompt"] = "date";
        dr6["Format"] = "yyyyMMdd";
        dr6["Image"] = "https://picsum.photos/200";
        dt.Rows.Add(dr6);

        var dr7 = dt.NewRow();
        dr7["Type"] = "time";
        dr7["ChecklistId"] = "1";
        dr7["Code"] = "007";
        dr7["Prompt"] = "time";
        dr7["Format"] = "hhmmss";
        dr7["Image"] = "https://picsum.photos/200";
        dt.Rows.Add(dr7);

        var dr8 = dt.NewRow();
        dr8["Type"] = "choice";
        dr8["ChecklistId"] = "1";
        dr8["Code"] = "008";
        dr8["Prompt"] = "choice";
        dt.Rows.Add(dr8);

        var dr9 = dt.NewRow();
        dr9["Type"] = "multiple_choice";
        dr9["ChecklistId"] = "1";
        dr9["Code"] = "009";
        dr9["Prompt"] = "multiple_choice";
        dt.Rows.Add(dr9);

        return dt;
    }

    private static DataTable GetSelectOptionsDataTable()
    {
        var dt = new DataTable("SelectOptions");
        dt.Columns.Add("ChecklistId", typeof(string));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("OptionCode", typeof(string));
        dt.Columns.Add("Description", typeof(string));

        var dr1 = dt.NewRow();
        dr1["ChecklistId"] = "1";
        dr1["Code"] = "008";
        dr1["OptionCode"] = "01";
        dr1["Description"] = "Option 1";
        dt.Rows.Add(dr1);

        var dr2 = dt.NewRow();
        dr2["ChecklistId"] = "1";
        dr2["Code"] = "009";
        dr2["OptionCode"] = "01";
        dr2["Description"] = "Option 1";
        dt.Rows.Add(dr2);

        return dt;
    }
}

