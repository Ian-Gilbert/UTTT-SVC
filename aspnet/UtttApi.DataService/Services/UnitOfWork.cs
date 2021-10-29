using MongoDB.Driver;
using UtttApi.DataService.Interfaces;
using UtttApi.DataService.Repositories;
using UtttApi.DataService.Settings;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataService.Services
{
    /// <summary>
    /// Contains all services and methods required to store data
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