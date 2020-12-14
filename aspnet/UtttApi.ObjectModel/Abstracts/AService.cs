using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using UtttApi.ObjectModel.Interfaces;

namespace UtttApi.ObjectModel.Abstracts
{
    public abstract class AService<TEntity> : IService<TEntity> where TEntity : AEntity
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public AService(IUtttDatabaseSettings settings, string CollectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TEntity>(CollectionName);
        }

        public virtual async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(d => d.Id == id);

        public virtual async Task<TEntity> InsertAsync(TEntity document)
        {
            await _collection.InsertOneAsync(document);
            return document;
        }

        public virtual async Task<TEntity> SelectAsync(string id) =>
            await _collection.Find<TEntity>(d => d.Id == id).FirstOrDefaultAsync();

        public virtual async Task<IEnumerable<TEntity>> SelectAsync() =>
            await _collection.Find<TEntity>(d => true).ToListAsync();

        public virtual async Task UpdateAsync(TEntity document)
        {
            await _collection.ReplaceOneAsync(d => d.Id == document.Id, document);
        }
    }
}