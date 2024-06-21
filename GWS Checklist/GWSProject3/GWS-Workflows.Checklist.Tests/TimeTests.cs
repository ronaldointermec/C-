using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class TimeTests
{
    [Fact]
    public void Time()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Time { Prompt = "Time test", Format = "hhmmss" };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,label,""LABEL_ASK_""
2,say,""Time test"",0,1
3,assignStr,""AUX""
4,assignStr,""HOUR""
5,assignStr,""MINUTE""
6,assignStr,""SECOND""
7,getDigits,""HOUR"",""Hour"",""Two digits"",10,0,0,,""2"",""2"",""0"",""23"",0,,,1,,,0
8,concat,""ANSWER"",""ANSWER"",""HOUR""
9,concat,""AUX"",""AUX"",""Hour""
10,concat,""AUX"",""AUX"",""HOUR""
11,concat,""AUX"",""AUX"","",""
12,getDigits,""MINUTE"",""Minute"",""Two digits"",10,0,0,,""2"",""2"",""0"",""59"",0,,,1,,,0
13,concat,""ANSWER"",""ANSWER"",""MINUTE""
14,concat,""AUX"",""AUX"",""Minute""
15,concat,""AUX"",""AUX"",""MINUTE""
16,concat,""AUX"",""AUX"","",""
17,getDigits,""SECOND"",""Second"",""Two digits"",10,0,0,,""2"",""2"",""0"",""59"",0,,,1,,,0
18,concat,""ANSWER"",""ANSWER"",""SECOND""
19,concat,""AUX"",""AUX"",""Second""
20,concat,""AUX"",""AUX"",""SECOND""
21,concat,""AUX"",""AUX"","",""
22,say,""$AUX"",0,0

");
    }

    [Fact]
    public void Time_Image()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Time { Prompt = "Time test", Format = "hhmmss", Image = new Uri("https://picsum.photos/200") };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,label,""LABEL_ASK_""
2,say,""Time test"",0,1
3,assignStr,""AUX""
4,assignStr,""HOUR""
5,assignStr,""MINUTE""
6,assignStr,""SECOND""
7,getDigits,""HOUR"",""Hour"",""Two digits"",10,0,0,,""2"",""2"",""0"",""23"",0,,,1,,,0,""https://picsum.photos/200""
8,concat,""ANSWER"",""ANSWER"",""HOUR""
9,concat,""AUX"",""AUX"",""Hour""
10,concat,""AUX"",""AUX"",""HOUR""
11,concat,""AUX"",""AUX"","",""
12,getDigits,""MINUTE"",""Minute"",""Two digits"",10,0,0,,""2"",""2"",""0"",""59"",0,,,1,,,0,""https://picsum.photos/200""
13,concat,""ANSWER"",""ANSWER"",""MINUTE""
14,concat,""AUX"",""AUX"",""Minute""
15,concat,""AUX"",""AUX"",""MINUTE""
16,concat,""AUX"",""AUX"","",""
17,getDigits,""SECOND"",""Second"",""Two digits"",10,0,0,,""2"",""2"",""0"",""59"",0,,,1,,,0,""https://picsum.photos/200""
18,concat,""ANSWER"",""ANSWER"",""SECOND""
19,concat,""AUX"",""AUX"",""Second""
20,concat,""AUX"",""AUX"",""SECOND""
21,concat,""AUX"",""AUX"","",""
22,say,""$AUX"",0,0

");
    }

    [Fact]
    public void Time_Confirmation()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Time { Prompt = "Time test", Format = "hhmmss", ConfirmationEnabled = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,label,""LABEL_ASK_""
2,say,""Time test"",0,1
3,assignStr,""AUX""
4,assignStr,""HOUR""
5,assignStr,""MINUTE""
6,assignStr,""SECOND""
7,getDigits,""HOUR"",""Hour"",""Two digits"",10,0,0,,""2"",""2"",""0"",""23"",0,,,1,,,0
8,concat,""ANSWER"",""ANSWER"",""HOUR""
9,concat,""AUX"",""AUX"",""Hour""
10,concat,""AUX"",""AUX"",""HOUR""
11,concat,""AUX"",""AUX"","",""
12,getDigits,""MINUTE"",""Minute"",""Two digits"",10,0,0,,""2"",""2"",""0"",""59"",0,,,1,,,0
13,concat,""ANSWER"",""ANSWER"",""MINUTE""
14,concat,""AUX"",""AUX"",""Minute""
15,concat,""AUX"",""AUX"",""MINUTE""
16,concat,""AUX"",""AUX"","",""
17,getDigits,""SECOND"",""Second"",""Two digits"",10,0,0,,""2"",""2"",""0"",""59"",0,,,1,,,0
18,concat,""ANSWER"",""ANSWER"",""SECOND""
19,concat,""AUX"",""AUX"",""Second""
20,concat,""AUX"",""AUX"",""SECOND""
21,concat,""AUX"",""AUX"","",""
22,concat,""AUX"",""AUX"",""?""
23,ask,""DUMMY"",""$AUX"",,0,0,""1"",""0""
24,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,1
25,assignStr,""ANSWER""
26,goTo,""LABEL_ASK_""
27,doEndIf,,,,,,,,,,,,,,,1

");
    }
}

