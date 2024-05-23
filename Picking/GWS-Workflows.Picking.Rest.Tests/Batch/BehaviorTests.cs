using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Batch;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using System.Globalization;
using System.Net;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Rest.Tests.Batch;

public class BehaviorTests
{
    [Fact]
    public async Task GetWorkOrdersAsync_WithException_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IEnumerable<IGetWorkOrderItem>>>();
        var ex = new InvalidOperationException("An exception");
        restResponseMock.SetupGet(x => x.ErrorException).Returns(ex);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IEnumerable<IGetWorkOrderItem>>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.GetWorkOrdersAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[GetWorkOrdersAsync] {Resources.Error}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("-> Work"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("<- Exception (System.InvalidOperationException): An exception - "))));

    }

    [Fact]
    public async Task GetWorkOrdersAsync_InvalidResponse_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IEnumerable<IGetWorkOrderItem>>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IEnumerable<IGetWorkOrderItem>>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.GetWorkOrdersAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[GetWorkOrdersAsync] {Resources.InvalidResponse}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("-> Work"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("<- Invalid response (Unauthorized): "))));
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

        var restResponseMock = new Mock<IRestResponse<IEnumerable<IGetWorkOrderItem>>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);
        restResponseMock.SetupGet(x => x.Data).Returns((IEnumerable<IGetWorkOrderItem>)null!);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IEnumerable<IGetWorkOrderItem>>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        await behavior.Awaiting(x => x.GetWorkOrdersAsync(operatorName, deviceMock.Object.DeviceID)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[GetWorkOrdersAsync] {Resources.InvalidResponseContent}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("-> Work"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("<- Invalid response content (): "))));
    }

    [Fact]
    public async Task GetWorkOrdersAsync_Success()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IEnumerable<IGetWorkOrderItem>>>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var data = new IGetWorkOrderItem[] { new BeginPickingOrder("001", "Picking order 001"), new PickingLine("001-01", null!), new PlaceInDock("001-End") };
        restResponseMock.SetupGet(x => x.Data).Returns(data);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync<IEnumerable<IGetWorkOrderItem>>(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = await behavior.GetWorkOrdersAsync(operatorName, deviceMock.Object.DeviceID);

        res.Should().BeEquivalentTo(data);

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|GetWorkOrdersAsync|user|9999999999|") && x.EndsWith("-> Work"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|GetWorkOrdersAsync|user|9999999999|") && x.Contains("<-"))));
    }

    [Fact]
    public async Task SetWorkOrderAsync_WithException_ThrowsException()
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
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res1 = new BeginPickingOrderResult("001");
        var res2 = new PickingLineResult("002");
        var res3 = new PlaceInDockResult("003");
        var res4 = new AskQuestionResult("004");

        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res1)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res2)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res3)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res4)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.Error}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> Work/{code}"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|SetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Exception (System.InvalidOperationException): An exception - "))));

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

        var restResponseMock = new Mock<IRestResponse>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res1 = new BeginPickingOrderResult("001");
        var res2 = new PickingLineResult("002");
        var res3 = new PlaceInDockResult("003");
        var res4 = new AskQuestionResult("004");

        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res1)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res2)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res3)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res4)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[SetWorkOrderAsync] {Resources.InvalidResponse}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> Work/{code}"))), Times.Exactly(4));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|SetWorkOrderAsync|user|9999999999|") && x.EndsWith("<- Invalid response (Unauthorized): "))), Times.Exactly(4));
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

        var restResponseMock = new Mock<IRestResponse>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res1 = new BeginPickingOrderResult("001");
        var res2 = new PickingLineResult("002");
        var res3 = new PlaceInDockResult("003");
        var res4 = new AskQuestionResult("004");

        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res1)).Should().NotThrowAsync();
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res2)).Should().NotThrowAsync();
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res3)).Should().NotThrowAsync();
        await behavior.Awaiting(x => x.SetWorkOrderAsync(operatorName, deviceMock.Object.DeviceID, res4)).Should().NotThrowAsync();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|SetWorkOrderAsync|user|9999999999|") && x.Contains("-> Work/{code}"))), Times.Exactly(4));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|SetWorkOrderAsync|user|9999999999|") && x.EndsWith("<-"))), Times.Exactly(4));
    }

    [Fact]
    public async Task PrintLabelsBatchAsync_WithException_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse<IEnumerable<IGetWorkOrderItem>>>();
        var ex = new InvalidOperationException("An exception");
        restResponseMock.SetupGet(x => x.ErrorException).Returns(ex);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = new PrintLabelsBatch("001");
        await behavior.Awaiting(x => x.PrintLabelsBatchAsync(operatorName, deviceMock.Object.DeviceID, res)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[PrintLabelsBatchAsync] {string.Format(Resources.Error_PrintLabels, res.Code, ex.Message)}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|PrintLabelsBatchAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Print"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|PrintLabelsBatchAsync|user|9999999999|") && x.EndsWith("<- Exception (System.InvalidOperationException): An exception - "))));
    }

    [Fact]
    public async Task PrintLabelsBatchAsync_InvalidResponse_ThrowsException()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var logMock = new Mock<IServerLog>();
        var jsonConverter = new Mock<JsonConverter>();

        var deviceMock = new Mock<IDevice>();
        deviceMock.Setup(x => x.Culture).Returns(CultureInfo.CurrentUICulture);
        deviceMock.Setup(x => x.DeviceID).Returns("9999999999");

        var restResponseMock = new Mock<IRestResponse>();
        restResponseMock.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.BadRequest);

        var restClientMock = new Mock<IRestClient>();
        restClientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), default(CancellationToken))).ReturnsAsync(restResponseMock.Object);

        Mock<IConfigService> configServiceMock = ConfigMock.GetConfigService();
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = new PrintLabelsBatch("001");
        await behavior.Awaiting(x => x.PrintLabelsBatchAsync(operatorName, deviceMock.Object.DeviceID, res)).Should().ThrowAsync<InvalidOperationException>().WithMessage($"[PrintLabelsBatchAsync] {Resources.InvalidResponse}");

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|PrintLabelsBatchAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Print"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|PrintLabelsBatchAsync|user|9999999999|") && x.EndsWith("<- Invalid response (BadRequest): "))));
    }

    [Fact]
    public async Task PrintLabelsBatchAsync_Success()
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
        var behaviorMock = new Mock<Behavior>(new PickingBehaviorSettings(configServiceMock.Object), logMock.Object, jsonConverter.Object) { CallBase = true };

        behaviorMock.SetupGet(x => x.RestClient).Returns(restClientMock.Object);

        var behavior = behaviorMock.Object;

        var res = new PrintLabelsBatch("001");
        await behavior.Awaiting(x => x.PrintLabelsBatchAsync(operatorName, deviceMock.Object.DeviceID, res)).Should().NotThrowAsync();

        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|PrintLabelsBatchAsync|user|9999999999|") && x.EndsWith("-> Operators/{name}/Print"))));
        logMock.Verify(x => x.WriteLog(It.Is<string>(x => x.StartsWith("Rest|Behavior|Batch|PrintLabelsBatchAsync|user|9999999999|") && x.Contains("<-"))));
    }
}