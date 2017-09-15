# Doctor Code

## Description
Doctor Code teaches a developer how to implement few basic functionalities in a Skype bot. You can now build a bot using a bot. Currently, Doctor Code teaches you the following.
- Greet a user: How to send a greeting to your users when your bot meets the users for the first time.
- Build help options: How to provide your users the ability to ask for help in the middle of the conversation.
- Show delay indicators: How to prevent your users from thinking that your bot is broken while your bot is working on time consuming operations.
The list will continue to grow, so be excited and stay tuned!


## Bot Demo
To add Doctor Code to your Skype account, click [here](https://join.skype.com/bot/f3add69f-56bc-4acb-9ff1-40527ddb3d8a).

## How it Works
- Activities: MessagesController is the entry point of activities. An activity can be a message between a bot and a user, or can be other types of activities such as contactRelationUpdate activity that indicates that the bot was added or removed from a user's contact list. Learn more about activities [here](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-activities).
- Dialogs: Doctor Code operates on five dialogs - the root dialog, help dialog, and the three additional dialogs for handling the button clicks, each representing the functionality that the bot can teach. Learn more about dialogs [here](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-dialogs).
- Global Message Handlers: Users commonly attempt to seek help by typing "help" in the middle of the conversation. You can handle these types of user requests by implementing global message handlers. Learn more about global message handlers [here](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-global-handlers).
- Rich Cards: Welcome dialog and help dialog sends hero cards, a type of rich cards, to the users. Learn more about rich cards [here](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-add-rich-card-attachments).

## Do It Yourself (Deploy the Bot Sample)
In this section, we will go over how to deploy this Doctor Code sample from start to end.

#### Prerequisites
Set up the environment for your bot as described [here](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-quickstart). Install Visual Studio 2017 for Windows and update all extensions. Download the Bot Application, Bot Controller, and Bot Dialog templates and install it as instructed.

#### Register the Sample Bot
Register the sample bot following this [link](https://docs.microsoft.com/en-us/bot-framework/portal-register-bot), and make a note of the Microsoft App ID and Password to update the configurations of your bot.

#### Update Configurations
- In Web.config file, replace $MicrosoftAppId$ and $MicrosoftAppPassword$ with values obtained during the bot registration from the previous step. Also, replace $BotId$ with what you want your bot Id to be. Conventionally, Bot Id is set to "28:$MicrosoftAppId$".
```
<appSettings>
    <!-- update these with your BotId, Microsoft App Id and your Microsoft App Password-->
    <add key="BotId" value="$BotId$" />
    <add key="MicrosoftAppId" value="$MicrosoftAppId$" />
    <add key="MicrosoftAppPassword" value="$MicrosoftAppPassword$" />
</appSettings>
```

#### Deploy the bot to the cloud
Prerequisites and instructions for deploying the bot are [here](https://docs.microsoft.com/en-us/bot-framework/deploy-bot-overview). After deploying the bot, you can add your bot as a contact by using its join link. You can access the bot's join link from Microsoft Bot Framework, by clicking on your bot and clicking on the **Skype** in Connect to channels tab.


## More Information
To get more information about the Microsoft Bot Framework, please review the following resources:
- [Bot Builder SDK for .NET](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-overview)
- [Activities Overview](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-activities)
- [Add rich card attachments to messages](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-add-rich-card-attachments)
- [Implement global message handlers](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-global-handlers)
- [Global message handlers using scorables](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-scorable-dialogs)
- [Global Message Handlers sample](https://github.com/Microsoft/BotBuilder-Samples/tree/master/CSharp/core-GlobalMessageHandlers) <br /> <br />