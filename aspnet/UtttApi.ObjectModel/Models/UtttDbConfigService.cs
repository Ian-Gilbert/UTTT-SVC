using UtttApi.ObjectModel.Interfaces;

namespace UtttApi.ObjectModel.Models
{
    public class UtttDatabaseSettings : IUtttDatabaseSettings
    {
        public string GamesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}