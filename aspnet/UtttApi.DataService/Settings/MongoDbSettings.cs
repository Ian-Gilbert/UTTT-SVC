namespace UtttApi.DataService.Settings
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string GamesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}