// <copyright file="OracleBehavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Resources = Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle.Properties.Resources;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle;

/// <summary>
/// Base class for Oracle ConnectorBehavior.
/// </summary>
public abstract class OracleBehavior : PickingBehaviorBase
{
    private const string LogHeader = "Oracle|Behavior";

#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="OracleBehavior"/> class. This constructor is used by GWS Connector Service.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    protected OracleBehavior(PickingBehaviorSettings settings)
        : base(settings)
    {
    }
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="OracleBehavior"/> class. This constructor is used by GWS App.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="serverLog">ServerLog service instance.</param>
    /// <param name="dbDataReaderParser">Database reader parser service instance.</param>
    protected OracleBehavior(PickingBehaviorSettings settings, IServerLog serverLog, IDbDataReaderParser dbDataReaderParser)
        : base(settings, serverLog)
    {
        DbDataReaderParser = dbDataReaderParser;
    }

    /// <summary>
    /// Gets database parser for extracting data.
    /// </summary>
#if NETFRAMEWORK
    protected virtual IDbDataReaderParser DbDataReaderParser { get; } = new DbDataReaderParser();
#else
    protected IDbDataReaderParser DbDataReaderParser { get; }
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

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(Initialize)}| {string.Format(Resources.Error_DBNotAccessible, ex.GetType(), ex.Message)}");
        }
    }

    /// <inheritdoc/>
    public override ValueTask<ConnectResult> ConnectAsync(string operatorName, IDevice device)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| -> GetOperatorStart - language: '{device.Language}'");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "GetOperatorStart";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new OracleParameter("operator", OracleDbType.Varchar2, operatorName, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("device", OracleDbType.Varchar2, device.DeviceID, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("language", OracleDbType.Varchar2, device.Language, ParameterDirection.Input));
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
                    ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| ({sw.Elapsed}) <- Error accessing: no response received from the server");

                return new ValueTask<ConnectResult>(new ConnectResult(false, null, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error_ConnectMissingServerResponse))));
            }

            reader.Read();

            sw.Stop();

            var allowed = Convert.ToInt16(reader["Allowed"]) == 1;
            var pwd = reader.SafeGetString("Password");
            var msg = reader.SafeGetString("Message");

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| ({sw.Elapsed}) <- Allowed: '{allowed}' - Password: 'XXX' - Message: '{msg}'");

            if (!allowed)
                return new ValueTask<ConnectResult>(new ConnectResult(false, null, msg ?? device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error_UnknownReason))));
            else
                return new ValueTask<ConnectResult>(new ConnectResult(true, pwd, msg));
        }
        catch (OracleException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            return new ValueTask<ConnectResult>(new ConnectResult(false, null, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error))));
        }
    }

    /// <inheritdoc/>
    public override ValueTask<DisconnectResult> DisconnectAsync(string operatorName, string device, bool force)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| -> GetOperatorExit");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "GetOperatorExit";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new OracleParameter("operator", OracleDbType.Varchar2, operatorName, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("prc", OracleDbType.RefCursor, ParameterDirection.Output));

        var sw = Stopwatch.StartNew();

        try
        {
            cn.Open();

            using var reader = cmd.ExecuteReader();

            sw.Stop();

            if (!(reader as DbDataReader)!.HasRows)
            {
                sw.Stop();

                if (Settings.ServerLogEnabled)
                    ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Error disconnecting: no response received from the server");

                throw new InvalidOperationException($"[{nameof(DisconnectAsync)}] {Resources.Error_ServerResponse}");
            }

            reader.Read();

            var allowed = Convert.ToInt16(reader["Allowed"]) == 1;
            var msg = reader["Message"] is DBNull ? null : Convert.ToString(reader["Message"]);

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Allowed: '{allowed}' - Message: '{msg}'");

            return new ValueTask<DisconnectResult>(new DisconnectResult(force ? force : allowed, msg));
        }
        catch (OracleException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            throw new InvalidOperationException($"[{nameof(DisconnectAsync)}] {string.Format(Resources.Error_Disconnect, ex.Message)}", ex);
        }
    }

    /// <inheritdoc/>
    public override Task<string?> RegisterOperatorStartAsync(string operatorName, string device)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| -> SetOperatorStart");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "SetOperatorStart";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new OracleParameter("operator", OracleDbType.Varchar2, operatorName, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("device", OracleDbType.Varchar2, device, ParameterDirection.Input));
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
                    ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Error registering operator start: no response received from the server");

                throw new InvalidOperationException($"[{nameof(RegisterOperatorStartAsync)}] {Resources.Error_ServerResponse}");
            }

            reader.Read();

            sw.Stop();

            var msg = reader.SafeGetString("Message");

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Message: '{msg}'");

            return Task.FromResult(msg);
        }
        catch (OracleException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            throw new InvalidOperationException($"[{nameof(RegisterOperatorStartAsync)}] {string.Format(Resources.Error_RegisterOperator, ex.Message)}", ex);
        }
    }

    /// <inheritdoc/>
    public override Task BeginBreakAsync(string operatorName, string device, BeginBreak res)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(BeginBreakAsync)}|{operatorName}|{device}| -> BeginBreak - {JsonConvert.SerializeObject(res)}");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "BeginBreak";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new OracleParameter("operator", OracleDbType.Varchar2, operatorName, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("code", OracleDbType.Varchar2, res.Code, ParameterDirection.Input));
        cmd.Parameters.Add(new OracleParameter("reason", OracleDbType.Varchar2, res.Reason ?? (object)DBNull.Value, ParameterDirection.Input));

        var sw = Stopwatch.StartNew();

        try
        {
            cn.Open();

            cmd.ExecuteNonQuery();

            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(BeginBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Task completed");

            return Task.CompletedTask;
        }
        catch (OracleException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(BeginBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            throw new InvalidOperationException($"[{nameof(BeginBreakAsync)}] {string.Format(Resources.Error_BeginBreak, res.Code, ex.Message)}", ex);
        }
    }

    /// <inheritdoc/>
    public override Task EndBreakAsync(string operatorName, string device)
    {
        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(EndBreakAsync)}|{operatorName}|{device}| -> EndBreak");

        using var cn = CreateConnection();
        using var cmd = cn.CreateCommand();

        cmd.CommandText = "EndBreak";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add(new OracleParameter("operator", OracleDbType.Varchar2, operatorName, ParameterDirection.Input));

        var sw = Stopwatch.StartNew();

        try
        {
            cn.Open();

            cmd.ExecuteNonQuery();

            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(EndBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Task completed");

            return Task.CompletedTask;
        }
        catch (OracleException ex)
        {
            sw.Stop();

            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(EndBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({ex.GetType()}): {ex.Message} - {ex.StackTrace}");

            throw new InvalidOperationException($"[{nameof(EndBreakAsync)}] {string.Format(Resources.Error_EndBreak, operatorName, ex.Message)}", ex);
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DbConnection"/> class.
    /// </summary>
    /// <returns>A new <see cref="DbConnection"/> instance.</returns>
    internal virtual IDbConnection CreateConnection() => new OracleConnection(Settings.Server);

    /// <summary>
    /// Gets the parameter value from a PropertyInfo.
    /// </summary>
    /// <param name="prop">PropertyInfo object.</param>
    /// <param name="res">ISetWorkOrderItem object.</param>
    /// <returns>A new <see cref="object"/> instance.</returns>
    protected object GetParamValue(PropertyInfo prop, ISetWorkOrderItem res)
    {
        var value = prop.GetValue(res);

        if (value == null)
            return (object)DBNull.Value;

        if (value is IEnumerable && prop.PropertyType != typeof(string))
            return string.Join("|", value as IEnumerable<object>);

        return value;
    }

    /// <summary>
    /// Gets the OracleDbType from a PropertyInfo name.
    /// </summary>
    /// <param name="prop">PropertyInfo object.</param>
    /// <returns>A new <see cref="OracleDbType"/> instance.</returns>
    protected OracleDbType GetType(PropertyInfo prop)
    {
        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

        return type switch
        {
            Type _ when type == typeof(int) => OracleDbType.Int32,
            Type _ when type == typeof(decimal) => OracleDbType.Decimal,
            Type _ when type == typeof(bool) => OracleDbType.Int16,
            _ => OracleDbType.Varchar2,
        };
    }
}
