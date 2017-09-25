/**
* answersCarousel.js
* 
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*
*/

'use strict';

const _ = require('lodash');
const buildCardUtils = require('../utils/buildCardUtils');

module.exports = function () {
    bot.dialog('answersCarousel', [
        (session, args, next) => {
            session.endDialog(getShowAnswersCarousel(session, args.data));
        }
    ]);

    function getShowAnswersCarousel(session, data) {
        let meaningfulAnswerCounts = 0;
        let cards = new Array();

        _.forEach(data, (result) => {
            cards.push(buildCardUtils.buildCard(session, result));
            meaningfulAnswerCounts++;
        });

        let title = '\n**The top ' + meaningfulAnswerCounts + ' answers are:**\n\n';
        let reply = new builder.Message(session)
            .text(title)
            .attachmentLayout(builder.AttachmentLayout.carousel)
            .attachments(cards);
        session.send(reply);
    }
};