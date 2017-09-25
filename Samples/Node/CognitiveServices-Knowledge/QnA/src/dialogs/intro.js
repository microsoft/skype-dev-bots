/**
* intro.js
* 
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*
*/

const strings = require('../strings');

module.exports = function () {
    bot.dialog('intro', 
        (session, args) => {
            let user;
            if (session.message.user) {
                if (session.message.user.name) {
                    user = session.message.user.name;
                } else {
                    user = session.message.user.id;
                }
            }
            const card = getIntroCard(session, user);
            const message = new builder.Message(session).addAttachment(card);
            session.endDialog(message);
        }
    )
    .triggerAction({
        matches:/^(hi|Hi|hello|Hello|Intro)$/i
    });

    function getIntroCard(session, user) {
        return new builder.HeroCard()
            .title('Welcome ' + user + ', ' + strings.INTRO_TITLE)
            .text(strings.INTRO_TEXT)
            .buttons([
                new builder.CardAction(session).title(strings.INTRO_BUTTON_TITLE).value(strings.INTRO_BUTTON_VALUE).type('postBack'),
            ]);
    }
};
