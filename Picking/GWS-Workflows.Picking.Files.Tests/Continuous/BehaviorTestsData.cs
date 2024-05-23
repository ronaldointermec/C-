using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Tests.Continuous;

internal class BehaviorTestsData : TheoryData<ISetWorkOrderItem, string, Type>
{
    public BehaviorTestsData()
    {
        Add(new BeginPickingOrderResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, "BeginPickingOrderResult", typeof(BeginPickingOrderResult));
        Add(new PickingLineResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, "PickingLineResult", typeof(PickingLineResult));
        Add(new PlaceInDockResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, "PlaceInDockResult", typeof(PlaceInDockResult));
        Add(new AskQuestionResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, "AskQuestionResult", typeof(AskQuestionResult));
        Add(new PrintLabelsResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, "PrintLabelsResult", typeof(PrintLabelsResult));
        Add(new ValidatePrintingResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, "ValidatePrintingResult", typeof(ValidatePrintingResult));
    }
}
