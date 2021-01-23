using System.Collections.Generic;
using System.Threading.Tasks;
using UtttApi.ObjectModel.Interfaces;

namespace UtttApi.DataService.Interfaces
{
    public interface IDataService<TEntity> where TEntity : IEntity
    {
        void CheckParseId(string id);
        Task DeleteAsync(string id);
        Task<TEntity> InsertAsync(TEntity document);
        Task<TEntity> SelectAsync(string id);
        Task<IEnumerable<TEntity>> SelectAsync();
        Task UpdateAsync(TEntity document);
    }
}