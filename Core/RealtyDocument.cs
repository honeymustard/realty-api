using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Honeymustard
{
    public class RealtyDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement]
        public string RealtyId { get; set; }

        [BsonElement]
        public string ImageUri { get; set; }

        [BsonElement]
        public string Address { get; set; }

        [BsonElement]
        public string Description { get; set; }

        [BsonElement]
        public int SquareMeters { get; set; }

        [BsonElement]
        public int Price { get; set; }

        [BsonElement]
        public int SharedDept { get; set; }

        [BsonElement]
        public int SharedExpenses { get; set; }

        [BsonElement]
        public DateTime Added { get; set; }
    }
}