// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Continuous;

/// <summary>
/// Base implementation for Continuous REST ConnectorBehavior.
/// </summary>
public class Behavior : RestBehavior, IPickingContinuousBehavior
{
    private const string LogHeader = "Rest|Behavior|Continuous";

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
    /// <param name="converter">JSON Converter.</param>
    public Behavior(PickingBehaviorSettings settings, IServerLog serverLog, JsonConverter converter)
        : base(settings, serverLog, converter)
    {
    }

    /// <inheritdoc/>
    public async Task<IGetWorkOrderItem?> GetWorkOrderAsync(string operatorName, string device)
    {
        var request = new RestRequest("Work", Method.GET);

        request.AddQueryParameter("operator", operatorName);

        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| -> {request.Resource}");

        var sw = Stopwatch.StartNew();
        var response = await RestClient.ExecuteAsync<IGetWorkOrderItem>(request);
        sw.Stop();

        if (response.ErrorException is not null)
        {
            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({response.ErrorException.GetType()}): {response.ErrorException.Message} - {response.ErrorException.StackTrace}");

            throw new InvalidOperationException($"[{nameof(GetWorkOrderAsync)}] {Resources.Error}", response.ErrorException);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    if (response.Data is null)
                    {
                        if (Settings.ServerLogEnabled)
                            ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response content ({response.ContentType}): {response.Content}");

                        throw new InvalidOperationException($"[{nameof(GetWorkOrderAsync)}] {Resources.InvalidResponseContent}");
                    }

                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- {JsonConvert.SerializeObject(response.Data)}");

                    return response.Data;
                }

            default:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(GetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response ({response.StatusCode}): {response.Content}");

                    throw new InvalidOperationException($"[{nameof(GetWorkOrderAsync)}] {Resources.InvalidResponse}");
                }
        }
    }

    /// <inheritdoc/>
    public virtual async Task<IGetWorkOrderItem?> SetWorkOrderAsync(string operatorName, string device, ISetWorkOrderItem res)
    {
        var request = new RestRequest("Work/{code}", Method.POST);

        request.AddUrlSegment("code", res.Code);

        var jObject = JObject.FromObject(res);

        jObject["operatorName"] = operatorName;
        jObject["device"] = device;
        jObject.Remove("code");

        request.AddJsonBody(jObject);

        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| -> {request.Resource} - {JsonConvert.SerializeObject(jObject, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })}");

        var sw = Stopwatch.StartNew();
        var response = await RestClient.ExecuteAsync<IGetWorkOrderItem>(request);
        sw.Stop();

        if (response.ErrorException is not null)
        {
            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({response.ErrorException.GetType()}): {response.ErrorException.Message} - {response.ErrorException.StackTrace}");

            throw new InvalidOperationException($"[{nameof(SetWorkOrderAsync)}] {Resources.Error}", response.ErrorException);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    if (response.Data is null)
                    {
                        if (Settings.ServerLogEnabled)
                            ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response content ({response.ContentType}): {response.Content}");

                        if (string.IsNullOrEmpty(response.Content))
                            return null;
                        else
                            throw new InvalidOperationException($"[{nameof(SetWorkOrderAsync)}] {Resources.InvalidResponseContent}");
                    }

                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- {JsonConvert.SerializeObject(response.Data)}");

                    return response.Data;
                }

            default:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(SetWorkOrderAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response ({response.StatusCode}): {response.Content}");

                    throw new InvalidOperationException($"[{nameof(SetWorkOrderAsync)}] {Resources.InvalidResponse}");
                }
        }
    }
}
