import mock_catalyst
from mock_catalyst import EndOfApplication
from main import main

mock_catalyst.environment_properties["SwVersion.Locale"] = 'en';
mock_catalyst.environment_properties["Operator.Id"] = '7767';
mock_catalyst.environment_properties["Device.Id"] = '9999999999';

try:
    main()
except EndOfApplication as err:
    print ('Application error')
