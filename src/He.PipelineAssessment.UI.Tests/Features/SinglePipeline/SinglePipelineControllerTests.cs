using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.SinglePipeline;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.SinglePipeline
{
    public class SinglePipelineControllerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task StartWorkflow_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
           [Frozen] Mock<IMediator> mediator,
           Exception exception,
           SinglePipelineController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<SyncCommand>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.Sync();

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<SyncCommand>(), CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.IsAny<SyncCommand>()));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task StartWorkflow_ShouldRedirectToAction_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            SyncCommand command,
            SyncResponse response,
            SinglePipelineController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            //Act
            var result = await sut.Sync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);

        }
    }
}
