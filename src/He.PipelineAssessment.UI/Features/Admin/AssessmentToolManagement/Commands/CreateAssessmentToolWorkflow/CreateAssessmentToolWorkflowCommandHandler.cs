using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow
{
    public class CreateAssessmentToolWorkflowCommandHandler : IRequestHandler<CreateAssessmentToolWorkflowCommand, int>
    {
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly IAssessmentToolWorkflowMapper _assessmentToolWorkflowMapper;
        private readonly ILogger<CreateAssessmentToolWorkflowCommandHandler> _logger;

        public CreateAssessmentToolWorkflowCommandHandler(IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository,
                                                          IAssessmentToolWorkflowMapper assessmentToolWorkflowMapper, ILogger<CreateAssessmentToolWorkflowCommandHandler> logger)
        {
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _assessmentToolWorkflowMapper = assessmentToolWorkflowMapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateAssessmentToolWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _assessmentToolWorkflowMapper.CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(request);
                await _adminAssessmentToolWorkflowRepository.CreateAssessmentToolWorkflow(entity);

                return entity.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to create assessment tool workflow.");
            }
        }
    }
}
