using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Error;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Error
{
    public class ErrorControllerTests
    {
        [Theory]
        [AutoMoqData]
        public void Index_ShouldReturn(
            ErrorController sut)
        {
            //Arrange

            //Act
            var result = sut.Index("");

            //Assert
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
