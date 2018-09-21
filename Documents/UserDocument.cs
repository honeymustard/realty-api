using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Honeymustard
{
    public class UserDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        [BsonElement("username")]
        public string Username { get; set; }

        [BsonRequired]
        [BsonElement("password")]
        public string Password { get; set; }
    }
}