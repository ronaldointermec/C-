// <copyright file="DbDataSetParser.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;

/// <summary>
/// Utility class to extract the data from the database dataset object.
/// </summary>
public class DbDataSetParser : IDbDataSetParser
{
    /// <inheritdoc/>
    public virtual IEnumerable<IQuestion> Parse(DataSet ds)
    {
        if (ds == null)
            return Enumerable.Empty<IQuestion>();

        if (ds.Tables[0].Rows.Count == 0)
            return Enumerable.Empty<IQuestion>();

        var questionList = new List<IQuestion>();

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            var question = GetQuestion(row);
            if (question != null)
                questionList.Add(question);
        }

        return questionList;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>Question list to answer.</returns>
    /// <param name="row">Row object.</param>
    protected virtual IQuestion? GetQuestion(DataRow row)
    {
        if (row["Code"] is DBNull)
            return null;

        return row["Type"] switch
        {
            "message" => new Message()
            {
                Code = row.SafeGetString("Code") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Code")),
                Prompt = row.SafeGetString("Prompt") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Prompt")),
                SkipAllowed = row.SafeGetBool("SkipAllowed") ?? false,
                ReadyToContinue = row.SafeGetBool("ReadyToContinue") ?? false,
                Priority = row.SafeGetBool("Priority") ?? false,
                AdditionalInformation = row.SafeGetString("AdditionalInformation") ?? string.Empty,
                Image = row.SafeGetUri("Image"),
                EndTime = row.SafeGetDateTime("EndTime"),
                StartTime = row.SafeGetDateTime("StartTime"),
            },
            "ask" => new Ask()
            {
                Code = row.SafeGetString("Code") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Code")),
                Prompt = row.SafeGetString("Prompt") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Prompt")),
                SkipAllowed = row.SafeGetBool("SkipAllowed") ?? false,
                Priority = row.SafeGetBool("Priority") ?? false,
                AdditionalInformation = row.SafeGetString("AdditionalInformation") ?? string.Empty,
                Image = row.SafeGetUri("Image"),
                EndTime = row.SafeGetDateTime("EndTime"),
                StartTime = row.SafeGetDateTime("StartTime"),
            },
            "number" => new IntegerValue()
            {
                Code = row.SafeGetString("Code") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Code")),
                Prompt = row.SafeGetString("Prompt") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Prompt")),
                SkipAllowed = row.SafeGetBool("SkipAllowed") ?? false,
                ConfirmationEnabled = row.SafeGetBool("ConfirmationEnabled") ?? false,
                ScannerEnabled = row.SafeGetBool("ScannerEnabled") ?? false,
                MaxLength = row.SafeGetInt("MaxLength"),
                MinLength = row.SafeGetInt("MinLength"),
                MaxValue = row.SafeGetInt("MaxValue"),
                MinValue = row.SafeGetInt("MinValue"),
                AdditionalInformation = row.SafeGetString("AdditionalInformation") ?? string.Empty,
                Image = row.SafeGetUri("Image"),
                EndTime = row.SafeGetDateTime("EndTime"),
                StartTime = row.SafeGetDateTime("StartTime"),
            },
            "decimal" => new FloatValue()
            {
                Code = row.SafeGetString("Code") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Code")),
                Prompt = row.SafeGetString("Prompt") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Prompt")),
                SkipAllowed = row.SafeGetBool("SkipAllowed") ?? false,
                ConfirmationEnabled = row.SafeGetBool("ConfirmationEnabled") ?? false,
                ScannerEnabled = row.SafeGetBool("ScannerEnabled") ?? false,
                MaxLength = row.SafeGetInt("MaxLength"),
                MinLength = row.SafeGetInt("MinLength"),
                MaxValue = (float?)row.SafeGetDecimal("MaxValue"),
                MinValue = (float?)row.SafeGetDecimal("MinValue"),
                AdditionalInformation = row.SafeGetString("AdditionalInformation") ?? string.Empty,
                Image = row.SafeGetUri("Image"),
                EndTime = row.SafeGetDateTime("EndTime"),
                StartTime = row.SafeGetDateTime("StartTime"),
            },
            "string" => new StringValue()
            {
                Code = row.SafeGetString("Code") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Code")),
                Prompt = row.SafeGetString("Prompt") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Prompt")),
                SkipAllowed = row.SafeGetBool("SkipAllowed") ?? false,
                ConfirmationEnabled = row.SafeGetBool("ConfirmationEnabled") ?? false,
                ScannerEnabled = row.SafeGetBool("ScannerEnabled") ?? false,
                MaxLength = row.SafeGetInt("MaxLength"),
                MinLength = row.SafeGetInt("MinLength"),
                AdditionalInformation = row.SafeGetString("AdditionalInformation") ?? string.Empty,
                Image = row.SafeGetUri("Image"),
                EndTime = row.SafeGetDateTime("EndTime"),
                StartTime = row.SafeGetDateTime("StartTime"),
            },
            "date" => new Date()
            {
                Code = row.SafeGetString("Code") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Code")),
                Prompt = row.SafeGetString("Prompt") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Prompt")),
                SkipAllowed = row.SafeGetBool("SkipAllowed") ?? false,
                ConfirmationEnabled = row.SafeGetBool("ConfirmationEnabled") ?? false,
                Format = row.SafeGetString("Format") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Format")),
                AdditionalInformation = row.SafeGetString("AdditionalInformation") ?? string.Empty,
                Image = row.SafeGetUri("Image"),
                EndTime = row.SafeGetDateTime("EndTime"),
                StartTime = row.SafeGetDateTime("StartTime"),
            },
            "time" => new Time()
            {
                Code = row.SafeGetString("Code") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Code")),
                Prompt = row.SafeGetString("Prompt") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Prompt")),
                SkipAllowed = row.SafeGetBool("SkipAllowed") ?? false,
                ConfirmationEnabled = row.SafeGetBool("ConfirmationEnabled") ?? false,
                Format = row.SafeGetString("Format") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Format")),
                AdditionalInformation = row.SafeGetString("AdditionalInformation") ?? string.Empty,
                Image = row.SafeGetUri("Image"),
                EndTime = row.SafeGetDateTime("EndTime"),
                StartTime = row.SafeGetDateTime("StartTime"),
            },
            "choice" => new Select()
            {
                Code = row.SafeGetString("Code") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Code")),
                Prompt = row.SafeGetString("Prompt") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Prompt")),
                SkipAllowed = row.SafeGetBool("SkipAllowed") ?? false,
                ConfirmationEnabled = row.SafeGetBool("ConfirmationEnabled") ?? false,
                AdditionalInformation = row.SafeGetString("AdditionalInformation") ?? string.Empty,
                Options = GetOptions(row),
                EndTime = row.SafeGetDateTime("EndTime"),
                StartTime = row.SafeGetDateTime("StartTime"),
            },
            "multiple_choice" => new SelectMultiple()
            {
                Code = row.SafeGetString("Code") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Code")),
                Prompt = row.SafeGetString("Prompt") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Prompt")),
                SkipAllowed = row.SafeGetBool("SkipAllowed") ?? false,
                ConfirmationEnabled = row.SafeGetBool("ConfirmationEnabled") ?? false,
                AdditionalInformation = row.SafeGetString("AdditionalInformation") ?? string.Empty,
                Options = GetOptions(row),
                EndTime = row.SafeGetDateTime("EndTime"),
                StartTime = row.SafeGetDateTime("StartTime"),
            },
            _ => throw new InvalidOperationException(string.Format(Resources.DataSetParser_InvalidType, row["Type"])),
        };
    }

    private static IEnumerable<SelectOption> GetOptions(DataRow originRow)
    {
        var ds = originRow.Table.DataSet;

        if (ds.Tables.Count == 1)
            return Enumerable.Empty<SelectOption>();

        if (ds.Tables[1].Rows.Count == 0)
            return Enumerable.Empty<SelectOption>();

        var originCode = originRow.SafeGetString("Code");

        var selectOptions = new List<SelectOption>();

        foreach (DataRow row in ds.Tables[1].Rows)
        {
            var code = row.SafeGetString("Code");
            if (code != originCode)
                continue;

            selectOptions.Add(new SelectOption
            {
                Code = row.SafeGetShort("OptionCode") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "OptionCode")),
                Description = row.SafeGetString("Description") ?? throw new InvalidOperationException(string.Format(Resources.DataSetParser_PropertyEmpty, "Description")),
            });
        }

        return selectOptions;
    }
}
