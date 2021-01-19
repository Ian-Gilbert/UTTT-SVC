using UtttApi.ObjectModel.Interfaces;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataService.Services
{
    /// <inheritdoc cref="IUnitOfWork"/>
    public class UnitOfWork : IUnitOfWork
    {
        public IDataService<GameObject> Game { get; }

        public UnitOfWork(IUtttDatabaseSettings settings)
        {
            Game = new GameDataService(settings);
        }
    }
}