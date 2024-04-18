using AutoFixture.Xunit2;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.EditRollback
{
    public class EditRollbackRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionServiceErrors(
            [Frozen] Mock<IInterventionService> interventionService,
            Exception e,
            EditRollbackRequest request,
            EditRollbackRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.EditInterventionRequest(request)).Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnDtoWithCorrectWorkflowDefinitions_GivenSuccessfulCallToInterventionService(
            [Frozen] Mock<IInterventionService> interventionService,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            AssessmentInterventionDto dto,
            List<AssessmentToolWorkflow> assessmentToolWorkflows,
            List<TargetWorkflowDefinition> targetWorkflowDefinitions,
            EditRollbackRequest request,
            EditRollbackRequestHandler sut)
        {
            //Arrange
            dto.TargetWorkflowDefinitions = new List<TargetWorkflowDefinition>();
            interventionService.Setup(x => x.EditInterventionRequest(request))
                .ReturnsAsync(dto);
            interventionService.Setup(x =>
                    x.GetAssessmentToolWorkflowsForRollback(dto.AssessmentInterventionCommand.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflows);
            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows,
                    dto.AssessmentInterventionCommand.SelectedWorkflowDefinitions))
                .Returns(targetWorkflowDefinitions);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(dto.AssessmentInterventionCommand, result.AssessmentInterventionCommand);
            Assert.Equal(dto.InterventionReasons, result.InterventionReasons);
            Assert.Equal(targetWorkflowDefinitions, result.TargetWorkflowDefinitions);
        }
    }
}
