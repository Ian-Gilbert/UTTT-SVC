namespace UtttApi.DataContext.Settings
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string MongoUri { get; set; }
        public string UtttCollection { get; set; }
        public string UtttDb { get; set; }
    }
}