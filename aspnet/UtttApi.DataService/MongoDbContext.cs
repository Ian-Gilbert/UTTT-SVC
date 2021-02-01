using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using UtttApi.DataService.Settings;
using UtttApi.ObjectModel.Interfaces;

namespace UtttApi.DataService
{
    /// <summary>
    /// Context that sets up a MongoDb connection with the provided settings
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;
        private static readonly object _lock = new object();

        /// <summary>
        /// Creates MongoDb connection with provided settings
        /// </summary>
        /// <param name="settings"></param>
        public MongoDbContext(IMongoDbSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            _db = mongoClient.GetDatabase(settings.DatabaseName);
        }

        /// <summary>
        /// Sets up class map for TEntity and returns the collection with the given name.
        /// TEntity must implement IEntity.
        /// </summary>
        /// <param name="collectionName"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName) where TEntity : IEntity
        {
            if (!string.IsNullOrEmpty(collectionName))
            {
                // IsClassMapRegistered is not thread safe
                lock (_lock)
                {
                    // check if a class map for TEntity has already been registered
                    if (!BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
                    {
                        // Configure mapping from TEntity to BSON
                        // In particular, enure that the Id property is properly mapped to the _id BSON field
                        BsonClassMap.RegisterClassMap<TEntity>(cm =>
                        {
                            cm.AutoMap();
                            cm.MapIdMember(c => c.Id)
                              .SetSerializer(new StringSerializer(BsonType.ObjectId))
                              .SetIdGenerator(StringObjectIdGenerator.Instance);
                        });
                    }
                }

                return _db.GetCollection<TEntity>(collectionName);
            }

            return null;
        }
    }
}