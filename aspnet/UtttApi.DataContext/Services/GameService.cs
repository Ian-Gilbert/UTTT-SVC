using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using UtttApi.ObjectModel.Interfaces;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataContext.Services
{
    public class GameService : IService<GameObject>
    {
        private readonly IMongoCollection<GameObject> _collection;

        public GameService(IUtttDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _collection = db.GetCollection<GameObject>(settings.GamesCollectionName);
        }

        public Task DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task InsertAsync(GameObject document)
        {
            throw new System.NotImplementedException();
        }

        public Task<GameObject> SelectAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<GameObject>> SelectAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Update(GameObject document)
        {
            throw new System.NotImplementedException();
        }
    }
}