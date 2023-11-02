using He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Services;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.EditRollbackAssessor
{
    public class EditRollbackAssessorRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionServiceErrors(
            [Frozen] Mock<IInterventionService> interventionService,
            Exception e,
            EditRollbackAssessorRequest request,
            EditRollbackAssessorRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.EditInterventionAssessorRequest(request)).Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnDto_GivenSuccessfulCallToInterventionService(
            [Frozen] Mock<IInterventionService> interventionService,
            AssessmentInterventionDto dto,
            EditRollbackAssessorRequest request,
            EditRollbackAssessorRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.EditInterventionAssessorRequest(request)).ReturnsAsync(dto);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result, dto);
        }
    }
}
