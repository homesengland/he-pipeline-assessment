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
        public void Index_ShouldReturn(
            ErrorController sut,
            [Frozen]Mock<IExceptionHandlerFeature> handler,
            [Frozen]Mock<HttpContext> httpContextMock,
            Mock<IFeatureCollection> mockIFeatureCollection,
            Exception e)
        {
            //Arrange
            httpContextMock.Setup(p => p.Features).Returns(mockIFeatureCollection.Object);
            handler.SetupGet(x => x.Error).Returns(e);
            mockIFeatureCollection.Setup(p => p.Get<IExceptionHandlerFeature>())
                .Returns(handler.Object);
            httpContextMock.Setup(p => p.Features).Returns(mockIFeatureCollection.Object);

            sut.ControllerContext = new ControllerContext() { HttpContext = httpContextMock.Object };

            //Act
            var result = sut.Index();

            //Assert
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
