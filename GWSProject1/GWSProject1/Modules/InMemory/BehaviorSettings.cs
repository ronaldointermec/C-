using Honeywell.Firebird.CoreLibrary;
using Honeywell.GWS.Connector.SDK;
using System.Collections.Generic;

namespace GWSProject1.Modules.InMemory
{
    public class BehaviorSettings : ConnectorBehaviorSettingsBase, IBehaviorSettings
    {
        private const string MySettingKey = "MySetting";

        public BehaviorSettings(IConfigService configService) : base(configService)
        {
        }

        public bool MySetting
        {
            get => GetConfigValue<bool>(MySettingKey);
            set => SaveConfig(MySettingKey, value);
        }

        /// <inheritdoc/>
        protected override Dictionary<string, string> DefaultValues { get; } = new()
        {
            { ServerKey, string.Empty },
            { LogDeviceKey, "true" },
            { MySettingKey, "false" },
        };
    }
}