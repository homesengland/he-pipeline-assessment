using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollback
{
    public class EditRollbackRequestHandler : IRequestHandler<EditRollbackRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentInterventionMapper _mapper;
        private readonly IAssessmentRepository _repository;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly ILogger<EditRollbackRequestHandler> _logger;
        private readonly IRoleValidation _roleValidation;

        public EditRollbackRequestHandler(IAssessmentInterventionMapper mapper,
            IAssessmentRepository repo, IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository,
            ILogger<EditRollbackRequestHandler> logger, IRoleValidation roleValidation)
        {
            _mapper = mapper;
            _repository = repo;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _logger = logger;
            _roleValidation = roleValidation;
        }
        public async Task<AssessmentInterventionDto> Handle(EditRollbackRequest request, CancellationToken cancellationToken)
        {
            try
            {
                AssessmentIntervention? intervention = await _repository.GetAssessmentIntervention(request.InterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
                }

                var isAuthorised = await _roleValidation.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId);
                if (!isAuthorised)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                List<AssessmentToolWorkflow> assessmentToolWorkflows =
                    await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowsForRollback(intervention.AssessmentToolWorkflowInstance
                        .AssessmentToolWorkflow.AssessmentTool.Order);

                if (assessmentToolWorkflows == null || !assessmentToolWorkflows.Any())
                {
                    throw new NotFoundException($"No suitable assessment tool workflows found for rollback");
                }

                AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);

                var dto = new AssessmentInterventionDto
                {
                    AssessmentInterventionCommand = command,
                    TargetWorkflowDefinitions =
                        _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows)
                };
                return dto;
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to edit rollback. InterventionId: {request.InterventionId}");
            }

        }
    }
}
