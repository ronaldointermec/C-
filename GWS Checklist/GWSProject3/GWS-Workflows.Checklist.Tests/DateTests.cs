using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class DateTests
{
    [Fact]
    public void Date()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Date { Prompt = "Date with day, month and year", Format = "ddMMyy" };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,label,""LABEL_ASK_""
2,say,""Date with day, month and year"",0,1
3,assignStr,""AUX""
4,assignStr,""YEAR""
5,assignStr,""MONTH""
6,assignStr,""DAY""
7,getDigits,""DAY"",""Day"",""Two digits"",10,0,0,,""2"",""2"",""1"",""31"",0,,,1,,,0
8,concat,""ANSWER"",""ANSWER"",""DAY""
9,concat,""AUX"",""AUX"",""Day""
10,concat,""AUX"",""AUX"",""DAY""
11,concat,""AUX"",""AUX"","",""
12,getDigits,""MONTH"",""Month"",""Two digits"",10,0,0,,""2"",""2"",""1"",""12"",0,,,1,,,0
13,concat,""ANSWER"",""ANSWER"",""MONTH""
14,concat,""AUX"",""AUX"",""Month""
15,concat,""AUX"",""AUX"",""MONTH""
16,concat,""AUX"",""AUX"","",""
17,getDigits,""YEAR"",""Year"",""Two digits"",10,0,0,,""2"",""2"",,,0,,,1,,,0
18,concat,""ANSWER"",""ANSWER"",""YEAR""
19,concat,""AUX"",""AUX"",""Year""
20,concat,""AUX"",""AUX"",""YEAR""
21,concat,""AUX"",""AUX"","",""
22,doIf,""MONTH"",""2"",""="",""NUM"",,,,,,,,,,,1
23,doIf,""DAY"",""28"","">"",""NUM"",,,,,,,,,,,2
24,goTo,""LABEL_WRONG_""
25,doEndIf,,,,,,,,,,,,,,,2
26,doEndIf,,,,,,,,,,,,,,,1
27,doIf,""MONTH"",""4"",""="",""NUM"",,,,,,,,,,,3
28,doIf,""DAY"",""30"","">"",""NUM"",,,,,,,,,,,4
29,goTo,""LABEL_WRONG_""
30,doEndIf,,,,,,,,,,,,,,,4
31,doEndIf,,,,,,,,,,,,,,,3
32,doIf,""MONTH"",""6"",""="",""NUM"",,,,,,,,,,,5
33,doIf,""DAY"",""30"","">"",""NUM"",,,,,,,,,,,6
34,goTo,""LABEL_WRONG_""
35,doEndIf,,,,,,,,,,,,,,,6
36,doEndIf,,,,,,,,,,,,,,,5
37,doIf,""MONTH"",""9"",""="",""NUM"",,,,,,,,,,,7
38,doIf,""DAY"",""30"","">"",""NUM"",,,,,,,,,,,8
39,goTo,""LABEL_WRONG_""
40,doEndIf,,,,,,,,,,,,,,,8
41,doEndIf,,,,,,,,,,,,,,,7
42,doIf,""MONTH"",""11"",""="",""NUM"",,,,,,,,,,,9
43,doIf,""DAY"",""30"","">"",""NUM"",,,,,,,,,,,10
44,goTo,""LABEL_WRONG_""
45,doEndIf,,,,,,,,,,,,,,,10
46,doEndIf,,,,,,,,,,,,,,,9
47,goTo,""LABEL_OK_""
48,label,""LABEL_WRONG_""
49,say,""Invalid date"",0,1
50,goTo,""LABEL_ASK_""
51,label,""LABEL_OK_""
52,say,""$AUX"",0,0

");
    }

    [Fact]
    public void Date_LongYear()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Date { Prompt = "Date with day, month and year", Format = "ddMMyyyy" };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,label,""LABEL_ASK_""
