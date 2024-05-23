// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.SqlServer.Properties;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.SqlServer.Batch;

/// <summary>
/// Base implementation for SQL Server ConnectorBehavior.
/// </summary>
public class Behavior : SqlServerBehavior, IPickingBatchBehavior
{
    private const string LogHeader = "SqlServer|Batch|Behavior";

#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class. This constructor is used by GWS Connector Service.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    public Behavior(PickingBehaviorSettings settings)
        : base(settings)
    {
    }
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class. This constructor is used by GWS App.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="serverLog">ServerLog service instance.</param>
    /// <param name="dbDataReaderParser">Database reader parser service instance.</param>
    public Behavior(PickingBehaviorSettings settings, IServerLog serverLog, IDbDataReaderParser dbDataReaderParser)
        : base(settings, serverLog, dbDataReaderParser)
    {
    }

    /// <inheritdoc/>
    public Task<IEnumerable<IGetWorkOrderItem>> GetWorkOrdersAsync(string operatorName, string device)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrdersAsync)}|{operatorName}|{device}| -> GetWorkOrder");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "GetWorkOrder";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("operator", operatorName));

        var sw = Stopwatch.StartNew();

        try
        {
            cn.Open();

            using var reader = cmd.ExecuteReader();

            var orders = new List<IGetWorkOrderItem>();

            while (reader.Read())
            {
                var o = DbDataReaderParser.Parse(reader);
                if (o != null)
                    orders.Add(o);
            }

            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrdersAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Orders: '{JsonConvert.SerializeObject(orders)}'");

            return Task.FromResult<IEnumerable<IGetWorkOrderItem>>(orders);
        }
        catch (SqlException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrdersAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            throw new InvalidOperationException($"[{nameof(GetWorkOrdersAsync)}] {string.Format(Resources.Error, ex.Message)}", ex);
        }
    }

    /// <inheritdoc/>
    public virtual Task SetWorkOrderAsync(string operatorName, string device, ISetWorkOrderItem res)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| -> SetWorkOrder - {JsonConvert.SerializeObject(res, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })}");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "SetWorkOrder";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("Operator", operatorName));
        cmd.Parameters.Add(new SqlParameter("Device", device));
        cmd.Parameters.Add(new SqlParameter("Code", res.Code));
        cmd.Parameters.Add(new SqlParameter("Started", res.Started));
        cmd.Parameters.Add(new SqlParameter("Finished", res.Finished));
        cmd.Parameters.Add(new SqlParameter("Status", res.Status));

        Type t = res.GetType();
        PropertyInfo[] props = t.GetProperties(BindingFlags.Public
                            | BindingFlags.Instance
                            | BindingFlags.DeclaredOnly);
        foreach (var prop in props)
            cmd.Parameters.Add(new SqlParameter(prop.Name, GetParamValue(prop, res)));

        var sw = Stopwatch.StartNew();

        try
        {
            cn.Open();

            cmd.ExecuteNonQuery();

            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Task completed");

            return Task.CompletedTask;
        }
        catch (SqlException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            throw new InvalidOperationException($"[{nameof(SetWorkOrderAsync)}] {string.Format(Resources.Error_SetWork, res.Code, ex.Message)}", ex);
        }
    }

    /// <inheritdoc/>
    public Task PrintLabelsBatchAsync(string operatorName, string device, PrintLabelsBatch res)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(PrintLabelsBatchAsync)}|{operatorName}|{device}| -> PrintLabels - {JsonConvert.SerializeObject(res, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })}");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "PrintLabels";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new SqlParameter("operator", operatorName));
        cmd.Parameters.Add(new SqlParameter("code", res.Code));
        cmd.Parameters.Add(new SqlParameter("count", res.Count));

        var sw = Stopwatch.StartNew();

        try
        {
            cn.Open();

            cmd.ExecuteNonQuery();

            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(PrintLabelsBatchAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Task completed");

            return Task.CompletedTask;
        }
        catch (SqlException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(PrintLabelsBatchAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            throw new InvalidOperationException($"[{nameof(PrintLabelsBatchAsync)}] {string.Format(Resources.Error_PrintLabels, res.Code, ex.Message)}", ex);
        }
    }
}
