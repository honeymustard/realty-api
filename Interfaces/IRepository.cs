using System.Collections.Generic;
using MongoDB.Driver;

namespace Honeymustard
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        void InsertOne(T document);
        IEnumerable<T> InsertMany(IEnumerable<T> documents);
        IEnumerable<T> FindAny(FilterDefinition<T> filter);
    }
}