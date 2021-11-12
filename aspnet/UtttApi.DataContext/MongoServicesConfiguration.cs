using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UtttApi.DataContext.Interfaces;
using UtttApi.DataContext.Services;
using UtttApi.DataContext.Settings;


namespace UtttApi.DataContext
{
    public static class MongoServicesConfiguration
    {
        public static void RegisterMongoDbServices(this IServiceCollection services)
        {
            // Create MongoDbSettings singleton, add values from configuration
            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value
            );

            // Create MongoClient Singleton
            services.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var settings = s.GetRequiredService<IMongoDbSettings>();
                return new MongoClient(settings.MongoUri);
            });

            // Create UnitOfWork singleton, which takes in MongoDbSettings and MongoClient
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
        }
    }
}