2,say,""Date with day, month and year"",0,1
3,assignStr,""AUX""
4,assignStr,""YEAR""
5,assignStr,""MONTH""
6,assignStr,""DAY""
7,getDigits,""DAY"",""Day"",""Two digits"",10,0,0,,""2"",""2"",""1"",""31"",0,,,1,,,0
8,concat,""ANSWER"",""ANSWER"",""DAY""
9,concat,""AUX"",""AUX"",""Day""
10,concat,""AUX"",""AUX"",""DAY""
11,concat,""AUX"",""AUX"","",""
12,getDigits,""MONTH"",""Month"",""Two digits"",10,0,0,,""2"",""2"",""1"",""12"",0,,,1,,,0
13,concat,""ANSWER"",""ANSWER"",""MONTH""
14,concat,""AUX"",""AUX"",""Month""
15,concat,""AUX"",""AUX"",""MONTH""
16,concat,""AUX"",""AUX"","",""
17,getDigits,""YEAR"",""Year"",""Four digits"",10,0,0,,""4"",""4"",""1900"",""2100"",0,,,1,,,0
18,concat,""ANSWER"",""ANSWER"",""YEAR""
19,concat,""AUX"",""AUX"",""Year""
20,concat,""AUX"",""AUX"",""YEAR""
21,concat,""AUX"",""AUX"","",""
22,doIf,""MONTH"",""2"",""="",""NUM"",,,,,,,,,,,1
23,doIf,""DAY"",""28"","">"",""NUM"",,,,,,,,,,,2
24,goTo,""LABEL_WRONG_""
25,doEndIf,,,,,,,,,,,,,,,2
26,doEndIf,,,,,,,,,,,,,,,1
27,doIf,""MONTH"",""4"",""="",""NUM"",,,,,,,,,,,3
28,doIf,""DAY"",""30"","">"",""NUM"",,,,,,,,,,,4
29,goTo,""LABEL_WRONG_""
30,doEndIf,,,,,,,,,,,,,,,4
31,doEndIf,,,,,,,,,,,,,,,3
32,doIf,""MONTH"",""6"",""="",""NUM"",,,,,,,,,,,5
33,doIf,""DAY"",""30"","">"",""NUM"",,,,,,,,,,,6
34,goTo,""LABEL_WRONG_""
35,doEndIf,,,,,,,,,,,,,,,6
36,doEndIf,,,,,,,,,,,,,,,5
37,doIf,""MONTH"",""9"",""="",""NUM"",,,,,,,,,,,7
38,doIf,""DAY"",""30"","">"",""NUM"",,,,,,,,,,,8
39,goTo,""LABEL_WRONG_""
40,doEndIf,,,,,,,,,,,,,,,8
41,doEndIf,,,,,,,,,,,,,,,7
42,doIf,""MONTH"",""11"",""="",""NUM"",,,,,,,,,,,9
43,doIf,""DAY"",""30"","">"",""NUM"",,,,,,,,,,,10
44,goTo,""LABEL_WRONG_""
45,doEndIf,,,,,,,,,,,,,,,10
46,doEndIf,,,,,,,,,,,,,,,9
47,goTo,""LABEL_OK_""
48,label,""LABEL_WRONG_""
49,say,""Invalid date"",0,1
50,goTo,""LABEL_ASK_""
51,label,""LABEL_OK_""
52,say,""$AUX"",0,0

");
    }

    [Fact]
    public void Date_Confirmation()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Date { Prompt = "Date with day, month and year", Format = "ddMMYY", ConfirmationEnabled = true };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,label,""LABEL_ASK_""
2,say,""Date with day, month and year"",0,1
3,assignStr,""AUX""
4,assignStr,""YEAR""
5,assignStr,""MONTH""
6,assignStr,""DAY""
7,getDigits,""DAY"",""Day"",""Two digits"",10,0,0,,""2"",""2"",""1"",""31"",0,,,1,,,0
8,concat,""ANSWER"",""ANSWER"",""DAY""
9,concat,""AUX"",""AUX"",""Day""
10,concat,""AUX"",""AUX"",""DAY""
11,concat,""AUX"",""AUX"","",""
12,getDigits,""MONTH"",""Month"",""Two digits"",10,0,0,,""2"",""2"",""1"",""12"",0,,,1,,,0
13,concat,""ANSWER"",""ANSWER"",""MONTH""
14,concat,""AUX"",""AUX"",""Month""
15,concat,""AUX"",""AUX"",""MONTH""
16,concat,""AUX"",""AUX"","",""
17,goTo,""LABEL_OK_""
18,label,""LABEL_WRONG_""
19,say,""Invalid date"",0,1
20,goTo,""LABEL_ASK_""
21,label,""LABEL_OK_""
22,concat,""AUX"",""AUX"",""?""
23,ask,""DUMMY"",""$AUX"",,0,0,""1"",""0""
24,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,1
25,assignStr,""ANSWER""
26,goTo,""LABEL_ASK_""
27,doEndIf,,,,,,,,,,,,,,,1

");
    }

    [Fact]
    public void Date_Image()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var step = new Date { Prompt = "Date with day, month and year", Format = "ddMMYY", Image = new Uri("https://picsum.photos/200") };
        var instr = new InstructionSet();


        step.BuildDialog(new Mock<IDevice>().Object, instr, "0", string.Empty);

        var res = instr.PrepareResponse();

        res.Should().Be(
            @"0,setCommands,LABEL_LEAVEJOB
1,label,""LABEL_ASK_""
2,say,""Date with day, month and year"",0,1
3,assignStr,""AUX""
4,assignStr,""YEAR""
5,assignStr,""MONTH""
6,assignStr,""DAY""
7,getDigits,""DAY"",""Day"",""Two digits"",10,0,0,,""2"",""2"",""1"",""31"",0,,,1,,,0,""https://picsum.photos/200""
8,concat,""ANSWER"",""ANSWER"",""DAY""
9,concat,""AUX"",""AUX"",""Day""
10,concat,""AUX"",""AUX"",""DAY""
11,concat,""AUX"",""AUX"","",""
12,getDigits,""MONTH"",""Month"",""Two digits"",10,0,0,,""2"",""2"",""1"",""12"",0,,,1,,,0,""https://picsum.photos/200""
13,concat,""ANSWER"",""ANSWER"",""MONTH""
14,concat,""AUX"",""AUX"",""Month""
15,concat,""AUX"",""AUX"",""MONTH""
16,concat,""AUX"",""AUX"","",""
17,goTo,""LABEL_OK_""
18,label,""LABEL_WRONG_""
19,say,""Invalid date"",0,1
20,goTo,""LABEL_ASK_""
21,label,""LABEL_OK_""
22,say,""$AUX"",0,0

");
    }
}