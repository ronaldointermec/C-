using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Files.Code;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Files.Tests;

public class YamlSerializationTests
{
    readonly YamlParser parser = new();

    [Fact]
    public void Deserialize()
    {
        var yaml = @"questions:
- !!message
  code: 001
  prompt: Start checklist questions
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
  image: https://picsum.photos/200
  operator: 
  startTime: 
  endTime: 
- !!ask
  code: 002
  prompt: Question without priority
  additionalInformation: Additional information
  skipAllowed: false
  priority: true
  image: https://picsum.photos/200
  operator: Operator 1
  answer: true
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!number
  code: '003'
  prompt: Indicate number with confirmation
  additionalInformation:
  skipAllowed: false
  confirmationEnabled: true
  scannerEnabled: false
  image: https://picsum.photos/200
- !!decimal
  code: '004'
  prompt: Indicate decimal number with range of 2 to 3 digits
  additionalInformation:
  skipAllowed: false
  confirmationEnabled: false
  minLength: 2
  maxLength: 3
  scannerEnabled: false
  image: https://picsum.photos/200
- !!string
  code: '005'
  prompt: Indicate alphanumeric that can be omitted
  additionalInformation:
  skipAllowed: true
  confirmationEnabled: false
  scannerEnabled: false
  image: https://picsum.photos/200
- !!date
  code: '006'
  prompt: Indicate date with day, month and long year, with confirmation
  additionalInformation:
  skipAllowed: false
  format: ddMMyyyy
  confirmationEnabled: true
  image: https://picsum.photos/200
- !!time
  code: '007'
  prompt: Indicate time with hour, minute and second, with confirmation
  additionalInformation:
  skipAllowed: false
  format: hhmmss
  confirmationEnabled: true
  image: https://picsum.photos/200
- !!choice
  code: '008'
  prompt: Indicate option with confirmation
  additionalInformation:
  skipAllowed: false
  confirmationEnabled: true
  options:
  - code: 1
    description: Option 1
  - code: 2
    description: Option 2
  - code: 3
    description: Option 3
- !!multiple_choice
  code: '009'
  prompt: Indicate multiple choice with confirmation
  additionalInformation:
  skipAllowed: false
  confirmationEnabled: true
  options:
  - code: 1
    description: Option 1
  - code: 2
    description: Option 2
  - code: 3
    description: Option 3
";

        var checklist = parser.Parse<Models.Checklist>(yaml);

        Assert.NotNull(checklist);
        Assert.Collection(checklist.Questions,
            q1 => { var m = Assert.IsType<Message>(q1); m.Image.Should().NotBeNull(); },
            q2 => { var a = Assert.IsType<Ask>(q2); a.Image.Should().NotBeNull(); },
            q3 => { var i = Assert.IsType<IntegerValue>(q3); i.Image.Should().NotBeNull(); },
            q4 => { var f = Assert.IsType<FloatValue>(q4); f.Image.Should().NotBeNull(); },
            q5 => { var s = Assert.IsType<StringValue>(q5); s.Image.Should().NotBeNull(); },
            q6 => { var d = Assert.IsType<Date>(q6); d.Image.Should().NotBeNull(); },
            q7 => { var t = Assert.IsType<Time>(q7); t.Image.Should().NotBeNull(); },
            q8 => { Assert.IsType<Select>(q8); },
            q9 => { Assert.IsType<SelectMultiple>(q9); }
            );
    }

