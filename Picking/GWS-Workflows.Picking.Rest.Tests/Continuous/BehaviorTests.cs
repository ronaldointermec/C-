using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Continuous;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using System.Globalization;
using System.Net;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Tests.Continuous;

public class BehaviorTests
{
    [Fact]
    public async Task GetWorkOrderAsync_WithException_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IGetWorkOrderItem>>();
        var ex = new InvalidOperationException("An exception");
        restResponseMock.SetupGet(x => x.ErrorException).Returns(ex);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IGetWorkOrderItem>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.GetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[GetWorkOrderAsync] {Resources.Error}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> Work"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Exception (System.InvalidOperationException): An exception - "))));
    }

    [Fact]
    public async Task GetWorkOrderAsync_InvalidResponse_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IGetWorkOrderItem>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IGetWorkOrderItem>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.GetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[GetWorkOrderAsync] {Resources.InvalidResponse}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> Work"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Invalid response (Unauthorized): "))));
    }

    [Fact]
    public async Task GetWorkOrdersAsync_InvalidResponseContent_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IGetWorkOrderItem>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IGetWorkOrderItem>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.GetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[GetWorkOrderAsync] {Resources.InvalidResponseContent}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> Work"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Invalid response content (): "))));
    }

    [Fact]
    public async Task GetWorkOrderAsync_Success()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IGetWorkOrderItem>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var data = new BeginPickingOrder("001", "Picking order 001");
        restResponseMock.SetupGet(x => x.Data).Returns(data);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IGetWorkOrderItem>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.GetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID);

        res.Should().BeEquivalentTo(data);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|GetWorkOrderAsync|user|9999999999|") && x.EndsWith("-> Work"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|GetWorkOrderAsync|user|9999999999|") && x.Contains("<-"))));
    }

    [Fact]
    public async Task SetWorkOrdersAsync_WithException_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IGetWorkOrderItem>>();
        var ex = new InvalidOperationException("An exception");
        restResponseMock.SetupGet(x => x.ErrorException).Returns(ex);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IGetWorkOrderItem>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res1 = new BeginPickingOrderResult("001");
        var res2 = new PickingLineResult("002");
        var res3 = new PlaceInDockResult("003");
        var res4 = new AskQuestionResult("004");
        var res5 = new PrintLabelsResult("005");
        var res6 = new ValidatePrintingResult("006");

        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res1)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res2)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res3)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res4)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res5)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res6)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> Work/{code}"))), Times.Exactly(6));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.Contains("<- Exception (System.InvalidOperationException): An exception - "))), Times.Exactly(6));

    }

    [Fact]
    public async Task SetWorkOrderAsync_InvalidResponse_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IGetWorkOrderItem>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IGetWorkOrderItem>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res1 = new BeginPickingOrderResult("001");
        var res2 = new PickingLineResult("002");
        var res3 = new PlaceInDockResult("003");
        var res4 = new AskQuestionResult("004");
        var res5 = new PrintLabelsResult("005");
        var res6 = new ValidatePrintingResult("006");

        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res1)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res2)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res3)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res4)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res5)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res6)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> Work/{code}"))), Times.Exactly(6));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Invalid response (Unauthorized): "))), Times.Exactly(6));
    }

    [Fact]
    public async Task SetWorkOrderAsync_InvalidResponseContent_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IGetWorkOrderItem>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);
        restResponseMock.SetupGet(x => x.Content).Returns("some content");

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IGetWorkOrderItem>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res1 = new BeginPickingOrderResult("001");
        var res2 = new PickingLineResult("002");
        var res3 = new PlaceInDockResult("003");
        var res4 = new AskQuestionResult("004");
        var res5 = new PrintLabelsResult("005");
        var res6 = new ValidatePrintingResult("006");

        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res1)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponseContent}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res2)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponseContent}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res3)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponseContent}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res4)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponseContent}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res5)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponseContent}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res6)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponseContent}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> Work/{code}"))), Times.Exactly(6));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Invalid response content (): some content"))), Times.Exactly(6));
    }

    [Fact]
    public async Task SetWorkOrderAsync_EmptyResponseContent_ReturnsNull()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IGetWorkOrderItem>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IGetWorkOrderItem>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res1 = new BeginPickingOrderResult("001");
        var res2 = new PickingLineResult("002");
        var res3 = new PlaceInDockResult("003");
        var res4 = new AskQuestionResult("004");
        var res5 = new PrintLabelsResult("005");
        var res6 = new ValidatePrintingResult("006");

        (await behavior.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res1)).Should().BeNull();
        (await behavior.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res2)).Should().BeNull();
        (await behavior.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res3)).Should().BeNull();
        (await behavior.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res4)).Should().BeNull();
        (await behavior.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res5)).Should().BeNull();
        (await behavior.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res6)).Should().BeNull();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> Work/{code}"))), Times.Exactly(6));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Invalid response content (): "))), Times.Exactly(6));
    }

    [Fact]
    public async Task SetWorkOrderAsync_Success()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IGetWorkOrderItem>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);
        restResponseMock.Setup(x => x.Data).Returns(new BeginPickingOrder("100", null));

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IGetWorkOrderItem>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res1 = new BeginPickingOrderResult("001");
        var res2 = new PickingLineResult("002");
        var res3 = new PlaceInDockResult("003");
        var res4 = new AskQuestionResult("004");
        var res5 = new PrintLabelsResult("005");
        var res6 = new ValidatePrintingResult("006");

        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res1)).Should().NotThrowAsync();
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res2)).Should().NotThrowAsync();
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res3)).Should().NotThrowAsync();
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res4)).Should().NotThrowAsync();
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res5)).Should().NotThrowAsync();
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res6)).Should().NotThrowAsync();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> Work/{code}"))), Times.Exactly(6));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Continuous|SetWorkOrderAsync|user|9999999999|") && x.Contains("<-"))), Times.Exactly(6));
    }
}