using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Rollback.DeleteRollback;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.DeleteRollback
{
    public class DeleteRollbackCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
            DeleteRollbackCommand command,
            DeleteRollbackCommandHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to delete rollback. WorkflowInstanceId: {command.WorkflowInstanceId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenUserIsNotPermitted(
            [Frozen]Mock<IAssessmentRepository> repository,
            AssessmentIntervention intervention,
            DeleteRollbackCommand command,
            DeleteRollbackCommandHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldDelete(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            AssessmentIntervention intervention,
            DeleteRollbackCommand command,
            DeleteRollbackCommandHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);

            roleValidation.Setup(x =>
                x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(true);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            repository.Verify(x=>x.DeleteIntervention(intervention),Times.Once);

        }
    }
}
