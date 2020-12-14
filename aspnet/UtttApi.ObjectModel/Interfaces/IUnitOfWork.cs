using UtttApi.ObjectModel.Models;

namespace UtttApi.ObjectModel.Interfaces
{
    public interface IUnitOfWork
    {
        IService<GameObject> Game { get; }
    }
}