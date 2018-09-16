using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Honeymustard
{
    public class RealtyRepository : IRepository<RealtyDocument>
    {
        protected IMongoDatabase Db { get; set; }

        public static FilterDefinition<RealtyDocument> FilterToday {
            get => Builders<RealtyDocument>.Filter.Gte("Added", DateTime.Now.Date);
        }

        public RealtyRepository(IDatabase database)
        {
            Db = database.GetDatabase();
        }

        public IEnumerable<RealtyDocument> GetAll()
        {
            return Db.GetCollection<RealtyDocument>("realties").Find(new BsonDocument()).ToEnumerable();
        }

        public void InsertOne(RealtyDocument document)
        {
            Db.GetCollection<RealtyDocument>("realties").InsertOne(document);
        }

        public IEnumerable<RealtyDocument> InsertMany(IEnumerable<RealtyDocument> documents)
        {
            Db.GetCollection<RealtyDocument>("realties").InsertMany(documents);
            return documents;
        }

        public IEnumerable<RealtyDocument> FindAny(FilterDefinition<RealtyDocument> filter)
        {
            return Db.GetCollection<RealtyDocument>("realties").Find(filter).ToEnumerable();
        }
    }
}