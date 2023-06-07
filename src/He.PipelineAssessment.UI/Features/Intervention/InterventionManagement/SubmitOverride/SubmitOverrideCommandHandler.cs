using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride
{
    public class SubmitOverrideCommandHandler : IRequestHandler<SubmitOverrideCommand>
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
            var intervention =
                await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
            }

            if (command.Status != null)
            {
                intervention.Status = command.Status;
            }
            intervention.DateSubmitted = _dateTimeProvider.UtcNow();
            await _assessmentRepository.UpdateAssessmentIntervention(intervention);

            if (intervention.Status == InterventionStatus.Approved)
            {
                var workflowsToDelete =
                    await _assessmentRepository.GetSubsequentWorkflowInstances(intervention
                        .AssessmentToolWorkflowInstance.WorkflowInstanceId);
                var currentWorkflow = workflowsToDelete.First(x => x.Id == intervention.AssessmentToolWorkflowInstanceId);
                workflowsToDelete.Remove(currentWorkflow);
                foreach (var workflowInstance in workflowsToDelete)
                {
                    workflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Deleted;
                }

                await _assessmentRepository.SaveChanges();
                await CreateNextWorkflow(intervention);
            }
            
            return Unit.Value;
        }

        private async Task CreateNextWorkflow(AssessmentIntervention intervention)
        {
            var nextWorkflow = await _assessmentRepository.GetAssessmentToolInstanceNextWorkflow(intervention.AssessmentToolWorkflowInstanceId,
                                    intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId);

            if (nextWorkflow == null)
            {
                var previousWorkflows =
                    await _assessmentRepository.GetPreviousAssessmentToolWorkflowInstances(intervention.AssessmentToolWorkflowInstance.WorkflowInstanceId);

                if (previousWorkflows != null && previousWorkflows.Any())
                {
                    var lastWorkflowInstance = previousWorkflows.OrderByDescending(x => x.CreatedDateTime).First();
                    nextWorkflow =
                        AssessmentToolInstanceNextWorkflow(lastWorkflowInstance.AssessmentId,
                            lastWorkflowInstance.Id, intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId);

                    await _assessmentRepository.CreateAssessmentToolInstanceNextWorkflows(
                        new List<AssessmentToolInstanceNextWorkflow>() { nextWorkflow });
                }
            }
            else
            {
                nextWorkflow.IsStarted = false;
                await _assessmentRepository.SaveChanges();

                await _assessmentRepository.DeleteSubsequentNextWorkflows(nextWorkflow);
            }

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
