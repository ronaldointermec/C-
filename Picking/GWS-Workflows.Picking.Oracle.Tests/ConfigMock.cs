using Honeywell.Firebird.CoreLibrary;
using Moq;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Oracle.Tests;

public static class ConfigMock
{
    public static Mock<IConfigService> GetConfigService(string server = "Server=")
    {
        var serverKey = "Server";
        var logDeviceKey = "Log:Device";
        var logServerKey = "Log:Server";

        var serverConfigParamMock = GetConfigParam(serverKey, server);
        var logDeviceKeyConfigParamMock = GetConfigParam(logDeviceKey, true.ToString());
        var logServerKeyConfigParamMock = GetConfigParam(logServerKey, true.ToString());

        var allConfigs = new List<IConfigParam>
        {
            serverConfigParamMock.Object,
            logDeviceKeyConfigParamMock.Object,
            logServerKeyConfigParamMock.Object
        };

        var configServiceMock = new Mock<IConfigService>();
        configServiceMock.Setup(x => x.GetAllConfigs(It.IsAny<IConfigCategory>())).Returns(allConfigs);
        configServiceMock.Setup(x => x.GetOrCreateConfig(serverKey, It.IsAny<IConfigCategory>())).Returns(serverConfigParamMock.Object);
        configServiceMock.Setup(x => x.GetOrCreateConfig(logDeviceKey, It.IsAny<IConfigCategory>())).Returns(logDeviceKeyConfigParamMock.Object);
        configServiceMock.Setup(x => x.GetOrCreateConfig(logServerKey, It.IsAny<IConfigCategory>())).Returns(logServerKeyConfigParamMock.Object);
        return configServiceMock;
    }

    private static Mock<IConfigParam> GetConfigParam(string key, string value = "")
    {
        var configParamMock = new Mock<IConfigParam>();
        configParamMock.SetupGet(x => x.Key).Returns(key);
        configParamMock.SetupGet(x => x.Value).Returns(value);

        return configParamMock;
    }
}
