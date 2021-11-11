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
            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value
            );

            services.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var settings = s.GetRequiredService<IMongoDbSettings>();
                return new MongoClient(settings.MongoUri);
            });

            services.AddSingleton<IUnitOfWork, UnitOfWork>();
        }
    }
}