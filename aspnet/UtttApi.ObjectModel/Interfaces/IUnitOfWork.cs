using UtttApi.ObjectModel.Models;

namespace UtttApi.ObjectModel.Interfaces
{
    /// <summary>
    /// Contains all services and methods required to store data
    /// </summary>
    public interface IUnitOfWork
    {
        IDataService<UtttObject> Game { get; }
    }
}