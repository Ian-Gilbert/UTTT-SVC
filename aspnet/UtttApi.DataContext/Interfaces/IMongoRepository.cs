using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtttApi.ObjectModel.Abstracts;

namespace UtttApi.DataContext.Interfaces
{
    public interface IMongoRepository<TEntity> where TEntity : AEntity
    {
        void CheckParseId(string id);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<TEntity> CreateAsync(TEntity document);
        Task<TEntity> FindAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity document);
    }
}