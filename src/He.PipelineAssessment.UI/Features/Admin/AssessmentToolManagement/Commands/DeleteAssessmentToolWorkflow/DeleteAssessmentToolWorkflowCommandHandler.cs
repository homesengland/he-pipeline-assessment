using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentToolWorkflow
{
    public class DeleteAssessmentToolWorkflowCommandHandler : IRequestHandler<DeleteAssessmentToolWorkflowCommand, int>
    {
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;

        public DeleteAssessmentToolWorkflowCommandHandler(IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository)
        {
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
        }

        public async Task<int> Handle(DeleteAssessmentToolWorkflowCommand request, CancellationToken cancellationToken)
        {
            var entity = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowById(request.Id);
            ArgumentNullException.ThrowIfNull(entity, "Assessment Tool Workflow not found");

            return await _adminAssessmentToolWorkflowRepository.DeleteAssessmentToolWorkflow(entity);
        }
    }
}
