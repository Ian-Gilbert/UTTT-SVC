using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UtttApi.ObjectModel.Interfaces;

namespace UtttApi.ObjectModel.Abstracts
{
    /// <inheritdoc cref="IService"/>
    public abstract class AService<TEntity> : IService<TEntity> where TEntity : AEntity
    {
        protected readonly IMongoCollection<TEntity> _collection;

        /// <summary>
        /// Provide the db connection settings and the name of the collection.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="CollectionName"></param>
        public AService(IUtttDatabaseSettings settings, string CollectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TEntity>(CollectionName);
        }

        public virtual async Task<bool> DeleteAsync(string id)
        {
            long count = 0;
            if (ObjectId.TryParse(id, out _))
            {
                DeleteResult deleteResult = await _collection.DeleteOneAsync(d => d.Id == id);
                count = deleteResult.DeletedCount;
            }
            return count == 1;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity document)
        {
            await _collection.InsertOneAsync(document);
            return document;
        }

        public virtual async Task<TEntity> SelectAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return null;
            }

            return await _collection.Find<TEntity>(d => d.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> SelectAsync() =>
            await _collection.Find<TEntity>(d => true).ToListAsync();

        public virtual async Task UpdateAsync(TEntity document) =>
            await _collection.ReplaceOneAsync(d => d.Id == document.Id, document);
    }
}