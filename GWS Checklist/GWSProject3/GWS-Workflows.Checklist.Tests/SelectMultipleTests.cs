using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class SelectMultipleTests
{
    [Fact]
    public void SelectMultiple()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new SelectMultiple { Prompt = "This is a select", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB,,,LABEL_COMPLETE_
1,assignStr,""PROMPT"",""This is a select""
2,assignStr,""AUX""
3,label,""LABEL_ASK_""
4,getMenu,""DUMMY"",""$PROMPT"",,,0,,0,""Wrong"",1
5,doIf,""DUMMY"",""1"",""="",""NUM"",,,,,,,,,,,1
6,say,""Option 1"",0,1
7,concat,""AUX"",""AUX"","", Option 1""
8,doElseIf,""DUMMY"",""2"",""="",""NUM"",,,,,,,,,,,1
9,say,""Option 2"",0,1
10,concat,""AUX"",""AUX"","", Option 2""
11,doElseIf,""DUMMY"",""3"",""="",""NUM"",,,,,,,,,,,1
12,say,""Option 3"",0,1
13,concat,""AUX"",""AUX"","", Option 3""
14,doEndIf,,,,,,,,,,,,,,,1
15,concat,""ANSWER"",""ANSWER"",""DUMMY""
16,assignStr,""PROMPT"",""Next""
17,goTo,""LABEL_ASK_""
18,label,""LABEL_COMPLETE_""
19,say,""$AUX"",0,0

");
    }

    [Fact]
    public void SelectMultiple_Confirmation()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new SelectMultiple { Prompt = "This is a select", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") }, ConfirmationEnabled = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB,,,LABEL_COMPLETE_
1,assignStr,""PROMPT"",""This is a select""
2,assignStr,""AUX""
3,label,""LABEL_ASK_""
4,getMenu,""DUMMY"",""$PROMPT"",,,0,,0,""Wrong"",1
5,doIf,""DUMMY"",""1"",""="",""NUM"",,,,,,,,,,,1
6,say,""Option 1"",0,1
7,concat,""AUX"",""AUX"","", Option 1""
8,doElseIf,""DUMMY"",""2"",""="",""NUM"",,,,,,,,,,,1
9,say,""Option 2"",0,1
10,concat,""AUX"",""AUX"","", Option 2""
11,doElseIf,""DUMMY"",""3"",""="",""NUM"",,,,,,,,,,,1
12,say,""Option 3"",0,1
13,concat,""AUX"",""AUX"","", Option 3""
14,doEndIf,,,,,,,,,,,,,,,1
15,concat,""ANSWER"",""ANSWER"",""DUMMY""
16,assignStr,""PROMPT"",""Next""
17,goTo,""LABEL_ASK_""
18,label,""LABEL_COMPLETE_""
19,concat,""AUX"",""AUX"",""?""
20,ask,""DUMMY"",""$AUX"",,0,0,""1"",""0""
21,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,2
22,assignStr,""PROMPT"",""This is a select""
23,assignStr,""ANSWER""
24,assignStr,""AUX""
25,goTo,""LABEL_ASK_""
26,doEndIf,,,,,,,,,,,,,,,2

");
    }
}

