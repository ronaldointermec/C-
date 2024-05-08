#if NETFRAMEWORK
using Honeywell.GWS.Connector.SDK.Service;

namespace GWSProject1.Modules.InMemory;

public class ServiceConnector : ConnectorBase<Behavior>
{
    public ServiceConnector(Behavior connectorBehavior) : base(connectorBehavior)
    {
    }
}
#endif