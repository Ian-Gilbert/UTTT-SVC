using UtttApi.DataService.Interfaces;
using UtttApi.DataService.Settings;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataService.Services
{
    /// <summary>
    /// Contains all services and methods required to store data
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public IDataService<UtttObject> Game { get; }

        public UnitOfWork(IUtttDatabaseSettings settings)
        {
            var context = new MongoDbContext(settings);
            Game = new DataService<UtttObject>(context.GetCollection<UtttObject>(settings.GamesCollectionName));
        }
    }
}