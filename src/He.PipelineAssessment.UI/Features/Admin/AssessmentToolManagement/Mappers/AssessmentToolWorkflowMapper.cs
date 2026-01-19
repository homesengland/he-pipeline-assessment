using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers
{
    public interface IAssessmentToolWorkflowMapper
    {
        AssessmentToolWorkflow CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(CreateAssessmentToolWorkflowCommand assessmentToolWorkflowCommand);
    }
    public class AssessmentToolWorkflowMapper : IAssessmentToolWorkflowMapper
    {
        public AssessmentToolWorkflow CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(CreateAssessmentToolWorkflowCommand assessmentToolWorkflowCommand)
        {
            return new AssessmentToolWorkflow
            {
                AssessmentToolId = assessmentToolWorkflowCommand.AssessmentToolId,
                IsFirstWorkflow = assessmentToolWorkflowCommand.IsFirstWorkflow,
                IsEconomistWorkflow = assessmentToolWorkflowCommand.IsEconomistWorkflow,
                IsAmendable = assessmentToolWorkflowCommand.IsAmendableWorkflow,
                Version = assessmentToolWorkflowCommand.Version,
                IsLatest = assessmentToolWorkflowCommand.IsLatest,
                WorkflowDefinitionId = assessmentToolWorkflowCommand.WorkflowDefinitionId,
                Name = assessmentToolWorkflowCommand.Name,
                IsVariation = assessmentToolWorkflowCommand.IsVariation,
                IsEarlyStage = assessmentToolWorkflowCommand.IsEarlyStage,
                IsLast = assessmentToolWorkflowCommand.IsLast,
                AssessmentFundId = assessmentToolWorkflowCommand.AssessmentFundId
            };
        }
    }
}
