using UtttApi.ObjectModel.Interfaces;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataService.Services
{
    /// <inheritdoc cref="IUnitOfWork"/>
    public class UnitOfWork : IUnitOfWork
    {
        public IDataService<UtttObject> Game { get; }

        public UnitOfWork(IUtttDatabaseSettings settings)
        {
            Game = new GameDataService(settings);
        }
    }
}