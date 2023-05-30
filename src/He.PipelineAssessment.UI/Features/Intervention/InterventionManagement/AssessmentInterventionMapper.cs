using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention.Constants;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride
{
    public interface IAssessmentInterventionMapper
    {
        AssessmentIntervention FromAssessmentInterventionCommand(AssessmentInterventionCommand command);

        AssessmentInterventionCommand ToAssessmentInterventionCommand(AssessmentIntervention intervention);
    }

    public class AssessmentInterventionMapper : IAssessmentInterventionMapper
    {

        public AssessmentIntervention FromAssessmentInterventionCommand(AssessmentInterventionCommand command)
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

        public AssessmentInterventionCommand ToAssessmentInterventionCommand(AssessmentIntervention intervention)
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
    }
}
