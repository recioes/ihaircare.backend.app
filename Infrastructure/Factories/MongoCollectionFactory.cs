using Infrastructure.Factories.Interfaces;
using Infrastructure.Providers.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Factories
{
    public class MongoCollectionFactory<T> : IMongoCollectionFactory<T>
    {
        private readonly IMongoClient _mongoClient;
        private readonly IEnvironmentVariablesProvider _envProvider;

        public MongoCollectionFactory(IMongoClient mongoClient, IEnvironmentVariablesProvider envProvider)
        {
            _mongoClient = mongoClient;
            _envProvider = envProvider;
        }

        public IMongoCollection<T> GetCollection()
        {
            var database = _mongoClient.GetDatabase(_envProvider.Mongo_Db_Name);

            return database.GetCollection<T>(typeof(T).Name.ToLower());
        }
    }
}
