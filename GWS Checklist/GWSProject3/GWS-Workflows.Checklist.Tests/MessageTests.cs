using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class MessageTests
{
    [Fact]
    public void Message()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Message { Prompt = "This is a message", Priority = false };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,say,""This is a message"",0,0

");
    }

    [Fact]
    public void Message_Priority()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Message { Prompt = "This is a message", Priority = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,say,""This is a message"",0,1

");
    }

    [Fact]
    public void Message_Image()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Message { Prompt = "This is a message", Image = new Uri("https://picsum.photos/200") };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,say,""This is a message"",0,0,,""https://picsum.photos/200""

");
    }
}

