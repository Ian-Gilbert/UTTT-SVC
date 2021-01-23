using UtttApi.ObjectModel.Models;

namespace UtttApi.DataService.Interfaces
{
    public interface IUnitOfWork
    {
        IDataService<UtttObject> Game { get; }
    }
}