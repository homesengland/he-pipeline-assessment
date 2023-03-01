using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Mappers
{
    public class AssessmentToolWorkflowMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void ShouldCreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow_Returns(
           CreateAssessmentToolWorkflowCommand assessmentToolWorkflowCommand,
           AssessmentToolWorkflowMapper sut
       )
        {
            //Arrange

            //Act
            var result = sut.CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(assessmentToolWorkflowCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(assessmentToolWorkflowCommand.AssessmentToolId, result.AssessmentToolId);
            Assert.Equal(assessmentToolWorkflowCommand.IsFirstWorkflow, result.IsFirstWorkflow);
            Assert.Equal(assessmentToolWorkflowCommand.Version, result.Version);
            Assert.Equal(assessmentToolWorkflowCommand.IsLatest, result.IsLatest);
            Assert.Equal(assessmentToolWorkflowCommand.WorkflowDefinitionId, result.WorkflowDefinitionId);
            Assert.Equal(assessmentToolWorkflowCommand.Name, result.Name);
        }
    }
}
