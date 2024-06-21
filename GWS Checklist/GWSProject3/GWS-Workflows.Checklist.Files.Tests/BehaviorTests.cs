using FluentAssertions;
using Honeywell.Firebird.CoreLibrary;
using Honeywell.GuidedWork.AppBase.Services.DataService;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Files.Code;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Models;
using Honeywell.GWS.Connector.Library.Workflows.Checklist.Properties;
using Moq;
using System.Globalization;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Files.Tests;

public class BehaviorTests
{
    private static Mock<IConfigService> GetConfigService(string server, string fileFormat)
    {
        var serverKey = "Server";
        var logDeviceKey = "Log:Device";
        var fileFormatKey = "File:Format";

        var serverConfigParamMock = GetConfigParam(serverKey, server);
        var logDeviceKeyConfigParamMock = GetConfigParam(logDeviceKey, "True");
        var fileFormatKeyConfigParamMock = GetConfigParam(fileFormatKey, fileFormat);


        var allConfigs = new List<IConfigParam>
        {
            serverConfigParamMock.Object,
            fileFormatKeyConfigParamMock.Object,
        };



        var configServiceMock = new Mock<IConfigService>();
        configServiceMock.Setup(x => x.GetAllConfigs(It.IsAny<IConfigCategory>())).Returns(allConfigs);
        configServiceMock.Setup(x => x.GetOrCreateConfig(serverKey, It.IsAny<IConfigCategory>())).Returns(serverConfigParamMock.Object);
        configServiceMock.Setup(x => x.GetOrCreateConfig(logDeviceKey, It.IsAny<IConfigCategory>())).Returns(logDeviceKeyConfigParamMock.Object);
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

    [Fact]
    public void Initialize_ServerNullOrEmpty_ThrowsException()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("", "Json");

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { }, new Mock<ICustomDataPath>().Object, new FileSystem(), new ParserFactory());

