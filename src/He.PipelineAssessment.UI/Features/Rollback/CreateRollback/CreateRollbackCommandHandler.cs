using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.CreateRollback
{
    public class CreateRollbackCommandHandler : IRequestHandler<CreateRollbackCommand, int>
    {
        private readonly ICreateRollbackMapper _mapper;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentToolWorkflowInstanceHelpers _assessmentToolWorkflowInstanceHelpers;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<CreateRollbackCommandHandler> _logger;

        public CreateRollbackCommandHandler(IAssessmentRepository assessmentRepository, ICreateRollbackMapper mapper, ILogger<CreateRollbackCommandHandler> logger, IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers, IRoleValidation roleValidation)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
            _logger = logger;
            _assessmentToolWorkflowInstanceHelpers = assessmentToolWorkflowInstanceHelpers;
            _roleValidation = roleValidation;
        }


        public async Task<int> Handle(CreateRollbackCommand command, CancellationToken cancellationToken)
        {
            try
            {
                AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId);
                if (workflowInstance == null)
                {
                    throw new NotFoundException($"Assessment Tool Workflow Instance with Id {command.WorkflowInstanceId} not found");
                }

                var isAuthorised = await _roleValidation.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId);
                if (!isAuthorised)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                var isLatest = _assessmentToolWorkflowInstanceHelpers.IsLatestSubmittedWorkflow(workflowInstance);
                if (!isLatest)
                {
                    throw new Exception(
                        $"Unable to create rollback for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment. WorkflowInstanceId: {command.WorkflowInstanceId}");
                }

                var assessmentIntervention = _mapper.CreateRollbackCommandToAssessmentIntervention(command);

                await _assessmentRepository.CreateAssessmentIntervention(assessmentIntervention);

                return assessmentIntervention.Id;
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
               _logger.LogError(e,e.Message);
                throw new ApplicationException($"Unable to create rollback. WorkflowInstanceId: {command.WorkflowInstanceId}");
            }
           
        }
    }
}
