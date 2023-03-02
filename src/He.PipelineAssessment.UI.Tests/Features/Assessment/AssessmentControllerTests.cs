using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using He.PipelineAssessment.UI.Features.Assessments;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.SinglePipeline
{
    public class AssessmentControllerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Index_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
           [Frozen] Mock<IMediator> mediator,
           Exception exception,
           AssessmentController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<AssessmentListCommand>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.Index();

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<AssessmentListCommand>(), CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.IsAny<AssessmentListCommand>()));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task Index_ShouldRedirectToAction_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            AssessmentListCommand command,
            List<AssessmentDataViewModel> response,
            AssessmentController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            //Act
            var result = await sut.Index();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

        }

        [Theory]
        [AutoMoqData]
        public async Task Summary_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
           [Frozen] Mock<IMediator> mediator,
           Exception exception,
           AssessmentController sut,
           int correlationId,
           int assessmentId)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.Summary(assessmentId, correlationId);

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.IsAny<AssessmentSummaryRequest>()));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task Summary_ShouldRedirectToAction_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            AssessmentListCommand command,
            List<AssessmentDataViewModel> response,
            AssessmentController sut,
            int correlationId,
            int assessmentId)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            //Act
            var result = await sut.Summary(assessmentId, correlationId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

        }
    }
}
