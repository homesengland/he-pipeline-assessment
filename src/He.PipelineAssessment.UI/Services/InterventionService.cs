using Azure.Core;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;

namespace He.PipelineAssessment.UI.Services
{
    public class InterventionService : IInterventionService
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<ConfirmRollbackCommandHandler> _logger;
        private readonly IAssessmentToolWorkflowInstanceHelpers _assessmentToolWorkflowInstanceHelpers;
        private readonly IRoleValidation _roleValidation;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly IUserProvider _userProvider;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;

        public InterventionService(IAssessmentRepository assessmentRepository, IRoleValidation roleValidation, IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers, IAssessmentInterventionMapper mapper, IUserProvider userProvider, ILogger<ConfirmRollbackCommandHandler> logger, IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _assessmentToolWorkflowInstanceHelpers = assessmentToolWorkflowInstanceHelpers;
            _roleValidation = roleValidation;
            _mapper = mapper;
            _userProvider = userProvider;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
    }

        public async Task ConfirmIntervention(AssessmentInterventionCommand command)
        {
            try
            {
                var intervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }

                intervention.Status = InterventionStatus.Pending;
                await _assessmentRepository.UpdateAssessmentIntervention(intervention);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Confirm rollback failed. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }
        }

        public async Task<int> CreateAssessmentIntervention(AssessmentInterventionCommand command)
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
                        $"Unable to create {command.DecisionType} for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment. WorkflowInstanceId: {command.WorkflowInstanceId}");
                }

                var assessmentIntervention = _mapper.AssessmentInterventionFromAssessmentInterventionCommand(command);

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
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to create {command.DecisionType}. WorkflowInstanceId: {command.WorkflowInstanceId}");
            }
        }

        public async Task<AssessmentInterventionDto> CreateInterventionRequest(CreateInterventionRequest request)
        {
            try
            {
                AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);
                if (workflowInstance == null)
                {
                    throw new NotFoundException($"Assessment Tool Workflow Instance with Id {request.WorkflowInstanceId} not found");
                }

                var isAuthorised = await _roleValidation.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId);
                if (!isAuthorised)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                var isLatest = _assessmentToolWorkflowInstanceHelpers.IsLatestSubmittedWorkflow(workflowInstance);
                if (!isLatest)
                {
                    throw new ApplicationException(
                        $"Unable to create intervention for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment.");
                }

                var activeInterventionsForAssessment = await _assessmentRepository.GetOpenAssessmentInterventions(workflowInstance.AssessmentId);
                if (activeInterventionsForAssessment.Any())
                {
                    throw new ApplicationException(
                        $"Unable to create request as an open request already exists for this assessment.");
                }

                var userName = _userProvider.GetUserName()!;
                var email = _userProvider.GetUserEmail()!;

                var dtoConfig = new DtoConfig()
                {
                    UserName = userName,
                    UserEmail = email,
                    DecisionType = InterventionDecisionTypes.Rollback, //TODO: this needs to be different depending on intervention type
                    Status = InterventionStatus.Draft
                };

                //override
                //var dtoConfig = new DtoConfig()
                //{
                //    AdministratorName = userName,
                //    UserName = userName,
                //    AdministratorEmail = email,
                //    UserEmail = email,
                //    DecisionType = InterventionDecisionTypes.Override,
                //    Status = InterventionStatus.Pending
                //};

                var interventionReasons = await _assessmentRepository.GetInterventionReasons();

                var dto = _mapper.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, interventionReasons, dtoConfig);

                return dto;
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to create intervention request. WorkflowInstanceId: {request.WorkflowInstanceId}");
            }
        }

        public async Task<int> DeleteIntervention(AssessmentInterventionCommand command)
        {
            try
            {
                var intervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }

                var isAuthorised = await _roleValidation.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId);
                if (!isAuthorised)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                return await _assessmentRepository.DeleteIntervention(intervention);
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to delete rollback. WorkflowInstanceId: {command.WorkflowInstanceId}");
            }
        }

        public async Task<int> EditIntervention(AssessmentInterventionCommand command)
        {
            try
            {
                var assessmentIntervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (assessmentIntervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }

                var isAuthorised = await _roleValidation.ValidateRole(assessmentIntervention.AssessmentToolWorkflowInstance.AssessmentId, assessmentIntervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId);
                if (!isAuthorised)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                assessmentIntervention.AdministratorRationale = command.AdministratorRationale;
                assessmentIntervention.SignOffDocument = command.SignOffDocument;
                assessmentIntervention.TargetAssessmentToolWorkflowId = command.TargetWorkflowId;
                assessmentIntervention.Administrator = command.Administrator;
                assessmentIntervention.AdministratorEmail = command.AdministratorEmail;
                await _assessmentRepository.SaveChanges();
                return assessmentIntervention.Id;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to edit {command.DecisionType}. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }
        }

        public async Task<int> EditInterventionAssessor(AssessmentInterventionCommand command)
        {
            try
            {
                var assessmentIntervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (assessmentIntervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }
                assessmentIntervention.AssessorRationale = command.AssessorRationale;
                assessmentIntervention.InterventionReasonId = command.InterventionReasonId;
                await _assessmentRepository.SaveChanges();
                return assessmentIntervention.Id;
            }

            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to edit {command.DecisionType}. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }
        }


        public async Task<AssessmentInterventionDto> EditInterventionAssessorRequest(EditInterventionAssessorRequest request)
        {
            try
            {
                AssessmentIntervention? intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
                }
                AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);

                var interventionReasons = await _assessmentRepository.GetInterventionReasons();

                var dto = new AssessmentInterventionDto
                {
                    AssessmentInterventionCommand = command,
                    InterventionReasons = interventionReasons
                };
                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to edit rollback. InterventionId: {request.InterventionId}");
            }
        }

        public async Task<AssessmentInterventionDto> EditInterventionRequest(EditInterventionRequest request)
        {
            try
            {
                //TODO: find decision type
                AssessmentIntervention? intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
                }

                var isAuthorised = await _roleValidation.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId);
                if (!isAuthorised)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);

                var dto = new AssessmentInterventionDto
                {
                    AssessmentInterventionCommand = command
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
                throw new ApplicationException($"Unable to edit intervention. InterventionId: {request.InterventionId}");
            }
        }

        public async Task<AssessmentInterventionCommand> LoadInterventionCheckYourAnswerAssessorRequest(LoadInterventionCheckYourAnswersAssessorRequest request)
        {
            try
            {
                var intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
                }
                var command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
                return command;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to load rollback check your answers. InterventionId: {request.InterventionId}");
            }
        }

        public async Task<AssessmentInterventionCommand> LoadInterventionCheckYourAnswersRequest(LoadInterventionCheckYourAnswersRequest request)
        {
            try
            {
                var intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
                }
                AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);

                return command;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to load rollback. InterventionId: {request.InterventionId}");
            }
        }

        public async Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForOverride(string workflowInstanceId)
        {
            AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(workflowInstanceId);
            if (workflowInstance == null)
            {
                throw new NotFoundException($"Assessment Tool Workflow Instance with Id {workflowInstanceId} not found");
            }

            List<AssessmentToolWorkflow> assessmentToolWorkflows =
                await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowsForOverride(workflowInstance
                    .AssessmentToolWorkflow.AssessmentTool.Order);

            if (assessmentToolWorkflows == null || !assessmentToolWorkflows.Any())
            {
                throw new NotFoundException($"No suitable assessment tool workflows found for override");
            }

            return assessmentToolWorkflows;
        }

        public async Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForRollback(string workflowInstanceId)
        {
            AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(workflowInstanceId);
            if (workflowInstance == null)
            {
                throw new NotFoundException($"Assessment Tool Workflow Instance with Id {workflowInstanceId} not found");
            }

            List<AssessmentToolWorkflow> assessmentToolWorkflows =
                await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowsForRollback(workflowInstance
                    .AssessmentToolWorkflow.AssessmentTool.Order);

            if (assessmentToolWorkflows == null || !assessmentToolWorkflows.Any())
            {
                throw new NotFoundException($"No suitable assessment tool workflows found for rollback");
            }

            return assessmentToolWorkflows;
        }
    }
}
