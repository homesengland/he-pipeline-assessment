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
                var workflowsToDelete =
                    await _assessmentRepository.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.TargetAssessmentToolWorkflow!.AssessmentTool.Order);

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
                await _assessmentRepository.DeleteAllNextWorkflows(intervention.AssessmentToolWorkflowInstance.AssessmentId);

                nextWorkflow =
                    AssessmentToolInstanceNextWorkflow(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                        intervention.AssessmentToolWorkflowInstanceId, intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId);

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
