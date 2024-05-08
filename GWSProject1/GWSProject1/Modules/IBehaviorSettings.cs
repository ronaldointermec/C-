using Honeywell.GWS.Connector.SDK.Interfaces;

namespace GWSProject1.Modules
{
    public interface IBehaviorSettings : IConnectorBehaviorSettings
    {
        public bool MySetting { get; }
    }
}