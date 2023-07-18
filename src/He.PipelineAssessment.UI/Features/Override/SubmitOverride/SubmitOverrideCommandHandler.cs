using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace He.PipelineAssessment.UI.Features.Override.SubmitOverride
{
    public class SubmitOverrideCommandHandler : IRequestHandler<SubmitOverrideCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger _logger;

        public SubmitOverrideCommandHandler(IAssessmentRepository assessmentRepository, IDateTimeProvider dateTimeProvider, ILogger logger)
        {
            _assessmentRepository = assessmentRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<Unit> Handle(SubmitOverrideCommand command, CancellationToken cancellationToken)
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
                        await _assessmentRepository.GetSubsequentWorkflowInstancesForOverride(intervention
                            .AssessmentToolWorkflowInstance.WorkflowInstanceId);

                    foreach (var workflowInstance in workflowsToDelete)
                    {
                        workflowInstance.Status = AssessmentToolWorkflowInstanceConstants.SuspendedRollBack;
                    }

                    await _assessmentRepository.SaveChanges();
                    await CreateNextWorkflow(intervention);
                }

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException($"Unalbe to submit override. AssessmentInterventionId: {command.AssessmentInterventionId}.");
            }
        }

        private async Task CreateNextWorkflow(AssessmentIntervention intervention)
        {
            var nextWorkflow = AssessmentToolInstanceNextWorkflow(
                intervention.AssessmentToolWorkflowInstance.AssessmentId,
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
