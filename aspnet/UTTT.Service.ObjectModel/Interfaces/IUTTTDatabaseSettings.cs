namespace UTTT.Service.ObjectModel.Interfaces
{
    public interface IUTTTDatabaseSettings
    {
        string GamesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}