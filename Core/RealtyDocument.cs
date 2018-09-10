using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Honeymustard
{
    public class RealtyDocument
    {
        public ObjectId Id { get; set; }

        [BsonElement("RealtyId")]
        public string RealtyId { get; set; }

        [BsonElement("ImageUri")]
        public string ImageUri { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("SquareMeters")]
        public int SquareMeters { get; set; }

        [BsonElement("Price")]
        public int Price { get; set; }

        [BsonElement("SharedDept")]
        public int SharedDept { get; set; }

        [BsonElement("SharedExpenses")]
        public int SharedExpenses { get; set; }

        [BsonElement("Added")]
        public DateTime Added { get; set; }
    }
}