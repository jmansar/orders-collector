# Orders Collector

[![Build status](https://jmansar.visualstudio.com/Orders%20Collector/_apis/build/status/1?branch=master)](https://jmansar.visualstudio.com/Orders%20Collector/_build/latest?definitionId=1&branch=master)

Orders Collector is a simple Skype bot that facilitates the process of organizing office lunches by co-workers.
The bot reads lunch orders posted by users on a designated channel, groups orders by restaurant and presents them in a structured form.

> Note!
> The bot currently only accepts commands and writes responses in **Polish**.

## Technical

The bot is ASP.NET MVC Core application which exposes bot API endpoint consumed by [Azure Bot Service](https://azure.microsoft.com/en-gb/services/bot-service/).

Stack:

- .NET Core 2.0
- SQLite
- Entity Framework Core 2.0
- [BotBuilder.Standard](https://github.com/CXuesong/BotBuilder.Standard/wiki) - An unofficial CoreCLR targeted .NET Standard ported version of BotBuilder.

### Development

#### Prerequisites

- .NET Core 2.0 SDK

#### Build

```
dotnet build
```

### Run on local host

```
dotnet run --project src\OrdersCollector\OrdersCollector.csproj
```

The project will run by default on port 3978.

### Test on local host using Bot Framework Emulator

You can send messages to the bot on local host using [Bot Framework Emulator]( https://github.com/Microsoft/BotFramework-Emulator).
Download the stable version of the emulator (v3) and configure it to use the local bot endpoint - http://localhost:3978/api/messages

To learn more about how to use the emulator see: https://docs.microsoft.com/en-us/azure/bot-service/bot-service-debug-emulator

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

