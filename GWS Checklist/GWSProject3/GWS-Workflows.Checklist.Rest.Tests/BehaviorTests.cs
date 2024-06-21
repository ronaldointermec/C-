using Common.Logging;
using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using System.Globalization;
using System.Net;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Rest.Tests;
public class BehaviorTests
{
    public BehaviorTests()
    {
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
    }
    static BehaviorTests()
    {
        var logMock = new Mock<ILog>();
        var loggerMock = new Mock<ILoggerFactoryAdapter>();
        loggerMock.Setup(x => x.GetLogger(It.IsAny<Type>())).Returns(logMock.Object);

        TinyIoC.TinyIoCContainer.Current.Register(loggerMock.Object);
    }
    private static Mock<IConfigService> GetConfigService()
    {
        var serverKey = "Server";
        var logDeviceKey = "Log:Device";

        var serverConfigParamMock = GetConfigParam(serverKey);
        var logDeviceKeyConfigParamMock = GetConfigParam(logDeviceKey);

        var configServiceMock = new Mock<IConfigService>();
        configServiceMock.Setup(x => x.GetAllConfigs(It.IsAny<IConfigCategory>())).Returns(Enumerable.Empty<IConfigParam>().ToList());
        configServiceMock.Setup(x => x.GetOrCreateConfig(serverKey, It.IsAny<IConfigCategory>())).Returns(serverConfigParamMock.Object);
        configServiceMock.Setup(x => x.GetOrCreateConfig(logDeviceKey, It.IsAny<IConfigCategory>())).Returns(logDeviceKeyConfigParamMock.Object);
        return configServiceMock;
    }

    private static Mock<IConfigParam> GetConfigParam(string key)
    {
        var configParamMock = new Mock<IConfigParam>();

        configParamMock.SetupGet(x => x.Key).Returns(key);

        return configParamMock;
    }

    [Fact]
    public void Initialize_ServerNullOrEmpty_ThrowsException()
    {
        Mock<IConfigService> configServiceMock = GetConfigService();

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { }, new Mock<JsonConverter>().Object) { CallBase = true };

        behaviorMock.Invoking(x => x.Object.Initialize()).Should().Throw<InvalidOperationException>().WithMessage(Resources.ServerEmpty);
    }

    [Fact]
    public void Initialize_WithUrl_Success()
    {
        Mock<IConfigService> configServiceMock = GetConfigService();

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object);
        var behavior = behaviorMock.Object;
        behavior.Initialize();
        behavior.Should().NotBeNull();
    }

    [Fact]
    public void GetChecklist_Success()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";

        var code = "1";

        var restResponseMock = new Mock<IRestResponse<Models.Checklist?>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var data = new Models.Checklist()
        {
            Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist messages" },
                    new Message { Code = "002", Prompt = "Priority message", Priority = true },
                    new Message { Code = "003", Prompt = "Message with confirmation", ReadyToContinue = true },
                    new Message { Code = "004", Prompt = "Message with additional information", ReadyToContinue = true, AdditionalInformation = "Additional information" },
                    new Message { Code = "005", Prompt = "Message that can be omitted", ReadyToContinue = true, SkipAllowed = true },
                },
        };
        restResponseMock.SetupGet(x => x.Data).Returns(data);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Models.Checklist?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.RetrieveChecklist(operatorName, code);

        res.Should().BeEquivalentTo(data);
    }

    [Fact]
    public void GetChecklist_Null()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";

        var code = "1";

        var restResponseMock = new Mock<IRestResponse<Models.Checklist?>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Models.Checklist?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object);

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.RetrieveChecklist(operatorName, code);

        res.Should().Be(null);
    }

    [Fact]
    public void GetChecklist_NotFound()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";
        var deviceMock = new Mock<IDevice>();

        var code = "1";

        var restResponseMock = new Mock<IRestResponse<Models.Checklist?>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.NotFound);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Models.Checklist?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object);

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.RetrieveChecklist(operatorName, code);

        res.Should().Be(null);
    }
    [Fact]
    public void GetChecklist_DefaultRequest()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";

        var code = "1";

        var restResponseMock = new Mock<IRestResponse<Models.Checklist?>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Conflict);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Models.Checklist?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        behavior.Invoking(x => x.RetrieveChecklist(operatorName, code)).Should().Throw<InvalidOperationException>().WithMessage($"[RetrieveChecklist] {string.Format(Resources.StatusCodeDefault, HttpStatusCode.Conflict)}");
    }

    [Fact]
    public void GetChecklist_ResponseException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";

        var code = "1";

        var restResponseMock = new Mock<IRestResponse<Models.Checklist?>>();
        restResponseMock.SetupGet(x => x.ErrorException).Returns(new InvalidOperationException());

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Models.Checklist?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        behavior.Invoking(x => x.RetrieveChecklist(operatorName, code)).Should().Throw<InvalidOperationException>();
    }


    [Fact]
    public void GetOperator_Success()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";

        var restResponseMock = new Mock<IRestResponse<Operator?>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var data = new Operator()
        {
            Name = "user",
            Password = null
        };
        restResponseMock.SetupGet(x => x.Data).Returns(data);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Operator?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.GetOperator(operatorName);

        res.Should().BeEquivalentTo(data);


    }
    [Fact]
    public void GetOperator_Null()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";

        var restResponseMock = new Mock<IRestResponse<Operator?>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Operator?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object);

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.GetOperator(operatorName);

        res.Should().Be(null);


    }

    [Fact]
    public void GetOperator_NotFound()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";

        var restResponseMock = new Mock<IRestResponse<Operator?>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.NotFound);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Operator?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object);

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = behavior.GetOperator(operatorName);

        res.Should().Be(null);
    }

    [Fact]
    public void GetOperator_DefaultRequest()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";

        var restResponseMock = new Mock<IRestResponse<Operator?>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Conflict);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Operator?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        behavior.Invoking(x => x.GetOperator(operatorName)).Should().Throw<InvalidOperationException>().WithMessage($"[GetOperator] {string.Format(Resources.StatusCodeDefault, HttpStatusCode.Conflict)}");
    }

    [Fact]
    public void GetOperator_ResponseException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        Mock<IConfigService> configServiceMock = GetConfigService();

        var operatorName = "user";

        var restResponseMock = new Mock<IRestResponse<Operator?>>();
        restResponseMock.SetupGet(x => x.ErrorException).Returns(new InvalidOperationException());

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.Execute<Operator?>(It.IsAny<RestRequest>())).Returns(restResponseMock.Object);

        var behaviorMock = new Mock<Behavior>(new ConnectorBehaviorSettingsBase(configServiceMock.Object) { Server = "https://some-url.com" }, new Mock<JsonConverter>().Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        behavior.Invoking(x => x.GetOperator(operatorName)).Should().Throw<InvalidOperationException>();
    }
}
