using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public interface IAssessmentInterventionMapper
    {
        AssessmentIntervention AssessmentInterventionFromAssessmentInterventionCommand(AssessmentInterventionCommand command);

        AssessmentInterventionCommand AssessmentInterventionCommandFromAssessmentIntervention(AssessmentIntervention intervention);

        List<TargetWorkflowDefinition> TargetWorkflowDefinitionsFromAssessmentToolWorkflows(
            List<AssessmentToolWorkflow> assessmentToolWorkflows);
    }

    public class AssessmentInterventionMapper : IAssessmentInterventionMapper
    {

        public AssessmentIntervention AssessmentInterventionFromAssessmentInterventionCommand(AssessmentInterventionCommand command)
        {
            AssessmentIntervention intervention = new AssessmentIntervention()
            {
                Id = command.AssessmentInterventionId,
                AssessmentToolWorkflowInstanceId = command.AssessmentToolWorkflowInstanceId,
                TargetAssessmentToolWorkflowId = command.TargetWorkflowId,
                RequestedBy = command.RequestedBy,
                RequestedByEmail = command.RequestedByEmail,
                Administrator = command.Administrator,
                AdministratorEmail = command.AdministratorEmail,
                SignOffDocument = command.SignOffDocument,
                DecisionType = command.DecisionType,
                AssessorRationale = command.AssessorRationale,
                AdministratorRationale = command.AdministratorRationale,
                DateSubmitted = command.DateSubmitted,
                Status = command.Status
            };

            return intervention;
        }

        public AssessmentInterventionCommand AssessmentInterventionCommandFromAssessmentIntervention(AssessmentIntervention intervention)
        {
            AssessmentInterventionCommand command = new AssessmentInterventionCommand()
            {
                AssessmentInterventionId = intervention.Id,
                AssessmentToolWorkflowInstanceId = intervention.AssessmentToolWorkflowInstanceId,
                WorkflowInstanceId = intervention.AssessmentToolWorkflowInstance.WorkflowInstanceId,
                AssessmentName = intervention.AssessmentToolWorkflowInstance.WorkflowName,
                AssessmentResult = intervention.AssessmentToolWorkflowInstance.Result,
                ProjectReference = intervention.AssessmentToolWorkflowInstance.Assessment.Reference,
                RequestedBy = intervention.RequestedBy,
                RequestedByEmail = intervention.RequestedByEmail,
                Administrator = intervention.Administrator,
                AdministratorEmail = intervention.AdministratorEmail,
                SignOffDocument = intervention.SignOffDocument,
                DecisionType = intervention.DecisionType,
                AssessorRationale = intervention.AdministratorRationale,
                DateSubmitted = intervention.DateSubmitted,
                Status = intervention.Status,
                TargetWorkflowId = intervention.TargetAssessmentToolWorkflowId,
                TargetWorkflowDefinitionId = intervention.TargetAssessmentToolWorkflow.WorkflowDefinitionId,
                TargetWorkflowDefinitionName = intervention.TargetAssessmentToolWorkflow.Name
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
    }
}
