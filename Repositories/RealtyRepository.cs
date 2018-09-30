using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Honeymustard
{
    public class RealtyRepository : IRealtyRepository
    {
        protected IMongoDatabase Db { get; set; }
        protected IMongoCollection<RealtyDocument> Collection { get; set; }

        public static FilterDefinition<RealtyDocument> FilterToday {
            get => Builders<RealtyDocument>.Filter.Gte("Added", DateTime.Now.Date);
        }

        public RealtyRepository(IDatabase<IMongoDatabase> database)
        {
            Db = database.GetDatabase();
            Collection = Db.GetCollection<RealtyDocument>("realties");
        }

        public IEnumerable<RealtyDocument> GetAll()
        {
            return Collection.Find(new BsonDocument()).ToEnumerable();
        }

        public void InsertOne(RealtyDocument document)
        {
            Collection.InsertOne(document);
        }

        public IEnumerable<RealtyDocument> InsertMany(IEnumerable<RealtyDocument> documents)
        {
            Collection.InsertMany(documents);
            return documents;
        }

        public IEnumerable<RealtyDocument> FindAny(FilterDefinition<RealtyDocument> filter)
        {
            return Collection.Find(filter).ToEnumerable();
        }
    }
}