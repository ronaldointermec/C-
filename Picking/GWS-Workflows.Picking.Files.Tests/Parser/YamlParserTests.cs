using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using System.Globalization;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Tests.Parser;

public class YamlParserTests
{
    readonly YamlParser parser = new();

    [Fact]
    public void Parse_GetWorkOrderItem_InvalidType()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        var order = @"!!AskQuestions
  code: 001
  message: AskQuestion
  image: https://picsum.photos/200";

        parser.Invoking(x => x.Parse<IGetWorkOrderItem>(order)).Should().Throw<YamlDotNet.Core.YamlException>().WithMessage("Encountered an unresolved tag 'tag:yaml.org,2002:AskQuestions'");
    }

    [Fact]
    public void Parse_GetWorkOrderItem()
    {
        var order = @"!!AskQuestion
image: https://picsum.photos/200
code: 001
message: AskQuestion";
        var result = parser.Parse<IGetWorkOrderItem>(order);

        result.Should().NotBeNull();
        result.Should().BeOfType<AskQuestion>();
        result.Should().BeEquivalentTo(new AskQuestion("001", "AskQuestion") { Image = new Uri("https://picsum.photos/200") });
    }

    [Fact]
    public void Parse_GetWorkOrderItemList()
    {
        var orderList = @"- !!AskQuestion
  code: 001
  message: AskQuestion
  image: https://picsum.photos/200
- !!BeginPickingOrder
  code: 002
  message: BeginPickingOrder
- !!PickingLine
  stockCounting: No
  code: 003
  message: PickingLine
- !!PlaceInDock
  code: 004
  message: PlaceInDock
- !!PrintLabels
  code: 005
  message: PrintLabels
- !!ValidatePrinting
  code: 006
  message: ValidatePrinting";

        var result = parser.Parse<List<IGetWorkOrderItem>>(orderList);
        result.Should().NotBeNull();
        result[0].Should().BeOfType<AskQuestion>();
        result[1].Should().BeOfType<BeginPickingOrder>();
        result[2].Should().BeOfType<PickingLine>();
        result[3].Should().BeOfType<PlaceInDock>();
        result[4].Should().BeOfType<PrintLabels>();
        result[5].Should().BeOfType<ValidatePrinting>();
    }

    [Fact]
    public void Parse_ObjectList()
    {
        var objectList = @"- code: 001
  status: TestingStatus 1
  started: 0001-01-01T00:00:00.0000000
  finished: 0001-01-01T00:00:00.0000000
- picked: 3
  code: 002
  status: TestingStatus 2
  started: 0001-01-01T00:00:00.0000000
  finished: 0001-01-01T00:00:00.0000000
- picked: 2
  code: 003
  status: TestingStatus 3
  started: 0001-01-01T00:00:00.0000000
  finished: 0001-01-01T00:00:00.0000000
";

        var result = parser.Parse<List<object>>(objectList);

        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }

    [Fact]
    public void ParseOperator()
    {
        var oper = @"name: TestingName
startTime: 0001-01-01T00:00:00.0000000";

        var res = parser.Parse<Operator>(oper);

        res.Should().NotBeNull();
        res.Name.Should().Be("TestingName");
    }

    [Fact]
    public void Serialize_Operator()
    {
        var order = new Operator
        {
            Name = "TestingName",
            StartTime = Convert.ToDateTime("0001-01-01T00:00:00.0000000"),
        };

        var result = parser.Serialize(order);

        result.Should().NotBeNull();
        result.Should().Be(@"name: TestingName
startTime: 0001-01-01T00:00:00.0000000
");
    }

    [Fact]
    public void Serialize_SetWorkOrderItem()
    {
        var order = new BeginPickingOrderResult("001")
        {
            Status = "TestingStatus"
        };

        var result = parser.Serialize(order);

        result.Should().NotBeNull();
        result.Should().Be(@"code: 001
status: TestingStatus
started: 0001-01-01T00:00:00.0000000
finished: 0001-01-01T00:00:00.0000000
");
    }

    [Fact]
    public void Serialize_SetWorkOrderItemList()
    {
        var setWorkOrderBatchResultList = new List<object>
        {
            new BeginPickingOrderResult("001")
            {
                Status = "TestingStatus 1"
            },
            new PickingLineResult("002")
            {
                Status = "TestingStatus 2",
                Picked = 3,
            },
            new PickingLineResult("003")
            {
                Status = "TestingStatus 3",
                Picked = 2
            }
        };

        var result = parser.Serialize(setWorkOrderBatchResultList);

        result.Should().NotBeNull();
        result.Should().Be(@"- code: 001
  status: TestingStatus 1
  started: 0001-01-01T00:00:00.0000000
  finished: 0001-01-01T00:00:00.0000000
- picked: 3
  code: 002
  status: TestingStatus 2
  started: 0001-01-01T00:00:00.0000000
  finished: 0001-01-01T00:00:00.0000000
- picked: 2
  code: 003
  status: TestingStatus 3
  started: 0001-01-01T00:00:00.0000000
  finished: 0001-01-01T00:00:00.0000000
");
    }
}