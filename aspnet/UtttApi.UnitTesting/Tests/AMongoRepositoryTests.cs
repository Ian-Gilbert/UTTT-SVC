using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using UtttApi.DataContext.Repositories;
using UtttApi.ObjectModel.Abstracts;
using UtttApi.ObjectModel.Exceptions;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class AMongoRepositoryTests
    {
        public class Entity : AEntity { }

        private Mock<IAsyncCursor<Entity>> entityCursor;
        private readonly Mock<IMongoCollection<Entity>> mockCollection;
        private readonly Mock<AMongoRepository<Entity>> repo;

        // list of invalid 24 digit hex strings
        public static IEnumerable<object[]> InvalidIds =>
            new List<object[]>()
            {
                new object[] { null },
                new object[] { "" },
                new object[] { "1234567890" }, // too short
                new object[] { "123456789012345678901234567890" }, // too long
                new object[] { "12345678901234567890123g" } // non-hex character
            };

        public AMongoRepositoryTests()
        {
            mockCollection = new Mock<IMongoCollection<Entity>>();

            entityCursor = new Mock<IAsyncCursor<Entity>>();
            entityCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            entityCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            repo = new Mock<AMongoRepository<Entity>>(mockCollection.Object) { CallBase = true };
        }

        [Theory]
        [MemberData(nameof(InvalidIds))]
        public void CheckParseId_ThrowsException_WhenIdIsNotValid(string id)
        {
            var result = Assert.Throws<HttpResponseException>(() => repo.Object.CheckParseId(id));
            Assert.Equal(400, (int)result.StatusCode);
        }

        [Theory]
        [MemberData(nameof(InvalidIds))]
        public async void FindAsync_ThrowsException_WhenIdIsNotValid(string id)
        {
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => repo.Object.FindAsync(id));
            Assert.Equal(400, (int)result.StatusCode);
        }

        [Fact]
        public async void FindAsync_ThrowsException_WhenDocumentDoesNotExist()
        {
            entityCursor.Setup(_ => _.Current).Returns(new List<Entity>());

            mockCollection.Setup(c => c.FindAsync<Entity>(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<FindOptions<Entity, Entity>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(entityCursor.Object);

            var id = "123456789012345678901234";
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => repo.Object.FindAsync(id));
            Assert.Equal(404, (int)result.StatusCode);

            mockCollection.Verify(c => c.FindAsync(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<FindOptions<Entity>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void FindAsync_ReturnsSingleDocument_WhenGivenValidId()
        {
            var entity = new Entity() { Id = "1234567890ab1234567890ab" };

            entityCursor.Setup(_ => _.Current).Returns(new List<Entity>() { entity });

            mockCollection.Setup(c => c.FindAsync<Entity>(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<FindOptions<Entity, Entity>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(entityCursor.Object);

            var result = await repo.Object.FindAsync(entity.Id);
            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);

            mockCollection.Verify(c => c.FindAsync(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<FindOptions<Entity>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void FindAsync_ReturnsListOfDocuments_WhenGivenNoParameters()
        {
            var entity1 = new Entity() { Id = "1234567890ab1234567890ab" };
            var entity2 = new Entity() { Id = "123456789012345678901234" };

            entityCursor.Setup(_ => _.Current).Returns(new List<Entity>() { entity1, entity2 });

            mockCollection.Setup(c => c.FindAsync<Entity>(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<FindOptions<Entity, Entity>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(entityCursor.Object);

            var result = await repo.Object.FindAllAsync();
            Assert.NotEmpty(result);

            mockCollection.Verify(c => c.FindAsync(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<FindOptions<Entity>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void CreateAsync_ReturnsDocument()
        {
            var entity = new Entity() { Id = "1234567890ab1234567890ab" };

            mockCollection.Setup(c => c.InsertOneAsync(
                It.IsAny<Entity>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()
            ));

            var result = await repo.Object.CreateAsync(entity);
            Assert.Equal(entity.Id, result.Id);

            mockCollection.Verify(c => c.InsertOneAsync(
                It.IsAny<Entity>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void UpdateAsync_CallsReplaceOneAsync()
        {
            mockCollection.Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<Entity>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()
            ));

            var entity = new Entity() { Id = "1234567890ab1234567890ab" };
            await repo.Object.UpdateAsync(entity);

            mockCollection.Verify(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<Entity>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Theory]
        [MemberData(nameof(InvalidIds))]
        public async void DeleteAsync_ThrowsException_WhenIdIsNotValid(string id)
        {
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => repo.Object.DeleteAsync(id));
            Assert.Equal(400, (int)result.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_ThrowsException_WhenDocumentDoesNotExist()
        {
            var mockDeletedResult = new Mock<DeleteResult>();
            mockDeletedResult.SetupGet(d => d.DeletedCount).Returns(0L);

            mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(mockDeletedResult.Object);

            var id = "1234567890ab1234567890ab";
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => repo.Object.DeleteAsync(id));
            Assert.Equal(404, (int)result.StatusCode);


            mockCollection.Verify(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_CallsDeleteOneAsync()
        {
            var mockDeletedResult = new Mock<DeleteResult>();
            mockDeletedResult.SetupGet(d => d.DeletedCount).Returns(1L);

            mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(mockDeletedResult.Object);

            var id = "1234567890ab1234567890ab";
            await repo.Object.DeleteAsync(id);

            mockCollection.Verify(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Entity>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}