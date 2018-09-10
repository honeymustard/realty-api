using MongoDB.Driver;

namespace Honeymustard
{
    public interface IDatabase {
        IMongoDatabase GetDatabase();
    }
}