// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Modules;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Rest
{
    /// <summary>
    /// An implementation of <see cref="ConnectorBehavior"></see> based on Rest.
    /// </summary>
    public class Behavior : ConnectorBehavior
    {
#if NETFRAMEWORK
        /// <summary>
        /// Initializes a new instance of the <see cref="Behavior"/> class.
        /// </summary>
        /// <param name="settings">Settings instance.</param>
        public Behavior(ConnectorBehaviorSettingsBase settings)
            : base(settings)
        {
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="Behavior"/> class.
        /// </summary>
        /// <param name="settings">Settings instance.</param>
        /// <param name="converter">JSON Converter.</param>
        public Behavior(ConnectorBehaviorSettingsBase settings, JsonConverter converter)
            : base(settings)
        {
            Converter = converter;
        }

        /// <summary>
        /// Gets REST client used to perform requests to remote server.
        /// </summary>
        internal virtual IRestClient RestClient { get; private set; } = null!;

        /// <summary>
        /// Gets database parser for extracting data.
        /// </summary>
#if NETFRAMEWORK
        protected virtual JsonConverter Converter { get; } = new QuestionJsonConverter();
#else
        protected JsonConverter Converter { get; }
#endif

        /// <inheritdoc/>
        public override void Initialize()
        {
            base.Initialize();

            if (string.IsNullOrEmpty(Settings.Server))
                throw new InvalidOperationException(Resources.ServerEmpty);

            RestClient = new RestClient(Settings.Server)
                .UseJson()
                .UseNewtonsoftJson(new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { Converter },
                    NullValueHandling = NullValueHandling.Ignore,
                });
        }

        /// <inheritdoc/>
        public override Operator? GetOperator(string @operator)
        {
            var request = new RestRequest("Operators/{operator}", Method.GET);

            request.AddUrlSegment("operator", @operator);

            var response = RestClient.Execute<Operator?>(request);
            if (response.ErrorException is not null)
                throw response.ErrorException;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    if (response.Data == null)
                    {
                        if (Settings.DeviceLogEnabled)
                            Log(string.Format(Resources.GetOperator_Null, response.Content), LogLevel.Error);

                        return null;
                    }

                    return response.Data;
                case HttpStatusCode.NotFound:
                    if (Settings.DeviceLogEnabled)
                        Log(string.Format(Resources.GetOperator_Null, response.Content), LogLevel.Error);

                    return null;
                default:
                    if (Settings.DeviceLogEnabled)
                        Log(string.Format(Resources.StatusCodeDefault, response.Content), LogLevel.Error);

                    throw new InvalidOperationException($"[{nameof(GetOperator)}] {string.Format(Resources.StatusCodeDefault, response.StatusCode)}");
            }
        }

        /// <inheritdoc/>
        public override Models.Checklist? RetrieveChecklist(string @operator, string id)
        {
            var request = new RestRequest("Checklist", Method.GET);

            request.AddQueryParameter("code", id);

            var response = RestClient.Execute<Models.Checklist?>(request);

            if (response.ErrorException is not null)
                throw response.ErrorException;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    if (response.Data == null)
                    {
                        if (Settings.DeviceLogEnabled)
                            Log(string.Format(Resources.Checklist_Null, response.Content), LogLevel.Error);

                        return null;
                    }

                    return response.Data;
                case HttpStatusCode.NotFound:
                    if (Settings.DeviceLogEnabled)
                        Log(string.Format(Resources.Checklist_Null, response.Content), LogLevel.Error);

                    return null;
                default:
                    if (Settings.DeviceLogEnabled)
                        Log(string.Format(Resources.StatusCodeDefault, response.Content), LogLevel.Error);

                    throw new InvalidOperationException($"[{nameof(RetrieveChecklist)}] {string.Format(Resources.StatusCodeDefault, response.StatusCode)}");
            }
        }

        /// <inheritdoc/>
        public override void UpdateChecklist(string @operator, string id, Models.Checklist checklist)
        {
            var request = new RestRequest("Checklist", Method.POST);

            request.AddQueryParameter("code", id);
            request.AddJsonBody(checklist);

            var sw = Stopwatch.StartNew();
            var response = RestClient.Execute(request);
            sw.Stop();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"[{nameof(UpdateChecklist)}] {Resources.Behavior_NotFound}");
        }
    }
}
