// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle.Continuous;

/// <summary>
/// Base implementation for Oracle ConnectorBehavior.
/// </summary>
public class Behavior : OracleBehavior, IPickingContinuousBehavior
{
    private const string LogHeader = "Oracle|Continuous|Behavior";

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
    public Task<IGetWorkOrderItem?> GetWorkOrderAsync(string operatorName, string device)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| -> GetWorkOrder");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "GetWorkOrder";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new OracleParameter("operator", OracleDbType.Varchar2, operatorName, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("prc", OracleDbType.RefCursor, ParameterDirection.Output));

        var sw = Stopwatch.StartNew();

        try
        {
            cn.Open();

            using var reader = cmd.ExecuteReader();

            if (!(reader as DbDataReader)!.HasRows)
            {
                sw.Stop();

                if (Settings.ServerLogEnabled)
                    ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- No work order received");

                return Task.FromResult((IGetWorkOrderItem?)null);
            }

            reader.Read();

            sw.Stop();

            var workOrder = DbDataReaderParser.Parse(reader);

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Work Order: '{JsonConvert.SerializeObject(workOrder)}'");

            return Task.FromResult(workOrder);
        }
        catch (OracleException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            throw new InvalidOperationException($"[{nameof(GetWorkOrderAsync)}] {string.Format(Resources.Error, ex.Message)}", ex);
        }
    }

    /// <inheritdoc/>
    public virtual Task<IGetWorkOrderItem?> SetWorkOrderAsync(string operatorName, string device, ISetWorkOrderItem res)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| -> SetWorkOrder - {JsonConvert.SerializeObject(res, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })}");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        // The SetWorkOrder SP has optional parameters, so it is necessary to bind them by name instead of by position (default).
        if (cmd is OracleCommand oraCmd)
            oraCmd.BindByName = true;
        cmd.CommandText = "SetWorkOrder";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new OracleParameter("Operator", OracleDbType.Varchar2, operatorName, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("Device", OracleDbType.Varchar2, device, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("Code", OracleDbType.Varchar2, res.Code, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("Started", OracleDbType.Varchar2, res.Started, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("Finished", OracleDbType.Varchar2, res.Finished, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("Status", OracleDbType.Varchar2, res.Status, ParameterDirection.Input));

        Type t = res.GetType();
        PropertyInfo[] props = t.GetProperties(BindingFlags.Public
                            | BindingFlags.Instance
                            | BindingFlags.DeclaredOnly);
        foreach (var prop in props)
            cmd.Parameters.Add(new OracleParameter(prop.Name, GetType(prop), GetParamValue(prop, res), ParameterDirection.Input));

        cmd.Parameters.Add(new OracleParameter("prc", OracleDbType.RefCursor, ParameterDirection.Output));

        var sw = Stopwatch.StartNew();

        try
        {
            cn.Open();

            using var reader = cmd.ExecuteReader();

            if (!(reader as DbDataReader)!.HasRows)
            {
                sw.Stop();

                if (Settings.ServerLogEnabled)
                    ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- No work order received");

                return Task.FromResult((IGetWorkOrderItem?)null);
            }

            reader.Read();

            sw.Stop();

            var workOrder = DbDataReaderParser.Parse(reader);

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Work Order: '{JsonConvert.SerializeObject(workOrder)}'");

            return Task.FromResult(workOrder);
        }
        catch (OracleException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            throw new InvalidOperationException($"[{nameof(SetWorkOrderAsync)}] {string.Format(Resources.Error_SetWork, res.Code, ex.Message)}", ex);
        }
    }
}
