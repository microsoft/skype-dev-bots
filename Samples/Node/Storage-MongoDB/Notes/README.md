# Note Sample

## Description
Note is a bot that helps you manage personal notes. The bot uses MongoDB as storage. You can add a note, search for a note, and delete all the notes.

## Bot Demo
To add the Note Bot to your Skype account, click [here](https://join.skype.com/bot/9d35aa86-9465-4391-8bd5-2463fe357d89).

## Run locally
Set up the environment for your bot as described [here](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-quickstart). Install Node.js and npm if not already installed, and install the Bot Builder SDK for Node.js and restify as instructed.

#### Creating Azure Cosmos DB store with MongoDB

Use the following links to help you setup an Azure Cosmos DB store with MongoDB.  Make note of the connection string as you will use it later to establish a connection to the MongoDB store.

- [Build a MongoDB API web app with .NET and the Azure portal](https://docs.microsoft.com/en-us/azure/cosmos-db/create-mongodb-dotnet)
- [Build a MongoDB API console app with Java and the Azure portal](https://docs.microsoft.com/en-us/azure/cosmos-db/create-mongodb-java)
- [Connect a MongoDB application to Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/connect-mongodb-account)

#### Update Config

- In the .env file, add the value for MONGO_DB_STRING that you obtained earlier when setting up the Azure Cosmos DB with MongoDB. It should look something like this:
```
MONGO_DB_STRING=mongodb://username:password@host:port/[database]?ssl=true
```
- Also, replace $DB_NAME$ with your database name and $COLLECTION_NAME$ with your collection name that you will be using for the bot.
```
DB_NAME=$DB_NAME$
COLLECTION_NAME=$COLLECTION_NAME$
```

## Deploy the Bot Sample

#### Register the Sample Bot
Register the sample bot following this [link](https://docs.microsoft.com/en-us/bot-framework/portal-register-bot), and make a note of the Microsoft App ID and Password to update the configurations of your bot.

#### Update Config

- In the .env file, add values for MICROSOFT_APP_ID, and MICROSOFT_APP_PASSWORD with values obtained during the bot registration process.
```
MICROSOFT_APP_ID='value'
MICROSOFT_APP_PASSWORD='value'
```

## More Information
To get more information about the Microsoft Bot Framework and Azure Cosmos DB with MongoDB, please review the following resources:
- [Bot Builder SDK for Node.js](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-overview)
- [Introduction to Azure Cosmos DB: API for MongoDB](https://docs.microsoft.com/en-us/azure/cosmos-db/mongodb-introduction)
