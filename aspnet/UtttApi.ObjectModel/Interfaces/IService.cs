using System.Collections.Generic;
using System.Threading.Tasks;

namespace UtttApi.ObjectModel.Interfaces
{
    public interface IService<TEntity> where TEntity : class
    {
        Task<bool> DeleteAsync(string id);
        Task<TEntity> InsertAsync(TEntity document);
        Task<TEntity> SelectAsync(string id);
        Task<IEnumerable<TEntity>> SelectAsync();
        Task UpdateAsync(TEntity document);
    }
}