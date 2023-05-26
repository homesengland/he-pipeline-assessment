namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersCommand : AssessmentInterventionCommand
    {
        public LoadOverrideCheckYourAnswersCommand()
        {

        }
        public LoadOverrideCheckYourAnswersCommand(AssessmentInterventionCommand command)
        {
            RequestedBy = command.RequestedBy;
            Administrator = command.Administrator;
            AdministratorEmail = command.AdministratorEmail;
            AdministratorRationale = command.AdministratorRationale;
            AssessmentName = command.AssessmentName;
            AssessmentResult = command.AssessmentResult;
            AssessorRationale = command.AssessorRationale;
            DateSubmitted = command.DateSubmitted;
            DecisionType = command.DecisionType;
            ProjectReference = command.ProjectReference;
            RequestedByEmail = command.RequestedByEmail;
            SignOffDocument = command.SignOffDocument;
            Status = command.Status;
            TargetWorkflowDefinitionId = command.TargetWorkflowDefinitionId;
            WorkflowInstanceId = command.WorkflowInstanceId;
            AssessmentToolWorkflowInstanceId = command.AssessmentToolWorkflowInstanceId;
        }
    }
}
