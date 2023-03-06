using Elsa.Dashboard.PageModels;
using He.PipelineAssessment.Tests.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Elsa.Dashboard.Tests.Pages
{
    public class ErrorPageTests
    {
        [Theory]
        [AutoMoqData]
        public void ErrorPageCorrectlyIdentifiesError_GivenHttpContextException(
                Mock<IFeatureCollection> features,
                Mock<HttpContext> context,
                ILogger<ErrorModel> logger)
        {

            // Arrange
            features.Setup(p => p.Get<IExceptionHandlerPathFeature>()).Returns(new ExceptionHandlerFeature
            {
                Path = "/",
                Error = new HttpRequestException()
            });
            context.Setup(p => p.Features).Returns(features.Object);
            var pageModel = new ErrorModel(logger);
            var pageContext = new PageContext
            {
                HttpContext = context.Object
            };
            pageModel.PageContext = pageContext;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.Equal("Sorry, there is a problem with the service.", pageModel.ErrorMessage);
            Assert.Equal("Please wait a few moments and try again.", pageModel.AdditionalMessage);
            Assert.True(pageModel.SuggestRetry);
        }

        [Theory]
        [AutoMoqData]
        public void ErrorPageCorrectlyIdentifiesError_GivenNullConfigException(
        Mock<IFeatureCollection> features,
        Mock<HttpContext> context,
        ILogger<ErrorModel> logger)
        {

            // Arrange
            features.Setup(p => p.Get<IExceptionHandlerPathFeature>()).Returns(new ExceptionHandlerFeature
            {
                Path = "/",
                Error = new NullReferenceException()
            });
            context.Setup(p => p.Features).Returns(features.Object);
            var pageModel = new ErrorModel(logger);
            var pageContext = new PageContext
            {
                HttpContext = context.Object
            };
            pageModel.PageContext = pageContext;

            // Act
            pageModel.OnGet();

            // Assert

            Assert.Equal("Sorry, there has been a problem whilst retrieving the required Data.", pageModel.ErrorMessage);
            Assert.Equal("Please contact the support team to investigate further.", pageModel.AdditionalMessage);
            Assert.False(pageModel.SuggestRetry);
        }

        [Theory]
        [AutoMoqData]
        public void ErrorPageCorrectlyIdentifiesError_GivenUnhandledException(
        Mock<IFeatureCollection> features,
        Mock<HttpContext> context,
        Mock<ILogger<ErrorModel>> logger)
        {

            // Arrange
            features.Setup(p => p.Get<IExceptionHandlerPathFeature>()).Returns(new ExceptionHandlerFeature
            {
                Path = "/",
                Error = new Exception()
            });
            context.Setup(p => p.Features).Returns(features.Object);
            var pageModel = new ErrorModel(logger.Object);
            var pageContext = new PageContext
            {
                HttpContext = context.Object
            };
            pageModel.PageContext = pageContext;

            // Act
            pageModel.OnGet();

            // Assert
            //logger.Verify(logger => logger.Log(It.Is(LogLevel.Error), It.Is(0), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<TState, Exception, string>>()), Times.Once);
            Assert.Equal("Sorry, something went wrong whilst trying to access this service.", pageModel.ErrorMessage);
            Assert.Equal("Please contact the support team to investigate further.", pageModel.AdditionalMessage);
            Assert.False(pageModel.SuggestRetry);
        }

        [Theory]
        [AutoMoqData]
        public void ErrorPageSuggestsBaseUrlRetry_GivenNoErrorIdentified(
            Mock<IFeatureCollection> features,
            Mock<HttpContext> context,
            Mock<ILogger<ErrorModel>> logger)
        {

            // Arrange
            features.Setup(p => p.Get<IExceptionHandlerPathFeature>()).Returns(new ExceptionHandlerFeature
            {
                Path = "/",
            });
            context.Setup(p => p.Features).Returns(features.Object);
            var pageModel = new ErrorModel(logger.Object);
            var pageContext = new PageContext
            {
                HttpContext = context.Object
            };
            pageModel.PageContext = pageContext;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.Equal("Sorry, there is a problem with the service.", pageModel.ErrorMessage);
            Assert.Equal("Please wait a few moments and try again.", pageModel.AdditionalMessage);
            Assert.True(pageModel.SuggestRetry);
        }
    }
}
