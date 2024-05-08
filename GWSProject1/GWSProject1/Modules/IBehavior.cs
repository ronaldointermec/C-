using Honeywell.GWS.Connector.SDK.Interfaces;

namespace GWSProject1.Modules
{
    public interface IBehavior<out TBehaviorSettings> : IConnectorBehavior<TBehaviorSettings>
        where TBehaviorSettings : class, IBehaviorSettings
    {
        OperationModel? GetOperation();
        void SetOperationResult(int id, float result);

    }
}