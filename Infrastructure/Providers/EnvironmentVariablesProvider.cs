using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Providers
{
    public static class EnvironmentVariablesProvider
    {
        private const string MONGO_DB_CONNECTION_STRING = "MongoDb__ConnectionString";

        public static string MongoDbConnectionString =>
            Environment.GetEnvironmentVariable(MONGO_DB_CONNECTION_STRING)
            ?? throw new InvalidOperationException($"Environment variable '{MONGO_DB_CONNECTION_STRING}' is not set.");
    }
}
