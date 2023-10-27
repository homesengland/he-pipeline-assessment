using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention;

namespace He.PipelineAssessment.UI.Services
{
    public class InterventionService : IInterventionService
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<InterventionService> _logger;
        private readonly IAssessmentToolWorkflowInstanceHelpers _assessmentToolWorkflowInstanceHelpers;
        private readonly IRoleValidation _roleValidation;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly IUserProvider _userProvider;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IElsaServerHttpClient _elsaServerHttpClient;

        public InterventionService(
            IAssessmentRepository assessmentRepository, 
            IRoleValidation roleValidation,
            IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers,
            IAssessmentInterventionMapper mapper, 
            IUserProvider userProvider, 
            ILogger<InterventionService> logger,
            IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository,
            IDateTimeProvider dateTimeProvider,
            IElsaServerHttpClient elsaServerHttpClient)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _assessmentToolWorkflowInstanceHelpers = assessmentToolWorkflowInstanceHelpers;
            _roleValidation = roleValidation;
            _mapper = mapper;
            _userProvider = userProvider;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _dateTimeProvider = dateTimeProvider;
            _elsaServerHttpClient = elsaServerHttpClient;
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
                throw new ApplicationException($"Confirm {command.DecisionType} failed. AssessmentInterventionId: {command.AssessmentInterventionId}");
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

                var isLatest = await _assessmentToolWorkflowInstanceHelpers.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance);
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

                var isLatest = await _assessmentToolWorkflowInstanceHelpers.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance);
                if (!isLatest)
                {
                    throw new ApplicationException(
                        $"Unable to create {request.DecisionType} for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment.");
                }

                var activeInterventionsForAssessment = await _assessmentRepository.GetOpenAssessmentInterventions(workflowInstance.AssessmentId);
                if (activeInterventionsForAssessment.Any())
                {
                    throw new ApplicationException(
                        $"Unable to create request as an open request already exists for this assessment.");
                }

                var userName = _userProvider.GetUserName()!;
                var email = _userProvider.GetUserEmail()!;
                var adminUserName = request.DecisionType == InterventionDecisionTypes.Override ? userName : null;
                var adminEmail = request.DecisionType == InterventionDecisionTypes.Override ? email : null;

                var dtoConfig = new DtoConfig
                {
                    UserName = userName,
                    UserEmail = email,
                    DecisionType = request.DecisionType,
                    Status = request.InitialStatus,
                    AdministratorName = adminUserName,
                    AdministratorEmail = adminEmail,
                };

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
                throw new ApplicationException($"Unable to create {request.DecisionType} request. WorkflowInstanceId: {request.WorkflowInstanceId}");
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
                throw new ApplicationException($"Unable to delete {command.DecisionType}. WorkflowInstanceId: {command.WorkflowInstanceId}");
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
                throw new ApplicationException($"Unable to edit intervention. InterventionId: {request.InterventionId}");
            }
        }

        public async Task<AssessmentInterventionDto> EditInterventionRequest(EditInterventionRequest request)
        {
            try
            {
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
                throw new ApplicationException($"Unable to load check your answers. InterventionId: {request.InterventionId}");
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
                throw new ApplicationException($"Unable to load intervention. InterventionId: {request.InterventionId}");
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

        public async Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForVariation(string workflowInstanceId)
        {
            List<AssessmentToolWorkflow> assessmentToolWorkflows =
                await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowsForVariation();

            if (assessmentToolWorkflows == null || !assessmentToolWorkflows.Any())
            {
                throw new NotFoundException($"No suitable assessment tool workflows found for variation");
            }

            return assessmentToolWorkflows;
        }

        public async Task SubmitIntervention(AssessmentInterventionCommand command)
        {
            try
            {
                var intervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }

                intervention.Status = command.Status;
                intervention.DateSubmitted = _dateTimeProvider.UtcNow();
                await _assessmentRepository.UpdateAssessmentIntervention(intervention);

                if (intervention.Status == InterventionStatus.Approved)
                {
                    List<AssessmentToolWorkflowInstance> workflowsToDelete = new();
                    switch (intervention.DecisionType)
                    {
                        case InterventionDecisionTypes.Rollback:
                            {
                                workflowsToDelete =
                                    await _assessmentRepository.GetWorkflowInstancesToDeleteForRollback(
                                        intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                        intervention.TargetAssessmentToolWorkflow!.AssessmentTool.Order);
                                break;
                            }
                        case InterventionDecisionTypes.Override:
                            {
                                workflowsToDelete = await _assessmentRepository.GetSubsequentWorkflowInstancesForOverride(
                                    intervention
                                        .AssessmentToolWorkflowInstance.WorkflowInstanceId);
                                break;
                            }
                        case InterventionDecisionTypes.Amendment:
                            {
                                workflowsToDelete =
                                await _assessmentRepository.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId, 
                                intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order);
                                break;
                            }
                    }
                    foreach (var workflowInstance in workflowsToDelete)
                    {
                        workflowInstance.Status = command.FinalInstanceStatus;
                    }
                    intervention.AssessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
                    await _assessmentRepository.SaveChanges();
                    await ClearNextWorkflowsByOrder(intervention, workflowsToDelete);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException($"Unable to submit {command.DecisionType}. AssessmentInterventionId: {command.AssessmentInterventionId}.");
            }
        }

        private async Task ClearNextWorkflowsByOrder(AssessmentIntervention intervention, List<AssessmentToolWorkflowInstance> workflowsToDelete)
        {
            if (intervention.DecisionType == InterventionDecisionTypes.Amendment)
            {
                await _assessmentRepository.DeleteAllNextWorkflowsByOrder(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order);
                await _elsaServerHttpClient.PostArchiveQuestions(workflowsToDelete.Select(x => x.WorkflowInstanceId).ToArray());
            }
        }

        private async Task CreateNextWorkflow(AssessmentIntervention intervention)
        {
            if (intervention.DecisionType == InterventionDecisionTypes.Rollback)
            {
                await _assessmentRepository.DeleteAllNextWorkflows(intervention.AssessmentToolWorkflowInstance
                    .AssessmentId);
            }

            var nextWorkflow =
                AssessmentToolInstanceNextWorkflow(intervention);

            await _assessmentRepository.CreateAssessmentToolInstanceNextWorkflows(
                new List<AssessmentToolInstanceNextWorkflow>() { nextWorkflow });
        }

        private AssessmentToolInstanceNextWorkflow AssessmentToolInstanceNextWorkflow(AssessmentIntervention intervention)
        {
            return new AssessmentToolInstanceNextWorkflow
            {
                AssessmentId = intervention.AssessmentToolWorkflowInstance.AssessmentId,
                AssessmentToolWorkflowInstanceId = intervention.AssessmentToolWorkflowInstanceId,
                NextWorkflowDefinitionId = intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId,
                IsVariation = intervention.DecisionType == InterventionDecisionTypes.Variation,
                IsLast = intervention.TargetAssessmentToolWorkflow.IsLast
            };
        }
    }
}
