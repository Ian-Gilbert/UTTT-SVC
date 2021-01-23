using System.Collections.Generic;
using System.Threading.Tasks;
using UtttApi.ObjectModel.Interfaces;

namespace UtttApi.DataService.Interfaces
{
    public interface IDataService<TEntity> where TEntity : IEntity
    {
        void CheckParseId(string id);
        Task DeleteAsync(string id);
        Task<TEntity> CreateAsync(TEntity document);
        Task<TEntity> FindAsync(string id);
        Task<IEnumerable<TEntity>> FindAsync();
        Task UpdateAsync(TEntity document);
    }
}