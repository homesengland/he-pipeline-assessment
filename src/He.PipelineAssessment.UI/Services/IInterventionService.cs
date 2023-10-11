using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using He.PipelineAssessment.UI.Features.Rollback.DeleteRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor;
using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;

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
    }
}
