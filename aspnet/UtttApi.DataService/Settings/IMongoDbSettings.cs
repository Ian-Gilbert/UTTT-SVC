namespace UtttApi.DataService.Settings
{
    public interface IMongoDbSettings
    {
        string GamesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}