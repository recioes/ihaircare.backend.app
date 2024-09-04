using Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Api.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddMongoDb();

            return services;
        }

        public static IServiceCollection AddMongoDb(this IServiceCollection services) 
        {
            string mongoConnectionString = EnvironmentVariablesProvider.MongoDbConnectionString;

            services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                return new MongoClient(mongoConnectionString);
            });

            return services;
        }

    }
}
