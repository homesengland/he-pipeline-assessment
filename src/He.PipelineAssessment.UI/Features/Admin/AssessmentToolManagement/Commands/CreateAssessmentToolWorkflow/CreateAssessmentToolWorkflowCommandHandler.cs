using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow
{
    public class CreateAssessmentToolWorkflowCommandHandler : IRequestHandler<CreateAssessmentToolWorkflowCommand, int>
    {
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly IAssessmentToolWorkflowMapper _assessmentToolWorkflowMapper;
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly ILogger<CreateAssessmentToolWorkflowCommandHandler> _logger;
        private readonly IAssessmentRepository _assessmentRepository;

        public CreateAssessmentToolWorkflowCommandHandler(
            IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository,
            IAdminAssessmentToolRepository adminAssessmentToolRepository,
            IAssessmentToolWorkflowMapper assessmentToolWorkflowMapper,
            ILogger<CreateAssessmentToolWorkflowCommandHandler> logger,
            IAssessmentRepository assessmentRepository)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _assessmentToolWorkflowMapper = assessmentToolWorkflowMapper;
            _logger = logger;
            _assessmentRepository = assessmentRepository;
        }

        public async Task<int> Handle(CreateAssessmentToolWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                bool isAnExistingWorkFlow = default;
                var oldWorkFlowDefinitionId = "";
                int workflowInstanceId = 0;
                var assessmentTool = await _adminAssessmentToolRepository.GetAssessmentToolById(request.AssessmentToolId);
                request.Name = request.SelectedOption;
                request.Category = $"{assessmentTool?.Name.Trim()}-{request.Name.Trim()}";
                if (assessmentTool != null && assessmentTool.AssessmentToolWorkflows != null && assessmentTool.AssessmentToolWorkflows.Where(x => x.Category == request.Category).Count() > 0)
                {
                    isAnExistingWorkFlow = true;
                    var oldAssessmentWorkflow = assessmentTool.AssessmentToolWorkflows.Where(x => x.Category == request.Category && x.IsLatest).FirstOrDefault();
                    request.Version  = assessmentTool.AssessmentToolWorkflows.Where(x => x.Category == request.Category).OrderByDescending(x => x.CreatedDateTime).First().Version + 1;
                    oldWorkFlowDefinitionId = oldAssessmentWorkflow?.WorkflowDefinitionId ?? "";
                    workflowInstanceId = oldAssessmentWorkflow?.Id ?? 0;
                }
                var entity = _assessmentToolWorkflowMapper.CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(request);
                await _adminAssessmentToolWorkflowRepository.CreateAssessmentToolWorkflow(entity);

                if (isAnExistingWorkFlow)
                {
                    await _adminAssessmentToolWorkflowRepository.UpdateIsLatest(request.AssessmentToolId, request.Category , oldWorkFlowDefinitionId);
                    await _assessmentRepository.UpdateNextWorkflowDefintionId(oldWorkFlowDefinitionId, request.WorkflowDefinitionId);
                }
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
