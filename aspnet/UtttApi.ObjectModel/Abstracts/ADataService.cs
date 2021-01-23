using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UtttApi.ObjectModel.Exceptions;
using UtttApi.ObjectModel.Interfaces;

namespace UtttApi.ObjectModel.Abstracts
{
    /// <inheritdoc cref="IService"/>
    public abstract class ADataService<TEntity> : IDataService<TEntity> where TEntity : AEntity
    {
        protected readonly IMongoCollection<TEntity> _collection;

        /// <summary>
        /// Provide the db connection settings and the name of the collection.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="CollectionName"></param>
        public ADataService(IUtttDatabaseSettings settings, string CollectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TEntity>(CollectionName);
        }

        protected void CheckParseID(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, $"'{id}' is not a valid 24 digit hex string");
            }
        }

        public virtual async Task DeleteAsync(string id)
        {
            CheckParseID(id);
            var deletedResult = await _collection.DeleteOneAsync(d => d.Id == id);

            if (deletedResult.DeletedCount == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, $"No results found @ id = {id}");
            }
        }

        public virtual async Task<TEntity> InsertAsync(TEntity document)
        {
            await _collection.InsertOneAsync(document);
            return document;
        }

        public virtual async Task<TEntity> SelectAsync(string id)
        {
            CheckParseID(id);
            var entity = await _collection.Find<TEntity>(d => d.Id == id).FirstOrDefaultAsync();

            if (entity is null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, $"No results found @ id = {id}");
            }

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> SelectAsync() =>
            await _collection.Find<TEntity>(d => true).ToListAsync();

        public virtual async Task UpdateAsync(TEntity document) =>
            await _collection.ReplaceOneAsync(d => d.Id == document.Id, document);
    }
}