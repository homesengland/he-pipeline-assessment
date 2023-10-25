using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Override.SubmitOverride;

namespace He.PipelineAssessment.UI.Services
{
    public interface IInterventionService
    {
        Task ConfirmIntervention(AssessmentInterventionCommand command);
        Task<int> CreateAssessmentIntervention(AssessmentInterventionCommand command);
        Task<AssessmentInterventionDto> CreateInterventionRequest(CreateInterventionRequest request);
        Task<int> DeleteIntervention(AssessmentInterventionCommand command);
        Task<int> EditIntervention(AssessmentInterventionCommand command);
        Task<int> EditInterventionAssessor(AssessmentInterventionCommand command);
        Task<AssessmentInterventionDto> EditInterventionAssessorRequest(EditInterventionAssessorRequest request);
        Task<AssessmentInterventionDto> EditInterventionRequest(EditInterventionRequest request);
        Task<AssessmentInterventionCommand> LoadInterventionCheckYourAnswerAssessorRequest(LoadInterventionCheckYourAnswersAssessorRequest request);
        Task<AssessmentInterventionCommand> LoadInterventionCheckYourAnswersRequest(LoadInterventionCheckYourAnswersRequest request);
        Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForOverride(string workflowInstanceId);
        Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForRollback(string workflowInstanceId);
        Task SubmitIntervention(AssessmentInterventionCommand command);
    }
}
