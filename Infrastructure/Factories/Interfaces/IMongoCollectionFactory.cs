using MongoDB.Driver;

namespace Infrastructure.Factories.Interfaces
{
    public interface IMongoCollectionFactory<T>
    {
        IMongoCollection<T> GetCollection();
    }
}
