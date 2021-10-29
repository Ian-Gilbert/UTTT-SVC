using UtttApi.DataService.Repositories;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataService.Interfaces
{
    public interface IUnitOfWork
    {
        IMongoRepository<UtttObject> Game { get; }
    }
}