namespace UtttApi.DataContext.Settings
{
    public interface IMongoDbSettings
    {
        string MongoUri { get; set; }
        string UtttCollection { get; set; }
        string UtttDb { get; set; }
    }
}