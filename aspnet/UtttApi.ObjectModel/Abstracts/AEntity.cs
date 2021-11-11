using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UtttApi.ObjectModel.Abstracts
{
    public abstract class AEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}