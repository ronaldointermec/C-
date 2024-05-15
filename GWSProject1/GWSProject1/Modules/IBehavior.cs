using Honeywell.GWS.Connector.SDK.Interfaces;
using System.Threading.Tasks;

namespace GWSProject1.Modules
{
    public interface IBehavior<out TBehaviorSettings> : IConnectorBehavior<TBehaviorSettings>
        where TBehaviorSettings : class, IBehaviorSettings
    {
        Task<bool> SignOnAsync(string @operator);
        OperationModel? GetOperation();
        void SetOperationResult(int id, float result);

    }
}