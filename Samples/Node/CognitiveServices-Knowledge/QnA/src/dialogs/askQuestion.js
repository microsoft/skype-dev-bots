/**
* askQuestion.js
* 
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*
*/

const _ = require('lodash');
const qnaQueryService = require('../controller/qnaQueryService');
const strings = require('../strings');
var fs = require('fs');

require('../dialogs/answersCarousel')();
require('../dialogs/proposeQuestionsCard')();

module.exports = function () {
    var data;

    bot.dialog('askQuestion', [
        (session, args, next) => {
            session.options.autoBatchDelay = 1000;
            let question = session.message.text;

            if (question) {
                qnaQueryService
                    .query(question, strings.QUERY_RESULT_COUNTS)
                    .then(results => {
                        handleResult(session, results);
                    },
                    err => {
                        return err;
                    }
                    );
            }

            function handleResult(session, results) {
                if (results) {
                    var resultsJson = JSON.parse(results);
                    data = resultsJson.answers;

                    if (data && !_.isEmpty(data)) {
                        if (data.length == 1 && data[0].answer === strings.NO_ANSWER_FOUND) {
                            session.send(strings.NO_MATCH_FROM_SERVICE);

                            // Get the first 3 questions from faq, and build a list carousel
                            session.beginDialog('proposeQuestionsCard');
                        } else {
                            session.beginDialog('answersCarousel', { data: data });
                        }
                    }
                }
            }
        }
    ]).triggerAction({
        matches: /(what is|Question:|How to|How do|How|Does|Do|I have a question|Is there|where).*\?$/i
    });
};
