using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentToolWorkflow
{
    public class DeleteAssessmentToolWorkflowCommandHandler : IRequestHandler<DeleteAssessmentToolWorkflowCommand, int>
    {
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly ILogger<DeleteAssessmentToolWorkflowCommandHandler> _logger;

        public DeleteAssessmentToolWorkflowCommandHandler(IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository, ILogger<DeleteAssessmentToolWorkflowCommandHandler> logger)
        {
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _logger = logger;
        }

        public async Task<int> Handle(DeleteAssessmentToolWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowById(request.Id);
                ArgumentNullException.ThrowIfNull(entity, "Assessment Tool Workflow not found");
                entity.Status = AssessmentToolStatus.Deleted;
                return await _adminAssessmentToolWorkflowRepository.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to delete assessment tool workflow.");
            }
           
        }
    }
}
