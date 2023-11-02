using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.LoadRollbackCheckYourAnswersAssessor
{
    public class LoadRollbackCheckYourAnswersAssessorRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionServiceErrors(
            [Frozen] Mock<IInterventionService> interventionService,
            Exception e,
            LoadRollbackCheckYourAnswersAssessorRequest request,
            LoadRollbackCheckYourAnswersAssessorRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.LoadInterventionCheckYourAnswerAssessorRequest(request)).Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnConfirmRollbackCommand(
            [Frozen] Mock<IInterventionService> interventionService,
            AssessmentInterventionCommand command,
            LoadRollbackCheckYourAnswersAssessorRequest request,
            LoadRollbackCheckYourAnswersAssessorRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.LoadInterventionCheckYourAnswerAssessorRequest(request))
                .ReturnsAsync(command);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ConfirmRollbackCommand>(result);
            Assert.Equal(command.CorrelationId, result.CorrelationId);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldThrow_GivenInterventionServiceReturnsNonConfirmRollbackCommand(
            [Frozen] Mock<IInterventionService> interventionService,
            AssessmentInterventionCommand command,
            LoadRollbackCheckYourAnswersAssessorRequest request,
            LoadRollbackCheckYourAnswersAssessorRequestHandler sut)
        {
            //TODO: not sure how easy it will be to do though
        }
    }
}
