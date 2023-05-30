using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers
{
    public class CreateAssessmentInterventionRequestHandler : IRequestHandler<LoadOverrideCheckYourAnswersRequest, LoadOverrideCheckYourAnswersCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;

        public CreateAssessmentInterventionRequestHandler(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        public async Task<LoadOverrideCheckYourAnswersCommand> Handle(LoadOverrideCheckYourAnswersRequest request, CancellationToken cancellationToken)
        {
            var intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
            LoadOverrideCheckYourAnswersCommand dto = InterventionToLoadCheckYourAnswersOverrideCommand(intervention);
            return dto;

        }

        private LoadOverrideCheckYourAnswersCommand InterventionToLoadCheckYourAnswersOverrideCommand(AssessmentIntervention intervention)
        {
            return new LoadOverrideCheckYourAnswersCommand
            {
                AssessmentInterventionId = intervention.Id,
                AssessmentName = intervention.AssessmentToolWorkflowInstance.WorkflowName,
                WorkflowInstanceId = intervention.AssessmentToolWorkflowInstance.WorkflowInstanceId,
                DateSubmitted = intervention.DateSubmitted,
                AdministratorRationale = intervention.AdministratorRationale,
                AssessorRationale = intervention.AssessorRationale,
                Status = intervention.Status,
                DecisionType = intervention.DecisionType,
                RequestedBy = intervention.RequestedBy,
                RequestedByEmail = intervention.RequestedByEmail,
                SignOffDocument = intervention.SignOffDocument,
                TargetWorkflowDefinitionId = intervention.TargetAssessmentToolWorkflow.WorkflowDefinitionId,
                TargetWorkflowDefinitionName = intervention.TargetAssessmentToolWorkflow.Name


            };
        }
    }
}
