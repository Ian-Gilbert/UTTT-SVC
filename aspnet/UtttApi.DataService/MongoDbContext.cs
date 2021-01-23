using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using UtttApi.DataService.Settings;
using UtttApi.ObjectModel.Interfaces;

namespace UtttApi.DataService
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IMongoDbSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            _db = mongoClient.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName) where TEntity : IEntity
        {
            BsonClassMap.RegisterClassMap<TEntity>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId))
                  .SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            return _db.GetCollection<TEntity>(collectionName);
        }
    }
}