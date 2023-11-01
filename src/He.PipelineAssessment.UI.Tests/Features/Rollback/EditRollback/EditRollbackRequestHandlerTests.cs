using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.EditRollback
{
    public class EditRollbackRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
            EditRollbackRequest request,
            EditRollbackRequestHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to edit rollback. InterventionId: {request.InterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenNotPermitted(
            [Frozen]Mock<IAssessmentRepository> repository,
            AssessmentIntervention intervention,
            EditRollbackRequest request,
            EditRollbackRequestHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(request.InterventionId))
                .ReturnsAsync(intervention);

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenNoSuitableWorkflowsToRollBackTo(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            AssessmentIntervention intervention,
            EditRollbackRequest request,
            EditRollbackRequestHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(request.InterventionId))
                .ReturnsAsync(intervention);

            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(true);

            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowsForRollback(intervention
                .AssessmentToolWorkflowInstance
                .AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(new List<AssessmentToolWorkflow>());

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to edit rollback. InterventionId: {request.InterventionId}", ex.Message);
            adminAssessmentToolWorkflowRepository.Verify(x=>x.GetAssessmentToolWorkflowsForRollback(intervention
                .AssessmentToolWorkflowInstance
                .AssessmentToolWorkflow.AssessmentTool.Order),Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturn(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock <IAssessmentInterventionMapper> interventionMapper,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command,
            List<AssessmentToolWorkflow> listSource,
            List<TargetWorkflowDefinition> listTarget,
            EditRollbackRequest request,
            EditRollbackRequestHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(request.InterventionId))
                .ReturnsAsync(intervention);

            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(true);

            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowsForRollback(intervention
                .AssessmentToolWorkflowInstance
                .AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(listSource);

            interventionMapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention))
                .Returns(command);

            //interventionMapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(listSource))
            //    .Returns(listTarget);

            //Act
            var result =  await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AssessmentInterventionDto>(result);
            Assert.Equal(command,result.AssessmentInterventionCommand);
            Assert.Equal(listTarget, result.TargetWorkflowDefinitions);

            adminAssessmentToolWorkflowRepository.Verify(x => x.GetAssessmentToolWorkflowsForRollback(intervention
                .AssessmentToolWorkflowInstance
                .AssessmentToolWorkflow.AssessmentTool.Order), Times.Once);

            interventionMapper.Verify(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention),Times.Once);
            //interventionMapper.Verify(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(listSource),Times.Once);
        }
    }
}
