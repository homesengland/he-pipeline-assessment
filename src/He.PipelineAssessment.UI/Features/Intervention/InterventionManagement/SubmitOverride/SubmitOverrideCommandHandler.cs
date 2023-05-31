using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention.Constants;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride
{
    public class SubmitOverrideCommandHandler : IRequestHandler<SubmitOverrideCommand, Unit>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SubmitOverrideCommandHandler(IAssessmentRepository assessmentRepository, IDateTimeProvider dateTimeProvider)
        {
            _assessmentRepository = assessmentRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(SubmitOverrideCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var intervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (intervention == null)
                {
                    throw new ApplicationException(
                        $"Unable to find intervention with Id: {command.AssessmentInterventionId}");
                }
                intervention.DateSubmitted = _dateTimeProvider.UtcNow();
                await _assessmentRepository.UpdateAssessmentIntervention(intervention);

                if (command.Status == InterventionStatus.Approved)
                {
                    await _assessmentRepository.DeleteSubsequentWorkflowInstances(command.WorkflowInstanceId);
                    await StartNextWorkflow(command);
                }
            }
            catch(Exception e)
            {
               
            }

            return Unit.Value;
        }

        private async Task StartNextWorkflow(SubmitOverrideCommand command)
        {
            var previousWorkflow = await _assessmentRepository
            var nextWorkflow = await _assessmentRepository.GetNonStartedAssessmentToolInstanceNextWorkflow(command.AssessmentToolWorkflowInstanceId,
                                    command.TargetWorkflowDefinitionId);

            if (nextWorkflow == null)
            {
                var assessmentToolInstanceNextWorkflow =
                    AssessmentToolInstanceNextWorkflow(currentAssessmentToolWorkflowInstance.AssessmentId,
                        currentAssessmentToolWorkflowInstance.Id, workflowDefinitionId);
                nextWorkflows.Add(assessmentToolInstanceNextWorkflow);
            }
        }
    }
}
