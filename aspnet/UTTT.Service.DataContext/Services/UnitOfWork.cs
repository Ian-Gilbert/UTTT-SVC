using UTTT.Service.ObjectModel.Interfaces;
using UTTT.Service.ObjectModel.Models;

namespace UTTT.Service.DataContext.Serices
{
    public class UnitOfWork : IUnitOfWork
    {
        public IService<GameObject> Game { get; }

        public UnitOfWork(IUTTTDatabaseSettings settings)
        {
            Game = new GameService(settings);
        }
    }
}