using MongoDB.Driver;

namespace Honeymustard
{
    public interface IDatabase {

        /// <summary>
        /// Gets the database object.
        /// </summary>
        IMongoDatabase GetDatabase();
    }
}