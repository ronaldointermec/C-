// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.SDK;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules.InMemory;

/// <summary>
/// An implementation of <see cref="ConnectorBehavior"></see> based on file storage.
/// </summary>
public class Behavior : ConnectorBehavior
{
    private readonly JsonSerializerSettings _settings = new() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore, Converters = new[] { new QuestionJsonConverter() } };

    private readonly IDictionary<string, Models.Checklist> _checklists = new Dictionary<string, Models.Checklist>
    {
        {
            "1",
            new Models.Checklist()
            {
                Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist messages" },
                    new Message { Code = "002", Prompt = "Priority message", Priority = true },
                    new Message { Code = "003", Prompt = "Message with confirmation", ReadyToContinue = true },
                    new Message { Code = "004", Prompt = "Message with additional information", ReadyToContinue = true, AdditionalInformation = "Additional information" },
                    new Message { Code = "005", Prompt = "Message that can be omitted", ReadyToContinue = true, SkipAllowed = true },
                },
            }
        },
        {
            "2",
            new Models.Checklist()
            {
                Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist questions" },
                    new Ask { Code = "002", Prompt = "Question without priority", AdditionalInformation = "Additional information" },
                    new Ask { Code = "003", Prompt = "Question with priority", Priority = true },
                    new Ask { Code = "004", Prompt = "Question that can be omitted", SkipAllowed = true },
                },
            }
        },
        {
            "3",
            new Models.Checklist()
            {
                Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist integers" },
                    new IntegerValue { Code = "002", Prompt = "Indicate number" },
                    new IntegerValue { Code = "003", Prompt = "Indicate number with scanner", ScannerEnabled = true },
                    new IntegerValue { Code = "004", Prompt = "Indicate number with confirmation", ConfirmationEnabled = true },
                    new IntegerValue { Code = "005", Prompt = "Indicate number with range of 2 to 3 digits", MinLength = 2, MaxLength = 3 },
                    new IntegerValue { Code = "006", Prompt = "Indicate number with a minimum of 10 to 30", MinValue = 10, MaxValue = 30 },
                    new IntegerValue { Code = "007", Prompt = "Indicate number that can be omitted", SkipAllowed = true },
                },
            }
        },
        {
            "4",
            new Models.Checklist()
            {
                Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist decimal numbers" },
                    new FloatValue { Code = "002", Prompt = "Indicate decimal number" },
                    new FloatValue { Code = "003", Prompt = "Indicate decimal number with scanner", ScannerEnabled = true },
                    new FloatValue { Code = "004", Prompt = "Indicate decimal number with confirmation", ConfirmationEnabled = true },
                    new FloatValue { Code = "005", Prompt = "Indicate decimal number with range of 2 to 3 digits", MinLength = 2, MaxLength = 3 },
                    new FloatValue { Code = "006", Prompt = "Indicate decimal number with a minimum of 10 to 30", MinValue = 10, MaxValue = 30 },
                    new FloatValue { Code = "007", Prompt = "Indicate decimal number that can be omitted", SkipAllowed = true },
                },
            }
        },
        {
            "5",
            new Models.Checklist()
            {
                Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist alphanumeric values" },
                    new StringValue { Code = "002", Prompt = "Indicate alphanumeric" },
                    new StringValue { Code = "003", Prompt = "Indicate alphanumeric with scanner", ScannerEnabled = true },
                    new StringValue { Code = "004", Prompt = "Indicate alphanumeric with confirmation", ConfirmationEnabled = true },
                    new StringValue { Code = "005", Prompt = "Indicate alphanumeric with range of 2 to 3 digits", MinLength = 2, MaxLength = 3 },
                    new StringValue { Code = "006", Prompt = "Indicate alphanumeric that can be omitted", SkipAllowed = true },
                },
            }
        },
        {
            "6",
            new Models.Checklist()
            {
                Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist dates" },
                    new Date { Code = "002", Prompt = "Indicate date with day, month and long year, with confirmation", Format = "ddMMyyyy", ConfirmationEnabled = true },
                    new Date { Code = "003", Prompt = "Indicate date with day, month and short year", Format = "ddMMyy" },
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
                    new Date { Code = "016", Prompt = "Indicate date that can be omitted", Format = "ddMMyyyy", SkipAllowed = true },
                },
            }
        },
        {
            "7",
            new Models.Checklist()
            {
                Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist times" },
                    new Time { Code = "002", Prompt = "Indicate time with hour, minute and second, with confirmation", Format = "hhmmss", ConfirmationEnabled = true },
                    new Time { Code = "003", Prompt = "Indicate time with hour and minute", Format = "hhmm" },
                    new Time { Code = "004", Prompt = "Indicate time with minute and second", Format = "mmss" },
                    new Time { Code = "005", Prompt = "Indicate time with hour", Format = "hh" },
                    new Time { Code = "006", Prompt = "Indicate time with minute", Format = "mm" },
                    new Time { Code = "007", Prompt = "Indicate time with second", Format = "ss" },
                    new Time { Code = "008", Prompt = "Indicate time that can be omitted", Format = "hhmmss", SkipAllowed = true },
                },
            }
        },
        {
            "8",
            new Models.Checklist()
            {
                Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist selection" },
                    new Select { Code = "002", Prompt = "Indicate option", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                    new Select { Code = "003", Prompt = "Indicate option with confirmation", ConfirmationEnabled = true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                    new Select { Code = "004", Prompt = "Indicate option that can be omitted", SkipAllowed = true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                },
            }
        },
        {
            "9",
            new Models.Checklist()
            {
                Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist multiple choice" },
                    new SelectMultiple { Code = "002", Prompt = "Indicate multiple choice", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                    new SelectMultiple { Code = "003", Prompt = "Indicate multiple choice with confirmation", ConfirmationEnabled = true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                    new SelectMultiple { Code = "004", Prompt = "Indicate option that can be omitted", SkipAllowed = true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                },
            }
        },
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    public Behavior(ConnectorBehaviorSettingsBase settings)
        : base(settings)
    {
    }

    /// <inheritdoc/>
    public override Operator? GetOperator(string @operator) => new() { Name = @operator, Password = null };

    /// <inheritdoc/>
    public override Models.Checklist RetrieveChecklist(string @operator, string id)
    {
        _checklists.TryGetValue(id, out var checklist);

        return checklist;
    }

    /// <inheritdoc/>
    public override void UpdateChecklist(string @operator, string id, Models.Checklist checklist)
    {
        Log(string.Format("[{0}] Results: \r\n{1}", id, JsonConvert.SerializeObject(checklist, _settings)), LogLevel.Information);
    }
}