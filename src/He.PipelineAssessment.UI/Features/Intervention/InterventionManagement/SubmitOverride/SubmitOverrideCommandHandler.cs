using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
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


            var nextWorkflow = await _assessmentRepository.GetNonStartedAssessmentToolInstanceNextWorkflow(command.AssessmentToolWorkflowInstanceId,
                                    command.TargetWorkflowDefinitionId!);

            if (nextWorkflow == null)
            {
                var previousWorkflows =
                    await _assessmentRepository.GetPreviousAssessmentToolWorkflowInstances(command.WorkflowInstanceId);

                if (previousWorkflows != null && previousWorkflows.Any())
                {
                    var lastWorkflowInstance = previousWorkflows.OrderByDescending(x => x.CreatedDateTime).First();
                    nextWorkflow =
                        AssessmentToolInstanceNextWorkflow(lastWorkflowInstance.AssessmentId,
                            lastWorkflowInstance.Id, command.TargetWorkflowDefinitionId!);

                    await _assessmentRepository.CreateAssessmentToolInstanceNextWorkflows(
                        new List<AssessmentToolInstanceNextWorkflow>() { nextWorkflow });
                }
                else
                {
                    //something has gone wrong
                }
            }
            else
            {
                nextWorkflow.IsStarted = false;
                await _assessmentRepository.SaveChanges();
            }
            await _assessmentRepository.DeleteSubsequentNextWorkflows(nextWorkflow);
        }

        private AssessmentToolInstanceNextWorkflow AssessmentToolInstanceNextWorkflow(int assessmentId, int assessmentToolWorkflowInstanceId, string workflowDefinitionId)
        {
            return new AssessmentToolInstanceNextWorkflow
            {
                AssessmentId = assessmentId,
                AssessmentToolWorkflowInstanceId = assessmentToolWorkflowInstanceId,
                NextWorkflowDefinitionId = workflowDefinitionId,
                IsStarted = false
            };
        }
    }
}
