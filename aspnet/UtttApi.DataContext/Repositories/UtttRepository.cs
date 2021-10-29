using MongoDB.Driver;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataContext.Repositories
{
    public class UtttRepository : AMongoRepository<UtttObject>
    {
        public UtttRepository(IMongoCollection<UtttObject> utttCollection) : base(utttCollection) { }
    }
}