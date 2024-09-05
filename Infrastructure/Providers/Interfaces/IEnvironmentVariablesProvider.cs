namespace Infrastructure.Providers.Interfaces
{
    public interface IEnvironmentVariablesProvider
    {
         string MongoDb__ConnectionString { get; }
         string Mongo_Db_Name { get; }
         string Mongo_Schedule_Collection { get; }
    }
}
