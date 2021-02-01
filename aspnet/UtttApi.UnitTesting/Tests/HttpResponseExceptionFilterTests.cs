using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using UtttApi.ObjectModel.Exceptions;
using UtttApi.WebApi.Filters;
using Xunit;

namespace UtttApi.UnitTesting.Tests
{
    public class HttpResponseExceptionFilterTests
    {
        private readonly ActionExecutedContext context;
        private readonly HttpResponseExceptionFilter filter;

        public HttpResponseExceptionFilterTests()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("", "error");
            var httpContext = new DefaultHttpContext();
            context = new ActionExecutedContext
            (
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                new List<IFilterMetadata>(),
                new Mock<Controller>().Object
            );

            filter = new HttpResponseExceptionFilter();
        }

        [Fact]
        public void Order_ShouldBeMaxIntMinus10()
        {
            Assert.Equal(int.MaxValue - 10, filter.Order);
        }

        [Fact]
        public void OnActionExecuted_CreatesObjectResultAndSetsExceptionHandledTrue_WhenExceptionIsHttpResponseException()
        {
            var status = HttpStatusCode.OK;
            var message = "test message";
            context.Exception = new HttpResponseException(status, message);

            filter.OnActionExecuted(context);

            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal((int)status, result.StatusCode);
            Assert.Equal(message, result.Value);
            Assert.True(context.ExceptionHandled);
        }

        [Fact]
        public void OnActionExceuted_ShouldDoNothing_WhenExceptionIsNotHttpresponseException()
        {
            filter.OnActionExecuted(context);

            Assert.Null(context.Result);
            Assert.False(context.ExceptionHandled);
        }
    }
}