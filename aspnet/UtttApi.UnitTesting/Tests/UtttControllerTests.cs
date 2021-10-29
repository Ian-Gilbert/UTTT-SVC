using Microsoft.AspNetCore.Mvc;
using Moq;
using UtttApi.DataService.Interfaces;
using UtttApi.ObjectModel.Enums;
using UtttApi.ObjectModel.Models;
using UtttApi.WebApi.Controllers;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class UtttControllerTests
    {
        private readonly UtttObject uttt;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly UtttController controller;

        public UtttControllerTests()
        {
            var id = "test123";
            uttt = new UtttObject() { Id = id };

            var mockDataService = new Mock<IDataService<UtttObject>>();
            mockDataService.Setup(s => s.FindAsync(id, default)).ReturnsAsync(uttt);
            mockDataService.Setup(s => s.CreateAsync(It.IsAny<UtttObject>())).ReturnsAsync(uttt);
            mockDataService.Setup(s => s.UpdateAsync(uttt));
            mockDataService.Setup(s => s.DeleteAsync(id, default));

            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Game).Returns(mockDataService.Object);

            controller = new UtttController(mockUnitOfWork.Object);
        }

        [Fact]
        public async void Get_Returns200Ok()
        {
            var result = Assert.IsType<OkObjectResult>(await controller.Get(uttt.Id));
            var resultObject = (UtttObject)result.Value;

            Assert.Equal(uttt.Id, resultObject.Id);

            mockUnitOfWork.Verify(u => u.Game.FindAsync(uttt.Id, default), Times.Once);
        }

        [Fact]
        public async void Post_Returns201Created()
        {
            var result = Assert.IsType<CreatedAtActionResult>(await controller.Post());
            var resultObject = (UtttObject)result.Value;

            Assert.Equal(uttt.Id, resultObject.Id);
            Assert.Equal(nameof(controller.Get), result.ActionName);
            Assert.Equal(uttt.Id, result.RouteValues["id"]);

            mockUnitOfWork.Verify(u => u.Game.CreateAsync(It.IsAny<UtttObject>()), Times.Once);
        }

        [Fact]
        public async void Put_Returns202Accepted_AndMakesMove()
        {
            var move = new Move() { Mark = MarkType.PLAYER1, LbIndex = 0, MarkIndex = 0 };

            var result = Assert.IsType<AcceptedResult>(await controller.Put(uttt.Id, move));
            var resultObject = (UtttObject)result.Value;

            Assert.Equal(uttt.Id, resultObject.Id);
            Assert.Equal(move.Mark, resultObject.GlobalBoard.LocalBoards[move.LbIndex].Board[move.MarkIndex]);

            mockUnitOfWork.Verify(u => u.Game.UpdateAsync(uttt), Times.Once);
        }

        [Fact]
        public async void Delete_Returns204NoContent()
        {
            var result = Assert.IsType<NoContentResult>(await controller.Delete(uttt.Id));

            mockUnitOfWork.Verify(u => u.Game.DeleteAsync(uttt.Id, default), Times.Once);
        }
    }
}