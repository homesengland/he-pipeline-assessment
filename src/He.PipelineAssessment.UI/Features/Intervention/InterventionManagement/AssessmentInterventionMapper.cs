using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public interface IAssessmentInterventionMapper
    {
        AssessmentInterventionCommand AssessmentInterventionCommandFromAssessmentIntervention(AssessmentIntervention intervention);

        List<TargetWorkflowDefinition> TargetWorkflowDefinitionsFromAssessmentToolWorkflows(
            List<AssessmentToolWorkflow> assessmentToolWorkflows);

        AssessmentInterventionDto AssessmentInterventionDtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance, string userName, string userEmail);
    }

    public class AssessmentInterventionMapper : IAssessmentInterventionMapper
    {
        public AssessmentInterventionCommand AssessmentInterventionCommandFromAssessmentIntervention(AssessmentIntervention intervention)
        {
            AssessmentInterventionCommand command = new AssessmentInterventionCommand()
            {
                AssessmentInterventionId = intervention.Id,
                AssessmentToolWorkflowInstanceId = intervention.AssessmentToolWorkflowInstanceId,
                WorkflowInstanceId = intervention.AssessmentToolWorkflowInstance.WorkflowInstanceId,
                AssessmentName = intervention.AssessmentToolWorkflowInstance.WorkflowName,
                AssessmentResult = intervention.AssessmentResult,
                ProjectReference = intervention.AssessmentToolWorkflowInstance.Assessment.Reference,
                RequestedBy = intervention.RequestedBy,
                RequestedByEmail = intervention.RequestedByEmail,
                Administrator = intervention.Administrator,
                AdministratorEmail = intervention.AdministratorEmail,
                AdministratorRationale = intervention.AdministratorRationale,
                SignOffDocument = intervention.SignOffDocument,
                DecisionType = intervention.DecisionType,
                AssessorRationale = intervention.AssessorRationale,
                DateSubmitted = intervention.DateSubmitted,
                Status = intervention.Status,
                TargetWorkflowId = intervention.TargetAssessmentToolWorkflowId,
                TargetWorkflowDefinitionId = intervention.TargetAssessmentToolWorkflow?.WorkflowDefinitionId,
                TargetWorkflowDefinitionName = intervention.TargetAssessmentToolWorkflow?.Name,
                AssessmentId = intervention.AssessmentToolWorkflowInstance.Id,
                CorrelationId = intervention.AssessmentToolWorkflowInstance.Assessment.SpId
            };

            return command;
        }

        public List<TargetWorkflowDefinition> TargetWorkflowDefinitionsFromAssessmentToolWorkflows(List<AssessmentToolWorkflow> assessmentToolWorkflows)
        {
            return assessmentToolWorkflows.Select(x => new TargetWorkflowDefinition
            {
                Id = x.Id,
                WorkflowDefinitionId = x.WorkflowDefinitionId,
                Name = x.Name
            }).ToList();
        }

        public AssessmentInterventionDto AssessmentInterventionDtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance, string userName, string email)
        {

            AssessmentInterventionDto dto = new AssessmentInterventionDto()
            {
                AssessmentInterventionCommand = new AssessmentInterventionCommand
                {
                    AssessmentToolWorkflowInstanceId = instance.Id,
                    WorkflowInstanceId = instance.WorkflowInstanceId,
                    AssessmentResult = instance.Result,
                    AssessmentName = instance.WorkflowName,
                    RequestedBy = userName,
                    RequestedByEmail = email,
                    Administrator = userName,
                    AdministratorEmail = email,
                    DecisionType = InterventionDecisionTypes.Override,
                    Status = InterventionStatus.NotSubmitted,
                    ProjectReference = instance.Assessment.Reference
                }
            };
            return dto;
        }
    }
}
