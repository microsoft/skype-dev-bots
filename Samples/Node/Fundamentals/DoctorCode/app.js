/**
* app.js
*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*/

// set environmental variables
require('dotenv').config();

// import restify & botbuilder
var restify = require("restify");
var builder = require("botbuilder");

//import constant strings
var strings = require("./strings");

// setup restify server
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
    console.log("listening...");
});

// create chat connector
var connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});

// listen for messages from users
server.post("/api/messages", connector.listen());

// create a universal bot
var bot = new builder.UniversalBot(connector, [
    (session) => {
        session.replaceDialog("welcome");
    }
]);

// send a welcome message when the user adds the bot
bot.on("contactRelationUpdate", function (message) {
    if (message.action == "add") {
        bot.beginDialog(message.address, "welcome");
    }
});

// WELCOME dialog
bot.dialog("welcome", [
    function (session) {
        session.endDialog(getWelcomeMessage(session));
    }
]);

// HELP dialog
bot.dialog("help", [
    function (session, args) {
        session.endDialog(getHelpMessage(session));
    }
]).triggerAction({
    // the dialog is triggered when the user types in help(case insensitive)
    matches: new RegExp("help", "i"),
    onSelectAction: (session, args) => {
        session.beginDialog(args.action, args);
    }
});

// GREET BUTTON CLICK
bot.dialog("greetButtonClick", [
    function (session) {
        session.send(strings.GREET_INTRO);
        session.send(getWelcomeMessage(session));
        session.send(strings.GREET_TRY_MSG);
        var codeMsg = new builder.Message(session)
            .text(strings.GREET_CODE)
            .textFormat("xml");
        session.send(codeMsg);
        session.send(strings.GREET_EXPLAIN);
        session.endDialog(strings.GREET_MORE);
    }
]).triggerAction({
    matches: new RegExp(strings.CHOICE_GREET, "i")
});

// HELP BUTTON CLICK
bot.dialog("helpButtonClick", [
    function (session, args) {
        session.send()
        session.send(strings.HELP_INTRO);
        session.send(getHelpMessage(session));
        session.send(strings.HELP_TRY_MSG);

        var codeMsg = new builder.Message(session)
            .text(strings.HELP_CODE)
            .textFormat("xml");
        session.send(codeMsg);

        session.send(strings.HELP_EXPLAIN);
        session.endDialog(strings.HELP_MORE);
    }
]).triggerAction({
    matches: new RegExp(strings.CHOICE_HELP, "i")
});

// TYPING BUTTON CLICK
bot.dialog("typingButtonClick", [
    function (session) {
        session.send(strings.TYPING_INTRO);
        session.send(strings.TYPING_TRY_MSG_1);
        session.sendTyping();
        // send the typing indicator for few seconds (10 sec)
        setTimeout(function() {
            session.send(strings.TYPING_TRY_MSG_2);

            var codeMsg = new builder.Message(session)
                .text(strings.TYPING_CODE)
                .textFormat("xml");
            session.send(codeMsg);

            session.send(strings.TYPING_EXPLAIN);
            session.endDialog(strings.TYPING_MORE);
        }, 10000);
    }
]).triggerAction({
    matches: new RegExp(strings.CHOICE_TYPING, "i"),
    onSelectAction: (session, args) => {
        session.replaceDialog(args.action, args)
    }
});

// builds a message with hero card for the welcome dialog
function getWelcomeMessage(session) {
    return new builder.Message(session)
        .summary(strings.GREET)
        .addAttachment(
        new builder.HeroCard(session)
            .text(strings.GREET)
            .images([builder.CardImage.create(session, strings.FACE_IMAGE_URL)])
            .buttons([
                builder.CardAction.imBack(session, strings.CHOICE_GREET, strings.CHOICE_GREET),
                builder.CardAction.imBack(session, strings.CHOICE_HELP, strings.CHOICE_HELP),
                builder.CardAction.imBack(session, strings.CHOICE_TYPING, strings.CHOICE_TYPING)
            ]));
}

// builds a message with hero card for the help dialog
function getHelpMessage(session) {
    return new builder.Message(session)
        .addAttachment(
        new builder.HeroCard(session)
            .title(strings.HELP_TITLE)
            .text(strings.HELP)
            .buttons([
                builder.CardAction.imBack(session, strings.CHOICE_GREET, strings.CHOICE_GREET),
                builder.CardAction.imBack(session, strings.CHOICE_HELP, strings.CHOICE_HELP),
                builder.CardAction.imBack(session, strings.CHOICE_TYPING, strings.CHOICE_TYPING)
            ]));
}