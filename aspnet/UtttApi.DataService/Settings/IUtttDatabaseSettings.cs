namespace UtttApi.DataService.Settings
{
    public interface IUtttDatabaseSettings
    {
        string GamesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}