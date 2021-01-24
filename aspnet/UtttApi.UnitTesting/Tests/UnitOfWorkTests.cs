using MongoDB.Driver;
using Moq;
using UtttApi.DataService.Interfaces;
using UtttApi.DataService.Services;
using UtttApi.DataService.Settings;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class UnitOfWorkTests
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMongoDbSettings settings;

        public UnitOfWorkTests()
        {
            settings = new MongoDbSettings()
            {
                ConnectionString = "mongodb://test123",
                DatabaseName = "Test",
                GamesCollectionName = "TestCollection"
            };

            var mockDb = new Mock<IMongoDatabase>();
            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(c => c.GetDatabase(
                settings.DatabaseName,
                It.IsAny<MongoDatabaseSettings>()
            )).Returns(mockDb.Object);

            unitOfWork = new UnitOfWork(settings);
        }

        [Fact]
        public void UnitOfWork_ShouldBeCreated()
        {
            Assert.NotNull(unitOfWork);
            Assert.NotNull(unitOfWork.Game);
        }
    }
}