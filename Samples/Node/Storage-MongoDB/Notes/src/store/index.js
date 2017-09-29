const strings = require('../Strings');
const dbHelper = require('./DbHelper');
const noteUtil = require('../util/NoteUtil');
const databaseSingleton = require('./Database');
const Note = require('../models/Note');

module.exports = function() {
  function saveNote(userId, content, callback) {
    if (!userId) {
      return;
    }

    const ticks = noteUtil.dateToTicks(Date.now());
    const note = new Note(userId + "_" + ticks, userId, content, ticks);
    const db = databaseSingleton.getDB();
    const collection_name = process.env.COLLECTION_NAME;

    dbHelper.addDocument(db, collection_name, note)
      .then(result => {
        callback(null, result.ops[0]);
      })
      .catch((error) => {
        callback(error);
      });
  }

  function getNotes(userId, searchText, callback) {
    if (!userId) {
      return;
    }

    const db = databaseSingleton.getDB();
    const filter = {
      UserId: userId
    };
    
    if (searchText && searchText.length > 0) {
      filter['Content'] = { $regex: searchText };
    }

    dbHelper.queryForDocuments(db, collection_name, filter)
      .then(docs => {
        callback(docs)
      })
      .catch(error => {
      });
  }

  function deleteAllNotes(userId, callback) {
    if (!userId) {
      return;
    }

    const db = databaseSingleton.getDB();
    const filter = {
      UserId: userId
    };

    dbHelper.removeAllDocuments(db, collection_name, filter)
      .then(results => {
        callback(results.deletedCount);
      })
      .catch(error => {
      });
  }


  return {
    saveNote: saveNote,
    getNotes: getNotes,
    deleteAllNotes: deleteAllNotes
  }
}();
