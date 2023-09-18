using AutoFixture.Xunit2;
using He.PipelineAssessment.Models.ViewModels;
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
        public async Task Sync_ShouldReturnView_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            SyncCommand command,
            SyncModel response,
            SinglePipelineController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            //Act
            ViewResult result = (ViewResult) await sut.Sync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Sync", result.ViewName);
        }

        [Theory]
        [AutoMoqData]
        public void Index_ShouldReturnView_GivenNoExceptionsThrow(
    [Frozen] Mock<IMediator> mediator,
    SyncCommand command,
    SyncModel response,
    SinglePipelineController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            //Act
            ViewResult result = (ViewResult)sut.Index();
            SyncModel? model = (SyncModel?)result.Model;
            
            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal("Sync", result.ViewName);
            Assert.NotNull(model);
            Assert.False( model!.Synced);
            Assert.Equal(0, model.NewAssessmentCount);
            Assert.Equal(0, model.UpdatedAssessmentCount);
        }
    }
}
