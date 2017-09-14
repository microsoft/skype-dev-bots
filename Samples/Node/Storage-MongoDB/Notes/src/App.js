require('dotenv').config({path: __dirname + '/../.env'});
const restify = require('restify');
const noteBot = require('./bot/NoteBot.js');
const database = require('./store/Database');
const dbHelper = require('./store/DbHelper');

// Setup Restify Server
const server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
   console.log('%s listening to %s', server.name, server.url);
   dbHelper.connect(process.env.MONGO_DB_STRING)
    .then(db => database.setDB(db))
    .catch(error => console.log(error));
});
// Listen for messages from users
server.post('/api/messages', noteBot.connector('*').listen());
