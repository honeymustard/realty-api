using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Honeymustard
{
    public class UserDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement]
        public string Username { get; set; }
    }
}