using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Notes.Models
{
    [Serializable]
    public sealed class Note
    {
        [BsonId]
        public string NoteId { get; set; }

        [BsonElement]
        public string UserId { get; set; }

        [BsonElement]
        public string Content { get; set; }

        [BsonElement]
        public long Timestamp { get; set; }
    }
}