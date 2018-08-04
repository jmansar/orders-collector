# Orders Collector

[![Build status](https://jmansar.visualstudio.com/Orders%20Collector/_apis/build/status/1?branch=master)](https://jmansar.visualstudio.com/Orders%20Collector/_build/latest?definitionId=1&branch=master)

Orders Collector is a simple Skype bot that facilitates the process of organizing office lunches by co-workers.
The bot reads lunch orders posted by users on a designated channel, groups orders by restaurant and presents them in a structured form.

> Note!
> The bot currently only accepts commands and writes responses in **Polish**.

## Technical

The bot is ASP.NET MVC Core application which exposes bot API endpoint consumed by [Azure Bot Service](https://azure.microsoft.com/en-gb/services/bot-service/).

Stack:

-   .NET Core 2.0
-   SQLite
-   Entity Framework Core 2.0
-   [BotBuilder.Standard](https://github.com/CXuesong/BotBuilder.Standard/wiki) - An unofficial CoreCLR targeted .NET Standard ported version of BotBuilder.

### Development

#### Prerequisites

-   .NET Core 2.0 SDK

#### Build

```
dotnet build
```

#### Run on local host

```
dotnet run --project src\OrdersCollector\OrdersCollector.csproj
```

The project will run by default on port 3978.

#### Test on local host using Bot Framework Emulator

You can send messages to the bot on local host using [Bot Framework Emulator](https://github.com/Microsoft/BotFramework-Emulator).
Download the stable version of the emulator (v3) and configure it to use the local bot endpoint - http://localhost:3978/api/messages

To learn more about how to use the emulator see: https://docs.microsoft.com/en-us/azure/bot-service/bot-service-debug-emulator

### Adding the bot to your group conversation

#### Self-hosted

The current implementation of the bot can only support a single group conversation. To use the bot in your group you need to host your own instance.
As the project is targeting .NET Core you have wide range of hosting options.
The requirement is to make your bot messaging endpoint publicly accessible over HTTPS with a valid SSL/TLS certificate.

To make your bot available in Skype (or other messaging service) you need to create a bot service registration. For that you'll need an Azure subscription.
See: https://docs.microsoft.com/en-us/azure/bot-service/bot-service-quickstart-registration?view=azure-bot-service-3.0

As part of the registration process the you'll get MS App Id and password generated. Copy these values to your bot instance `appsettings.json` file. Also, set `BotId` to the name of the bot specified on the registration form.

```javascript
  "MicrosoftAppId": "<APP_ID>",
  "MicrosoftAppPassword": "<APP_PASS>",
  "BotId": "<BOT_NAME>",
```

The next step is to add Skype channel to your bot registration. In Azure Management portal open your bot registration, go to `Bot Management -> Channels` select Skype channel and complete the steps to add the channel.

After the channel has been added click on the channel in channels list, you'll be offered to add the bot to your contacts in Skype. After that you can add your bot to existing or create a new group conversation.

The last step is to limit the access to the bot only to your designated conversation.

In `appsettings.json` set:

```javascript
  "AllowedConversations": [],
```

This will make the bot disabled for all conversations. Then, communicate with the bot on designated channel (use any command, e.g. `#pomoc`), the bot will respond with:

```
OrdersCollector is not enabled on this conversation. ConversationId = <CONVERSATION_ID>
```

Copy the conversation id to `appsettings.json`:

```javascript
  "AllowedConversations": ["<CONVERSATION_ID>"]
```

Confirm that the bot responds to commands in your conversation.

#### Hosted

Due to the current limitations of the bot the hosted version is not available yet.

## Usage

### Commands

#### Adding restaurants / suppliers

```
#nowyDostawca <NAME>
```

> TODO: Document all the commands.

### Example

```
User1: [Restaurant1] cheese burger
Bot  : Utw. zam. 'Restaurant1', 'User1' -> 'Restaurant1'
User2: [Restaurant1] chicken burger
Bot  : 'User2' -> 'Restaurant1'
User3: [Restaurant1] cheese burger
Bot  : 'User3' -> 'Restaurant1'


User1: #losuj Restaurant1
Bot  : Operatorem zamówienia do 'Restaurant1' z dnia 2018-07-29 zostaje User2. Dokonano losowania wśród (User1, User2, User3).

User2: #lista Restaurant1
Bot  : Zamówienie z dnia 2018-07-27
       Dostawca:
       Operator: User2
       Pozycje:
       * [User1] cheese burger
       * [User3] cheese burger
       * [User2] chicken burger
       Razem: 3
```

> Note.
>
> Due to Skype restrictions the bot can only respond to Skype group messages starting with the bot mention.
> See:
>
> ```
> @(BotName) #lista Restaurant1
> ```

## Timeline

-   **2012** - The first version of the bot released targeting .NET Framework 4.0. This version utilized now discontinued Skype Desktop API and required Skype desktop instance to be running in the background.
-   **2013** - Added bot web dashboard that allowed to see details of active orders and displayed various statistics.
-   **2014** - Skype Desktop API discontinued. The bot eventually stops working.
-   **2016** - The bot is ported to use newly released MS Bot Framework.
-   **2017** - The codebase is ported to .NET Core. Refactoring and removal of legacy code. Some of the less used / not used features got removed such as web dashboard.
-   **2018** - Further refactoring and code clean up. The current version of the code released as open source.

