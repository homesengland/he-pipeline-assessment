using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand
{
    public class UpdateAssessmentToolWorkflowCommandHandler : IRequestHandler<UpdateAssessmentToolWorkflowCommand, int>
    {
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly ILogger<UpdateAssessmentToolWorkflowCommandHandler> _logger;
        private readonly IAssessmentRepository _assessmentRepository;


        public UpdateAssessmentToolWorkflowCommandHandler(IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository, 
            ILogger<UpdateAssessmentToolWorkflowCommandHandler> logger,
            IAssessmentRepository assessmentRepository)
        {
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _logger = logger;
            _assessmentRepository = assessmentRepository;
        }

        public async Task<int> Handle(UpdateAssessmentToolWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowById(request.Id);
                ArgumentNullException.ThrowIfNull(entity, "Assessment Tool Workflow not found");
                entity.WorkflowDefinitionId = request.WorkflowDefinitionId;
                entity.Name = request.Name.Trim();
                entity.IsFirstWorkflow = request.IsFirstWorkflow;
                entity.IsEconomistWorkflow = request.IsEconomistWorkflow;
                entity.IsVariation = request.IsVariation;
                entity.IsAmendable = request.IsAmendableWorkflow;
                entity.IsLast = request.IsLast;
                entity.IsEarlyStage = request.IsEarlyStage;
                entity.Category = entity.Category;
                entity.IsLatest = request.IsLatest;

                var response = await _adminAssessmentToolWorkflowRepository.UpdateAssessmentToolWorkflow(entity);
                var oldAssessment = await _adminAssessmentToolWorkflowRepository.GetExistingAssessmentWorkFlow(request.AssessmentToolId, entity.Category);

                if(response == 1)
                {
                    if (oldAssessment != null && oldAssessment.IsLatest == request.IsLatest && oldAssessment.Id != request.Id)
                    {
                            oldAssessment.IsLatest = false;
                            await _adminAssessmentToolWorkflowRepository.UpdateAssessmentToolWorkflow(oldAssessment);
                            await _assessmentRepository.UpdateNextWorkflowDefintionId(oldAssessment.WorkflowDefinitionId, request.WorkflowDefinitionId);
                    }
                }
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to update assessment tool workflow. AssessmentToolWorkflowId: {request.Id}");
            }

        }
    }
}
