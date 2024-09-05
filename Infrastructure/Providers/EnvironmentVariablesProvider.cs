using Infrastructure.Providers.Interfaces;

namespace Infrastructure.Providers
{
    public class EnvironmentVariablesProvider : IEnvironmentVariablesProvider
    {
        public string MongoDb__ConnectionString { get; set; }
        public string Mongo_Db_Name { get; set; }
        public string Mongo_Schedule_Collection { get; set; }

        public EnvironmentVariablesProvider(string mongoDb__ConnectionString, string mongo_Db_Name, string mongo_Schedule_Collection)
        {
            MongoDb__ConnectionString = GetRequiredStringVariable(VariableKeys.MONGO_DB_CONNECTION_STRING);
            Mongo_Db_Name = GetRequiredStringVariable(VariableKeys.MONGO_DB_NAME);
            Mongo_Schedule_Collection = GetRequiredStringVariable(VariableKeys.MONGO_SCHEDULE_COLLECTION_NAME);
        }

        public static string GetRequiredStringVariable(string environmentVariable)
        {
            return Environment.GetEnvironmentVariable(environmentVariable)
                ?? throw new InvalidOperationException($"Environment variable '{environmentVariable}' is not set.");
        }

    }
}