        behavior.Invoking(x => x.Initialize()).Should().Throw<InvalidOperationException>().WithMessage(Resources.ServerEmpty);
    }

    [Fact]
    public void Initialize_WithJsonFormat()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("testingServer", "Json");

        var customPath = "CustomPath";
        var server = "testingServer";

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns(customPath);

        var fileSystemMock = new MockFileSystem();

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { Server = server, FileFormat = FileFormat.Json }, customPathMock.Object, fileSystemMock, new ParserFactory());

        behavior.Initialize();

        var parser = behavior.GetType().GetField("_parser", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(behavior) as IParser;
        parser.Should().NotBeNull().And.BeOfType<JsonParser>();

        var extension = behavior.GetType().GetField("_fileExtension", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(behavior) as string;
        extension.Should().Be(".json");

        fileSystemMock.Directory.Exists(fileSystemMock.Path.Combine(customPath, server, "Operators")).Should().BeTrue();
        fileSystemMock.Directory.Exists(fileSystemMock.Path.Combine(customPath, server, "Checklists")).Should().BeTrue();


    }


    [Fact]
    public void Initialize_WithYamlFormat()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("testingServer", "Yaml");

        var customPath = "CustomPath";
        var server = "testingServer";

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns(customPath);

        var fileSystemMock = new MockFileSystem();

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { Server = server, FileFormat = FileFormat.Yaml }, customPathMock.Object, fileSystemMock, new ParserFactory());

        behavior.Initialize();

        var parser = behavior.GetType().GetField("_parser", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(behavior) as IParser;
        parser.Should().NotBeNull().And.BeOfType<YamlParser>();

        var extension = behavior.GetType().GetField("_fileExtension", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(behavior) as string;
        extension.Should().Be(".yaml");

        fileSystemMock.Directory.Exists(fileSystemMock.Path.Combine(customPath, server, "Operators")).Should().BeTrue();
        fileSystemMock.Directory.Exists(fileSystemMock.Path.Combine(customPath, server, "Checklists")).Should().BeTrue();

    }

    [Fact]
    public void RetrieveChecklist_NullObject_json()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("testingServer", "Json");

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var deviceId = "9999999999";

        var pathMock = new Mock<IPath>();

        var customPath = "CustomPath";
        var server = "testingServer";

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns(customPath);

        var fileSystemMock = new MockFileSystem();
        pathMock.Setup(x => x.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns("TestingCombinedPath");


        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { Server = server, FileFormat = FileFormat.Json }, customPathMock.Object, fileSystemMock, new ParserFactory());

        behavior.Initialize();


        var res = behavior.RetrieveChecklist(operatorName, deviceId);

        res.Should().BeNull();
    }

    [Fact]
    public void RetrieveChecklist_NullObject_yaml()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("testingServer", "Yaml");

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var deviceId = "9999999999";

        var pathMock = new Mock<IPath>();

        var customPath = "CustomPath";
        var server = "testingServer";

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns(customPath);

        var fileSystemMock = new MockFileSystem();
        pathMock.Setup(x => x.Combine(It.IsAny<string>(), It.IsAny<string>())).Returns("TestingCombinedPath");


        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { Server = server, FileFormat = FileFormat.Yaml }, customPathMock.Object, fileSystemMock, new ParserFactory());

        behavior.Initialize();


        var res = behavior.RetrieveChecklist(operatorName, deviceId);

        res.Should().BeNull();
    }

    [Fact]
    public void RetrieveChecklist_json_Success()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("testingServer", "Json");

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var deviceId = "1";

        var checklist = new Models.Checklist
        {
            Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist multiple choice" },
                    new SelectMultiple { Code = "002", Prompt = "Indicate multiple choice", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                    new SelectMultiple { Code = "003", Prompt = "Indicate multiple choice with confirmation", ConfirmationEnabled = true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                    new SelectMultiple { Code = "004", Prompt = "Indicate option that can be omitted", SkipAllowed = true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                },
        };

        var parser = new JsonParser(new QuestionJsonConverter());
        var fileSystemMock = new MockFileSystem();
        fileSystemMock.AddFile(fileSystemMock.Path.Combine("CustomPath", "testingServer", "Checklists", $"{deviceId}.json"), new MockFileData(parser.Serialize(checklist)));

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { Server = "testingServer", FileFormat = FileFormat.Json }, customPathMock.Object, fileSystemMock, new ParserFactory());

        behavior.Initialize();

        var res = behavior.RetrieveChecklist(operatorName, deviceId);

        res.Should().BeEquivalentTo(checklist);
    }


    [Fact]
    public void RetrieveChecklist_Yaml_Success()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("testingServer", "Yaml");

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var deviceId = "1";

        var checklist = new Models.Checklist
        {
            Questions = new IQuestion[]
                {
                    new Message { Code = "001", Prompt = "Start checklist multiple choice" },
                    new SelectMultiple { Code = "002", Prompt = "Indicate multiple choice", Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                    new SelectMultiple { Code = "003", Prompt = "Indicate multiple choice with confirmation", ConfirmationEnabled = true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                    new SelectMultiple { Code = "004", Prompt = "Indicate option that can be omitted", SkipAllowed = true, Options = new[] { new SelectOption(01, "Option 1"), new SelectOption(02, "Option 2"), new SelectOption(03, "Option 3") } },
                },
        };
        var parser = new YamlParser();
        var fileSystemMock = new MockFileSystem();
        fileSystemMock.AddFile(fileSystemMock.Path.Combine("CustomPath", "testingServer", "Checklists", $"{deviceId}.yaml"), new MockFileData(parser.Serialize(checklist)));

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { Server = "testingServer", FileFormat = FileFormat.Yaml }, customPathMock.Object, fileSystemMock, new ParserFactory());

        behavior.Initialize();

        var res = behavior.RetrieveChecklist(operatorName, deviceId);

        res.Should().BeEquivalentTo(checklist);
    }

    [Fact]
    public void GetOperator_json()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("testingServer", "Json");

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var checklist = new Operator
        {
            Name = "user",
            Password = null
        };
        var parser = new JsonParser(new QuestionJsonConverter());
        var fileSystemMock = new MockFileSystem();
        fileSystemMock.AddFile(fileSystemMock.Path.Combine("CustomPath", "testingServer", "Operators", $"{operatorName}.json"), new MockFileData(parser.Serialize(checklist)));

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { Server = "testingServer", FileFormat = FileFormat.Json }, customPathMock.Object, fileSystemMock, new ParserFactory());

        behavior.Initialize();

        var res = behavior.GetOperator(operatorName);

        res.Should().BeEquivalentTo(checklist);
    }

    [Fact]
    public void GetOperator_yaml()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("testingServer", "Yaml");

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var checklist = new Operator
        {
            Name = "user",
            Password = null
        };
        var parser = new JsonParser(new QuestionJsonConverter());
        var fileSystemMock = new MockFileSystem();
        fileSystemMock.AddFile(fileSystemMock.Path.Combine("CustomPath", "testingServer", "Operators", $"{operatorName}.yaml"), new MockFileData(parser.Serialize(checklist)));

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { Server = "testingServer", FileFormat = FileFormat.Yaml }, customPathMock.Object, fileSystemMock, new ParserFactory());

        behavior.Initialize();

        var res = behavior.GetOperator(operatorName);

        res.Should().BeEquivalentTo(checklist);
    }

    [Fact]
    public void Checklist_FileExist()
    {
        Mock<IConfigService> configServiceMock = GetConfigService("testingServer", "Json");

        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        var operatorName = "user";

        var deviceId = "1";

        var checklist = new Models.Checklist
        {
            Questions = new[]
               {
                    new Message { Code = "001", Prompt = "Inicio checklist mensajes"},
                    new Message { Code = "002", Prompt = "Mensaje prioritario", Priority = true, StartTime = new DateTime(2022,10,24,10,0,0), EndTime = new DateTime(2022, 10, 24, 10, 1, 0), Operator = "Operador 1"},
                    new Message { Code = "003", Prompt = "Mensaje con confirmación", ReadyToContinue = true},
                    new Message { Code = "004", Prompt = "Mensaje con información adicional", ReadyToContinue = true, AdditionalInformation = "Información adicional"},
                    new Message { Code = "005", Prompt = "Mensaje que permite ser omitido", ReadyToContinue = true, SkipAllowed = true},
            }
        };
        var parser = new JsonParser(new QuestionJsonConverter());
        var fileSystemMock = new MockFileSystem();
        fileSystemMock.AddFile(fileSystemMock.Path.Combine("CustomPath", "testingServer", "Checklists", $"{deviceId}.json"), new MockFileData(parser.Serialize(checklist)));

        var customPathMock = new Mock<ICustomDataPath>();
        customPathMock.SetupGet(x => x.Path).Returns("CustomPath");

        var behavior = new Behavior(new BehaviorSettings(configServiceMock.Object) { Server = "testingServer", FileFormat = FileFormat.Json }, customPathMock.Object, fileSystemMock, new ParserFactory());

        var parserMock = new Mock<IParser>();


        parserMock.Setup(x => x.Parse<Models.Checklist>(It.IsAny<string>())).Returns(checklist);

        behavior.Initialize();

        var res = behavior.RetrieveChecklist(operatorName, deviceId);
        fileSystemMock.File.Exists(fileSystemMock.Path.Combine("CustomPath", "testingServer", "Checklists", "1.json")).Should().BeTrue();
    }
}
