using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand
{
    public class UpdateAssessmentToolWorkflowCommandHandler : IRequestHandler<UpdateAssessmentToolWorkflowCommand, int>
    {
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly ILogger<UpdateAssessmentToolWorkflowCommandHandler> _logger;

        public UpdateAssessmentToolWorkflowCommandHandler(IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository, ILogger<UpdateAssessmentToolWorkflowCommandHandler> logger)
        {
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _logger = logger;
        }

        public async Task<int> Handle(UpdateAssessmentToolWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowById(request.Id);
                ArgumentNullException.ThrowIfNull(entity, "Assessment Tool Workflow not found");
                entity.WorkflowDefinitionId = request.WorkflowDefinitionId;
                entity.Name = request.Name;
                entity.IsFirstWorkflow = request.IsFirstWorkflow;
                entity.IsEconomistWorkflow = request.IsEconomistWorkflow;
                entity.IsAmendable = request.IsAmendableWorkflow;
                return await _adminAssessmentToolWorkflowRepository.UpdateAssessmentToolWorkflow(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to update assessment tool workflow. AssessmentToolWoirkflowId: {request.Id}");
            }

        }
    }
}
