const MongoClient = require('mongodb').MongoClient;
const strings = require('.././Strings');

module.exports = function(database) {

  function connect(uri) {
    return new Promise(function(resolve, reject) {
      MongoClient.connect(uri)
        .then(database => {
            resolve(database.db(strings.NOTES_DB));
          })
        .catch(err => {
          reject(err.message);
        });
    });
  }

  function close(db) {
    return;
    if (db) {
      db.close()
        .then()
        .catch(error => {
          reject(err.message);
        });
    }
  }

  function removeAllDocuments(db, collection, filter) {
    return new Promise(function(resolve, reject) {
      db.collection(collection, filter, (error, collection) => {
        if (error) {
          reject(error.message);
        } else {
          collection.deleteMany(filter, (error, result) => {
            if (error) {
              reject(error.message);
            } else {
              resolve(result);
            }
          });
        }
      })
    });
  }

  function addDocument(db, collection, doc) {
    return new Promise(function(resolve, reject) {
      db.db("notes_db").collection(collection, (error, collection) => {
        if (error) {
          reject(error.message);
        } else {
          collection.insert(doc, {
              w: "majority"
            })
            .then(result => {
              resolve(result)
            })
            .catch(error => {
              reject(error.message);
            });
        }
      })
    });
  }

  function queryForDocuments(db, collection, filter) {
    return new Promise((resolve, reject) => {
      db.collection(collection, (error, collection) => {
        if (error) {
          reject(error.message);
        } else {
          collection.find(filter).toArray((error, docs) => {
            if (error) {
              reject(error.message);
            } else {
              resolve(docs);
            }
          });
        }
      });
    });
  }

  return {
    connect: connect,
    close: close,
    removeAllDocuments: removeAllDocuments,
    addDocument: addDocument,
    queryForDocuments: queryForDocuments
  }
}();
