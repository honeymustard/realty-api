using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Honeymustard
{
    public class Database : IDatabase<IMongoDatabase>
    {
        protected MongoClient Client;
        protected IMongoDatabase Db;

        public Database(ICredentials credentials)
        {
            Client = new MongoClient(credentials.Connection);
            Db = Client.GetDatabase(credentials.Database);
        }

        public IMongoDatabase GetDatabase()
        {
            return Db;
        }
    }
}