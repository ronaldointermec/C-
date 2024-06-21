using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules;
using Honeywell.GWS.Connector.SDK;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Tests;

public class GeneralTests
{
    [Fact]
    public void CheckAnswerNullability()
    {
        var ask = new Ask();
        Assert.Null(ask.Answer);

        var getDigits = new IntegerValue();
        Assert.Null(getDigits.Answer);

        var getFloat = new FloatValue();
        Assert.Null(getFloat.Answer);

        var getString = new StringValue();
        Assert.Null(getString.Answer);

        var getDate = new Date();
        Assert.Null(getDate.Answer);

        var getTime = new Time();
        Assert.Null(getTime.Answer);

        var getSelect = new Select();
        Assert.Null(getSelect.Answer);
    }

    private readonly Regex _dateFormatRegex = new(@"D{2}|M{2}|Y{4}|Y{2}", RegexOptions.Compiled);

    [Theory]
    [InlineData(3, "DDMMYYYY")]
    public void CheckDateFormatRegex(int matchesExpected, string format)
    {
        var res = _dateFormatRegex.Matches(format);

        Assert.Equal(matchesExpected, res.Count);
    }

    [Theory]
    [ClassData(typeof(DateAnswers))]
    public void ParseDateAnswer(DateTime dateExpected, string value, string format)
    {
        var method = typeof(ConnectorBehavior<ConnectorBehaviorSettingsBase>).GetMethod("ParseDateAnswer", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);

        var resObj = method!.Invoke(null, new[] { value, format });
        Assert.NotNull(resObj);

        var res = (DateTime)resObj!;

        Assert.Equal(dateExpected, res);
    }

    public class DateAnswers : TheoryData<DateTime, string, string>
    {
        public DateAnswers()
        {
            Add(new DateTime(2022, 10, 21), "21 10 2022", "ddMMyyyy");
            Add(new DateTime(2022, 10, 21), "21 10 22", "ddMMyy");
            Add(new DateTime(2022, 10, 21), "2022 10 21", "yyyyMMdd");
            Add(new DateTime(2022, 10, 21), "22 10 21", "yyMMdd");

            Add(new DateTime(2022, 10, 1), "2022 10", "yyyyMM");
            Add(new DateTime(2022, 10, 1), "22 10", "yyMM");
            Add(new DateTime(2022, 10, 1), "10 2022", "MMyyyy");
            Add(new DateTime(2022, 10, 1), "10 22", "MMyy");

            Add(new DateTime(DateTime.Now.Year, 10, 21), "10 21", "MMdd");
            Add(new DateTime(DateTime.Now.Year, 10, 21), "21 10", "ddMM");

            Add(new DateTime(2022, 1, 1), "2022", "yyyy");
            Add(new DateTime(2022, 1, 1), "22", "yy");

            Add(new DateTime(DateTime.Now.Year, 10, 1), "10", "MM");
            Add(new DateTime(DateTime.Now.Year, 1, 21), "21", "dd");
        }
    }

    [Theory]
    [ClassData(typeof(TimeAnswers))]
    public void ParseTimeAnswer(TimeSpan timeExpected, string value, string format)
    {
        var method = typeof(ConnectorBehavior<ConnectorBehaviorSettingsBase>).GetMethod("ParseTimeAnswer", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);

        var resObj = method!.Invoke(null, new[] { value, format });
        Assert.NotNull(resObj);

        var res = (TimeSpan)resObj!;

        Assert.Equal(timeExpected, res);
    }

    public class TimeAnswers : TheoryData<TimeSpan, string, string>
    {
        public TimeAnswers()
        {
            Add(new TimeSpan(13, 45, 50), "13 45 50", "hhmmss");
            Add(new TimeSpan(13, 45, 0), "13 45", "hhmm");
            Add(new TimeSpan(0, 45, 50), "45 50", "mmss");
            Add(new TimeSpan(13, 0, 0), "13", "hh");
            Add(new TimeSpan(0, 45, 0), "45", "mm");
            Add(new TimeSpan(0, 0, 50), "50", "ss");
        }
    }
}