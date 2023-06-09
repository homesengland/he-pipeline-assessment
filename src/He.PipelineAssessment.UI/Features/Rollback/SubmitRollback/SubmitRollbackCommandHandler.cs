using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.SubmitRollback
{
    public class SubmitRollbackCommandHandler : IRequestHandler<SubmitRollbackCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SubmitRollbackCommandHandler(IAssessmentRepository assessmentRepository, IDateTimeProvider dateTimeProvider)
        {
            _assessmentRepository = assessmentRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(SubmitRollbackCommand command, CancellationToken cancellationToken)
        {
            var intervention =
                await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
            }

            intervention.Status = command.Status;
            intervention.DateSubmitted = _dateTimeProvider.UtcNow();
            await _assessmentRepository.UpdateAssessmentIntervention(intervention);

            if (intervention.Status == InterventionStatus.Approved)
            {
                //get instance for rollback
                var instance = await _assessmentRepository.GetAssessmentToolWorkflowInstanceForRollback(intervention);
                if (instance == null)
                {
                    instance = await _assessmentRepository.GetSameStageAssessmentToolWorkflowInstanceForRollback(intervention);
                }

                if (instance == null)
                    throw new NotFoundException("Unable to found workflow instance to roll back to");

                var workflowsToDelete =
                    await _assessmentRepository.GetSubsequentWorkflowInstancesForRollback(instance);

                foreach (var workflowInstance in workflowsToDelete)
                {
                    workflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Deleted;
                }

                await _assessmentRepository.SaveChanges();
                await CreateNextWorkflow(intervention, instance);
            }

            return Unit.Value;
        }

        private async Task CreateNextWorkflow(AssessmentIntervention intervention, AssessmentToolWorkflowInstance instance)
        {
            var previousWorkflows =
                await _assessmentRepository.GetPreviousAssessmentToolWorkflowInstances(instance);

            if (previousWorkflows != null && previousWorkflows.Any())
            {
                var lastWorkflowInstance = previousWorkflows.OrderByDescending(x => x.CreatedDateTime).First();

                var nextWorkflow = await _assessmentRepository.GetAssessmentToolInstanceNextWorkflow(lastWorkflowInstance.Id,
                                        intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId);

                if (nextWorkflow == null)
                {
                    nextWorkflow =
                        AssessmentToolInstanceNextWorkflow(lastWorkflowInstance.AssessmentId,
                            lastWorkflowInstance.Id, intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId);

                    await _assessmentRepository.CreateAssessmentToolInstanceNextWorkflows(
                        new List<AssessmentToolInstanceNextWorkflow>() { nextWorkflow });
                }
                else
                {
                    nextWorkflow.IsStarted = false;
                    await _assessmentRepository.SaveChanges();

                    await _assessmentRepository.DeleteSubsequentNextWorkflows(nextWorkflow);
                }
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
