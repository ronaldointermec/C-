using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class FloatTests
{
    [Fact]
    public void Float()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new FloatValue { Prompt = "Float number" };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getFloat,""ANSWER"",""Float number"",,10,0,0,,""1"",""20"",,,0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Float_MinValue()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new FloatValue { Prompt = "Float number", MinValue = 1 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getFloat,""ANSWER"",""Float number"",,10,0,0,,""1"",""20"",""1"",,0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Float_MaxValue()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new FloatValue { Prompt = "Float number", MaxValue = 3 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getFloat,""ANSWER"",""Float number"",,10,0,0,,""1"",""20"",,""3"",0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Float_MinLength()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new FloatValue { Prompt = "Float number", MinLength = 2 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getFloat,""ANSWER"",""Float number"",,10,0,0,,""2"",""20"",,,0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Float_MaxLength()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new FloatValue { Prompt = "Float number", MaxLength = 2 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getFloat,""ANSWER"",""Float number"",,10,0,0,,""1"",""2"",,,0,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Float_WithScanner()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new FloatValue { Prompt = "Float number", ScannerEnabled = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getFloat,""ANSWER"",""Float number"",,10,0,0,,""1"",""20"",,,1,,,1,,,0
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void Float_Image()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new FloatValue { Prompt = "Float number", Image = new Uri("https://picsum.photos/200") };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getFloat,""ANSWER"",""Float number"",,10,0,0,,""1"",""20"",,,0,,,1,,,0,""https://picsum.photos/200""
2,say,""$ANSWER"",0,0

");
    }
}

