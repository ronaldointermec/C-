using Honeywell.Firebird.CoreLibrary;
using Moq;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Files.Tests;

public static class ConfigMock
{
    public static Mock<IConfigService> GetConfigService(string server = "TestServer", string fileFormat = "Json")
    {
        var serverKey = "Server";
        var logDeviceKey = "Log:Device";
        var logServerKey = "Log:Server";
        var fileFormatKey = "File:Format";

        var serverConfigParamMock = GetConfigParam(serverKey, server);
        var logDeviceKeyConfigParamMock = GetConfigParam(logDeviceKey, "True");
        var logServerKeyConfigParamMock = GetConfigParam(logServerKey, "True");
        var fileFormatKeyConfigParamMock = GetConfigParam(fileFormatKey, fileFormat);

        var allConfigs = new List<IConfigParam>
        {
            serverConfigParamMock.Object,
            fileFormatKeyConfigParamMock.Object,
            logDeviceKeyConfigParamMock.Object,
            logServerKeyConfigParamMock.Object,
        };

        var configServiceMock = new Mock<IConfigService>();
        configServiceMock.Setup(x => x.GetAllConfigs(It.IsAny<IConfigCategory>())).Returns(allConfigs);
        configServiceMock.Setup(x => x.GetOrCreateConfig(serverKey, It.IsAny<IConfigCategory>())).Returns(serverConfigParamMock.Object);
        configServiceMock.Setup(x => x.GetOrCreateConfig(logDeviceKey, It.IsAny<IConfigCategory>())).Returns(logDeviceKeyConfigParamMock.Object);
        configServiceMock.Setup(x => x.GetOrCreateConfig(logServerKey, It.IsAny<IConfigCategory>())).Returns(logServerKeyConfigParamMock.Object);
        configServiceMock.Setup(x => x.GetOrCreateConfig(fileFormatKey, It.IsAny<IConfigCategory>())).Returns(fileFormatKeyConfigParamMock.Object);
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
