using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Tests.Batch;

internal class BehaviorTestsData : TheoryData<ISetWorkOrderItem, Type>
{
    public BehaviorTestsData()
    {
        Add(new BeginPickingOrderResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, typeof(BeginPickingOrderResult));
        Add(new PickingLineResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, typeof(PickingLineResult));
        Add(new PlaceInDockResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, typeof(PlaceInDockResult));
        Add(new AskQuestionResult("001") { Started = DateTime.Now, Finished = DateTime.Now, Status = "TestingStatus" }, typeof(AskQuestionResult));
    }
}
