using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Honeymustard
{
    public class UserRepository : IUserRepository
    {
        protected IMongoDatabase Db { get; set; }
        protected IMongoCollection<UserDocument> Collection { get; set; }

        public UserRepository(IDatabase<IMongoDatabase> database)
        {
            Db = database.GetDatabase();
            Collection = Db.GetCollection<UserDocument>("users");
        }

        public static FilterDefinition<UserDocument> FilterMatch(UserDocument user)
        {
            var builder = Builders<UserDocument>.Filter;
            return builder.And(
                builder.Eq("username", user.Username),
                builder.Eq("password", user.Password)
            );
        }

        public IEnumerable<UserDocument> GetAll()
        {
            return Collection.Find(new BsonDocument()).ToEnumerable();
        }

        public void InsertOne(UserDocument document)
        {
            Collection.InsertOne(document);
        }

        public IEnumerable<UserDocument> InsertMany(IEnumerable<UserDocument> documents)
        {
            Collection.InsertMany(documents);
            return documents;
        }

        public IEnumerable<UserDocument> FindAny(FilterDefinition<UserDocument> filter)
        {
            return Collection.Find(filter).ToEnumerable();
        }
    }
}
