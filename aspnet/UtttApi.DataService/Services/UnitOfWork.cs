using UtttApi.ObjectModel.Interfaces;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataService.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IService<GameObject> Game { get; }

        public UnitOfWork(IUtttDatabaseSettings settings)
        {
            Game = new GameService(settings);
        }
    }
}