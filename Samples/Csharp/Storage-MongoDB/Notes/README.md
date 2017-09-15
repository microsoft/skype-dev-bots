# Note Sample

## Description
Note is a bot that helps you manage personal notes. The bot uses MongoDB as storage. You can add a note, search for a note, and delete all the notes.

## Bot Demo
To add the Note Bot to your Skype account, click [here](https://join.skype.com/bot/d567a1c2-ffe6-400d-b955-efc15f64f085).

## Run locally
Set up the environment for your bot as described [here](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-quickstart). Install Visual Studio 2017 for Windows and update all extensions. Download the Bot Application, Bot Controller, and Bot Dialog templates and install it as instructed.

#### Creating Azure Cosmos DB store with MongoDB

Use the following links to help you setup an Azure Cosmos DB store with MongoDB.  Make note of the connection string as you will use it later to establish a connection to the MongoDB store.

- [Build a MongoDB API web app with .NET and the Azure portal](https://docs.microsoft.com/en-us/azure/cosmos-db/create-mongodb-dotnet)
- [Build a MongoDB API console app with Java and the Azure portal](https://docs.microsoft.com/en-us/azure/cosmos-db/create-mongodb-java)
- [Connect a MongoDB application to Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/connect-mongodb-account)

#### Update Config

- In Web.config file, replace $DBName$ with the name of your database.
```
<!-- Database Name for the Mongo DB-->
<add key="DBName" value="$DBName$"/>
```
- Also, add the value for $MongoDBConnectionString$ that you obtained earlier when setting up the Azure Cosmos DB with MongoDB. It should look something like this:
```
<connectionStrings>
    <add name="MongoDB" connectionString="mongodb://username:password@host:port/[database]?ssl=true"/>
</connectionStrings>
```

## Deploy the Bot Sample

#### Register the Sample Bot
Register the sample bot following this [link](https://docs.microsoft.com/en-us/bot-framework/portal-register-bot), and make a note of the Microsoft App ID and Password to update the configurations of your bot.

#### Update Config

- In Web.config file, replace $MicrosoftAppId$ and $MicrosoftAppPassword$ with values obtained during the bot registration from the previous step. Also, replace $BotId$ with what you want your bot Id to be. Conventionally, Bot Id is set to "28:$MicrosoftAppId$".
```
<appSettings>
    <!-- update these with your BotId, Microsoft App Id and your Microsoft App Password-->
    <add key="BotId" value="$BotId$" />
    <add key="MicrosoftAppId" value="$MicrosoftAppId$" />
    <add key="MicrosoftAppPassword" value="$MicrosoftAppPassword$" />
</appSettings>
```

#### Deploy to Cloud

Follow the instructions [here](https://docs.microsoft.com/en-us/bot-framework/deploy-bot-overview) to deploy the bot to Azure.

## More Information
To get more information about the Microsoft Bot Framework and Azure Cosmos DB with MongoDB, please review the following resources:
- [Bot Builder SDK for .NET](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-overview)
- [Introduction to Azure Cosmos DB: API for MongoDB](https://docs.microsoft.com/en-us/azure/cosmos-db/mongodb-introduction)
