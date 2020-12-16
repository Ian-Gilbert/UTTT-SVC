using UtttApi.ObjectModel.Abstracts;
using UtttApi.ObjectModel.Interfaces;
using UtttApi.ObjectModel.Models;

namespace UtttApi.DataService.Services
{
    /// <inheritdoc cref="AService"/>
    public class GameService : AService<GameObject>
    {
        public GameService(IUtttDatabaseSettings settings) : base(settings, settings.GamesCollectionName) { }
    }
}