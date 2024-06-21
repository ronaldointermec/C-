using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class AskTests
{
    [Fact]
    public void Ask()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var step = new Ask { Prompt = "This is a question", Priority = false };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,ask,""ANSWER"",""This is a question"",,0,0,""true"",""false""
2,doIf,""ANSWER"",""true"",""="",""STR"",,,,,,,,,,,1
3,say,""VYES"",0,0
4,doElse,,,,,,,,,,,,,,,1
5,say,""VNO"",0,0
6,doEndIf,,,,,,,,,,,,,,,1

");
    }

    [Fact]
    public void Ask_WithPriority()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var step = new Ask { Prompt = "This is a question", Priority = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,ask,""ANSWER"",""This is a question"",,1,0,""true"",""false""
2,doIf,""ANSWER"",""true"",""="",""STR"",,,,,,,,,,,1
3,say,""VYES"",0,0
4,doElse,,,,,,,,,,,,,,,1
5,say,""VNO"",0,0
6,doEndIf,,,,,,,,,,,,,,,1

");
    }

    [Fact]
    public void Ask_Image()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var step = new Ask { Prompt = "This is a question", Image = new Uri("https://picsum.photos/200") };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,ask,""ANSWER"",""This is a question"",,0,0,""true"",""false"",,""https://picsum.photos/200""
2,doIf,""ANSWER"",""true"",""="",""STR"",,,,,,,,,,,1
3,say,""VYES"",0,0
4,doElse,,,,,,,,,,,,,,,1
5,say,""VNO"",0,0
6,doEndIf,,,,,,,,,,,,,,,1

");
    }
}

