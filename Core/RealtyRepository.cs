using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Honeymustard
{
    public class RealtyRepository
    {
        protected IMongoDatabase Db { get; set; }

        public RealtyRepository(IDatabase database)
        {
            Db = database.GetDatabase();
        }

        public IEnumerable<RealtyDocument> GetAll()
        {
            return Db.GetCollection<RealtyDocument>("realties").Find(new BsonDocument()).ToList();
        }

        public void InsertOne(RealtyDocument document)
        {
            Db.GetCollection<RealtyDocument>("realties").InsertOne(document);
        }

        public void InsertMany(IEnumerable<RealtyDocument> document)
        {
            Db.GetCollection<RealtyDocument>("realties").InsertMany(document);
        }
    }
}