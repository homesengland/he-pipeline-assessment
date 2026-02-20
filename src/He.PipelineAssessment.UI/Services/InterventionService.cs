using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Integration.ServiceBusSend;
using MediatR;

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
        private readonly IServiceBusMessageSender _serviceBusMessageSender;

        public InterventionService(
            IAssessmentRepository assessmentRepository, 
            IRoleValidation roleValidation,
            IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers,
            IAssessmentInterventionMapper mapper, 
            IUserProvider userProvider, 
            ILogger<InterventionService> logger,
            IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository,
            IDateTimeProvider dateTimeProvider,
            IElsaServerHttpClient elsaServerHttpClient,
            IServiceBusMessageSender serviceBusMessageSender
            )
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
            _serviceBusMessageSender = serviceBusMessageSender;
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
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                throw;
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
                    throw new InvalidDataException(
                        $"Unable to create {command.DecisionType} for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment. WorkflowInstanceId: {command.WorkflowInstanceId}");
                }

                var assessmentIntervention = _mapper.AssessmentInterventionFromAssessmentInterventionCommand(command);
                if (command.TargetWorkflowId.HasValue)
                {
                    UpdateTargetAssessmentToolWorkflows(assessmentIntervention,
                        new List<int> { command.TargetWorkflowId.Value });
                }

                await _assessmentRepository.CreateAssessmentIntervention(assessmentIntervention);

                return assessmentIntervention.Id;
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (InvalidDataException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch(ArgumentException e)
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
                        $"Unable to create {request.DecisionType} for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment. WorkflowInstanceId: {request.WorkflowInstanceId}");
                }

                var activeInterventionsForAssessment = await _assessmentRepository.GetOpenAssessmentInterventions(workflowInstance.AssessmentId);
                if (activeInterventionsForAssessment.Any())
                {
                    throw new ApplicationException(
                        $"Unable to create request as an open request already exists for this assessment.");
                }

                var userName = _userProvider.UserName()!;
                var email = _userProvider.Email()!;
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

                var interventionReasons = await _assessmentRepository.GetInterventionReasons(request.DecisionType == InterventionDecisionTypes.Variation);

                var dto = _mapper.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, interventionReasons, dtoConfig);

                return dto;
            }
            catch(NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                throw;
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
            catch(NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                throw;
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
                #pragma warning disable 0612, 0618
                assessmentIntervention.TargetAssessmentToolWorkflowId = null;
                #pragma warning restore 0612, 0618

                assessmentIntervention.Administrator = command.Administrator;
                assessmentIntervention.AdministratorEmail = command.AdministratorEmail;
                var targetAssessmentToolWorkflows = command.TargetWorkflowDefinitions.Where(x => x.IsSelected).Select(x => x.Id).ToList();

                if (command.TargetWorkflowId.HasValue)
                {
                    targetAssessmentToolWorkflows = new List<int> { command.TargetWorkflowId.Value };
                }

                UpdateTargetAssessmentToolWorkflows(assessmentIntervention, targetAssessmentToolWorkflows); 
                await _assessmentRepository.SaveChanges();
                return assessmentIntervention.Id;
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                    throw;
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to edit {command.DecisionType}. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }
        }

        private void UpdateTargetAssessmentToolWorkflows(AssessmentIntervention assessmentIntervention, List<int> commandTargetWorkflowDefinitions)
        {
            assessmentIntervention.TargetAssessmentToolWorkflows.RemoveAll(x =>
                !commandTargetWorkflowDefinitions.Contains(x.AssessmentToolWorkflowId));

            foreach (var commandTargetWorkflowDefinitionId in commandTargetWorkflowDefinitions)
            {
                if (!assessmentIntervention.TargetAssessmentToolWorkflows.Select(x => x.AssessmentToolWorkflowId)
                    .Contains(commandTargetWorkflowDefinitionId))
                {
                    var newTargetAssessmentToolWorkflow = new TargetAssessmentToolWorkflow
                    {
                        AssessmentInterventionId = assessmentIntervention.Id,
                        AssessmentToolWorkflowId = commandTargetWorkflowDefinitionId
                    };
                    assessmentIntervention.TargetAssessmentToolWorkflows.Add(newTargetAssessmentToolWorkflow);
                }
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
            catch(NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                throw;
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

                var interventionReasons = await _assessmentRepository.GetInterventionReasons(intervention.DecisionType == InterventionDecisionTypes.Variation);

                var dto = new AssessmentInterventionDto
                {
                    AssessmentInterventionCommand = command,
                    InterventionReasons = interventionReasons
                };
                return dto;
            }
            catch(NotFoundException e)
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
            catch(NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                throw;
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
            catch(NotFoundException e)
            {
                _logger.LogError(e.Message, e);
                throw;
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
            catch(NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                throw;
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
                                    intervention.TargetAssessmentToolWorkflows.First().AssessmentToolWorkflow.AssessmentTool.Order);
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
                                intervention.AssessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
                                break;
                            }
                        case InterventionDecisionTypes.Variation:
                            break;
                        default:
                            {
                                throw new Exception($"No Decision Type set for intervention: {intervention.Id}");
                                
                            }
                    }

                    foreach (var workflowInstance in workflowsToDelete)
                    {
                        workflowInstance.Status = command.FinalInstanceStatus;
                    }

                    await _assessmentRepository.SaveChanges();
                    await ClearNextWorkflowsByOrder(intervention, workflowsToDelete);
                    await CreateNextWorkflows(intervention);

                    foreach (var workflowInstance in workflowsToDelete)
                    {
                        this._serviceBusMessageSender.SendMessage(workflowInstance);
                    }
                }
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
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

            if (intervention.DecisionType == InterventionDecisionTypes.Rollback)
            {
                await _assessmentRepository.DeleteAllNextWorkflows(intervention.AssessmentToolWorkflowInstance.AssessmentId);
                await _elsaServerHttpClient.PostArchiveQuestions(workflowsToDelete.Select(x => x.WorkflowInstanceId).ToArray());
            }
        }

        private async Task CreateNextWorkflows(AssessmentIntervention intervention)
        {
            var nextWorkflows = new List<AssessmentToolInstanceNextWorkflow>();

            foreach (var targetAssessmentToolWorkflow in intervention.TargetAssessmentToolWorkflows)
            {
                var nextWorkflow =
                    AssessmentToolInstanceNextWorkflow(intervention, targetAssessmentToolWorkflow);
                nextWorkflows.Add(nextWorkflow);
            }

            await _assessmentRepository.CreateAssessmentToolInstanceNextWorkflows(nextWorkflows);
        }

        private AssessmentToolInstanceNextWorkflow AssessmentToolInstanceNextWorkflow(
            AssessmentIntervention intervention, TargetAssessmentToolWorkflow targetAssessmentToolWorkflow)
        {
            return new AssessmentToolInstanceNextWorkflow
            {
                AssessmentId = intervention.AssessmentToolWorkflowInstance.AssessmentId,
                AssessmentToolWorkflowInstanceId = intervention.AssessmentToolWorkflowInstanceId,
                NextWorkflowDefinitionId = targetAssessmentToolWorkflow.AssessmentToolWorkflow.WorkflowDefinitionId,
                IsVariation = intervention.DecisionType == InterventionDecisionTypes.Variation,
                IsLast = targetAssessmentToolWorkflow.AssessmentToolWorkflow.IsLast
            };
        }
    }
}
