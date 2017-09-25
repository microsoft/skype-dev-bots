/**
* app.js
* 
* Copyright (c) Microsoft Corporation. All rights reserved.
* Licensed under the MIT license.
*
*/

require('dotenv').config();

const restify = require('restify');
const qnamaker = require('./src/bot/qnamaker.js');

// Setup Restify Server
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
    console.log('%s listening to %s', server.name, server.url);
});

// Listen for messages from users 
server.post('/api/messages', qnamaker.connector('*').listen());