using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.LoadRollbackCheckYourAnswersAssessor
{
    public class LoadRollbackCheckYourAnswersAssessorRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionNotFound(
            LoadRollbackCheckYourAnswersAssessorRequest request,
            LoadRollbackCheckYourAnswersAssessorRequestHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to load rollback check your answers. InterventionId: {request.InterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturn(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            AssessmentInterventionCommand command,
            AssessmentIntervention intervention,
            LoadRollbackCheckYourAnswersAssessorRequest request,
            LoadRollbackCheckYourAnswersAssessorRequestHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(request.InterventionId))
                .ReturnsAsync(intervention);

            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention))
                .Returns(command);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ConfirmRollbackCommand>(result);
        }
    }
}
