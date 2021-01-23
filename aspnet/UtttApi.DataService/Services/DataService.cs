using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using UtttApi.DataService.Interfaces;
using UtttApi.DataService.Settings;
using UtttApi.ObjectModel.Interfaces;
using UtttApi.ObjectModel.Exceptions;

namespace UtttApi.DataService.Services
{
    /// <summary>
    /// A generic service to provide CRUD methods for storing data in a mongo database.
    /// TEntity must implement IEntity (ensures Id property to map to), and all methods are virtual.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DataService<TEntity> : IDataService<TEntity> where TEntity : IEntity
    {
        protected readonly IMongoCollection<TEntity> _collection;

        /// <summary>
        /// Provide the db connection settings and the name of the collection.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="CollectionName"></param>
        public DataService(IUtttDatabaseSettings settings, string collectionName)
        {
            // Define BSON representation for TEntity
            // Removes mogodb.driver dependency in ObjectModel lib
            BsonClassMap.RegisterClassMap<TEntity>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId))
                  .SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        /// <summary>
        /// Check that an id is a valid 24 digid hex string
        /// </summary>
        /// <param name="id"></param>
        public void CheckParseId(string id)
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
        public virtual async Task DeleteAsync(string id)
        {
            CheckParseId(id);
            var deletedResult = await _collection.DeleteOneAsync(d => d.Id == id);

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
        public virtual async Task<TEntity> InsertAsync(TEntity document)
        {
            await _collection.InsertOneAsync(document);
            return document;
        }

        /// <summary>
        /// Select a document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> SelectAsync(string id)
        {
            CheckParseId(id);
            var entity = await _collection.Find<TEntity>(d => d.Id == id).FirstOrDefaultAsync();

            if (entity is null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, $"No results found @ id = {id}");
            }

            return entity;
        }

        /// <summary>
        /// Select all documents in collection
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> SelectAsync() =>
            await _collection.Find<TEntity>(d => true).ToListAsync();

        /// <summary>
        /// Update a document by replacing the whole document in db
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(TEntity document) =>
            await _collection.ReplaceOneAsync(d => d.Id == document.Id, document);
    }
}