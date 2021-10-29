using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UtttApi.ObjectModel.Exceptions;
using System.Threading;
using UtttApi.ObjectModel.Abstracts;
using UtttApi.DataService.Interfaces;

namespace UtttApi.DataService.Repositories
{
    /// <summary>
    /// A generic service to provide CRUD methods for storing data in a mongo database.
    /// TEntity must implement IEntity.
    /// All methods are virtual.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : AEntity
    {
        protected readonly IMongoCollection<TEntity> _collection;

        /// <summary>
        /// Provide the db collection
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="CollectionName"></param>
        public MongoRepository(IMongoCollection<TEntity> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Check that an id is a valid 24 digid hex string
        /// </summary>
        /// <param name="id"></param>
        public virtual void CheckParseId(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, $"'{id}' is not a valid 24 digit hex string");
            }
        }

        /// <summary>
        /// Delete a document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            CheckParseId(id);
            var deletedResult = await _collection.DeleteOneAsync(d => d.Id == id, cancellationToken);

            if (deletedResult.DeletedCount == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, $"No results found @ id = {id}");
            }
        }

        /// <summary>
        /// Insert a document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> CreateAsync(TEntity document)
        {
            await _collection.InsertOneAsync(document);
            return document;
        }

        /// <summary>
        /// Find a document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindAsync(string id, CancellationToken cancellationToken = default)
        {
            CheckParseId(id);
            var entity = await _collection.Find<TEntity>(d => d.Id == id).FirstOrDefaultAsync(cancellationToken);

            if (entity is null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, $"No results found @ id = {id}");
            }

            return entity;
        }

        /// <summary>
        /// Find all documents in collection
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default) =>
            await _collection.Find<TEntity>(d => true).ToListAsync(cancellationToken);

        /// <summary>
        /// Update a document by replacing the whole document in db
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(TEntity document) =>
            await _collection.ReplaceOneAsync(d => d.Id == document.Id, document);
    }
}