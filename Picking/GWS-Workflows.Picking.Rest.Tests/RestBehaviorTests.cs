using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using System.Globalization;
using System.Net;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Tests;

public class RestBehaviorTests
{
    [Fact]
    public void Initialize_ServerNullOrEmpty_ThrowsException()
    {
        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService(string.Empty);
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), new Mock<IServerLog>().Object, new Mock<JsonConverter>().Object) { CallBase = true };

        behaviorMock.Invoking(x => x.Object.Initialize()).Should().Throw<InvalidOperationException>().WithMessage(Resources.ServerEmpty);
    }

    [Fact]
    public void Initialize_WithWrongServer_ThrowsException()
    {
        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService("not-a-url");
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), new Mock<IServerLog>().Object, new Mock<JsonConverter>().Object) { CallBase = true };

        behaviorMock.Invoking(x => x.Object.Initialize()).Should().Throw<UriFormatException>();
    }

    [Fact]
    public void Initialize_WithUrl_Success()
    {
        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        var behavior = behaviorMock.Object;
        behavior.Initialize();
        behavior.RestClient.Should().NotBeNull();
    }

    [Fact]
    public async Task ConnectToServerAsync_WithException_ReturnsNotAllowed()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<ConnectModel>>();
        var ex = new InvalidOperationException("An exception");
        restResponseMock.SetupGet(x => x.ErrorException).Returns(ex);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<ConnectModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.ConnectAsync(operatorName, deviceMock.Object);

        res.Should().NotBeNull();
        res.Allowed.Should().BeFalse();
        res.Password.Should().BeNull();
        res.Message.Should().Be(Resources.Error);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<- Exception (System.InvalidOperationException): An exception - "))));
    }

    [Fact]
    public async Task ConnectToServerAsync_NotFound_ReturnsNotAllowed()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<ConnectModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.NotFound);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<ConnectModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.ConnectAsync(operatorName, deviceMock.Object);

        res.Should().NotBeNull();
        res.Allowed.Should().BeFalse();
        res.Password.Should().BeNull();
        res.Message.Should().Be(string.Format(Resources.UserNotFound, operatorName));

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<- User 'user' not found"))));
    }

    [Fact]
    public async Task ConnectToServerAsync_InvalidResponse_ReturnsNotAllowed()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<ConnectModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.InternalServerError);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<ConnectModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.ConnectAsync(operatorName, deviceMock.Object);

        res.Should().NotBeNull();
        res.Allowed.Should().BeFalse();
        res.Password.Should().BeNull();
        res.Message.Should().Be(Resources.InvalidResponse);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<- Invalid response (InternalServerError): "))));
    }

    [Fact]
    public async Task ConnectToServerAsync_InvalidResponseContent_ReturnsNotAllowed()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<ConnectModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<ConnectModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.ConnectAsync(operatorName, deviceMock.Object);

        res.Should().NotBeNull();
        res.Allowed.Should().BeFalse();
        res.Password.Should().BeNull();
        res.Message.Should().Be(Resources.InvalidResponseContent);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<- Invalid response content (): "))));
    }

    [Fact]
    public async Task ConnectToServerAsync_Success()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<ConnectModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var data = new ConnectModel("User", true, "1234", "Welcome user!");
        restResponseMock.SetupGet(x => x.Data).Returns(data);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<ConnectModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.ConnectAsync(operatorName, deviceMock.Object);

        res.Should().NotBeNull();
        res.Allowed.Should().Be(data.Allowed);
        res.Password.Should().Be(data.Pwd);
        res.Message.Should().Be(data.Message);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|ConnectAsync|user|9999999999|") && x.EndsWith("<-"))));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DisconnectAsync_WithException_ThrowsException(bool force)
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<DisconnectModel>>();
        var ex = new InvalidOperationException("An exception");
        restResponseMock.SetupGet(x => x.ErrorException).Returns(ex);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<DisconnectModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.DisconnectAsync(operatorName, deviceMock.Object.DeviceID, force)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[DisconnectAsync] {string.Format(Resources.Error_Disconnect, ex.Message)}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Disconnect"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("<- Exception (System.InvalidOperationException): An exception - "))));

    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DisconnectAsync_InvalidResponse_ThrowsException(bool force)
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<DisconnectModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.BadRequest);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<DisconnectModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.DisconnectAsync(operatorName, deviceMock.Object.DeviceID, force)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[DisconnectAsync] {Resources.InvalidResponse}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Disconnect"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("<- Invalid response (BadRequest): "))));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DisconnectAsync_InvalidResponseContent_ThrowsException(bool force)
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<DisconnectModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<DisconnectModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.DisconnectAsync(operatorName, deviceMock.Object.DeviceID, force)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[DisconnectAsync] {Resources.InvalidResponseContent}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Disconnect"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("<- Invalid response content (): "))));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DisconnectAsync_Success(bool force)
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<DisconnectModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var data = new DisconnectModel(false, "You can't leave!");
        restResponseMock.SetupGet(x => x.Data).Returns(data);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<DisconnectModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.DisconnectAsync(operatorName, deviceMock.Object.DeviceID, force);
        res.Should().NotBeNull();
        res.Allowed.Should().Be(force);
        res.Message.Should().Be(data.Message);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Disconnect"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|DisconnectAsync|user|9999999999|") && x.EndsWith("<-"))));
    }

    [Fact]
    public async Task RegisterOperatorStartAsync_WithException_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<RegisterOperatorModel>>();
        var ex = new InvalidOperationException("An exception");
        restResponseMock.SetupGet(x => x.ErrorException).Returns(ex);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<RegisterOperatorModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.RegisterOperatorStartAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[RegisterOperatorStartAsync] {string.Format(Resources.Error_RegisterOperator, ex.Message)}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("<- Exception (System.InvalidOperationException): An exception - "))));

    }

    [Fact]
    public async Task RegisterOperatorStartAsync_InvalidResponse_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<RegisterOperatorModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Conflict);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<RegisterOperatorModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.RegisterOperatorStartAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[RegisterOperatorStartAsync] {Resources.InvalidResponse}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("<- Invalid response (Conflict): "))));
    }

    [Fact]
    public async Task RegisterOperatorStartAsync_InvalidResponseContent_ReturnsNull()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<RegisterOperatorModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<RegisterOperatorModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.RegisterOperatorStartAsync(operatorName, deviceMock.Object.DeviceID);
        res.Should().BeNull();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|RegisterOperatorStartAsync|user|9999999999|") && x.EndsWith("<- Invalid response content (): "))));
    }

    [Fact]
    public async Task RegisterOperatorStartAsync_Success()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<RegisterOperatorModel>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var data = new RegisterOperatorModel("A message for the operator");
        restResponseMock.SetupGet(x => x.Data).Returns(data);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<RegisterOperatorModel>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.RegisterOperatorStartAsync(operatorName, deviceMock.Object.DeviceID);

        res.Should().Be(data.Message);
    }

    [Fact]
    public async Task BeginBreakAsync_WithException_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse>();
        var ex = new InvalidOperationException("An exception");
        restResponseMock.SetupGet(x => x.ErrorException).Returns(ex);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.BeginBreakAsync(operatorName, deviceMock.Object.DeviceID, new BeginBreak("1234"))).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[BeginBreakAsync] {string.Format(Resources.Error_BeginBreak, "1234", ex.Message)}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Break"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("<- Exception (System.InvalidOperationException): An exception - "))));

    }


    [Fact]
    public async Task BeginBreakAsync_InvalidResponse_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.TemporaryRedirect);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.BeginBreakAsync(operatorName, deviceMock.Object.DeviceID, new BeginBreak(""))).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[BeginBreakAsync] {Resources.InvalidResponse}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Break"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("<- Invalid response (TemporaryRedirect): "))));
    }

    [Fact]
    public async Task BeginBreakAsync_Success()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.BeginBreakAsync(operatorName, deviceMock.Object.DeviceID, new BeginBreak(""))).Should().NotThrowAsync();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Break"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|BeginBreakAsync|user|9999999999|") && x.EndsWith("<-"))));
    }

    [Fact]
    public async Task EndBreakAsync_WithException_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse>();
        var ex = new InvalidOperationException("An exception");
        restResponseMock.SetupGet(x => x.ErrorException).Returns(ex);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.EndBreakAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[EndBreakAsync] {string.Format(Resources.Error_EndBreak, ex.Message)}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Break"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("<- Exception (System.InvalidOperationException): An exception - "))));

    }


    [Fact]
    public async Task EndBreakAsync_InvalidResponse_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.TemporaryRedirect);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.EndBreakAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[EndBreakAsync] {Resources.InvalidResponse}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Break"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("<- Invalid response (TemporaryRedirect): "))));
    }

    [Fact]
    public async Task EndBreakAsync_Success()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<RestBehavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.EndBreakAsync(operatorName, deviceMock.Object.DeviceID)).Should().NotThrowAsync();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Break"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|EndBreakAsync|user|9999999999|") && x.EndsWith("<-"))));
    }
}