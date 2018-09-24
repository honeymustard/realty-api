using MongoDB.Driver;

namespace Honeymustard
{
    public interface IDatabase<T> {

        /// <summary>
        /// Gets the database object.
        /// </summary>
        T GetDatabase();
    }
}