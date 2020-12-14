using UTTT.Service.ObjectModel.Models;

namespace UTTT.Service.ObjectModel.Interfaces
{
    public interface IUnitOfWork
    {
        IService<GameObject> Game { get; }
    }
}