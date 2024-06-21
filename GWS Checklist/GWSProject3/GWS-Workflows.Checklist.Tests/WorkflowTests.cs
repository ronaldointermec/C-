using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;
using IConnectorBehavior = Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules.IConnectorBehavior;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class WorkflowTests
{
    public WorkflowTests()
    {
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
    }

    [Fact]
    public async Task GetAnchorWordsAsync_Success()
    {
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        var anchorMock = await workflowMock.GetAnchorWordsAsync(new Mock<IDevice>().Object);
        anchorMock.PrepareResponse().Should().Be(@"

");
    }
    [Fact]
    public async Task GetVariableSetAsync_Success()
    {
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        var variablesMock = await workflowMock.GetVariableSetAsync(new Mock<Dictionary<string, string>>().Object, new Mock<IDevice>().Object);
        variablesMock.PrepareResponse().Should().Be(
            @"DLG,""00"",1
PROMPT,,0
AUX,,0
DUMMY,,0
COMPLETE,""1"",0
ID,,0
CODE,,0
ANSWER,,0
UNDO,,0
START_TIME,,0
END_TIME,,0
YEAR,,0
MONTH,,0
DAY,,0
HOUR,,0
MINUTE,,0
SECOND,,0

");
    }
    [Fact]
    public void GetMenuOptionsAsync_Success()
    {
        var device = new Mock<IDevice>().Object;
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        workflowMock.Invoking(x => x.GetMenuOptionsAsync(device, string.Empty)).Should().Throw<NullReferenceException>();
    }
    [Fact]
    public async Task ConnectAsync_OperatorNotFound()
    {
        var device = new Mock<IDevice>();
        device.SetupGet(x => x.Operator).Returns("testOperator");
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        var connectMock = await workflowMock.ConnectAsync(device.Object);
        connectMock.PrepareResponse().Should().Be("0,0,\"Operator testOperator not found.\"\r\n\r\n");
    }
    [Fact]
    public async Task ConnectAsync_SuccesWithoutPassword()
    {
        var device = new Mock<IDevice>().Object;
        var connectorBehaviorMock = new Mock<IConnectorBehavior>();
        var testOperator = new Operator() { Name = "testOperator", Password = null };
        connectorBehaviorMock.Setup(x => x.GetOperator(device.Operator)).Returns(testOperator);

        var workflowMock = new Workflow(connectorBehaviorMock.Object);
        var connectMock = await workflowMock.ConnectAsync(device);
        connectMock.PrepareResponse().Should().Be("0,1,,,\"Welcome!\",,,,,0\r\n\r\n");
    }
    [Fact]
    public async Task ConnectAsync_SuccesWithPassword()
    {
        var device = new Mock<IDevice>().Object;
        var connectorBehaviorMock = new Mock<IConnectorBehavior>();
        var testOperator = new Operator() { Name = "testOperator", Password = "testPassword" };
        connectorBehaviorMock.Setup(x => x.GetOperator(device.Operator)).Returns(testOperator);

        var workflowMock = new Workflow(connectorBehaviorMock.Object);
        var connectMock = await workflowMock.ConnectAsync(device);
        connectMock.PrepareResponse().Should().Be(@"1,1,,,""Welcome!"",""testPassword"",""Password"",""Say password for the operator testOperator"",""Wrong password"",0

");
    }
    [Fact]
    public void ProcessDataAsync_MissingId()
    {
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        workflowMock.Invoking(x => x.ProcessDataAsync(new Mock<Dictionary<string, string>>().Object, new Mock<IDevice>().Object)).Should().Throw<ArgumentException>().WithMessage(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));
    }
    [Fact]
    public void ProcessDataAsync_MissingCode()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("ID", "9999");
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        workflowMock.Invoking(x => x.ProcessDataAsync(dictionary, new Mock<IDevice>().Object)).Should().Throw<ArgumentException>().WithMessage(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));
    }
    [Fact]
    public void ProcessDataAsync_MissingStartTime()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("ID", "9999");
        dictionary.Add("CODE", "9999");
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        workflowMock.Invoking(x => x.ProcessDataAsync(dictionary, new Mock<IDevice>().Object)).Should().Throw<ArgumentException>().WithMessage(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));
    }
    [Fact]
    public async Task ProcessDataAsync_success()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("ID", "9999");
        dictionary.Add("CODE", "9999");
        dictionary.Add("START_TIME", "9999");
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        await workflowMock.Invoking(async x => await x.ProcessDataAsync(dictionary, new Mock<IDevice>().Object)).Should().NotThrowAsync();
    }
    [Fact]
    public void BuildInstructionsAsync_MissingDeviceState()
    {
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        workflowMock.Invoking(x => x.BuildInstructionsAsync(new Mock<InstructionSet>().Object, new Mock<Dictionary<string, string>>().Object, new Mock<IDevice>().Object)).Should().Throw<InvalidOperationException>().WithMessage(DialogResources.MissingDeviceState);
    }
    [Fact]
    public void BuildInstructionsAsync_UnknownState()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("DLG", "{0}");
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        workflowMock.Invoking(x => x.BuildInstructionsAsync(new Mock<InstructionSet>().Object, dictionary, new Mock<IDevice>().Object)).Should().Throw<InvalidOperationException>().WithMessage(DialogResources.UnknownState);
    }
    [Fact]
    public async Task BuildInstructionsAsync_StartSuccess()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("DLG", Dialogs.Start.ToString());
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        await workflowMock.Invoking(async x => await x.BuildInstructionsAsync(new Mock<InstructionSet>().Object, dictionary, new Mock<IDevice>().Object)).Should().NotThrowAsync();
    }
    [Fact]
    public async Task BuildInstructionsAsync_DoChecklistSuccess()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("DLG", Dialogs.DoChecklist.ToString());
        dictionary.Add("ID", "99999");
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        await workflowMock.Invoking(async x => await x.BuildInstructionsAsync(new Mock<InstructionSet>().Object, dictionary, new Mock<IDevice>().Object)).Should().NotThrowAsync();
    }
    [Fact]
    public void BuildInstructionsAsync_DoChecklistMissingId()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("DLG", Dialogs.DoChecklist.ToString());
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        workflowMock.Invoking(x => x.BuildInstructionsAsync(new Mock<InstructionSet>().Object, dictionary, new Mock<IDevice>().Object)).Should().Throw<ArgumentException>().WithMessage(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));
    }
    [Fact]
    public async Task BuildInstructionsAsync_FinishedSuccess()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("DLG", Dialogs.Finished.ToString());
        dictionary.Add("ID", "99999");
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        await workflowMock.Invoking(async x => await x.BuildInstructionsAsync(new Mock<InstructionSet>().Object, dictionary, new Mock<IDevice>().Object)).Should().NotThrowAsync();
    }
    [Fact]
    public void BuildInstructionsAsync_FinishedMissingId()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("DLG", Dialogs.Finished.ToString());
        var workflowMock = new Workflow(new Mock<IConnectorBehavior>().Object);
        workflowMock.Invoking(x => x.BuildInstructionsAsync(new Mock<InstructionSet>().Object, dictionary, new Mock<IDevice>().Object)).Should().Throw<ArgumentException>().WithMessage(string.Format(DialogResources.MissingRequiredVariable, Vars.ID));
    }

    [Fact]
    public async Task BuildInstructionsAsync_Finished_Completed()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("DLG", Dialogs.Finished.ToString());
        dictionary.Add("ID", "99999");

        var deviceMock = new Mock<IDevice>();
        deviceMock.SetupGet(x => x.Operator).Returns("testOperator");

        var behaviorMock = new Mock<IConnectorBehavior>();
        behaviorMock.Setup(x => x.CompleteChecklist(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

        var instructionSet = new InstructionSet();

        var data = new Dictionary<string, string>
        {
            { Vars.DLG, Dialogs.Finished.ToString() },
            { Vars.ID, "testingId" }
        };

        var workflowMock = new Workflow(behaviorMock.Object);
        await workflowMock.BuildInstructionsAsync(instructionSet, data, deviceMock.Object);

        var res = instructionSet.PrepareResponse();

        res.Should().Be(@"0,setCommands
1,assignNum,""COMPLETE"",0
2,setSendHostFlag,""ID"",0
3,setSendHostFlag,""COMPLETE"",0
4,say,""Job finished. To continue, say VCONFIRM"",1,1
5,assignNum,""DLG"",0

");
    }

    [Fact]
    public async Task BuildInstructionsAsync_Finished_NotCompleted()
    {
        var dictionary = new Mock<Dictionary<string, string>>().Object;
        dictionary.Add("DLG", Dialogs.Finished.ToString());
        dictionary.Add("ID", "99999");

        var deviceMock = new Mock<IDevice>();
        deviceMock.SetupGet(x => x.Operator).Returns("testOperator");

        var behaviorMock = new Mock<IConnectorBehavior>();
        behaviorMock.Setup(x => x.CompleteChecklist(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(false);

        var instructionSet = new InstructionSet();

        var data = new Dictionary<string, string>
        {
            { Vars.DLG, Dialogs.Finished.ToString() },
            { Vars.ID, "testingId" }
        };

        var workflowMock = new Workflow(behaviorMock.Object);
        await workflowMock.BuildInstructionsAsync(instructionSet, data, deviceMock.Object);

        var res = instructionSet.PrepareResponse();

        res.Should().Be(@"0,setCommands,,,,LABEL_COMPLETE
1,say,""Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM"",1,1
2,assignNum,""DLG"",0
3,goTo,""LABEL_END""
4,label,""LABEL_COMPLETE""
5,setSendHostFlag,""COMPLETE"",1
6,label,""LABEL_END""

");
    }
}

