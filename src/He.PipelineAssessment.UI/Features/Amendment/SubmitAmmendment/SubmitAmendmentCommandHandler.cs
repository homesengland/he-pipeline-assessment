using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment
{
    public class SubmitAmendmentCommandHandler : IRequestHandler<SubmitAmendmentCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<SubmitAmendmentCommandHandler> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IElsaServerHttpClient _elsaServerHttpClient;

        public SubmitAmendmentCommandHandler(IAssessmentRepository assessmentRepository, IDateTimeProvider dateTimeProvider, ILogger<SubmitAmendmentCommandHandler> logger, IElsaServerHttpClient elsaServerHttpClient)
        {
            _assessmentRepository = assessmentRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task Handle(SubmitAmendmentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var intervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }

                intervention.Status = InterventionStatus.Approved;
                intervention.DateSubmitted = _dateTimeProvider.UtcNow();
                await _assessmentRepository.UpdateAssessmentIntervention(intervention);

                var workflowsToDelete =
                await _assessmentRepository.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order);


                foreach (var workflowInstance in workflowsToDelete)
                {
                    workflowInstance.Status = AssessmentToolWorkflowInstanceConstants.SuspendedAmendment;
                }

                intervention.AssessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
                await _assessmentRepository.SaveChanges();
                await _assessmentRepository.DeleteAllNextWorkflows(intervention.AssessmentToolWorkflowInstance.AssessmentId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Confirm amendment failed. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }

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
