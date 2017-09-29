/**
* qnaQueryService.js
* 
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*
*/

require('dotenv').config();

const request = require('request');

module.exports = function () {
    const headers = {
        'content-type': 'application/json',
        'Ocp-Apim-Subscription-Key': process.env.OCP_APIM_SUBSCRIPTION_KEY
    };

    function query(question, counts) {
        return new Promise(
            (resolve, reject) => {
                let url = buildUrl() + '/generateAnswer';
                if (url && headers) {
                    const requestData = {
                        url: url,
                        headers: headers,
                        body: JSON.stringify({
                            question: question,
                            top: counts
                        })
                    };

                    request.post(requestData, (error, response, body) => {
                        if (error || response.statusCode != 200) {
                            reject(error);
                        } else {
                            resolve(body);
                        }
                    });
                } else {
                    reject('The request url or headers is not valid.');
                }
            }
        );
    }

    function getFaqUrl() {
        return new Promise(
            (resolve, reject) => {
                let url = buildUrl();
                if (url && headers) {
                    const requestData = {
                        url: url,
                        headers: headers
                    };

                    request.get(requestData, (error, response, body) => {
                        if (error || response.statusCode != 200) {
                            reject(error);
                        } else {
                            resolve(body);
                        }
                    });
                } else {
                    reject('The request url or headers is not valid.');
                }
            }
        );
    }

    function buildUrl() {
        const url = process.env.QNA_SERVICE_API_URL;
        return url + process.env.KNOWLEDGE_BASE;
    }

    return {
        query: query,
        getFaqUrl: getFaqUrl
    };
}();
