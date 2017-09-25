/**
* qnamaker.js
* 
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*
*/

'use strict';

global.builder = require('botbuilder');

const connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});

global.bot = new builder.UniversalBot(connector);

require('../dialogs/intro.js')();
require('../dialogs/help.js')();

bot.on('contactRelationUpdate', (message) => {
    if (message.action == 'add') {
        bot.beginDialog(message.address, 'intro');
    } else if (message.action == 'remove') {
        session.endConversation();
        session.userData = {};
        session.conversationData = {};
        session.privateConversationData = {};
        session.dialogData = {};
    }
});

bot.dialog('/', [
    (session, args) => {
        session.beginDialog('help');
    }
]);

module.exports = bot;