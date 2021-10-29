using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using UtttApi.DataService.Interfaces;
using UtttApi.DataService.Services;
using UtttApi.ObjectModel.Exceptions;
using UtttApi.ObjectModel.Models;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class DataServiceTests
    {
        private Mock<IAsyncCursor<UtttObject>> utttObjectCursor;
        private readonly Mock<IMongoCollection<UtttObject>> mockCollection;
        private readonly IDataService<UtttObject> service;

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

        public DataServiceTests()
        {
            mockCollection = new Mock<IMongoCollection<UtttObject>>();

            utttObjectCursor = new Mock<IAsyncCursor<UtttObject>>();
            utttObjectCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            utttObjectCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            service = new DataService<UtttObject>(mockCollection.Object);
        }

        [Theory]
        [MemberData(nameof(InvalidIds))]
        public void CheckParseId_ThrowsException_WhenIdIsNotValid(string id)
        {
            var result = Assert.Throws<HttpResponseException>(() => service.CheckParseId(id));
            Assert.Equal(400, (int)result.StatusCode);
        }

        [Theory]
        [MemberData(nameof(InvalidIds))]
        public async void FindAsync_ThrowsException_WhenIdIsNotValid(string id)
        {
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => service.FindAsync(id));
            Assert.Equal(400, (int)result.StatusCode);
        }

        [Fact]
        public async void FindAsync_ThrowsException_WhenDocumentDoesNotExist()
        {
            utttObjectCursor.Setup(_ => _.Current).Returns(new List<UtttObject>());

            mockCollection.Setup(c => c.FindAsync<UtttObject>(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<FindOptions<UtttObject, UtttObject>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(utttObjectCursor.Object);

            var id = "123456789012345678901234";
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => service.FindAsync(id));
            Assert.Equal(404, (int)result.StatusCode);

            mockCollection.Verify(c => c.FindAsync(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<FindOptions<UtttObject>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void FindAsync_ReturnsSingleDocument_WhenGivenValidId()
        {
            var utttObject = new UtttObject() { Id = "1234567890ab1234567890ab" };

            utttObjectCursor.Setup(_ => _.Current).Returns(new List<UtttObject>() { utttObject });

            mockCollection.Setup(c => c.FindAsync<UtttObject>(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<FindOptions<UtttObject, UtttObject>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(utttObjectCursor.Object);

            var result = await service.FindAsync(utttObject.Id);
            Assert.NotNull(result);
            Assert.Equal(utttObject.Id, result.Id);

            mockCollection.Verify(c => c.FindAsync(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<FindOptions<UtttObject>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void FindAsync_ReturnsListOfDocuments_WhenGivenNoParameters()
        {
            var utttObject1 = new UtttObject() { Id = "1234567890ab1234567890ab" };
            var utttObject2 = new UtttObject() { Id = "123456789012345678901234" };

            utttObjectCursor.Setup(_ => _.Current).Returns(new List<UtttObject>() { utttObject1, utttObject2 });

            mockCollection.Setup(c => c.FindAsync<UtttObject>(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<FindOptions<UtttObject, UtttObject>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(utttObjectCursor.Object);

            var result = await service.FindAllAsync();
            Assert.NotEmpty(result);

            mockCollection.Verify(c => c.FindAsync(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<FindOptions<UtttObject>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void CreateAsync_ReturnsDocument()
        {
            var utttObject = new UtttObject() { Id = "1234567890ab1234567890ab" };

            mockCollection.Setup(c => c.InsertOneAsync(
                It.IsAny<UtttObject>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()
            ));

            var result = await service.CreateAsync(utttObject);
            Assert.Equal(utttObject.Id, result.Id);

            mockCollection.Verify(c => c.InsertOneAsync(
                It.IsAny<UtttObject>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void UpdateAsync_CallsReplaceOneAsync()
        {
            mockCollection.Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<UtttObject>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()
            ));

            var utttObject = new UtttObject() { Id = "1234567890ab1234567890ab" };
            await service.UpdateAsync(utttObject);

            mockCollection.Verify(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<UtttObject>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Theory]
        [MemberData(nameof(InvalidIds))]
        public async void DeleteAsync_ThrowsException_WhenIdIsNotValid(string id)
        {
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => service.DeleteAsync(id));
            Assert.Equal(400, (int)result.StatusCode);
        }

        [Fact]
        public async void DeleteAsync_ThrowsException_WhenDocumentDoesNotExist()
        {
            var mockDeletedResult = new Mock<DeleteResult>();
            mockDeletedResult.SetupGet(d => d.DeletedCount).Returns(0L);

            mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(mockDeletedResult.Object);

            var id = "1234567890ab1234567890ab";
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => service.DeleteAsync(id));
            Assert.Equal(404, (int)result.StatusCode);


            mockCollection.Verify(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_CallsDeleteOneAsync()
        {
            var mockDeletedResult = new Mock<DeleteResult>();
            mockDeletedResult.SetupGet(d => d.DeletedCount).Returns(1L);

            mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(mockDeletedResult.Object);

            var id = "1234567890ab1234567890ab";
            await service.DeleteAsync(id);

            mockCollection.Verify(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<UtttObject>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}