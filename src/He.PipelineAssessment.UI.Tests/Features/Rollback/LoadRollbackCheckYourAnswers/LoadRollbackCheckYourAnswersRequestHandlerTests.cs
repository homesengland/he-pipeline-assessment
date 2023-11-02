using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor;
using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.LoadRollbackCheckYourAnswers
{
    public class LoadRollbackCheckYourAnswersRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionServiceErrors(
            [Frozen] Mock<IInterventionService> interventionService,
            Exception e,
            LoadRollbackCheckYourAnswersRequest request,
            LoadRollbackCheckYourAnswersRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.LoadInterventionCheckYourAnswersRequest(request)).Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSubmitRollbackCommand(
            [Frozen] Mock<IInterventionService> interventionService,
            AssessmentInterventionCommand command,
            LoadRollbackCheckYourAnswersRequest request,
            LoadRollbackCheckYourAnswersRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.LoadInterventionCheckYourAnswersRequest(request))
                .ReturnsAsync(command);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SubmitRollbackCommand>(result);
            Assert.Equal(command.CorrelationId, result.CorrelationId);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldThrow_GivenInterventionServiceReturnsNonSubmitRollbackCommand(
            [Frozen] Mock<IInterventionService> interventionService,
            AssessmentInterventionCommand command,
            LoadRollbackCheckYourAnswersRequest request,
            LoadRollbackCheckYourAnswersRequestHandler sut)
        {
            //TODO: not sure how easy it will be to do though
        }
    }
}
