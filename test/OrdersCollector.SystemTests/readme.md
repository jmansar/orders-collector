# System tests for Orders Collector.
This project contains end to end automation tests for Orders Collector.
The tests require bot registration resource created in Azure with Direct Line channel enabled.

## Configuration
The configuration values can be set either in `testsettings.json` or as environmental variables:
* **DirectLineSecret** - The secret key generated in Azure Portal for Direct Line channel.
* **BotId** - Id of the bot in Azure. E.g. orders-collector-test