    [Fact]
    public void Serialize_Message()
    {
        var checklist = new Models.Checklist
        {
            Questions = new[]
            {
                new Message { Code = "001", Prompt = "Start checklist messages"},
                new Message { Code = "002", Prompt = "Message priority", Priority = true, StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operator 1"},
                new Message { Code = "003", Prompt = "Message with confirmation", ReadyToContinue = true},
                new Message { Code = "004", Prompt = "Message with additional information", ReadyToContinue = true, AdditionalInformation = "Additional information"},
                new Message { Code = "005", Prompt = "Message that can be omitted", ReadyToContinue = true, SkipAllowed = true, Image = new Uri("https://picsum.photos/200")},
            }
        };

        var res = parser.Serialize(checklist);

        Assert.NotNull(res);
        Assert.Equal(@"questions:
- !!message
  code: 001
  prompt: Start checklist messages
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
- !!message
  code: 002
  prompt: Message priority
  additionalInformation: 
  skipAllowed: false
  priority: true
  readyToContinue: false
  operator: Operator 1
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!message
  code: 003
  prompt: Message with confirmation
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: true
- !!message
  code: 004
  prompt: Message with additional information
  additionalInformation: Additional information
  skipAllowed: false
  priority: false
  readyToContinue: true
- !!message
  code: 005
  prompt: Message that can be omitted
  additionalInformation: 
  skipAllowed: true
  priority: false
  readyToContinue: true
  image: https://picsum.photos/200
", res);
    }

    [Fact]
    public void Serialize_Ask()
    {
        var checklist = new Models.Checklist()
        {
            Questions = new IQuestion[]
            {
                new Message { Code = "001", Prompt = "Start checklist questions"},
                new Ask { Code = "002", Prompt = "Question without priority", AdditionalInformation = "Additional information", Priority = true, Answer = true, StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operator 1"},
                new Ask { Code = "003", Prompt = "Question with priority" },
                new Ask { Code = "004", Prompt = "Question that can be omitted", SkipAllowed = true, Image = new Uri("https://picsum.photos/200")},
            }
        };

        var res = parser.Serialize(checklist);

        Assert.NotNull(res);
        Assert.Equal(@"questions:
- !!message
  code: 001
  prompt: Start checklist questions
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
- !!ask
  code: 002
  prompt: Question without priority
  additionalInformation: Additional information
  skipAllowed: false
  priority: true
  operator: Operator 1
  answer: true
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!ask
  code: 003
  prompt: Question with priority
  additionalInformation: 
  skipAllowed: false
  priority: false
- !!ask
  code: 004
  prompt: Question that can be omitted
  additionalInformation: 
  skipAllowed: true
  priority: false
  image: https://picsum.photos/200
", res);
    }

    [Fact]
    public void Serialize_Number()
    {
        var checklist = new Models.Checklist()
        {
            Questions = new IQuestion[]
            {
                new Message { Code = "001", Prompt = "Start checklist integers"},
                new IntegerValue { Code = "002", Prompt = "Indicate number" },
                new IntegerValue { Code = "003", Prompt = "Indicate number with scanner", ScannerEnabled = true, Answer = 15, StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operator 1"},
                new IntegerValue { Code = "004", Prompt = "Indicate number with confirmation", ConfirmationEnabled = true },
                new IntegerValue { Code = "005", Prompt = "Indicate number with range of 2 to 3 digits", MinLength = 2, MaxLength= 3},
                new IntegerValue { Code = "006", Prompt = "Indicate number with a minimum of 10 to 30", MinValue = 10, MaxValue= 30},
                new IntegerValue { Code = "007", Prompt = "Indicate number that can be omitted", SkipAllowed = true, Image = new Uri("https://picsum.photos/200")},
            }
        };

        var res = parser.Serialize(checklist);

        Assert.NotNull(res);
        Assert.Equal(@"questions:
- !!message
  code: 001
  prompt: Start checklist integers
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
- !!number
  code: 002
  prompt: Indicate number
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  scannerEnabled: false
- !!number
  code: 003
  prompt: Indicate number with scanner
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  scannerEnabled: true
  operator: Operator 1
  answer: 15
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!number
  code: 004
  prompt: Indicate number with confirmation
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: true
  scannerEnabled: false
- !!number
  code: 005
  prompt: Indicate number with range of 2 to 3 digits
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  minLength: 2
  maxLength: 3
  scannerEnabled: false
- !!number
  code: 006
  prompt: Indicate number with a minimum of 10 to 30
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  minValue: 10
  maxValue: 30
  scannerEnabled: false
- !!number
  code: 007
  prompt: Indicate number that can be omitted
  additionalInformation: 
  skipAllowed: true
  confirmationEnabled: false
  scannerEnabled: false
  image: https://picsum.photos/200
", res);
    }

    [Fact]
    public void Serialize_Decimal()
    {
        var checklist = new Models.Checklist()
        {
            Questions = new IQuestion[]
            {
                new Message { Code = "001", Prompt = "Start checklist decimal numbers"},
                new FloatValue { Code = "002", Prompt = "Indicate decimal number" },
                new FloatValue { Code = "003", Prompt = "Indicate decimal number with scanner", ScannerEnabled = true, Answer = 15.34f, StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operator 1"},
                new FloatValue { Code = "004", Prompt = "Indicate decimal number with confirmation", ConfirmationEnabled = true },
                new FloatValue { Code = "005", Prompt = "Indicate decimal number with range of 2 to 3 digits", MinLength = 2, MaxLength= 3},
                new FloatValue { Code = "006", Prompt = "Indicate decimal number with minimum of 10 to 30", MinValue = 10, MaxValue= 30},
                new FloatValue { Code = "007", Prompt = "Indicate decimal number that can be omitted", SkipAllowed = true, Image = new Uri("https://picsum.photos/200")},
            }
        };

        var res = parser.Serialize(checklist);

        Assert.NotNull(res);
        Assert.Equal(@"questions:
- !!message
  code: 001
  prompt: Start checklist decimal numbers
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
- !!decimal
  code: 002
  prompt: Indicate decimal number
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  scannerEnabled: false
- !!decimal
  code: 003
  prompt: Indicate decimal number with scanner
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  scannerEnabled: true
  operator: Operator 1
  answer: 15.34
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!decimal
  code: 004
  prompt: Indicate decimal number with confirmation
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: true
  scannerEnabled: false
- !!decimal
  code: 005
  prompt: Indicate decimal number with range of 2 to 3 digits
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  minLength: 2
  maxLength: 3
  scannerEnabled: false
- !!decimal
  code: 006
  prompt: Indicate decimal number with minimum of 10 to 30
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  minValue: 10
  maxValue: 30
  scannerEnabled: false
- !!decimal
  code: 007
  prompt: Indicate decimal number that can be omitted
  additionalInformation: 
  skipAllowed: true
  confirmationEnabled: false
  scannerEnabled: false
  image: https://picsum.photos/200
", res);
    }

    [Fact]
    public void Serialize_String()
    {
        var checklist = new Models.Checklist()
        {
            Questions = new IQuestion[]
            {
                new Message { Code = "001", Prompt = "Start alphanumeric values checklist"},
                new StringValue { Code = "002", Prompt = "Indicate alphanumeric" },
                new StringValue { Code = "003", Prompt = "Indicate alphanumeric with scanner", ScannerEnabled = true, Answer = "A1B2C3", StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operator 1"},
                new StringValue { Code = "004", Prompt = "Indicate alphanumeric with confirmation", ConfirmationEnabled = true },
                new StringValue { Code = "005", Prompt = "Indicate alphanumeric with range of 2 to 3 digits", MinLength = 2, MaxLength= 3},
                new StringValue { Code = "006", Prompt = "Indicate alphanumeric that can be omitted", SkipAllowed = true, Image = new Uri("https://picsum.photos/200")},
            }
        };

        var res = parser.Serialize(checklist);

        Assert.NotNull(res);
        Assert.Equal(@"questions:
- !!message
  code: 001
  prompt: Start alphanumeric values checklist
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
- !!string
  code: 002
  prompt: Indicate alphanumeric
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  scannerEnabled: false
- !!string
  answer: A1B2C3
  code: 003
  prompt: Indicate alphanumeric with scanner
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  scannerEnabled: true
  operator: Operator 1
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!string
  code: 004
  prompt: Indicate alphanumeric with confirmation
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: true
  scannerEnabled: false
- !!string
  code: 005
  prompt: Indicate alphanumeric with range of 2 to 3 digits
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  minLength: 2
  maxLength: 3
  scannerEnabled: false
- !!string
  code: 006
  prompt: Indicate alphanumeric that can be omitted
  additionalInformation: 
  skipAllowed: true
  confirmationEnabled: false
  scannerEnabled: false
  image: https://picsum.photos/200
", res);
    }

    [Fact]
    public void Serialize_Date()
    {
        var checklist = new Models.Checklist()
        {
            Questions = new IQuestion[]
            {
                new Message { Code = "001", Prompt = "Start checklist dates"},
                new Date { Code = "002", Prompt = "Indicate date with day, month and long year, with confirmation", Format = "ddMMyyyy", ConfirmationEnabled = true },
                new Date { Code = "003", Prompt = "Indicate date with day, month and short year", Format = "ddMMyy", Answer = new DateTime(2022, 10, 15), StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operator 1"},
                new Date { Code = "004", Prompt = "Indicate date with long year, month and day", Format = "yyyyMMdd" },
                new Date { Code = "005", Prompt = "Indicate date with short year, month and day", Format = "yyMMdd" },
                new Date { Code = "006", Prompt = "Indicate date with long year and month", Format = "yyyyMM" },
                new Date { Code = "007", Prompt = "Indicate date with short year and month", Format = "yyMM" },
                new Date { Code = "008", Prompt = "Indicate date with month and long year", Format = "MMyyyy" },
                new Date { Code = "009", Prompt = "Indicate date with month and short year", Format = "MMyy" },
                new Date { Code = "010", Prompt = "Indicate date with month and day", Format = "MMdd" },
                new Date { Code = "011", Prompt = "Indicate date with day and month", Format = "ddMM" },
                new Date { Code = "012", Prompt = "Indicate date with long year", Format = "yyyy" },
                new Date { Code = "013", Prompt = "Indicate date with short year", Format = "yy" },
                new Date { Code = "014", Prompt = "Indicate date with month", Format = "MM" },
                new Date { Code = "015", Prompt = "Indicate date with day", Format = "dd" },
                new Date { Code = "016", Prompt = "Indicate date that can be omitted", Format = "ddMMyyyy", SkipAllowed = true, Image = new Uri("https://picsum.photos/200") },
            }
        };

        var res = parser.Serialize(checklist);

        Assert.NotNull(res);
        Assert.Equal(@"questions:
- !!message
  code: 001
  prompt: Start checklist dates
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
- !!date
  code: 002
  prompt: Indicate date with day, month and long year, with confirmation
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: true
  format: ddMMyyyy
- !!date
  code: 003
  prompt: Indicate date with day, month and short year
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: ddMMyy
  operator: Operator 1
  answer: 2022-10-15T00:00:00.0000000
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!date
  code: 004
  prompt: Indicate date with long year, month and day
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: yyyyMMdd
- !!date
  code: 005
  prompt: Indicate date with short year, month and day
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: yyMMdd
- !!date
  code: 006
  prompt: Indicate date with long year and month
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: yyyyMM
- !!date
  code: 007
  prompt: Indicate date with short year and month
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: yyMM
- !!date
  code: 008
  prompt: Indicate date with month and long year
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: MMyyyy
- !!date
  code: 009
  prompt: Indicate date with month and short year
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: MMyy
- !!date
  code: 010
  prompt: Indicate date with month and day
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: MMdd
- !!date
  code: 011
  prompt: Indicate date with day and month
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: ddMM
- !!date
  code: 012
  prompt: Indicate date with long year
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: yyyy
- !!date
  code: 013
  prompt: Indicate date with short year
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: yy
- !!date
  code: 014
  prompt: Indicate date with month
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: MM
- !!date
  code: 015
  prompt: Indicate date with day
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: dd
- !!date
  code: 016
  prompt: Indicate date that can be omitted
  additionalInformation: 
  skipAllowed: true
  confirmationEnabled: false
  format: ddMMyyyy
  image: https://picsum.photos/200
", res);
    }

    [Fact]
    public void Serialize_Time()
    {
        var checklist = new Models.Checklist()
        {
            Questions = new IQuestion[]
            {
                new Message { Code = "001", Prompt = "Start checklist time"},
                new Time { Code = "002", Prompt = "Indicate time with hour, minute and second, with confirmation", Format = "hhmmss", ConfirmationEnabled = true },
                new Time { Code = "003", Prompt = "Indicate time with hour and minute", Format = "hhmm", Answer=new TimeSpan(15, 34, 0), StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operator 1"},
                new Time { Code = "004", Prompt = "Indicate time with minute and second", Format = "mmss" },
                new Time { Code = "005", Prompt = "Indicate time with hour", Format = "hh" },
                new Time { Code = "006", Prompt = "Indicate time with minute", Format = "mm" },
                new Time { Code = "007", Prompt = "Indicate time with second", Format = "ss" },
                new Time { Code = "008", Prompt = "Indicate time that can be omitted", Format = "hhmmss", SkipAllowed = true, Image = new Uri("https://picsum.photos/200")},
            }
        };

        var res = parser.Serialize(checklist);

        Assert.NotNull(res);
        Assert.Equal(@"questions:
- !!message
  code: 001
  prompt: Start checklist time
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
- !!time
  code: 002
  prompt: Indicate time with hour, minute and second, with confirmation
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: true
  format: hhmmss
- !!time
  code: 003
  prompt: Indicate time with hour and minute
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: hhmm
  operator: Operator 1
  answer: 15:34:00
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!time
  code: 004
  prompt: Indicate time with minute and second
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: mmss
- !!time
  code: 005
  prompt: Indicate time with hour
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: hh
- !!time
  code: 006
  prompt: Indicate time with minute
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: mm
- !!time
  code: 007
  prompt: Indicate time with second
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  format: ss
- !!time
  code: 008
  prompt: Indicate time that can be omitted
  additionalInformation: 
  skipAllowed: true
  confirmationEnabled: false
  format: hhmmss
  image: https://picsum.photos/200
", res);
    }

    [Fact]
    public void Serialize_Choice()
    {
        var checklist = new Models.Checklist()
        {
            Questions = new IQuestion[]
            {
                new Message { Code = "001", Prompt = "Start checklist selection"},
                new Select { Code = "002", Prompt = "Indicate option", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") }, Answer= 2, StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operator 1"},
                new Select { Code = "003", Prompt = "Indicate option with confirmation", ConfirmationEnabled= true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                new Select { Code = "004", Prompt = "Indicate option that can be omitted", SkipAllowed = true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
            }
        };

        var res = parser.Serialize(checklist);

        Assert.NotNull(res);
        Assert.Equal(@"questions:
- !!message
  code: 001
  prompt: Start checklist selection
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
- !!choice
  code: 002
  prompt: Indicate option
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  options:
  - &o0
    code: 1
    description: Option 1
  - &o1
    code: 2
    description: Option 2
  - &o2
    code: 3
    description: Option 3
  operator: Operator 1
  answer: 2
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!choice
  code: 003
  prompt: Indicate option with confirmation
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: true
  options:
  - *o0
  - *o1
  - *o2
- !!choice
  code: 004
  prompt: Indicate option that can be omitted
  additionalInformation: 
  skipAllowed: true
  confirmationEnabled: false
  options:
  - *o0
  - *o1
  - *o2
", res);
    }

    [Fact]
    public void Serialize_MultipleChoice()
    {
        var checklist = new Models.Checklist()
        {
            Questions = new IQuestion[]
            {
                new Message { Code = "001", Prompt = "Start checklist multiple choice"},
                new SelectMultiple { Code = "002", Prompt = "Indicate multiple choice", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") }, Answer = new short[] { 2, 3 }, StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operator 1"},
                new SelectMultiple { Code = "003", Prompt = "Indicate multiple choice with confirmation", ConfirmationEnabled= true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                new SelectMultiple { Code = "004", Prompt = "Indicate multiple choice that can be omitted", SkipAllowed = true, Options = new[] {new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
            }
        };

        var res = parser.Serialize(checklist);

        Assert.NotNull(res);
        Assert.Equal(@"questions:
- !!message
  code: 001
  prompt: Start checklist multiple choice
  additionalInformation: 
  skipAllowed: false
  priority: false
  readyToContinue: false
- !!multiple_choice
  answer:
  - 2
  - 3
  code: 002
  prompt: Indicate multiple choice
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: false
  options:
  - &o0
    code: 1
    description: Option 1
  - &o1
    code: 2
    description: Option 2
  - &o2
    code: 3
    description: Option 3
  operator: Operator 1
  startTime: 2022-10-24T10:00:00.0000000
  endTime: 2022-10-24T10:01:00.0000000
- !!multiple_choice
  code: 003
  prompt: Indicate multiple choice with confirmation
  additionalInformation: 
  skipAllowed: false
  confirmationEnabled: true
  options:
  - *o0
  - *o1
  - *o2
- !!multiple_choice
  code: 004
  prompt: Indicate multiple choice that can be omitted
  additionalInformation: 
  skipAllowed: true
  confirmationEnabled: false
  options:
  - *o0
  - *o1
  - *o2
", res);
    }
}
