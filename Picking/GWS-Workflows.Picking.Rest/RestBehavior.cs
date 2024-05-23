// <copyright file="RestBehavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Rest;

/// <summary>
/// Base class for REST implementation.
/// </summary>
public abstract class RestBehavior : PickingBehaviorBase
{
    private const string LogHeader = "Rest|Behavior";

#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="RestBehavior"/> class. This constructor is used by GWS Connector Service.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    protected RestBehavior(PickingBehaviorSettings settings)
        : base(settings)
    {
    }
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="RestBehavior"/> class. This constructor is used by GWS App.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="serverLog">ServerLog service instance.</param>
    /// <param name="converter">JSON Converter.</param>
    protected RestBehavior(PickingBehaviorSettings settings, IServerLog serverLog, JsonConverter converter)
        : base(settings, serverLog)
    {
        Converter = converter;
    }

    /// <summary>
    /// Gets REST client used to perform requests to remote server.
    /// </summary>
    protected internal virtual IRestClient RestClient { get; private set; } = null!;

    /// <summary>
    /// Gets database parser for extracting data.
    /// </summary>
#if NETFRAMEWORK
    protected virtual JsonConverter Converter { get; } = new GetWorkOrderConverter();
#else
    protected JsonConverter Converter { get; }
#endif

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        if (string.IsNullOrEmpty(Settings.Server))
        {
            throw new InvalidOperationException(Resources.ServerEmpty);
        }

