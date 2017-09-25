/**
* buildCardUtils.js
* 
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*
*/

'use strict'

const strings = require('../strings');

module.exports = function () {
    function buildCard(session, result) {
        let subTitleText = result.question ? 
                        result.question : 
                        result.questions ? result.questions[0] : null;
        return new builder.HeroCard(session)
            .subtitle(subTitleText)
            .text(result.answer ? result.answer : null)
            .tap(new builder.CardAction(session).value(strings.FAQ_URL).type('openUrl'));
    }

    return {
        buildCard: buildCard
    }
}();