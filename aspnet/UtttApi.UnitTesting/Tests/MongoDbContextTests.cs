using UtttApi.DataService;
using UtttApi.DataService.Settings;
using UtttApi.ObjectModel.Models;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class MongoDbContextTests
    {
        private readonly MongoDbContext context;
        public MongoDbContextTests()
        {
            var settings = new MongoDbSettings()
            {
                ConnectionString = "mongodb://test123",
                DatabaseName = "Test"
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
            var collectionName = "testCollection";
            var collection = context.GetCollection<UtttObject>(collectionName);
            Assert.NotNull(collection);
            Assert.Equal(collectionName, collection.CollectionNamespace.CollectionName);
        }
    }
}