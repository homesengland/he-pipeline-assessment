using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using He.PipelineAssessment.UI.Features.Assessments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using Castle.Core.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace He.PipelineAssessment.UI.Tests.Features.SinglePipeline
{
    public class AssessmentControllerTests
    {
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

        [Theory]
        [AutoMoqData]
        public async Task TestSummary_ShouldRedirectToSummary_EnableTestSummaryPageIsFalse(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IConfiguration> configuration,
            AssessmentListCommand command,
            List<AssessmentDataViewModel> response,
            AssessmentController sut,
            int correlationId,
            int assessmentId)
        {
            //Arrange
            configuration.Setup(x => x["Environment:EnableTestSummaryPage"]).Returns("false");

            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);



            //Act
            var result = await sut.TestSummary(assessmentId, correlationId);

            //Assert
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Summary", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task TestSummary_ShouldDirectToTestSummaryView_EnableTestSummaryPageIsTrue(
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IConfiguration> configuration,
        AssessmentListCommand command,
        List<AssessmentDataViewModel> response,
        AssessmentController sut,
        int correlationId,
        int assessmentId)
        {
            //Arrange
            configuration.Setup(x => x["Environment:EnableTestSummaryPage"]).Returns("true");
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            //Act
            var result = await sut.TestSummary(assessmentId, correlationId);

            //Assert
            var viewResult = (ViewResult)result;
            Assert.Equal("TestSummary", viewResult.ViewName);
        }
    }
}
