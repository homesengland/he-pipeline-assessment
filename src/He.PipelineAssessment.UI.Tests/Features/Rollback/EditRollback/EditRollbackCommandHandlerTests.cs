using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.EditRollback
{
    public class EditRollbackCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
            EditRollbackCommand command,
            EditRollbackCommandHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to edit rollback. AssessmentInterventionId: {command.AssessmentInterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenUserIsNotPermitted(
            [Frozen] Mock<IAssessmentRepository> repository,
            AssessmentIntervention intervention,
            EditRollbackCommand command,
            EditRollbackCommandHandler sut)
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
        public async Task Handle_ShouldReturn(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            AssessmentIntervention intervention,
            EditRollbackCommand command,
            EditRollbackCommandHandler sut)
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
            Assert.Equal(intervention.Id,result);
            repository.Verify(x=>x.SaveChanges(),Times.Once);
        }
    }
}
