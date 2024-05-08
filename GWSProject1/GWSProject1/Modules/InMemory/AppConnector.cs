#if !NETFRAMEWORK
using Honeywell.Firebird.Module;
using Honeywell.GWS.Connector.SDK.App;

namespace GWSProject1.Modules.InMemory
{
    public class AppConnector : ConnectorBase<Behavior, BehaviorSettings, Workflow>
    {
        public AppConnector(IAppBaseModuleContext context) : base(context)
        {
        }

        public override string ConnectorName => "InMemory";
    }


}
#endif