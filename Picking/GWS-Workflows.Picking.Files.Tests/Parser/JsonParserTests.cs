using FluentAssertions;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Code;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Tests.Parser;
public class JsonParserTests
{
    readonly JsonParser parser = new(new PickingJsonConverter());

    [Fact]
    public void ParseGetWorkOrderItem()
    {
        var order = @"{
    ""type"": ""AskQuestion"",
    ""code"": ""001"",
    ""message"": ""TestingMessage""
  }";

        var result = parser.Parse<IGetWorkOrderItem>(order);

        result.Should().NotBeNull();
        result.Should().BeOfType<AskQuestion>();

    }

    [Fact]
    public void ParseGetWorkOrderItemList()
    {
        var orderList = @"[
  {
    ""type"": ""AskQuestion"",
    ""code"": ""001"",
    ""message"": ""TestingMessage""
  },
  {
    ""type"": ""BeginPickingOrder"",
    ""orderNumber"": ""TestingOrderNumber"",
    ""code"": ""001"",
    ""message"": ""TestingMessage""
  },
  {
    ""type"": ""PickingLine"",
    ""aisle"": ""TestAise"",
    ""productName"": ""TestProduct"",
    ""stockCounting"": 0,
    ""code"": ""001""
  },
  {
    ""type"": ""PlaceInDock"",
    ""code"": ""001""
  },
  {
    ""type"": ""PrintLabels"",
    ""printers"": [
      1,
      2,
      3
    ],
    ""code"": ""001""
  },
  {
    ""type"": ""ValidatePrinting"",
    ""code"": ""001""
  }
]";

        var result = parser.Parse<List<IGetWorkOrderItem>>(orderList);

        result.Should().NotBeNull();
        result.Count.Should().Be(6);
        result[0].Should().BeOfType<AskQuestion>();
        result[1].Should().BeOfType<BeginPickingOrder>();
        result[2].Should().BeOfType<PickingLine>();
        result[3].Should().BeOfType<PlaceInDock>();
        result[4].Should().BeOfType<PrintLabels>();
        result[5].Should().BeOfType<ValidatePrinting>();
    }

    [Fact]
    public void ParseOperator()
    {
        var oper = @"{
  ""name"": ""TestingName"",
  ""startTime"": ""2023-05-08T11:26:35.6639492+02:00""
}";

        var res = parser.Parse<Operator>(oper);

        res.Should().NotBeNull();
        res.Name.Should().Be("TestingName");
    }

    [Fact]
    public void ParseObjectList()
    {
        var orderList = @"[
  {
    ""type"": ""BeginPickingOrderResult"",
    ""code"": ""001"",
    ""status"": ""TestingStatus 1"",
    ""started"": ""0001-01-01T00:00:00"",
    ""finished"": ""0001-01-01T00:00:00""
  },
  {
    ""type"": ""PickingLineResult"",
    ""code"": ""001"",
    ""status"": ""TestingStatus 2"",
    ""started"": ""0001-01-01T00:00:00"",
    ""finished"": ""0001-01-01T00:00:00""
  }
]";

        var result = parser.Parse<List<object>>(orderList);

        result.Should().NotBeNull();
        result.Count.Should().Be(2);
    }

    [Fact]
    public void Serialize()
    {
        var order = new SetWorkOrderItemBase("001")
        {
            Status = "TestingStatus"
        };

        var result = parser.Serialize(order);

        result.Should().NotBeNull();
        result.Should().Be(@"{
  ""code"": ""001"",
  ""status"": ""TestingStatus"",
  ""started"": ""0001-01-01T00:00:00"",
  ""finished"": ""0001-01-01T00:00:00""
}");
    }

    [Fact]
    public void SerializeList()
    {
        var setWorkOrderBatchResultList = new List<ISetWorkOrderItem>
        {
            new BeginPickingOrderResult("001")
            {
                Status = "TestingStatus 1"
            },
            new PickingLineResult("001")
            {
                Status = "TestingStatus 2",
            }
        };

        var result = parser.Serialize(setWorkOrderBatchResultList);

        result.Should().NotBeNull();
        result.Should().Be(@"[
  {
    ""code"": ""001"",
    ""status"": ""TestingStatus 1"",
    ""started"": ""0001-01-01T00:00:00"",
    ""finished"": ""0001-01-01T00:00:00""
  },
  {
    ""code"": ""001"",
    ""status"": ""TestingStatus 2"",
    ""started"": ""0001-01-01T00:00:00"",
    ""finished"": ""0001-01-01T00:00:00""
  }
]");
    }
}
