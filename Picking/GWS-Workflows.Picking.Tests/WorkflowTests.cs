using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Properties;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Tests.Data;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using Moq;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Tests
{
    public class WorkflowTests
    {
        private readonly Workflow _Workflow;
        private readonly Mock<IPickingBehavior> _behaviorMock;

        public WorkflowTests()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            _behaviorMock = new Mock<IPickingBehavior>();

            _Workflow = new Workflow(_behaviorMock.Object);
        }

        [Fact]
        public void PluginWithoutBehavior_ThrowException()
        {
            new Action(() => _ = new Workflow(null!)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetAnchorWords_ReturnsEmptyString()
        {
            var mockDevice = new Mock<IDevice>();
            mockDevice.SetupGet(x => x.Language).Returns("en-US");

            var anchor = await _Workflow.GetAnchorWordsAsync(mockDevice.Object);
            var res = anchor.PrepareResponse();
            res.Should().Be(@"

");
        }

        [Fact]
        public async Task GetVariablesSet()
        {
            var res = await _Workflow.GetVariableSetAsync(null!, new Mock<IDevice>().Object);
            res.PrepareResponse().Should().Be(
                @"DLG,""00"",1
RESPONSETYPE,,0
MENU,,0
BATCH,,0
CODE,,0
STATUS,,0
START_TIME,,0
END_TIME,,0
PICKED,,0
SERVING,,0
WEIGHT,,0
STOCK,,0
DOCK,,0
WHEREAMI,,0
PRODUCT_DESCRIPTION,,0
PRODUCT_NUMBER,,0
UPC_NUMBER,,0
PROMPT,,0
QTY_REQUESTED,,0
QTY_REQUESTED_ORIG,,0
MAX_QTY_ALLOWED_PER_PICK,,0
TOTAL_WEIGHT,,0
TOTAL_PICKED,,0
QTY_UPPER,,0
QTY_LOWER,,0
DUMMY,,0
CURRENT_AISLE,,0
CURRENT_POSITION,,0
LABELS,,0
LABELS_COUNT,,0
SKIPPING_AISLE,,0
PRODUCT_CD,,0
BREAKAGE,,0
CUSTOMER,,0
PRINTER,,0
BREAK_REASON,,0
LABELS_1_LABEL,,0
LABELS_1_CODE,,0
LABELS_1_VALIDATED,,0
LABELS_2_LABEL,,0
LABELS_2_CODE,,0
LABELS_2_VALIDATED,,0
LABELS_3_LABEL,,0
LABELS_3_CODE,,0
LABELS_3_VALIDATED,,0
LABELS_4_LABEL,,0
LABELS_4_CODE,,0
LABELS_4_VALIDATED,,0
LABELS_5_LABEL,,0
LABELS_5_CODE,,0
LABELS_5_VALIDATED,,0
LABELS_6_LABEL,,0
LABELS_6_CODE,,0
LABELS_6_VALIDATED,,0
LABELS_7_LABEL,,0
LABELS_7_CODE,,0
LABELS_7_VALIDATED,,0
LABELS_8_LABEL,,0
LABELS_8_CODE,,0
LABELS_8_VALIDATED,,0
LABELS_9_LABEL,,0
LABELS_9_CODE,,0
LABELS_9_VALIDATED,,0
LABELS_10_LABEL,,0
LABELS_10_CODE,,0
LABELS_10_VALIDATED,,0
LABELS_11_LABEL,,0
LABELS_11_CODE,,0
LABELS_11_VALIDATED,,0
LABELS_12_LABEL,,0
LABELS_12_CODE,,0
LABELS_12_VALIDATED,,0
LABELS_13_LABEL,,0
LABELS_13_CODE,,0
LABELS_13_VALIDATED,,0
LABELS_14_LABEL,,0
LABELS_14_CODE,,0
LABELS_14_VALIDATED,,0
LABELS_15_LABEL,,0
LABELS_15_CODE,,0
LABELS_15_VALIDATED,,0
LABELS_16_LABEL,,0
LABELS_16_CODE,,0
LABELS_16_VALIDATED,,0
LABELS_17_LABEL,,0
LABELS_17_CODE,,0
LABELS_17_VALIDATED,,0
LABELS_18_LABEL,,0
LABELS_18_CODE,,0
LABELS_18_VALIDATED,,0
LABELS_19_LABEL,,0
LABELS_19_CODE,,0
LABELS_19_VALIDATED,,0
LABELS_20_LABEL,,0
LABELS_20_CODE,,0
LABELS_20_VALIDATED,,0
LABELS_21_LABEL,,0
LABELS_21_CODE,,0
LABELS_21_VALIDATED,,0
LABELS_22_LABEL,,0
LABELS_22_CODE,,0
LABELS_22_VALIDATED,,0
LABELS_23_LABEL,,0
LABELS_23_CODE,,0
LABELS_23_VALIDATED,,0
LABELS_24_LABEL,,0
LABELS_24_CODE,,0
LABELS_24_VALIDATED,,0
LABELS_25_LABEL,,0
LABELS_25_CODE,,0
LABELS_25_VALIDATED,,0
LABELS_26_LABEL,,0
LABELS_26_CODE,,0
LABELS_26_VALIDATED,,0
LABELS_27_LABEL,,0
LABELS_27_CODE,,0
LABELS_27_VALIDATED,,0
LABELS_28_LABEL,,0
LABELS_28_CODE,,0
LABELS_28_VALIDATED,,0
LABELS_29_LABEL,,0
LABELS_29_CODE,,0
LABELS_29_VALIDATED,,0
LABELS_30_LABEL,,0
LABELS_30_CODE,,0
LABELS_30_VALIDATED,,0
LABELS_31_LABEL,,0
LABELS_31_CODE,,0
LABELS_31_VALIDATED,,0
LABELS_32_LABEL,,0
LABELS_32_CODE,,0
LABELS_32_VALIDATED,,0
LABELS_33_LABEL,,0
LABELS_33_CODE,,0
LABELS_33_VALIDATED,,0
LABELS_34_LABEL,,0
LABELS_34_CODE,,0
LABELS_34_VALIDATED,,0
LABELS_35_LABEL,,0
LABELS_35_CODE,,0
LABELS_35_VALIDATED,,0
LABELS_36_LABEL,,0
LABELS_36_CODE,,0
LABELS_36_VALIDATED,,0
LABELS_37_LABEL,,0
LABELS_37_CODE,,0
LABELS_37_VALIDATED,,0
LABELS_38_LABEL,,0
LABELS_38_CODE,,0
LABELS_38_VALIDATED,,0
LABELS_39_LABEL,,0
LABELS_39_CODE,,0
LABELS_39_VALIDATED,,0
LABELS_40_LABEL,,0
LABELS_40_CODE,,0
LABELS_40_VALIDATED,,0
LABELS_41_LABEL,,0
LABELS_41_CODE,,0
LABELS_41_VALIDATED,,0
LABELS_42_LABEL,,0
LABELS_42_CODE,,0
LABELS_42_VALIDATED,,0
LABELS_43_LABEL,,0
LABELS_43_CODE,,0
LABELS_43_VALIDATED,,0
LABELS_44_LABEL,,0
LABELS_44_CODE,,0
LABELS_44_VALIDATED,,0
LABELS_45_LABEL,,0
LABELS_45_CODE,,0
LABELS_45_VALIDATED,,0
LABELS_46_LABEL,,0
LABELS_46_CODE,,0
LABELS_46_VALIDATED,,0
LABELS_47_LABEL,,0
LABELS_47_CODE,,0
LABELS_47_VALIDATED,,0
LABELS_48_LABEL,,0
LABELS_48_CODE,,0
LABELS_48_VALIDATED,,0
LABELS_49_LABEL,,0
LABELS_49_CODE,,0
LABELS_49_VALIDATED,,0
LABELS_50_LABEL,,0
LABELS_50_CODE,,0
LABELS_50_VALIDATED,,0
PRINTER_1_CODE,,0
PRINTER_2_CODE,,0
PRINTER_3_CODE,,0
PRINTER_4_CODE,,0
PRINTER_5_CODE,,0
PRINTER_6_CODE,,0
PRINTER_7_CODE,,0
PRINTER_8_CODE,,0
PRINTER_9_CODE,,0
PRINTER_10_CODE,,0
PRINTER_11_CODE,,0
PRINTER_12_CODE,,0
PRINTER_13_CODE,,0
PRINTER_14_CODE,,0
PRINTER_15_CODE,,0
PRINTER_16_CODE,,0
PRINTER_17_CODE,,0
PRINTER_18_CODE,,0
PRINTER_19_CODE,,0
PRINTER_20_CODE,,0
PRINTER_21_CODE,,0
PRINTER_22_CODE,,0
PRINTER_23_CODE,,0
PRINTER_24_CODE,,0
PRINTER_25_CODE,,0
PRINTER_26_CODE,,0
PRINTER_27_CODE,,0
PRINTER_28_CODE,,0
PRINTER_29_CODE,,0
PRINTER_30_CODE,,0
PRINTER_31_CODE,,0
PRINTER_32_CODE,,0
PRINTER_33_CODE,,0
PRINTER_34_CODE,,0
PRINTER_35_CODE,,0
PRINTER_36_CODE,,0
PRINTER_37_CODE,,0
PRINTER_38_CODE,,0
PRINTER_39_CODE,,0
PRINTER_40_CODE,,0
PRINTER_41_CODE,,0
PRINTER_42_CODE,,0
PRINTER_43_CODE,,0
PRINTER_44_CODE,,0
PRINTER_45_CODE,,0
PRINTER_46_CODE,,0
PRINTER_47_CODE,,0
PRINTER_48_CODE,,0
PRINTER_49_CODE,,0
PRINTER_50_CODE,,0
PRINTER_51_CODE,,0
PRINTER_52_CODE,,0
PRINTER_53_CODE,,0
PRINTER_54_CODE,,0
PRINTER_55_CODE,,0
PRINTER_56_CODE,,0
PRINTER_57_CODE,,0
PRINTER_58_CODE,,0
PRINTER_59_CODE,,0
PRINTER_60_CODE,,0
PRINTER_61_CODE,,0
PRINTER_62_CODE,,0
PRINTER_63_CODE,,0
PRINTER_64_CODE,,0
PRINTER_65_CODE,,0
PRINTER_66_CODE,,0
PRINTER_67_CODE,,0
PRINTER_68_CODE,,0
PRINTER_69_CODE,,0
PRINTER_70_CODE,,0
PRINTER_71_CODE,,0
PRINTER_72_CODE,,0
PRINTER_73_CODE,,0
PRINTER_74_CODE,,0
PRINTER_75_CODE,,0
PRINTER_76_CODE,,0
PRINTER_77_CODE,,0
PRINTER_78_CODE,,0
PRINTER_79_CODE,,0
PRINTER_80_CODE,,0
PRINTER_81_CODE,,0
PRINTER_82_CODE,,0
PRINTER_83_CODE,,0
PRINTER_84_CODE,,0
PRINTER_85_CODE,,0
PRINTER_86_CODE,,0
PRINTER_87_CODE,,0
PRINTER_88_CODE,,0
PRINTER_89_CODE,,0
PRINTER_90_CODE,,0
PRINTER_91_CODE,,0
PRINTER_92_CODE,,0
PRINTER_93_CODE,,0
PRINTER_94_CODE,,0
PRINTER_95_CODE,,0
PRINTER_96_CODE,,0
PRINTER_97_CODE,,0
PRINTER_98_CODE,,0
PRINTER_99_CODE,,0
PRINTER_100_CODE,,0

");
        }

        [Fact]
        public async Task BuildInstructions_WithoutDLG_ThrowException()
        {
            var instructionSet = new InstructionSet();
            var data = new Dictionary<string, string>();

            var mockDevice = new Mock<IDevice>();
            mockDevice.SetupGet(x => x.Language).Returns("en-US");

            await _Workflow.Invoking(async x => await x.BuildInstructionsAsync(instructionSet, data, mockDevice.Object)).Should().ThrowAsync<ArgumentException>().WithMessage(Resources.Error_MissingDeviceState);
        }

        [Fact]
        public async Task BuildInstructions_UnknownDLG_ThrowException()
        {
            var instructionSet = new InstructionSet();
            var data = new Dictionary<string, string>
            {
                { Vars.DLG, "WRONG" }
            };

            var deviceMock = new Mock<IDevice>();
            deviceMock.SetupGet(x => x.Language).Returns("en-US");

            await _Workflow.Invoking(async x => await x.BuildInstructionsAsync(instructionSet, data, deviceMock.Object)).Should().ThrowAsync<ArgumentException>().WithMessage(string.Format(Resources.Error_UnknownDialog, data[Vars.DLG]));
        }

        [Theory]
        [InlineData(ResponseTypes.BeginPickingResult)]
        [InlineData(ResponseTypes.AskQuestionResult)]
        [InlineData(ResponseTypes.PickingLineResult)]
        [InlineData(ResponseTypes.PrintLabelsResult)]
        [InlineData(ResponseTypes.ValidatePrintingResult)]
        [InlineData(ResponseTypes.PlaceInDockResult)]
        public async Task ProcessData_WithoutStartTimedData_LogsError(ResponseTypes responseType)
        {
            /* Arrange */
            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, responseType.ToString() },
                { Vars.CODE, "12345" },
            };

            /* Act */
            await _Workflow.Invoking(async x => await x.ProcessDataAsync(data, new Mock<IDevice>().Object)).Should().NotThrowAsync();

            /* Assert */
            _behaviorMock.Verify(x => x.Log(string.Format(Resources.Error_MissingData, Vars.START_TIME), SDK.LogLevel.Error));
        }

        [Theory]
        [InlineData(ResponseTypes.BeginPickingResult)]
        [InlineData(ResponseTypes.AskQuestionResult)]
        [InlineData(ResponseTypes.PickingLineResult)]
        [InlineData(ResponseTypes.PrintLabelsResult)]
        [InlineData(ResponseTypes.ValidatePrintingResult)]
        [InlineData(ResponseTypes.PlaceInDockResult)]
        public async Task ProcessData_WithoutEndTimedData_LogsError(ResponseTypes responseType)
        {
            /* Arrange */
            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, responseType.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME,"452945320" },
            };

            /* Act */
            await _Workflow.Invoking(async x => await x.ProcessDataAsync(data, new Mock<IDevice>().Object)).Should().NotThrowAsync();

            /* Assert */
            _behaviorMock.Verify(x => x.Log(string.Format(Resources.Error_MissingData, Vars.END_TIME), SDK.LogLevel.Error));
        }

        [Theory]
        [InlineData(ResponseTypes.BeginPickingResult)]
        [InlineData(ResponseTypes.AskQuestionResult)]
        [InlineData(ResponseTypes.PickingLineResult)]
        [InlineData(ResponseTypes.PrintLabelsResult)]
        [InlineData(ResponseTypes.ValidatePrintingResult)]
        [InlineData(ResponseTypes.PlaceInDockResult)]
        public async Task ProcessData_WithoutStatusData_LogsError(ResponseTypes responseType)
        {
            /* Arrange */
            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, responseType.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME,"452945320" },
                { Vars.END_TIME, "452945325" },
            };

            /* Act */
            await _Workflow.Invoking(async x => await x.ProcessDataAsync(data, new Mock<IDevice>().Object)).Should().NotThrowAsync();

            /* Assert */
            _behaviorMock.Verify(x => x.Log(string.Format(Resources.Error_MissingData, Vars.STATUS), SDK.LogLevel.Error));
        }

        [Fact]
        public async Task ProcessData_WithoutResponseTypeData_ReturnsNullAndLogError()
        {
            /* Arrange */
            var data = new Dictionary<string, string>();

            /* Act */
            await _Workflow.Invoking(async x => await x.ProcessDataAsync(data, new Mock<IDevice>().Object)).Should().NotThrowAsync();

            /* Assert */
            _behaviorMock.Verify(x => x.Log(string.Format(Resources.Error_MissingData, Vars.RESPONSETYPE), SDK.LogLevel.Error));
        }

        [Fact]
        public async Task ProcessData_WithoutCodeData_ReturnsNullAndLogError()
        {
            /* Arrange */
            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.BeginPickingResult.ToString() }
            };

            /* Act */
            await _Workflow.Invoking(async x => await x.ProcessDataAsync(data, new Mock<IDevice>().Object)).Should().NotThrowAsync();

            /* Assert */
            _behaviorMock.Verify(x => x.Log(string.Format(Resources.Error_MissingData, Vars.CODE), SDK.LogLevel.Error));
        }

        [Fact]
        public async Task ProcessData_LabelsBatchResultWithoutLabelsData_ReturnsNullAndLogError()
        {
            /* Arrange */
            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PrintLabelsBatchResult.ToString() },
                { Vars.CODE, "12345" },
            };

            /* Act */
            await _Workflow.Invoking(async x => await x.ProcessDataAsync(data, new Mock<IDevice>().Object)).Should().NotThrowAsync();

            /* Assert */
            _behaviorMock.Verify(x => x.Log(string.Format(Resources.Error_MissingData, Vars.LABELS), SDK.LogLevel.Error));
        }

        [Fact]
        public async Task ProcessData_AskQuestionResultInBatchBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.AskQuestionResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingBatchBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<AskQuestionResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime)));
        }

        [Fact]
        public async Task ProcessData_BeginPickingResultInBatchBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.BeginPickingResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingBatchBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<BeginPickingOrderResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime)));
        }

        [Fact]
        public async Task ProcessData_PickingLineResultWithoutOptionalDataInBatchBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PickingLineResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingBatchBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PickingLineResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime && x.Picked == null && x.ServingCode == null && x.Weight == null && x.Stock == null && x.Dock == null && x.ProductCD == null && x.Breakage == null && x.Batch == null)));
        }

        [Fact]
        public async Task ProcessData_PickingLineResultInhBatchBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PickingLineResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
                { Vars.TOTAL_PICKED, "1" },
                { Vars.SERVING, "UNITS" },
                { Vars.TOTAL_WEIGHT, "2.5" },
                { Vars.STOCK, "3" },
                { Vars.DOCK, "Dock 1" },
                { Vars.PRODUCT_CD, "123" },
                { Vars.BREAKAGE, "4" },
                { Vars.BATCH, "12345" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingBatchBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PickingLineResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime && x.Picked == 1 && x.ServingCode == "UNITS" && x.Weight == 2.5M && x.Stock == 3 && x.Dock == "Dock 1" && x.ProductCD == "123" && x.Breakage == 4 && x.Batch == "12345")));
        }

        [Fact]
        public async Task ProcessData_PrintLabelsBatchResultDataInBatchBehavior_Success()
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PrintLabelsBatchResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.STATUS, "OK" },
                { Vars.LABELS, "1" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.As<IPickingBatchBehavior>().Verify(x => x.PrintLabelsBatchAsync(@operator, deviceId,
                It.Is<PrintLabelsBatch>(x => x.Code == "12345" && x.Count == 1)));
        }

        [Fact]
        public async Task ProcessData_PrintLabelsBatchResultInContinuousBehavior_LogError()
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PrintLabelsBatchResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.STATUS, "OK" },
                { Vars.LABELS, "1" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.Verify(x => x.Log(string.Format(Resources.Error_IncorrectContinuousBehavior, "PrintLabelsBatch"), SDK.LogLevel.Error));
        }

        [Fact]
        public async Task ProcessData_BeginBreakWithoutOptionalData_Success()
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.BeginBreak.ToString() },
                { Vars.CODE, "12345" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.Verify(x => x.BeginBreakAsync(@operator, deviceId,
                It.Is<BeginBreak>(x => x.Code == "12345" && x.Reason == null)));
        }

        [Fact]
        public async Task ProcessData_BeginBreak_Success()
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.BeginBreak.ToString() },
                { Vars.CODE, "12345" },
                { Vars.BREAK_REASON, "1" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.Verify(x => x.BeginBreakAsync(@operator, deviceId,
                It.Is<BeginBreak>(x => x.Code == "12345" && x.Reason == 1)));
        }

        [Fact]
        public async Task ProcessData_EndBreak_Success()
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.EndBreak.ToString() },
                { Vars.CODE, "12345" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.Verify(x => x.EndBreakAsync(@operator, deviceId));
        }

        [Fact]
        public async Task ProcessData_PlaceInDockResultWithoutOptionalDataInBatchBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PlaceInDockResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.As<IPickingBatchBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PlaceInDockResult>(x => x.Code == "12345" && x.Dock == null)));
        }

        [Fact]
        public async Task ProcessData_PlaceInDockResultInBatchBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PlaceInDockResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.DOCK, "Dock 1" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.As<IPickingBatchBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PlaceInDockResult>(x => x.Code == "12345" && x.Dock == "Dock 1")));
        }

        [Fact]
        public async Task BuildInstructions_SetAskQuestionResultInContinuousBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var instructionSet = new InstructionSet();

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Start.ToString() },
                { Vars.RESPONSETYPE, ResponseTypes.AskQuestionResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.BuildInstructionsAsync(instructionSet, data, deviceMock.Object)).Should().NotThrowAsync();


            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingContinuousBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<AskQuestionResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime)));
        }

        [Fact]
        public async Task BuildInstructions_SetBeginPickingOrderResultInContinuousBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var instructionSet = new InstructionSet();

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Start.ToString() },
                { Vars.RESPONSETYPE, ResponseTypes.BeginPickingResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.BuildInstructionsAsync(instructionSet, data, deviceMock.Object)).Should().NotThrowAsync();


            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingContinuousBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<BeginPickingOrderResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime)));
        }

        [Fact]
        public async Task BuildInstructions_SetPickingLineResultWithoutOptionalDataInContinuousBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var instructionSet = new InstructionSet();

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Start.ToString() },
                { Vars.RESPONSETYPE, ResponseTypes.PickingLineResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.BuildInstructionsAsync(instructionSet, data, deviceMock.Object)).Should().NotThrowAsync();


            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingContinuousBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PickingLineResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime && x.Picked == null && x.ServingCode == null && x.Weight == null && x.Stock == null && x.Dock == null && x.ProductCD == null && x.Breakage == null && x.Batch == null)));
        }

        [Fact]
        public async Task BuildInstructions_SetPickingLineResultInContinuousBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var instructionSet = new InstructionSet();

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Start.ToString() },
                { Vars.RESPONSETYPE, ResponseTypes.PickingLineResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
                { Vars.TOTAL_PICKED, "1" },
                { Vars.SERVING, "UNITS" },
                { Vars.TOTAL_WEIGHT, "2.5" },
                { Vars.STOCK, "3" },
                { Vars.DOCK, "Dock 1" },
                { Vars.PRODUCT_CD, "123" },
                { Vars.BREAKAGE, "4" },
                { Vars.BATCH, "12345" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.BuildInstructionsAsync(instructionSet, data, deviceMock.Object)).Should().NotThrowAsync();


            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingContinuousBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PickingLineResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime && x.Picked == 1 && x.ServingCode == "UNITS" && x.Weight == 2.5M && x.Stock == 3 && x.Dock == "Dock 1" && x.ProductCD == "123" && x.Breakage == 4 && x.Batch == "12345")));
        }

        [Fact]
        public async Task BuildInstructions_SetPrintLabelsResultWithoutOptionalDataInContinuousBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PrintLabelsResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingContinuousBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PrintLabelsResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime && x.LabelsToPrint == null && x.Printer == null)));
        }

        [Fact]
        public async Task BuildInstructions_SetPrintLabelsResultInContinuousBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PrintLabelsResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
                { Vars.LABELS, "1" },
                { Vars.PRINTER, "2" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingContinuousBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PrintLabelsResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime && x.LabelsToPrint == 1 && x.Printer == 2)));
        }

        [Fact]
        public async Task BuildInstructions_ValidatePrintingResultInBatchBehavior_LogError()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.ValidatePrintingResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.STATUS, "OK" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.Verify(x => x.Log(string.Format(Resources.Error_IncorrectNonContinuousBehavior, "ValidatePrintingResult"), SDK.LogLevel.Error));
        }

        [Fact]
        public async Task BuildInstructions_ValidatePrintingResultInContinuousBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.ValidatePrintingResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
                { "LABELS_1_LABEL", "1" },
                { "LABELS_2_LABEL", "2" },
                { "LABELS_3_LABEL", "3" },
                { "LABELS_4_LABEL", "4" },
                { "LABELS_5_LABEL", "5" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            var startDateTime = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDateTime = new DateTime(2009, 1, 1).AddSeconds(end);

            behaviorMock.As<IPickingContinuousBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<ValidatePrintingResult>(x => x.Code == "12345" && x.Status == "OK" && x.Started == startDateTime && x.Finished == endDateTime && x.ReadLabels.Count() == 5 && x.ReadLabels.Any(x => x == "5"))));
        }

        [Fact]
        public async Task BuildInstructions_SetPlaceInDockResultWithoutOptionalDataInContinuousBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PlaceInDockResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.As<IPickingContinuousBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PlaceInDockResult>(x => x.Code == "12345" && x.Dock == null)));
        }

        [Fact]
        public async Task BuildInstructions_SetPlaceInDockResultInContinuousBehavior_Success()
        {
            /* Arrange */
            var start = 452945320;
            var end = 452945325;
            var deviceId = "9999999999";
            var @operator = "operator";

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);
            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);

            var data = new Dictionary<string, string>
            {
                { Vars.RESPONSETYPE, ResponseTypes.PlaceInDockResult.ToString() },
                { Vars.CODE, "12345" },
                { Vars.DOCK, "Dock 1" },
                { Vars.START_TIME, start.ToString() },
                { Vars.END_TIME, end.ToString() },
                { Vars.STATUS, "OK" },
            };

            /* Act */
            await plugin.Invoking(async x => await x.ProcessDataAsync(data, deviceMock.Object)).Should().NotThrowAsync();

            /* Assert */
            behaviorMock.As<IPickingContinuousBehavior>().Verify(x => x.SetWorkOrderAsync(@operator, deviceId,
                It.Is<PlaceInDockResult>(x => x.Code == "12345" && x.Dock == "Dock 1")));
        }

        [Fact]
        public async Task LoadMenuOptions()
        {
            /* Arrange */
            var behaviorMock = new Mock<IPickingBehavior>();
            var plugin = new Workflow(behaviorMock.Object);

            var mockDevice = new Mock<IDevice>();
            mockDevice.SetupGet(x => x.Language).Returns("en");

            behaviorMock.SetupGet(x => x.Settings.BreakOptions).Returns("WC|Break");

            /* Act */
            var menuOptions = await plugin.GetMenuOptionsAsync(mockDevice.Object, string.Empty);

            /* Assert */
            var res = menuOptions.PrepareResponse();
            res.Should().Be(
                @"LOWER_QUANTITY,01,Empty
LOWER_QUANTITY,02,Breakage
LOWER_QUANTITY,03,Complete
LOWER_QUANTITY,04,Dock
LOWER_QUANTITY,05,Continue
LOWER_QUANTITY,06,Cancel
BREAK,01,WC
BREAK,02,Break

");
        }


        [Fact]
        public async Task LoadMenuOptions_WithoutBreakOptions()
        {
            /* Arrange */
            var behaviorMock = new Mock<IPickingBehavior>();
            var plugin = new Workflow(behaviorMock.Object);

            var mockDevice = new Mock<IDevice>();
            mockDevice.SetupGet(x => x.Language).Returns("en");

            behaviorMock.SetupGet(x => x.Settings.BreakOptions).Returns((string?)null);

            /* Act */
            var menuOptions = await plugin.GetMenuOptionsAsync(mockDevice.Object, string.Empty);

            /* Assert */
            var res = menuOptions.PrepareResponse();
            res.Should().Be(
                @"LOWER_QUANTITY,01,Empty
LOWER_QUANTITY,02,Breakage
LOWER_QUANTITY,03,Complete
LOWER_QUANTITY,04,Dock
LOWER_QUANTITY,05,Continue
LOWER_QUANTITY,06,Cancel

");

        }

        [Fact]
        public async Task BuildInstructions_StartInBatchBehavior_ReturnsNoWork()
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);
            deviceMock.SetupGet(x => x.Language).Returns("en");

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();

            var plugin = new Workflow(behaviorMock.Object);

            var instructionSet = new InstructionSet();
            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Start.ToString() }
            };

            /* Act */
            await plugin.BuildInstructionsAsync(instructionSet, data, deviceMock.Object);

            /* Assert */
            behaviorMock.Verify(x => x.RegisterOperatorStartAsync(@operator, deviceId), Times.Once);

            var res = instructionSet.PrepareResponse();

            res.Should().Be(
                @"0,loadMenuOptions,""MENU""
1,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
2,assignNum,""DLG"",1
3,setCommands
4,say,""No work assigned. To try again, say VCONFIRM"",1,0

");
        }

        [Fact]
        public async Task BuildInstructions_StartInContinuousBehavior_ReturnsNoWorkAndSetOperatorMessage()
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);
            deviceMock.SetupGet(x => x.Language).Returns("en");

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();
            behaviorMock.Setup(x => x.RegisterOperatorStartAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("Hello from Start");

            var plugin = new Workflow(behaviorMock.Object);

            var instructionSet = new InstructionSet();
            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Start.ToString() }
            };

            /* Act */
            await plugin.BuildInstructionsAsync(instructionSet, data, deviceMock.Object);

            /* Assert */
            behaviorMock.Verify(x => x.RegisterOperatorStartAsync(@operator, deviceId), Times.Once);

            var res = instructionSet.PrepareResponse();

            res.Should().Be(
                @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,say,""Hello from Start"",0,0
3,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
4,assignNum,""DLG"",1
5,setCommands
6,say,""No work assigned. To try again, say VCONFIRM"",1,0

");
        }

        [Fact]
        public async Task BuildInstructions_StartInBatchBehavior_ReturnsNoWorkAndSetOperatorMessage()
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);
            deviceMock.SetupGet(x => x.Language).Returns("en");

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();
            behaviorMock.Setup(x => x.RegisterOperatorStartAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("Hello from Start");

            var plugin = new Workflow(behaviorMock.Object);

            var instructionSet = new InstructionSet();
            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Start.ToString() }
            };

            /* Act */
            await plugin.BuildInstructionsAsync(instructionSet, data, deviceMock.Object);

            /* Assert */
            behaviorMock.Verify(x => x.RegisterOperatorStartAsync(@operator, deviceId), Times.Once);

            var res = instructionSet.PrepareResponse();

            res.Should().Be(
                @"0,loadMenuOptions,""MENU""
1,say,""Hello from Start"",0,0
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,setCommands
5,say,""No work assigned. To try again, say VCONFIRM"",1,0

");
        }

        [Theory]
        [ClassData(typeof(GetWorkOrderItem_Start))]
        public async Task BuildInstructions_StartInContinuousBehavior_Success(string expectedDialog, GetWorkOrderItemBase workOrder, string breakOptions)
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);
            deviceMock.SetupGet(x => x.Language).Returns("en");

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();
            behaviorMock.As<IPickingContinuousBehavior>().Setup(x => x.GetWorkOrderAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(workOrder);

            behaviorMock.SetupGet(x => x.Settings.BreakOptions).Returns(breakOptions);


            var plugin = new Workflow(behaviorMock.Object);

            var instructionSet = new InstructionSet();
            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Start.ToString() }
            };

            /* Act */
            await plugin.BuildInstructionsAsync(instructionSet, data, deviceMock.Object);

            /* Assert */
            instructionSet.PrepareResponse().Should().Be(expectedDialog);
        }

        [Theory]
        [ClassData(typeof(GetWorkOrderItem_Work))]
        public async Task BuildInstructions_WorkInContinuousBehavior_Success(string expectedDialog, GetWorkOrderItemBase workOrder)
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);
            deviceMock.SetupGet(x => x.Language).Returns("en");

            var behaviorMock = new Mock<IPickingContinuousBehavior>().As<IPickingBehavior>();
            behaviorMock.As<IPickingContinuousBehavior>().Setup(x => x.GetWorkOrderAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(workOrder);

            behaviorMock.SetupGet(x => x.Settings.BreakOptions).Returns("WC|Break");


            var plugin = new Workflow(behaviorMock.Object);

            var instructionSet = new InstructionSet();
            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Work.ToString() }
            };

            /* Act */
            await plugin.BuildInstructionsAsync(instructionSet, data, deviceMock.Object);

            /* Assert */
            instructionSet.PrepareResponse().Should().Be(expectedDialog);
        }

        [Fact]
        public async Task BuildInstructions_StartInBatchBehavior_ReturnsWorkOrders()
        {
            /* Arrange */
            var deviceId = "9999999999";
            var @operator = "operator";

            var deviceMock = new Mock<IDevice>();

            deviceMock.Setup(x => x.DeviceID).Returns(deviceId);
            deviceMock.Setup(x => x.Operator).Returns(@operator);
            deviceMock.SetupGet(x => x.Language).Returns("en");

            var behaviorMock = new Mock<IPickingBatchBehavior>().As<IPickingBehavior>();
            behaviorMock.As<IPickingBatchBehavior>().Setup(x => x.GetWorkOrdersAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new IGetWorkOrderItem[] { new BeginPickingOrder("001", "Picking order 001"), new PickingLine("001-01", string.Empty) { ProductName = "ProductName", ProductNumber = "ProductNumber", UpcNumber = "UpcNumber" }, new PlaceInDock("001-End", string.Empty), new AskQuestion("002", "Ask question") });

            behaviorMock.SetupGet(x => x.Settings.BreakOptions).Returns("WC|Break");


            var plugin = new Workflow(behaviorMock.Object);

            var instructionSet = new InstructionSet();
            var data = new Dictionary<string, string>
            {
                { Vars.DLG, Dialogs.Start.ToString() }
            };

            /* Act */
            await plugin.BuildInstructionsAsync(instructionSet, data, deviceMock.Object);

            /* Assert */
            instructionSet.PrepareResponse().Should().Be(
                @"0,loadMenuOptions,""MENU""
1,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
2,assignNum,""DLG"",1
3,setSendHostFlag,""CODE"",1
4,setSendHostFlag,""RESPONSETYPE"",1
5,assignStr,""CODE"",""001""
6,assignStr,""CURRENT_AISLE""
7,assignStr,""CURRENT_POSITION""
8,assignNum,""START_TIME"",""#time""
9,label,""LABEL_START_001""
10,setCommands,,,LABEL_BREAK_001
11,assignStr,""PROMPT"",""Order ""
12,say,""$PROMPT"",1,1
13,say,""Picking order 001"",1,1
14,goTo,""LABEL_END_001""
15,label,""LABEL_BREAK_001""
16,setCommands,,,,,,,,,LABEL_START_001
17,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
18,setSendHostFlag,""BREAK_REASON"",1
19,setSendHostFlag,""RESPONSETYPE"",1
20,assignNum,""RESPONSETYPE"",5
21,getVariablesOdr
22,setSendHostFlag,""BREAK_REASON"",0
23,setCommands
24,say,""At break. To resume work, say VCONFIRM"",1,0
25,assignNum,""RESPONSETYPE"",6
26,getVariablesOdr
27,goTo,""LABEL_START_001""
28,label,""LABEL_END_001""
29,assignNum,""END_TIME"",""#time""
30,assignStr,""STATUS"",""OK""
31,assignNum,""RESPONSETYPE"",0
32,setSendHostFlag,""STATUS"",1
33,setSendHostFlag,""START_TIME"",1
34,setSendHostFlag,""END_TIME"",1
35,getVariablesOdr
36,setSendHostFlag,""STATUS"",0
37,setSendHostFlag,""START_TIME"",0
38,setSendHostFlag,""END_TIME"",0
39,assignStr,""CODE"",""001-01""
40,label,""LABEL_START_001-01""
41,assignStr,""STATUS""
42,assignStr,""PICKED""
43,assignStr,""SERVING""
44,assignStr,""WEIGHT""
45,assignStr,""STOCK""
46,assignStr,""DOCK""
47,assignStr,""BREAKAGE""
48,assignStr,""BATCH""
49,assignNum,""START_TIME"",""#time""
50,assignStr,""PRODUCT_DESCRIPTION"",""productname""
51,assignStr,""PRODUCT_NUMBER"",""productnumber""
52,assignStr,""UPC_NUMBER"",""upcnumber""
53,assignStr,""QTY_UPPER""
54,assignStr,""QTY_LOWER""
55,assignStr,""MAX_QTY_ALLOWED_PER_PICK""
56,assignStr,""TOTAL_PICKED""
57,assignStr,""TOTAL_WEIGHT""
58,setCommands,LABEL_EXCEPTION_LOCATION_001-01,LABEL_DOCK_001-01,LABEL_BREAK_001-01,,,Information not available,,,,,$CUSTOMER
59,doIf,""CURRENT_AISLE"",,""<>"",""STR"",,,,,,,,,,,1
60,assignStr,""CURRENT_AISLE""
61,assignStr,""CURRENT_POSITION""
62,say,""Aisle "",1,0
63,doEndIf,,,,,,,,,,,,,,,1
64,assignStr,""WHEREAMI"",""Aisle ""
65,setCommands,LABEL_EXCEPTION_CD_001-01,LABEL_DOCK_001-01,LABEL_BREAK_001-01,LABEL_SKIP_SLOT_001-01,,Information not available,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
66,label,""LABEL_ASK_BATCH_001-01""
67,label,""LABEL_RESET_PICK_001-01""
68,assignStr,""STATUS"",""OK""
69,label,""LABEL_CONFIRM_001-01""
70,assignNum,""END_TIME"",""#time""
71,assignNum,""RESPONSETYPE"",1
72,setSendHostFlag,""STATUS"",1
73,setSendHostFlag,""START_TIME"",1
74,setSendHostFlag,""END_TIME"",1
75,setSendHostFlag,""TOTAL_PICKED"",1
76,setSendHostFlag,""SERVING"",1
77,getVariablesOdr
78,setSendHostFlag,""STATUS"",0
79,setSendHostFlag,""START_TIME"",0
80,setSendHostFlag,""END_TIME"",0
81,setSendHostFlag,""TOTAL_PICKED"",0
82,setSendHostFlag,""SERVING"",0
83,setSendHostFlag,""TOTAL_WEIGHT"",0
84,setSendHostFlag,""STOCK"",0
85,setSendHostFlag,""BREAKAGE"",0
86,goTo,""LABEL_END_001-01""
87,label,""LABEL_FORMAT_001-01""
88,say,""Not allowed"",0,0
89,goTo,""LABEL_PICK_001-01""
90,label,""LABEL_BREAKAGE_001-01""
91,getDigits,""BREAKAGE"",""Breakage quantity?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
92,setSendHostFlag,""BREAKAGE"",1
93,goTo,""LABEL_PICK_001-01""
94,label,""LABEL_UNITS_001-01""
95,say,""Not allowed"",0,0
96,goTo,""LABEL_PICK_001-01""
97,label,""LABEL_EXCEPTION_LOCATION_001-01""
98,assignStr,""STATUS"",""BadLocation""
99,assignStr,""SERVING""
100,goTo,""LABEL_CONFIRM_001-01""
101,label,""LABEL_EXCEPTION_CD_001-01""
102,assignStr,""STATUS"",""NoCheck""
103,assignStr,""SERVING""
104,goTo,""LABEL_CONFIRM_001-01""
105,label,""LABEL_EXCEPTION_001-01""
106,assignStr,""STATUS"",""Cancelled""
107,assignStr,""SERVING""
108,goTo,""LABEL_CONFIRM_001-01""
109,label,""LABEL_SKIP_SLOT_001-01""
110,assignStr,""STATUS"",""Postponed""
111,assignStr,""SERVING""
112,goTo,""LABEL_CONFIRM_001-01""
113,label,""LABEL_DOCK_001-01""
114,say,""Place in dock"",0,0
115,setCommands,,,,,,,,,LABEL_DOCK_END_001-01,,$CUSTOMER
116,getDigits,""LABELS"",""How many labels?"",,10,0,0,,""1"",""2"",,,0,,,1,,,0
117,assignNum,""RESPONSETYPE"",7
118,label,""LABEL_PRINT_LABELS_001-01""
119,setSendHostFlag,""LABELS"",1
120,getVariablesOdr
121,setSendHostFlag,""LABELS"",0
122,setCommands,LABEL_DOCK_END_001-01,,,,,,,,,,$CUSTOMER
123,ask,""DUMMY"",""Correct printing?"",,1,0,""1"",""0""
124,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,2
125,say,""Retrying..."",0,0
126,goTo,""LABEL_PRINT_LABELS_001-01""
127,doEndIf,,,,,,,,,,,,,,,2
128,setCommands
129,getDigits,""DOCK"",""Pallet location"",,10,0,0,,""1"",""2"",,,0,,,1,,,0
130,assignNum,""RESPONSETYPE"",1
131,assignStr,""STATUS"",""EndPallet""
132,assignNum,""END_TIME"",""#time""
133,setSendHostFlag,""STATUS"",1
134,setSendHostFlag,""DOCK"",1
135,setSendHostFlag,""START_TIME"",1
136,setSendHostFlag,""END_TIME"",1
137,getVariablesOdr
138,setSendHostFlag,""STATUS"",0
139,setSendHostFlag,""DOCK"",0
140,setSendHostFlag,""START_TIME"",0
141,setSendHostFlag,""END_TIME"",0
142,say,""Pallet placed"",0,0
143,goTo,""LABEL_DOCK_END_001-01""
144,label,""LABEL_DOCK_END_001-01""
145,assignStr,""CURRENT_AISLE""
146,goTo,""LABEL_START_001-01""
147,label,""LABEL_BREAK_001-01""
148,assignStr,""CURRENT_AISLE""
149,assignStr,""CURRENT_POSITION""
150,setCommands,,,,,,,,,LABEL_START_001-01
151,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
152,setSendHostFlag,""BREAK_REASON"",1
153,setSendHostFlag,""RESPONSETYPE"",1
154,assignNum,""RESPONSETYPE"",5
155,getVariablesOdr
156,setSendHostFlag,""BREAK_REASON"",0
157,setCommands
158,say,""At break. To resume work, say VCONFIRM"",1,0
159,assignNum,""RESPONSETYPE"",6
160,getVariablesOdr
161,goTo,""LABEL_START_001-01""
162,label,""LABEL_END_001-01""
163,assignStr,""CODE"",""001-End""
164,label,""LABEL_DOCK_001-End""
165,assignStr,""STATUS""
166,assignNum,""START_TIME"",""#time""
167,setCommands,LABEL_DOCK_EXCEPTION_001-End
168,say,""Place in dock"",0,0
169,label,""LABEL_PRINT_LABELS_001-End""
170,getDigits,""LABELS"",""How many labels?"",,10,0,0,,""1"",""2"",,,0,,,1,,,0
171,assignNum,""RESPONSETYPE"",7
172,setSendHostFlag,""LABELS"",1
173,getVariablesOdr
174,setSendHostFlag,""LABELS"",0
175,ask,""DUMMY"",""Correct printing?"",,1,0,""1"",""0""
176,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,3
177,say,""Retrying..."",0,0
178,goTo,""LABEL_PRINT_LABELS_001-End""
179,doEndIf,,,,,,,,,,,,,,,3
180,getDigits,""DOCK"",""Specify destination"",,10,0,0,,""1"",""20"",,,0,,,1,,,0
181,setSendHostFlag,""DOCK"",1
182,assignStr,""STATUS"",""OK""
183,goTo,""LABEL_DOCK_END_001-End""
184,label,""LABEL_DOCK_EXCEPTION_001-End""
185,assignStr,""STATUS"",""Cancelled""
186,label,""LABEL_DOCK_END_001-End""
187,assignNum,""END_TIME"",""#time""
188,assignNum,""RESPONSETYPE"",4
189,setSendHostFlag,""STATUS"",1
190,setSendHostFlag,""DOCK"",1
191,setSendHostFlag,""START_TIME"",1
192,setSendHostFlag,""END_TIME"",1
193,getVariablesOdr
194,setSendHostFlag,""STATUS"",0
195,setSendHostFlag,""DOCK"",0
196,setSendHostFlag,""START_TIME"",0
197,setSendHostFlag,""END_TIME"",0
198,assignStr,""CURRENT_AISLE""
199,assignStr,""CODE"",""002""
200,setCommands
201,assignStr,""STATUS""
202,assignNum,""START_TIME"",""#time""
203,ask,""DUMMY"",""Ask question"",,1,0,""1"",""0""
204,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,4
205,assignStr,""STATUS"",""Cancelled""
206,doElse,,,,,,,,,,,,,,,4
207,assignStr,""STATUS"",""OK""
208,doEndIf,,,,,,,,,,,,,,,4
209,assignNum,""END_TIME"",""#time""
210,assignNum,""RESPONSETYPE"",8
211,setSendHostFlag,""STATUS"",1
212,setSendHostFlag,""START_TIME"",1
213,setSendHostFlag,""END_TIME"",1
214,getVariablesOdr
215,setSendHostFlag,""STATUS"",0
216,setSendHostFlag,""START_TIME"",0
217,setSendHostFlag,""END_TIME"",0
218,setSendHostFlag,""CODE"",0
219,setSendHostFlag,""RESPONSETYPE"",0

");
        }

        [Fact]
        public async Task ConnectAsync_NotAllowed_WithMessage()
        {
            var connectResult = new ConnectResult(false, null, "TestingNotAllowed");

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<IDevice>())).ReturnsAsync(connectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.ConnectAsync(new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"0,0,""TestingNotAllowed""

");
        }

        [Fact]
        public async Task ConnectAsync_NotAllowed_WithoutMessage()
        {
            var connectResult = new ConnectResult(false, null, null);

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<IDevice>())).ReturnsAsync(connectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.ConnectAsync(new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"0,0

");
        }

        [Fact]
        public async Task ConnectAsync_Allowed_NoPassword_WithMessage()
        {
            var connectResult = new ConnectResult(true, null, "TestingAllowed");

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<IDevice>())).ReturnsAsync(connectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.ConnectAsync(new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"0,1,,,""TestingAllowed"",,,,,0

");
        }

        [Fact]
        public async Task ConnectAsync_Allowed_NoPassword_WithoutMessage()
        {
            var connectResult = new ConnectResult(true, null, null);

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<IDevice>())).ReturnsAsync(connectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.ConnectAsync(new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"0,1,,,,,,,,0

");
        }

        [Fact]
        public async Task ConnectAsync_Allowed_WithPassword_WithMessage()
        {
            var connectResult = new ConnectResult(true, "TestingPassword", "TestingAllowed");

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<IDevice>())).ReturnsAsync(connectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.ConnectAsync(new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"1,1,,,""TestingAllowed"",""TestingPassword"",,,,0

");
        }

        [Fact]
        public async Task ConnectAsync_Allowed_WithPassword_WithoutMessage()
        {
            var connectResult = new ConnectResult(true, "TestingPassword", null);

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<IDevice>())).ReturnsAsync(connectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.ConnectAsync(new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"1,1,,,,""TestingPassword"",,,,0

");
        }

        [Fact]
        public async Task DisconnectAsync_NotAllowed_WithMessage()
        {
            var force = false;
            var disconnectResult = new DisconnectResult(force, "TestingNotAllowed");

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.DisconnectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(disconnectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.DisconnectAsync(force, new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"0,""TestingNotAllowed""

");
        }

        [Fact]
        public async Task DisconnectAsync_NotAllowed_WithoutMessage()
        {
            var force = false;
            var disconnectResult = new DisconnectResult(force, null);

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.DisconnectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(disconnectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.DisconnectAsync(force, new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"0

");
        }

        [Fact]
        public async Task DisconnectAsync_Allowed_WithMessage()
        {
            var force = true;
            var disconnectResult = new DisconnectResult(force, "TestingAllowed");

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.DisconnectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(disconnectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.DisconnectAsync(force, new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"1,""TestingAllowed""

");
        }

        [Fact]
        public async Task DisconnectAsync_Allowed_WithoutMessage()
        {
            var force = true;
            var disconnectResult = new DisconnectResult(force, null);

            var behaviorMock = new Mock<IPickingBehavior>();
            behaviorMock.Setup(x => x.DisconnectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(disconnectResult);

            var workflow = new Workflow(behaviorMock.Object);

            var res = await workflow.DisconnectAsync(force, new Mock<IDevice>().Object);

            res.Should().NotBeNull();
            res.PrepareResponse().Should().Be(@"1

");
        }
    }
}