// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Oracle;

/// <summary>
/// An implementation of <see cref="ConnectorBehavior"></see> based on Oracle.
/// </summary>
public class Behavior : ConnectorBehavior<ConnectorBehaviorSettingsBase>
{
#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class.
    /// This constructor is used by GWS Connector Service.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    public Behavior(ConnectorBehaviorSettingsBase settings)
        : base(settings)
    {
    }
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class.
    /// This constructor is used by GWS Connector App.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="dbDataSetParser">Database dataset parser service instance.</param>
    public Behavior(ConnectorBehaviorSettingsBase settings, IDbDataSetParser dbDataSetParser)
        : base(settings)
    {
        DbDataSetParser = dbDataSetParser;
    }

    /// <summary>
    /// Gets database parser for extracting data.
    /// </summary>
#if NETFRAMEWORK
    protected virtual IDbDataSetParser DbDataSetParser { get; } = new DbDataSetParser();
#else
    protected IDbDataSetParser DbDataSetParser { get; }
#endif

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        if (string.IsNullOrEmpty(Settings.Server))
        {
            throw new InvalidOperationException(Resources.ServerEmpty);
        }

        try
        {
            using var cn = CreateConnection();

            cn.Open();
        }
        catch (Exception ex)
        {
            Log(string.Format(Resources.Error_DBNotAccessible, ex.GetType(), ex.Message), LogLevel.Error);
        }
    }

    /// <inheritdoc/>
    public override Operator? GetOperator(string @operator)
    {
        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "GetOperator";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new OracleParameter("operator", OracleDbType.Varchar2, @operator, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("prc", OracleDbType.RefCursor, ParameterDirection.Output));

        var sw = Stopwatch.StartNew();

        try
        {
            cn.Open();
            using var reader = cmd.ExecuteReader();
            if (!(reader as DbDataReader)!.HasRows)
            {
                sw.Stop();
                return null;
            }

            reader.Read();
            sw.Stop();

            return new Operator
            {
                Name = reader.GetString(0),
                Password = reader.SafeGetString("password"),
            };
        }
        catch (OracleException)
        {
            sw.Stop();
            return null;
        }
    }

    /// <inheritdoc/>
    public override Models.Checklist? RetrieveChecklist(string @operator, string id)
    {
        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "GetChecklist";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new OracleParameter("operator", @operator));
        cmd.Parameters.Add(new OracleParameter("checklistId", OracleDbType.Int32, id, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("prc_first", OracleDbType.RefCursor, ParameterDirection.Output));
        cmd.Parameters.Add(new OracleParameter("prc_second", OracleDbType.RefCursor, ParameterDirection.Output));

        try
        {
            cn.Open();

            var ds = new DataSet();

            var adapter = CreateDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);

            var questions = DbDataSetParser.Parse(ds);
            if (!questions.Any())
                return null;

            return new Models.Checklist
            {
                Questions = questions,
            };
        }
        catch (OracleException)
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public override void UpdateChecklist(string @operator, string id, Models.Checklist checklist)
    {
        if (!string.IsNullOrEmpty(checklist.CompletedBy))
        {
            using var cn = CreateConnection();
            using var cmd = cn.CreateCommand();

            cmd.CommandText = "CompleteChecklist";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("operator", @operator));
            cmd.Parameters.Add(new OracleParameter("checklistId", OracleDbType.Int32, id, ParameterDirection.Input));
            cmd.Parameters.Add(new OracleParameter("completedAt", checklist.Completed));

            var sw = Stopwatch.StartNew();
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();

                sw.Stop();
            }
            catch (OracleException ex)
            {
                sw.Stop();

                throw new InvalidOperationException($"[{nameof(UpdateChecklist)}] {string.Format(Resources.Error_UpdateChecklist, id, ex.Message)}", ex);
            }
        }
        else
        {
            // Get the last question with a starttime value and update it, this is needed to update the last answered question or add information about the skipped one.
            var question = checklist.Questions.OrderBy(x => x.StartTime).Last(x => x.StartTime != null);

            using var cn = CreateConnection();
            using var cmd = cn.CreateCommand();

            cmd.CommandText = "UpdateChecklist";
            cmd.CommandType = CommandType.StoredProcedure;

            var answer = question.GetType().GetProperty("Answer")?.GetValue(question);

            var sqlAnswer = answer?.ToString() ?? string.Empty;

            if (question is SelectMultiple)
                sqlAnswer = string.Join(",", (answer as IEnumerable).Cast<short>());

            cmd.Parameters.Add(new OracleParameter("operator", @operator));
            cmd.Parameters.Add(new OracleParameter("checklistId", id));
            cmd.Parameters.Add(new OracleParameter("code", question.Code));
            cmd.Parameters.Add(new OracleParameter("started", question.StartTime));
            cmd.Parameters.Add(new OracleParameter("ended", question.EndTime ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("answer", sqlAnswer));

            var sw = Stopwatch.StartNew();
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();

                sw.Stop();
            }
            catch (OracleException ex)
            {
                sw.Stop();

                throw new InvalidOperationException($"[{nameof(UpdateChecklist)}] {string.Format(Resources.Error_UpdateChecklist, id, ex.Message)}", ex);
            }
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DbConnection"/> class.
    /// </summary>
    /// <returns>A new <see cref="DbConnection"/> instance.</returns>
    internal virtual IDbConnection CreateConnection() => new OracleConnection(Settings.Server);

    /// <summary>
    /// Creates a new instance of the <see cref="DbDataAdapter"/> class.
    /// </summary>
    /// <returns>A new <see cref="DbDataAdapter"/> instance.</returns>
    internal virtual IDbDataAdapter CreateDataAdapter() => new OracleDataAdapter();
}