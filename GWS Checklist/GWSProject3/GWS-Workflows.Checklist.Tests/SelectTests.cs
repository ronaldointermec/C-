using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class SelectTests
{
    [Fact]
    public void Select()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Select { Prompt = "This is a select", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getMenu,""ANSWER"",""This is a select"",,,0,,0,""Wrong"",1
2,doIf,""ANSWER"",""1"",""="",""NUM"",,,,,,,,,,,1
3,say,""Option 1"",0,1
4,doElseIf,""ANSWER"",""2"",""="",""NUM"",,,,,,,,,,,1
5,say,""Option 2"",0,1
6,doElseIf,""ANSWER"",""3"",""="",""NUM"",,,,,,,,,,,1
7,say,""Option 3"",0,1
8,doEndIf,,,,,,,,,,,,,,,1

");
    }

    [Fact]
    public void Select_Confirmation()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Select { Prompt = "This is a select", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") }, ConfirmationEnabled = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getMenu,""ANSWER"",""This is a select"",,,1,""?"",0,""Wrong"",1

");
    }
}

