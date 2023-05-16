using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand
{
    public class UpdateAssessmentToolWorkflowCommandHandler : IRequestHandler<UpdateAssessmentToolWorkflowCommand, int>
    {
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;

        public UpdateAssessmentToolWorkflowCommandHandler(IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository)
        {
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
        }

        public async Task<int> Handle(UpdateAssessmentToolWorkflowCommand request, CancellationToken cancellationToken)
        {
            var entity = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowById(request.Id);
            ArgumentNullException.ThrowIfNull(entity, "Assessment Tool Workflow not found");
            entity.WorkflowDefinitionId = request.WorkflowDefinitionId;
            entity.Name = request.Name;
            entity.IsFirstWorkflow = request.IsFirstWorkflow;
            entity.IsEconomistWorkflow = request.IsEconomistWorkflow;
            return await _adminAssessmentToolWorkflowRepository.UpdateAssessmentToolWorkflow(entity);
        }
    }
}
