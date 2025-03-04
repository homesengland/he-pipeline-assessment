using System.Net;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Error;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Error
{
    public class ErrorControllerTests
    {
        [Theory]
        [AutoMoqData]
        public void Index_ShouldReturn_ErrorView_GivenErrorOccurs(
            [Frozen]Mock<IErrorHelper> errorHelper,
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
