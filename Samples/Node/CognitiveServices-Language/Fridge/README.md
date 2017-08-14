# Fridge Bot Sample (Node.js)

## Description
Fridge Bot is a bot that helps you to manage refrigerator inventory. You can add or delete an item or see or clear the inventory of your fridge. The bot uses LUIS (Language Understanding Intelligent Service) Cognitive Service to recognize your intents so that you can talk to the bot as if you are talking to a person. This sample bot will serve as a good example of incorporating natural language processing service to Skype bots.

## Bot Demo
To add the Fridge Bot to your Skype account, click [here](https://join.skype.com/bot/f2cf7cce-f6c5-427f-84b6-9099c7b877a8).

## How it Works
- The LUIS service associated with this sample bot can understand six different intents: None, add, clear, help, remove and show. It has one entity named item, which is associated with add and remove intents. Each of the intents triggers an action associated with that intent and the action performs necessary operation and ends the dialog.
- Data that contains saved items is kept track of with userData for simplicity of the sample bot.
- The app listens for messages from users and the registered LUIS recognizer will trigger the dialog with the recognize intent.

```
// dialog for PUT: putting in ingredients
bot.dialog('put', [
    function (session, args, next) {
        var entity = builder.EntityRecognizer.findEntity(args.intent.entities, 'item');
        var item = entity ? entity.entity : null;

        if (item == null) {
            session.endDialog(text.NOT_YET_SUPPORTED);
        } else {
            util.addToFridge(session, item);
            session.endDialog(text.ADDED, item);
        }
    }
]).triggerAction({
    matches: 'add'
});
```
- Let's look at the 'put' dialog above. This dialog is for the 'add' intent. When the LUIS recognizer understands the user's intent as 'add', this dialog will be triggered. This 'put' dialog then, finds the 'item' entity which is the item that the user wants to put into the fridge. It uses the *args.intent.entities* which includes the entities of the recognized intent to do so. Finally, the dialog calls the helper function *util.addToFridge()* to put the item into the fridge and ends the dialog.


## Do It Yourself (Deploy the Bot Sample)
In this section, we will go over how to deploy this Fridge Bot sample from start to end.

#### Prerequisites
Set up the environment for your bot as described [here](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-quickstart). Install Node.js and npm if not already installed, and install the Bot Builder SDK for Node.js and restify as instructed.

#### Create a LUIS Application
This Fridge Bot Sample uses Language Understanding Intelligent Service, or, LUIS, to understand user's intents. Learn more about LUIS [here](https://docs.microsoft.com/en-us/azure/cognitive-services/LUIS/Home). Let's create a new LUIS Application for your Fridge Bot.

1. Create a new application <br />
![Create a new application](images/LUIS_create_a_new_app.png) <br />
Sign into [LUIS website](https://www.luis.ai) and go to **My Apps** tab. Click on **New App** button to create a new LUIS app.

2. Add a new intent <br />
![Add a new intent](images/LUIS_add_intent.png) <br />
Intents represent the actions that the user wants to perform. For this sample bot, your LUIS app will need to handle six different intents: None, add, clear, help, remove and show. To add a new intent, locate to your LUIS app and click on **Intents** on the left panel. Click on **Add Intent** button and type in the name of the intent you want to create. Here, we are creating the help intent.

3. Enter utterances <br />
![Enter utterances](images/LUIS_add_utterances.png) <br />
Utterances are textual input from the user. After creating an intent, you can add expected utterances to the intent. Don't forget to hit **Save** button after adding utterances.

4. Add a new entity <br />
![Add a new entity](images/LUIS_add_entity.png) <br />
Entities represent the object that is relevant to the intent. For example, for the utterance "add an apple", "add" is the intent and "apple" is the entity. For some intents such as "help", entites are not required. However, for some intents like the "add" example, entities are necessary. For this sample bot, your LUIS app will need to handle one custom simple entity, called item. To create a new entity, click on **entities** on the left panel and click on **add entity** button.

5. Label entity in utterances <br />
![Label entity in utterances](images/LUIS_label_entity.png) <br />
Locate to the intent where entities are need to be labeled. For example, "add" intent needs an entity. Click on the word that you want to label as an entity for the utterances and select type of the entity that you want to label it to.

6. Train, test and publish the app <br />
![Train, test and publish the app](images/LUIS_train_and_test.png) <br />
At this point, you have first, added six intents (None, add, clear, help, remove and show), second, added one entity (item), and third, added many utterances to each of the intents and correctly labeled the entities for each of the utterances. Now, you have to train the app with the information you provided. Go to the **Train & Test** from the left panel and click on the **Train Application** button to train your app. You can then test the model by typing in a test utterance. Then, go to the **Publish App** and click on **Publish** button to publish your app. The endpoint url for your LUIS app will be created. Make a note of this url to connect it with your sample bot.

#### Register the Sample Bot
Register the sample bot following this [link](https://docs.microsoft.com/en-us/bot-framework/portal-register-bot), and make a note of the Microsoft App ID and Password to update the configurations of your bot.

#### Update Configurations
- In config.js file, replace $MICROSOFT_APP_ID$ and $MICROSOFT_APP_PASSWORD$ with values obtained during the bot registration from the previous step.
```
process.env['MICROSOFT_APP_ID'] = '$MICROSOFT_APP_ID$'
process.env['MICROSOFT_APP_PASSWORD'] = '$MICROSOFT_APP_PASSWORD$';
```
- Also, replace $MICROSOFT_LUIS_ENDPOINT$ with the endpoint url to your LUIS app.
```
process.env['MICROSOFT_LUIS_ENDPOINT'] = '$MICROSOFT_LUIS_ENDPOINT$';
```

#### Deploy the bot to the cloud
Prerequisites and instructions for deploying the bot are [here](https://docs.microsoft.com/en-us/bot-framework/deploy-bot-overview). After deploying the bot, you can add your bot as a contact by using its join link. You can access the bot's join link from Microsoft Bot Framework, by clicking on your bot and clicking on the **Skype** in Connect to channels tab.


## More Information
To get more information about the Microsoft Bot Framework and LUIS, please review the following resources:
- [Bot Builder SDK for Node.js](https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-overview)
- [Add intelligence to bots with Cognitive Services](https://docs.microsoft.com/en-us/bot-framework/cognitive-services-bot-intelligence-overview#language-understanding)
- [Learn about Language Understanding Intelligent Service(LUIS)](https://docs.microsoft.com/en-us/bot-framework/cognitive-services-bot-intelligence-overview#language-understanding) <br /> <br />