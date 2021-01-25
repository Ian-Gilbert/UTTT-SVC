using Microsoft.AspNetCore.Mvc;
using UtttApi.WebApi.Controllers;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class ErrorControllerTests
    {
        private readonly ErrorController controller;
        public ErrorControllerTests()
        {
            controller = new ErrorController();
        }

        [Fact]
        public void Error_ReturnsProblem()
        {
            var result = Assert.IsType<ObjectResult>(controller.Error());
            Assert.Equal(500, result.StatusCode);
        }
    }
}