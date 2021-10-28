using UtttApi.DataService;
using UtttApi.DataService.Settings;
using UtttApi.ObjectModel.Models;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class MongoDbContextTests
    {
        private readonly MongoDbContext context;
        private IMongoDbSettings settings;

        public MongoDbContextTests()
        {
            settings = new MongoDbSettings()
            {
                MongoUri = "mongodb://test123",
                UtttCollection = "TestCollection",
                UtttDb = "Test",
            };
            context = new MongoDbContext(settings);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetCollection_ReturnsNull_WhenNameIsNullOrEmpty(string collectionName)
        {
            var collection = context.GetCollection<UtttObject>(collectionName);
            Assert.Null(collection);
        }

        [Fact]
        public void GetCollection_ReturnsCollection_WhenNameIsNotEmpty()
        {
            var collection = context.GetCollection<UtttObject>(settings.UtttCollection);
            Assert.NotNull(collection);
            Assert.Equal(settings.UtttCollection, collection.CollectionNamespace.CollectionName);
        }
    }
}