        RestClient = new RestClient(Settings.Server)
            .UseJson()
            .UseNewtonsoftJson(new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>() { Converter },
                NullValueHandling = NullValueHandling.Ignore,
            });
    }

    /// <inheritdoc/>
    public override async ValueTask<ConnectResult> ConnectAsync(string operatorName, IDevice device)
    {
        var request = new RestRequest("Operators/{name}", Method.GET);

        request.AddUrlSegment("name", operatorName);
        request.AddQueryParameter("device", device.DeviceID);
        request.AddQueryParameter("language", device.Language);

        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| -> {request.Resource}");

        var sw = Stopwatch.StartNew();
        var response = await RestClient.ExecuteAsync<ConnectModel>(request);
        sw.Stop();

        if (response.ErrorException is not null)
        {
            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| ({sw.Elapsed}) <- Exception ({response.ErrorException.GetType()}): {response.ErrorException.Message} - {response.ErrorException.StackTrace}");

            return new ConnectResult(false, null, device.Translate(Resources.ResourceManager, nameof(Resources.Error)));
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    if (response.Data is null)
                    {
                        if (Settings.ServerLogEnabled)
                            ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| ({sw.Elapsed}) <- Invalid response content ({response.ContentType}): {response.Content}");

                        return new ConnectResult(false, null, Resources.InvalidResponseContent);
                    }

                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| ({sw.Elapsed}) <-");

                    return new ConnectResult(response.Data.Allowed, response.Data.Pwd, response.Data.Message);
                }

            case HttpStatusCode.NotFound:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| ({sw.Elapsed}) <- User '{operatorName}' not found");

                    return new ConnectResult(false, null, device.Translate(Resources.ResourceManager, nameof(Resources.UserNotFound), operatorName));
                }

            default:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(ConnectAsync)}|{operatorName}|{device.DeviceID}| ({sw.Elapsed}) <- Invalid response ({response.StatusCode}): {response.Content}");

                    return new ConnectResult(false, null, device.Translate(Resources.ResourceManager, nameof(Resources.InvalidResponse)));
                }
        }
    }

    /// <inheritdoc/>
    public override async ValueTask<DisconnectResult> DisconnectAsync(string operatorName, string device, bool force)
    {
        var request = new RestRequest("Operators/{name}/Disconnect", Method.POST);
        request.AddUrlSegment("name", operatorName);

        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| -> {request.Resource}");

        var sw = Stopwatch.StartNew();
        var response = await RestClient.ExecuteAsync<DisconnectModel>(request);
        sw.Stop();

        if (response.ErrorException is not null)
        {
            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({response.ErrorException.GetType()}): {response.ErrorException.Message} - {response.ErrorException.StackTrace}");

            throw new InvalidOperationException($"[{nameof(DisconnectAsync)}] {string.Format(Resources.Error_Disconnect, response.ErrorException.Message)}", response.ErrorException);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    if (response.Data is null)
                    {
                        if (Settings.ServerLogEnabled)
                            ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response content ({response.ContentType}): {response.Content}");

                        throw new InvalidOperationException($"[{nameof(DisconnectAsync)}] {Resources.InvalidResponseContent}");
                    }

                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <-");

                    return new DisconnectResult(force ? force : response.Data.Allowed, response.Data.Message);
                }

            default:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(DisconnectAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response ({response.StatusCode}): {response.Content}");

                    throw new InvalidOperationException($"[{nameof(DisconnectAsync)}] {Resources.InvalidResponse}");
                }
        }
    }

    /// <inheritdoc/>
    public override async Task<string?> RegisterOperatorStartAsync(string operatorName, string device)
    {
        var request = new RestRequest("Operators/{name}", Method.POST);

        request.AddUrlSegment("name", operatorName);
        request.AddQueryParameter("device", device);

        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| -> {request.Resource}");

        var sw = Stopwatch.StartNew();
        var response = await RestClient.ExecuteAsync<RegisterOperatorModel>(request);
        sw.Stop();

        if (response.ErrorException is not null)
        {
            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({response.ErrorException.GetType()}): {response.ErrorException.Message} - {response.ErrorException.StackTrace}");

            throw new InvalidOperationException($"[{nameof(RegisterOperatorStartAsync)}] {string.Format(Resources.Error_RegisterOperator, response.ErrorException.Message)}", response.ErrorException);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    if (response.Data is null)
                    {
                        if (Settings.ServerLogEnabled)
                            ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response content ({response.ContentType}): {response.Content}");

                        return null;
                    }

                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <-");

                    return response.Data.Message;
                }

            default:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(RegisterOperatorStartAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response ({response.StatusCode}): {response.Content}");

                    throw new InvalidOperationException($"[{nameof(RegisterOperatorStartAsync)}] {Resources.InvalidResponse}");
                }
        }
    }

    /// <inheritdoc/>
    public override async Task BeginBreakAsync(string operatorName, string device, BeginBreak res)
    {
        var request = new RestRequest("Operators/{name}/Break", Method.POST);

        request.AddUrlSegment("name", operatorName);
        request.AddQueryParameter("code", res.Code);
        request.AddQueryParameter("reason", res.Reason?.ToString() ?? string.Empty);

        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(BeginBreakAsync)}|{operatorName}|{device}| -> {request.Resource}");

        var sw = Stopwatch.StartNew();
        var response = await RestClient.ExecuteAsync(request);
        sw.Stop();

        if (response.ErrorException is not null)
        {
            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(BeginBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({response.ErrorException.GetType()}): {response.ErrorException.Message} - {response.ErrorException.StackTrace}");

            throw new InvalidOperationException($"[{nameof(BeginBreakAsync)}] {string.Format(Resources.Error_BeginBreak, res.Code, response.ErrorException.Message)}", response.ErrorException);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(BeginBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <-");
                    return;
                }

            default:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(BeginBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response ({response.StatusCode}): {response.Content}");

                    throw new InvalidOperationException($"[{nameof(BeginBreakAsync)}] {Resources.InvalidResponse}");
                }
        }
    }

    /// <inheritdoc/>
    public override async Task EndBreakAsync(string operatorName, string device)
    {
        var request = new RestRequest("Operators/{name}/Break", Method.POST);

        request.AddUrlSegment("name", operatorName);

        if (Settings.ServerLogEnabled)
            ServerLog.WriteLog($"{LogHeader}|{nameof(EndBreakAsync)}|{operatorName}|{device}| -> {request.Resource}");

        var sw = Stopwatch.StartNew();
        var response = await RestClient.ExecuteAsync(request);
        sw.Stop();

        if (response.ErrorException is not null)
        {
            if (Settings.ServerLogEnabled)
                ServerLog.WriteLog($"{LogHeader}|{nameof(EndBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Exception ({response.ErrorException.GetType()}): {response.ErrorException.Message} - {response.ErrorException.StackTrace}");

            throw new InvalidOperationException($"[{nameof(EndBreakAsync)}] {string.Format(Resources.Error_EndBreak, response.ErrorException.Message)}", response.ErrorException);
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(EndBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <-");

                    return;
                }

            default:
                {
                    if (Settings.ServerLogEnabled)
                        ServerLog.WriteLog($"{LogHeader}|{nameof(EndBreakAsync)}|{operatorName}|{device}| ({sw.Elapsed}) <- Invalid response ({response.StatusCode}): {response.Content}");

                    throw new InvalidOperationException($"[{nameof(EndBreakAsync)}] {Resources.InvalidResponse}");
                }
        }
    }
}
