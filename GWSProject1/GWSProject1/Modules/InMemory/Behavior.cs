using Honeywell.GWS.Connector.SDK;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GWSProject1.Modules.InMemory
{
    public class Behavior : ConnectorBehaviorBase<BehaviorSettings>, IBehavior<BehaviorSettings>
    {
        private readonly List<OperationModel> operations = new()
        {
            //new OperationModel {Id = 1, Message = "This is the first operation", Operator1 = 5, Operation = OperationType.Add,  Operator2 = 3 },
            //new OperationModel {Id = 2,  Operator1 = 6, Operation = OperationType.Divide,  Operator2 = 2 },
            new OperationModel {Id = 3, Message = "This is the third operation", Operator1 = 8, Operation = OperationType.Divide},
            new OperationModel {Id = 4, Operation = OperationType.Multiply},
        };
        public Behavior(BehaviorSettings settings) : base(settings)
        {
        }

        public OperationModel? GetOperation()
        {
            return operations.Find(X => !X.Result.HasValue);
        }

        public void SetOperationResult(int id, float result)
        {
            var op = operations.SingleOrDefault(x => x.Id == id);

            if (op != null)
            {
                op.Result = result;
            }
        }

        public Task<bool> SignOnAsync(string @operator)
        {
            return Task.FromResult(true);
        }
    }
}