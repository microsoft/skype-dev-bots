// set environmental variables
require('./config.js');

// import restify & botbuilder
var restify = require('restify');
var builder = require('botbuilder');

//import constant texts and utils
var text = require('./text');
var util = require('./util');

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
server.post('/api/messages', connector.listen());

// create a universal bot
var bot = new builder.UniversalBot(connector);

// send a welcome message when the user adds the bot
bot.on('contactRelationUpdate', function (message) {
    if (message.action == 'add') {
        // send a greeting message when added
        bot.send(new builder.Message().address(message.address).text(text.GREET));
    } else if (message.action == 'remove') {
        // clear up the user data when removed
        bot.loadSession(message.address, function (err, session) {
            session.userData = {};
            session.save();
        });
    }
});

// endpoint to the LUIS recognizer
var luisEndpoint = process.env.MICROSOFT_LUIS_ENDPOINT;

// add a LUIS recognizer to the bot
bot.recognizer(new builder.LuisRecognizer(luisEndpoint));

// dialog for NONE: not supported yet
bot.dialog('none',
    function (session, args, next) {
        util.initFridge(session);
        session.endDialog(text.NOT_YET_SUPPORTED);
    }
).triggerAction({
    matches: 'None'
});

// dialog for PUT: putting in ingredients
bot.dialog('put', [
    function (session, args, next) {
        var entity = builder.EntityRecognizer.findEntity(args.intent.entities, 'item');
        var item = entity ? entity.entity : null;

        if (item == null) {
            session.endDialog(text.NO_ITEM_TO_ADD);
        } else {
            util.addToFridge(session, item);
            session.endDialog(text.ADDED, item);
        }
    }
]).triggerAction({
    matches: 'add'
});

// dialog for REMOVE: getting out ingredients
bot.dialog('remove', [
    function (session, args, next) {
        var entity = builder.EntityRecognizer.findEntity(args.intent.entities, 'item');
        var item = entity ? entity.entity : null;

        if (item == null) {
            session.endDialog(text.NO_ITEM_TO_REMOVE);
        }

        if (util.isInFridge(session, item)) {
            util.removeFromFridge(session, item);
            session.endDialog(text.REMOVED_SUCC, item);
        } else {
            session.endDialog(text.REMOVED_ERR, item);
        }
    }
]).triggerAction({
    matches: 'remove'
});

// dialog for SHOW: get list of ingredients
bot.dialog('show', [
    function (session, args, next) {
        session.endDialog(text.SHOW, util.itemsToString(session.userData.ingredients));
    }
]).triggerAction({
    matches: 'show'
});

// dialog for HELP: give instructions
bot.dialog('help',
    function (session, args, next) {
        session.endDialog(text.HELP);
    }
).triggerAction({
    matches: 'help'
});

// dialog for CLEAR: clear all inventory
bot.dialog('clear', [
    function (session, args, next) {
        builder.Prompts.text(session, text.CLEAR);
    },
    
    function (session, results) {
        if (results.response == 'yes') {
            util.removeAllFromFridge(session);
            session.endDialog(text.CLEAR_YES);
        } else {
            session.endDialog(text.CLEAR_NO);
        }
    }
]).triggerAction({
    matches: 'clear'
});