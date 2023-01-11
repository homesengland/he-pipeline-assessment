using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Assessment.AssessmentSummary
{
    public class AssessmentSummaryRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenRepoThrowsError(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentSummaryRequest request,
            Exception exception,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).Throws(exception);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentSummaryResponseWithNoStages_GivenNoStagesExistYet(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            Models.Assessment assessment,
            AssessmentSummaryRequest request,
            AssessmentSummaryRequestHandler sut
        )
        {

            //Arrange
            var emptyList = new List<AssessmentStageViewModel>();
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).ReturnsAsync(assessment);
            assessmentRepository.Setup(x => x.GetAssessmentStages(It.IsAny<int>())).ReturnsAsync(emptyList);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.CorrelationId, result!.CorrelationId);
            Assert.Equal(request.AssessmentId, result!.AssessmentId);
            Assert.Equal(assessment.SiteName, result!.SiteName);
            Assert.Equal(assessment.Counterparty, result!.CounterParty);
            Assert.Equal(assessment.Reference, result!.Reference);
            Assert.Empty(result.Stages);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsAssessmentSummaryResponseWithStages_GivenStagesExist(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            Models.Assessment assessment,
            List<AssessmentStageViewModel> stages,
            AssessmentSummaryRequest request,
            AssessmentSummaryRequestHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessment(It.IsAny<int>())).ReturnsAsync(assessment);
            assessmentRepository.Setup(x => x.GetAssessmentStages(It.IsAny<int>())).ReturnsAsync(stages);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.CorrelationId, result!.CorrelationId);
            Assert.Equal(request.AssessmentId, result!.AssessmentId);
            Assert.Equal(assessment.SiteName, result!.SiteName);
            Assert.Equal(assessment.Counterparty, result!.CounterParty);
            Assert.Equal(assessment.Reference, result!.Reference);
            Assert.NotEmpty(result.Stages);

        }
    }
}
