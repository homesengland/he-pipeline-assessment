using Azure.Core;
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
        private readonly ILogger<SubmitRollbackCommandHandler> _logger;

        public SubmitRollbackCommandHandler(IAssessmentRepository assessmentRepository, IDateTimeProvider dateTimeProvider, ILogger<SubmitRollbackCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<Unit> Handle(SubmitRollbackCommand command, CancellationToken cancellationToken)
        {
            try
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
                        workflowInstance.Status = AssessmentToolWorkflowInstanceConstants.SuspendedRollBack;
                    }

                    await _assessmentRepository.SaveChanges();
                    await CreateNextWorkflow(intervention);
                }

                return Unit.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to submit rollback. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }

        }

        private async Task CreateNextWorkflow(AssessmentIntervention intervention)
        {

            await _assessmentRepository.DeleteAllNextWorkflows(intervention.AssessmentToolWorkflowInstance
                .AssessmentId);

            var nextWorkflow =
                AssessmentToolInstanceNextWorkflow(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                    intervention.AssessmentToolWorkflowInstanceId,
                    intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId);

            await _assessmentRepository.CreateAssessmentToolInstanceNextWorkflows(
                new List<AssessmentToolInstanceNextWorkflow>() { nextWorkflow });
        }

        private AssessmentToolInstanceNextWorkflow AssessmentToolInstanceNextWorkflow(int assessmentId, int assessmentToolWorkflowInstanceId, string workflowDefinitionId)
        {
            return new AssessmentToolInstanceNextWorkflow
            {
                AssessmentId = assessmentId,
                AssessmentToolWorkflowInstanceId = assessmentToolWorkflowInstanceId,
                NextWorkflowDefinitionId = workflowDefinitionId
            };
        }
    }
}
