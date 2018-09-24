using System.Collections.Generic;
using MongoDB.Driver;

namespace Honeymustard
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets all the documents in the repository.
        /// </summary>
        /// <returns>Returns the full list of documents.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Inserts a single document.
        /// </summary>
        /// <param name="document">The document to insert</param>
        void InsertOne(T document);

        /// <summary>
        /// Inserts multiple documents.
        /// </summary>
        /// <returns>Returns a list of the inserted documents.</returns>
        IEnumerable<T> InsertMany(IEnumerable<T> documents);

        /// <summary>
        /// Finds any number of documents matching a given filter.
        /// </summary>
        /// <returns>Returns a list of the matching documents.</returns>
        IEnumerable<T> FindAny(FilterDefinition<T> filter);
    }
}