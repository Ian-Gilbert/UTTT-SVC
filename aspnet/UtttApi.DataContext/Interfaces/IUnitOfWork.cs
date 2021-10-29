using UtttApi.DataContext.Repositories;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataContext.Interfaces
{
    public interface IUnitOfWork
    {
        IMongoRepository<UtttObject> Game { get; }
    }
}