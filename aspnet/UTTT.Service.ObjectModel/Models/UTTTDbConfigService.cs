using UTTT.Service.ObjectModel.Interfaces;

namespace UTTT.Service.ObjectModel.Models
{
    public class UTTTDatabaseSettings : IUTTTDatabaseSettings
    {
        public string GamesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}