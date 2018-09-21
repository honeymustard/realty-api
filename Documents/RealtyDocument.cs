using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Honeymustard
{
    public class RealtyDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("realtyId")]
        public string RealtyId { get; set; }

        [BsonElement("imageUri")]
        public string ImageUri { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("squareMeters")]
        public int SquareMeters { get; set; }

        [BsonElement("price")]
        public int Price { get; set; }

        [BsonElement("sharedDept")]
        public int SharedDept { get; set; }

        [BsonElement("sharedExpenses")]
        public int SharedExpenses { get; set; }

        [BsonElement("added")]
        public DateTime Added { get; set; }
    }
}