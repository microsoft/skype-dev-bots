'use strict'

module.exports = class Note {
  constructor(_id, userId, content, timestamp) {
    this._id = _id;
    this.UserId = userId;
    this.Content = content;
    this.Timestamp = timestamp;
  }
}
