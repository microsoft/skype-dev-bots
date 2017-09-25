/**
* faqFetcher.js
* 
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*
*/

var request = require('request');
var url = require('url');
const qnaQueryService = require('../controller/qnaQueryService');
const strings = require('../strings');
var faqModel = require('../models/faqModel');

module.exports = function () {
    var data = [];

    function streamFaq() {
        return new Promise(
            (resolve, reject) => {
                qnaQueryService
                    .getFaqUrl()
                    .then(docUrl => {
                        streamDataFromUrl(JSON.parse(docUrl))
                        .then((response) => {
                            resolve(parseDataToArray(response));                
                        });
                    }
                );
            });
    }

    function streamDataFromUrl(docUrl) {
        var stream = '';

        return new Promise(
            (resolve, reject) => {
                request.
                    get(docUrl)
                    .on('response', response => {
                        if (response.statusCode != 200) {
                            reject(response.statusCode);
                        } 
                    })
                    .on('data', data => {
                        stream += data;
                    })
                    .on('end', () => {
                        if (stream == null) {
                            reject(new Error('failed to get data'));
                        } else {
                            resolve(stream);
                        }
                    }) 
                    .on('error', error => {
                        reject(error);
                    })
            }
        );
    }

    function parseDataToArray(response) {
        faq = response.split('\t');
        var counts = 0;

        for (var i = 0; i < faq.length; i++) {
            var item = faq[i];
            var position = item.lastIndexOf('?');

            if (position != -1 && counts < strings.QUERY_RESULT_COUNTS) {
                counts = counts + 1;
                item = item.substring(item.indexOf('\n') + 1).replace(/[\t|\r|\n]/g, '').replace('https://azure.microsoft.com/en-us/support/faq/', '');

                var model = new faqModel(item, faq[i + 1].replace(/[\t|\r?|\n|\r\\n?]/g, ''));

                data.push(model);
            }
        }

        return data;
    }

    return {
        streamFaq: streamFaq
    };
}();
