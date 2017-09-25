/**
* proposeQuestionsCard.js
* 
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*
*/

'use strict';

const _ = require('lodash');

const faqFetcher = require('../controller/faqFetcher');
const buildCardUtils = require('../utils/buildCardUtils');
const strings = require('../strings');

module.exports = function () {
    bot.dialog('proposeQuestionsCard', [
        (session, args, next) => {
            session.endDialog(getProposeQuestionsCard(session));
        }
    ]);

    function getProposeQuestionsCard(session) {
        let cards = new Array();
        let title = '\n**' + strings.PROPOSE_QUESTION_CARD_TITLE + '**\n\n';

        faqFetcher
            .streamFaq()
            .then(data => {
                _.forEach(data, (result) => {
                    cards.push(buildCardUtils.buildCard(session, result));
                });

                let reply = new builder.Message(session)
                    .text(title)
                    .attachmentLayout(builder.AttachmentLayout.carousel)
                    .attachments(cards);

                session.send(reply);
            })
    }
};