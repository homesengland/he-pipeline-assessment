using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Utility;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Common.Utility
{
    public class AssessmentFundIdProviderTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetCurrentFundId_ReturnsValidFundId_GivenRepositoryReturnsValue(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            int assessmentId,
            int fundId,
            AssessmentFundIdProvider sut)
        {
            //Arrange
            assessmentRepository
                .Setup(x => x.GetCurrentWorkflowFundId(assessmentId))
                .ReturnsAsync(fundId);

            //Act
            var result = await sut.GetCurrentFundId(assessmentId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(fundId, result.Value);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetCurrentFundId_ReturnsNull_GivenRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<ILogger<AssessmentFundIdProvider>> logger,
            int assessmentId,
            AssessmentFundIdProvider sut)
        {
            //Arrange
            assessmentRepository
                .Setup(x => x.GetCurrentWorkflowFundId(assessmentId))
                .ReturnsAsync((int?)null);

            //Act
            var result = await sut.GetCurrentFundId(assessmentId);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetCurrentFundId_ThrowsException_GivenRepositoryThrowsException(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<ILogger<AssessmentFundIdProvider>> logger,
            int assessmentId,
            AssessmentFundIdProvider sut)
        {
            //Arrange
            var expectedException = new Exception("Database error");
            assessmentRepository
                .Setup(x => x.GetCurrentWorkflowFundId(assessmentId))
                .ThrowsAsync(expectedException);

            //Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => sut.GetCurrentFundId(assessmentId));

            Assert.Equal("Database error", exception.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task AssignFundId_AssignsFundSuccessfully_GivenValidFundIdAndDifferentExistingFund(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<ILogger<AssessmentFundIdProvider>> logger,
            Assessment assessment,
            int newFundId,
            AssessmentFundIdProvider sut)
        {
            //Arrange
            assessment.FundId = 999; // Different from newFundId
            assessmentRepository
                .Setup(x => x.GetCurrentWorkflowFundId(assessment.Id))
                .ReturnsAsync(newFundId);
            assessmentRepository
                .Setup(x => x.GetAssessment(assessment.Id))
                .ReturnsAsync(assessment);
            assessmentRepository
                .Setup(x => x.SaveChanges())
                .ReturnsAsync(1);

            //Act
            await sut.AssignFundId(assessment.Id);

            //Assert
            Assert.Equal(newFundId, assessment.FundId);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task AssignFundId_DoesNotAssign_GivenFundIdAlreadyMatches(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            Assessment assessment,
            int fundId,
            AssessmentFundIdProvider sut)
        {
            //Arrange
            assessment.FundId = fundId; // Same as current fund
            assessmentRepository
                .Setup(x => x.GetCurrentWorkflowFundId(assessment.Id))
                .ReturnsAsync(fundId);
            assessmentRepository
                .Setup(x => x.GetAssessment(assessment.Id))
                .ReturnsAsync(assessment);

            //Act
            await sut.AssignFundId(assessment.Id);

            //Assert
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task AssignFundId_DoesNotAssign_GivenNoFundIdFound(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            int assessmentId,
            AssessmentFundIdProvider sut)
        {
            //Arrange
            assessmentRepository
                .Setup(x => x.GetCurrentWorkflowFundId(assessmentId))
                .ReturnsAsync((int?)null);

            //Act
            await sut.AssignFundId(assessmentId);

            //Assert
            assessmentRepository.Verify(x => x.GetAssessment(It.IsAny<int>()), Times.Never);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task AssignFundId_DoesNotAssign_GivenAssessmentIsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            int assessmentId,
            int fundId,
            AssessmentFundIdProvider sut)
        {
            //Arrange
            assessmentRepository
                .Setup(x => x.GetCurrentWorkflowFundId(assessmentId))
                .ReturnsAsync(fundId);
            assessmentRepository
                .Setup(x => x.GetAssessment(assessmentId))
                .ReturnsAsync((Assessment?)null);

            //Act
            await sut.AssignFundId(assessmentId);

            //Assert
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task AssignFundId_AssignsFundSuccessfully_GivenNullExistingFundId(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<ILogger<AssessmentFundIdProvider>> logger,
            Assessment assessment,
            int newFundId,
            AssessmentFundIdProvider sut)
        {
            //Arrange
            assessment.FundId = null; // No existing fund
            assessmentRepository
                .Setup(x => x.GetCurrentWorkflowFundId(assessment.Id))
                .ReturnsAsync(newFundId);
            assessmentRepository
                .Setup(x => x.GetAssessment(assessment.Id))
                .ReturnsAsync(assessment);
            assessmentRepository
                .Setup(x => x.SaveChanges())
                .ReturnsAsync(1);

            //Act
            await sut.AssignFundId(assessment.Id);

            //Assert
            Assert.Equal(newFundId, assessment.FundId);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
