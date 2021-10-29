using MongoDB.Driver;
using UtttApi.DataContext.Interfaces;
using UtttApi.DataContext.Repositories;
using UtttApi.DataContext.Settings;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataContext.Services
{
    /// <summary>
    /// Contains all repositories and services required to handle data
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public IMongoRepository<UtttObject> Game { get; }

        public UnitOfWork(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.MongoUri);
            var utttDb = client.GetDatabase(settings.UtttDb);

            Game = new MongoRepository<UtttObject>(utttDb.GetCollection<UtttObject>(settings.UtttCollection));
        }
    }
}