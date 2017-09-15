/**
* strings.js
*
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*/

// constant strings
module.exports = {
    FACE_IMAGE_URL: "http://drcodebotnodejs.azurewebsites.net/assets/2x1_bust_shot.png",

    NAME: "Doctor Code",
    GREET: "Welcome! I am Doctor Code. "
        + "I am here to teach you few basic things about building a Skype bot. "
        + "What do you want to learn today?",
    HELP: "You can say or click \'greet a user\', \'build help options\', or \'show delay indicators\' "
        + "to learn how to add those functionalities to your bot.",
    HELP_TITLE: "Help",
    
    CHOICE_GREET: "greet a user",
    CHOICE_HELP: "build help options",
    CHOICE_TYPING: "show delay indicators",
    
    GREET_INTRO: "Bot should introduce itself to the user and give an excellent first impression. "
        + "Let's learn how to greet a user on the first run.",
    GREET_TRY_MSG: "I sent you this greeting when we met for the first time. "
        + "Let's look into the code!",
    GREET_CODE: "<pre>// send a welcome message when the user adds the bot\n"
        + "bot.on(&quot;contactRelationUpdate&quot;, function (message) {\n"
        + "    if (message.action == &quot;add&quot;) {\n"
        + "        bot.beginDialog(message.address, &quot;welcome&quot;);\n"
        + "    }\n"
        + "});</pre>",
    GREET_EXPLAIN: "The contact relation update event notifies the bot "
        + "that a user has added the bot to the contact list. "
        + "\'bot.on\' registers an event listener for the contact relation update event, "
        + "triggering the function you provided as an argument.",
    GREET_MORE: "To learn more, "
        + "read <a href=\"https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-handle-conversation-events\">Handle user and conversation events</a>.",
    
    HELP_INTRO: "Bot should provide help options to the user. "
        + "Let's learn how to build these options.",
    HELP_TRY_MSG: "Any time the user types in \'help\', "
        + "the message that you just received will be sent to assist users. "
        + "Let's look into the code!",
    HELP_CODE: "<pre>bot.dialog(&quot;help&quot;, [\n"
        + "    function (session, args) {\n"
        + "        session.endDialog(getHelpMessage(session));\n"
        + "    }\n"
        + "]).triggerAction({\n"
        + "    // the dialog is triggered when the user types in help(case insensitive)\n"
        + "    matches: new RegExp(&quot;help&quot;, &quot;i&quot;),\n"
        + "    onSelectAction: (session, args) => {\n"
        + "    session.beginDialog(args.action, args);\n"
        + "    }\n"
        + "});</pre>",
    HELP_EXPLAIN: "The \'help\' dialog will be triggered when the user message matches the provided regular expression. "
        + "The default behavior of triggerAction() is to interrupt any existing dialog by clearing the stack. "
        + "However, as shown in this example, you can overwrite this behavior with the onSelectAction handler.",
    HELP_MORE: "To learn more, "
        + "read <a href=\"https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-dialog-actions\">Handle user actions</a>.",

    TYPING_INTRO: "For time consuming operations, "
        + "you want to visually inform user that the message from the bot is on its way. "
        + "Let's learn how to use typing indicators.",
    TYPING_TRY_MSG_1: "Let's try it out! "
        + "I will indicate that I am typing for 10 seconds.",
    TYPING_TRY_MSG_2: "Let's look into the code!",
    TYPING_CODE: "<pre>// TYPING BUTTON CLICK\n"
        + "bot.dialog(&quot;typingButtonClick&quot;, [\n"
        + "    function (session) {\n"
        + "        session.send(text.TYPING_INTRO);\n"
        + "        session.send(text.TYPING_TRY_MSG_1);\n"
        + "        // send the typing indicator for few seconds (10 sec)\n"
        + "        setTimeout(function() {\n"
        + "            session.send(text.TYPING_TRY_MSG);\n"
        + "            var codeMsg = new builder.Message(session)\n"
        + "                .text(text.TYPING_CODE)\n"
        + "                .textFormat(&quot;xml&quot;);\n"
        + "        session.send(codeMsg);\n"
        + "        session.send(text.TYPING_EXPLAIN);\n"
        + "        session.endDialog(text.TYPING_MORE);\n"
        + "        }, 10000);\n"
        + "    }\n"
        + "]).triggerAction({\n"
        + "    matches: new RegExp(text.CHOICE_TYPING, &quot;i&quot;)\n"
        + "});</pre>",
    TYPING_EXPLAIN: "sendTyping function sends the user an indication that the bot is typing. "
        + "The typing indicator clears out when the bot sends a message. ",
    TYPING_MORE: "To learn more, "
        + "read <a href=\"https://docs.microsoft.com/en-us/bot-framework/nodejs/bot-builder-nodejs-send-typing-indicator\">Send a typing indicator</a>."
};