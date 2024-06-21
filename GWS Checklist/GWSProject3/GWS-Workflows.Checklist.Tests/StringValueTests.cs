using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class StringValueTests
{
    [Fact]
    public void StringValue()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var step = new StringValue { Prompt = "String Value" };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getString,""ANSWER"",""String Value"",,10,0,0,,""1"",""20"",0,,,1
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void StringValue_Confirmation()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var step = new StringValue { Prompt = "String Value", ConfirmationEnabled = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getString,""ANSWER"",""String Value"",,10,0,1,""?"",""1"",""20"",0,,,1

");
    }

    [Fact]
    public void StringValue_MinLength()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var step = new StringValue { Prompt = "String Value", MinLength = 2 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getString,""ANSWER"",""String Value"",,10,0,0,,""2"",""20"",0,,,1
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void StringValue_MaxLength()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var step = new StringValue { Prompt = "String Value", MaxLength = 3 };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getString,""ANSWER"",""String Value"",,10,0,0,,""1"",""3"",0,,,1
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void StringValue_WithScanner()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var step = new StringValue { Prompt = "String Value", ScannerEnabled = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getString,""ANSWER"",""String Value"",,10,0,0,,""1"",""20"",1,,,1
2,say,""$ANSWER"",0,0

");
    }

    [Fact]
    public void StringValue_Image()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var step = new StringValue { Prompt = "String Value", Image = new Uri("https://picsum.photos/200") };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,getString,""ANSWER"",""String Value"",,10,0,0,,""1"",""20"",0,,,1,,,""https://picsum.photos/200""
2,say,""$ANSWER"",0,0

");
    }
}

