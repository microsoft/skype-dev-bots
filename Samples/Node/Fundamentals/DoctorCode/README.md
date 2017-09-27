# Doctor Code

[![Deploy to Azure][Deploy Button]][Deploy Node/Fundamentals/DoctorCode]

[Deploy Button]: https://azuredeploy.net/deploybutton.png
[Deploy Node/Fundamentals/DoctorCode]: https://azuredeploy.net

## Description
Doctor Code teaches a developer how to implement few basic functionalities in a Skype bot. You can now build a bot using a bot. Currently, Doctor Code teaches you the following.
- Greet a user: How to send a greeting to your user when your bot meets the user for the first time.
- Build help options: How to provide your users the ability to ask for help in the middle of the conversation.
- Show delay indicators: How to prevent your users from thinking that your bot is broken while your bot is working on time consuming operations.

The list will continue to grow, so be excited and stay tuned!

## Bot Demo
To add Doctor Code to your Skype account, click [here](https://join.skype.com/bot/fbe8a151-d202-4e8d-8abd-0c469c9b653d).

## How it Works
- Dialogs: Doctor Code operates on multiple dialogs (welcome, help, etc.),  each representing the functionality that the bot can teach. Learn more about dialogs [here](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-dialog-manage-conversation-flow).

- Actions: Doctor Code makes use of actions to understand users. Actions make it possible to handle interruptions any time during the conversation flow. Learn more about actions [here](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-dialog-actions).

- Rich Cards: Welcome dialog and help dialog sends hero cards, a type of rich cards, to the users. Learn more about rich cards [here](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-send-rich-cards).

## Do It Yourself (Deploy the Bot Sample)
In this section, we will go over how to deploy this Doctor Code sample from start to end.

#### Prerequisites
Set up the environment for your bot as described [here](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-quickstart). Install Node.js and npm if not already installed, and install the Bot Builder SDK for Node.js and restify as instructed. We use [dotenv](https://www.npmjs.com/package/dotenv) package to load environmental variables. Install the dotenv package as instructed in the link webpage.

#### Register the Sample Bot
Register the sample bot following this [link](https://docs.microsoft.com/en-us/bot-framework/portal-register-bot), and make a note of the Microsoft App ID and Password to update the configurations of your bot.

#### Update Configurations
- In .env file, replace $MICROSOFT_APP_ID$ and $MICROSOFT_APP_PASSWORD$ with values obtained during the bot registration from the previous step.
```
MICROSOFT_APP_ID=$MICROSOFT_APP_ID$
MICROSOFT_APP_PASSWORD=$MICROSOFT_APP_PASSWORD$
```

#### Deploy the bot to the cloud
Prerequisites and instructions for deploying the bot are [here](https://docs.microsoft.com/en-us/bot-framework/deploy-bot-overview). After deploying the bot, you can add your bot as a contact by using its join link. You can access the bot's join link from Microsoft Bot Framework, by clicking on your bot and clicking on the **Skype** in Connect to channels tab.


## More Information
To get more information about the Microsoft Bot Framework, please review the following resources:
- [Bot Builder SDK for Node.js](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-overview)
- [Handle user and conversation events](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-handle-conversation-events)
- [Handle user actions](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-dialog-actions)
- [Send a typing indicator](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-send-typing-indicator) <br /> <br />