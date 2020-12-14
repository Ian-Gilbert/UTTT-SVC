using System.Collections.Generic;
using System.Threading.Tasks;

namespace UTTT.Service.ObjectModel.Interfaces
{
    public interface IService<TEntity> where TEntity : class
    {
        Task DeleteAsync(string id);
        Task InsertAsync(TEntity document);
        Task<TEntity> SelectAsync(string id);
        Task<IEnumerable<TEntity>> SelectAsync();
        void Update(TEntity document);
    }
}