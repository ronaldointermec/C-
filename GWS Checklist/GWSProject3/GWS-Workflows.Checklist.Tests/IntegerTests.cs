using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class IntegerTests
{
    [Fact]
    public void Integer()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new IntegerValue { Prompt = "Integer number" };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getDigits,""ANSWER"",""Integer number"",,10,0,0,,""1"",""20"",,,0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Integer_MinValue()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new IntegerValue { Prompt = "Integer number", MinValue = 1 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getDigits,""ANSWER"",""Integer number"",,10,0,0,,""1"",""20"",""1"",,0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Integer_MaxValue()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new IntegerValue { Prompt = "Integer number", MaxValue = 3 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getDigits,""ANSWER"",""Integer number"",,10,0,0,,""1"",""20"",,""3"",0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Integer_MinLength()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new IntegerValue { Prompt = "Integer number", MinLength = 2 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getDigits,""ANSWER"",""Integer number"",,10,0,0,,""2"",""20"",,,0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Integer_MaxLength()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new IntegerValue { Prompt = "Integer number", MaxLength = 2 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getDigits,""ANSWER"",""Integer number"",,10,0,0,,""1"",""2"",,,0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Integer_WithScanner()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new IntegerValue { Prompt = "Integer number", ScannerEnabled = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getDigits,""ANSWER"",""Integer number"",,10,0,0,,""1"",""20"",,,1,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Integer_Image()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new IntegerValue { Prompt = "Integer number", Image = new Uri("https://picsum.photos/200") };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getDigits,""ANSWER"",""Integer number"",,10,0,0,,""1"",""20"",,,0,,,1,,,0,""https://picsum.photos/200""
2,say,""$ANSWER"",0,0

");
    }
}

