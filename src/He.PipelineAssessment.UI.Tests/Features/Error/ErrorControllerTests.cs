using System.Net;
using System.Text.RegularExpressions;
using AutoFixture.Xunit2;
using Castle.Core.Logging;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Error;
using HibernatingRhinos.Profiler.Appender;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Error
{
    public class ErrorControllerTests
    {
        [Theory]
        [AutoMoqData]
        public void Index_ShouldReturn_ErrorView_GivenErrorOccurs(
            [Frozen] Mock<IErrorHelper> errorHelper,
            [Frozen] Mock<ILogger<ErrorController>> logger,
            Exception exception,
            ErrorController sut
            )
        {
            //Arrange
            errorHelper.Setup(x => x.ExceptionHandlerFeatureGetException(It.IsAny<HttpContext>())).Returns(exception);

            //Act
            var result = sut.Index();

            //Assert
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<ViewResult>(result);
            VerifyLog(logger, LogLevel.Error, Times.Once, $"An error occurred while processing your request {exception.Message}");

        }

        private void VerifyLog(Mock<ILogger<ErrorController>> logger, LogLevel level, Func<Times> times, string regex)
        {
            logger.Verify(m => m.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((x, y) => regex == null || Regex.IsMatch(x.ToString(), regex)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times);
        }

        [Theory]
        [AutoMoqData]
        public void Index_ShouldReturn_NoContent_GivenNoErrorOccurs(
            [Frozen] Mock<IErrorHelper> errorHelper,
            ErrorController sut
        )
        {
            //Arrange
            errorHelper.Setup(x => x.ExceptionHandlerFeatureGetException(It.IsAny<HttpContext>())).Returns((Exception?)null);

            //Act
            var result = sut.Index();

            //Assert
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<NoContentResult>(result);
        }

    }
}
