using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;
using System.Text.Json;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Helper;
using He.PipelineAssessment.UI.Integration.ServiceBusSend;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequestHandler : IRequestHandler<LoadConfirmationScreenRequest, LoadConfirmationScreenResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUserProvider _userProvider;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAssessmentToolWorkflowInstanceHelpers _assessmentToolWorkflowInstanceHelpers;
        private readonly ILogger<LoadConfirmationScreenRequestHandler> _logger;
        private readonly IRoleValidation _roleValidation;

        private readonly IServiceBusMessageSender _serviceBusMessageSender;

        public LoadConfirmationScreenRequestHandler(
            IElsaServerHttpClient elsaServerHttpClient, 
            IAssessmentRepository assessmentRepository, 
            IUserProvider userProvider, 
            IDateTimeProvider dateTimeProvider, 
            IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers, 
            ILogger<LoadConfirmationScreenRequestHandler> logger, 
            IRoleValidation roleValidation,
            IServiceBusMessageSender serviceBusMessageSender)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _dateTimeProvider = dateTimeProvider;
            _assessmentToolWorkflowInstanceHelpers = assessmentToolWorkflowInstanceHelpers;
            _logger = logger;
            _roleValidation = roleValidation;
            _serviceBusMessageSender = serviceBusMessageSender;
        }

        public async Task<LoadConfirmationScreenResponse?> Handle(LoadConfirmationScreenRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var response = await _elsaServerHttpClient.LoadConfirmationScreen(new LoadWorkflowActivityDto
                {
                    WorkflowInstanceId = request.WorkflowInstanceId,
                    ActivityId = request.ActivityId,
                    ActivityType = ActivityTypeConstants.ConfirmationScreen
                });

                if (response != null)
                {
                    string jsonResponse = JsonSerializer.Serialize(response);
                    LoadConfirmationScreenResponse? result = JsonSerializer.Deserialize<LoadConfirmationScreenResponse>(jsonResponse);

                    var currentAssessmentToolWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(response.Data.WorkflowInstanceId);
                    if (currentAssessmentToolWorkflowInstance != null && result != null)
                    {
                        var validateSensitiveStatus =
                            _roleValidation.CanViewAssessment(currentAssessmentToolWorkflowInstance.Assessment);

                        if (!validateSensitiveStatus && currentAssessmentToolWorkflowInstance.Assessment.IsSensitiveRecord())
                        {
                            validateSensitiveStatus = await _roleValidation.IsUserWhitelistedForSensitiveRecord(currentAssessmentToolWorkflowInstance.AssessmentId);
                        }

                        if (!validateSensitiveStatus)
                        {
                            throw new UnauthorizedAccessException("You do not have permission to access this resource.");
                        }
                        var isRoleExist = await _roleValidation.ValidateRole(currentAssessmentToolWorkflowInstance!.AssessmentId,
                            currentAssessmentToolWorkflowInstance!.WorkflowDefinitionId);

                        if (!isRoleExist)
                        {
                            return new LoadConfirmationScreenResponse
                            {
                                IsAuthorised = false
                            };
                        }

                        if (currentAssessmentToolWorkflowInstance.Status == AssessmentToolWorkflowInstanceConstants.Draft)
                        {
                            var data = response.Data.ConfirmationTitle;
                            currentAssessmentToolWorkflowInstance.Result = data;
                            var submittedTime = _dateTimeProvider.UtcNow();
                            currentAssessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Submitted;
                            currentAssessmentToolWorkflowInstance.SubmittedDateTime = submittedTime;
                            currentAssessmentToolWorkflowInstance.SubmittedBy = _userProvider.UserName();
                            await _assessmentRepository.SaveChanges();


                            if (!string.IsNullOrEmpty(response.Data.NextWorkflowDefinitionIds) && !currentAssessmentToolWorkflowInstance.IsVariation)
                            { 
                                var nextWorkflows = new List<AssessmentToolInstanceNextWorkflow>();
                                var workflowDefinitionIds = response.Data.NextWorkflowDefinitionIds.Split(',', StringSplitOptions.TrimEntries);
                                foreach (var workflowDefinitionId in workflowDefinitionIds)
                                {
                                    var nextWorkflow =
                                        await _assessmentRepository.GetAssessmentToolInstanceNextWorkflow(currentAssessmentToolWorkflowInstance.Id,
                                            workflowDefinitionId);
                                    var allAssessmentToolWorkflowInstances = await _assessmentRepository.GetAssessmentToolWorkflowInstances(currentAssessmentToolWorkflowInstance.AssessmentId);
                                    var existingAssessmentToolWorkflowInstances = allAssessmentToolWorkflowInstances.Where(x => x.WorkflowDefinitionId == workflowDefinitionId);
     

                                    if (nextWorkflow == null && !existingAssessmentToolWorkflowInstances.Any())
                                    {
                                        var assessmentToolInstanceNextWorkflow =
                                            AssessmentToolInstanceNextWorkflow(currentAssessmentToolWorkflowInstance, workflowDefinitionId);
                                        nextWorkflows.Add(assessmentToolInstanceNextWorkflow);
                                    }
                                }

                                if (nextWorkflows.Any())
                                    await _assessmentRepository.CreateAssessmentToolInstanceNextWorkflows(nextWorkflows);
                            }

                            _serviceBusMessageSender.SendMessage(currentAssessmentToolWorkflowInstance);
                        }
                        result.CorrelationId = currentAssessmentToolWorkflowInstance.Assessment.SpId;
                        result.AssessmentId = currentAssessmentToolWorkflowInstance.AssessmentId;
                        if (currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow != null)
                        {
                            result.IsVariationAllowed = await _assessmentToolWorkflowInstanceHelpers.IsVariationAllowed(currentAssessmentToolWorkflowInstance);
                            result.IsLatestSubmittedWorkflow = await _assessmentToolWorkflowInstanceHelpers.IsOrderEqualToLatestSubmittedWorkflowOrder(currentAssessmentToolWorkflowInstance);
                            result.IsAmendableWorkflow = currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsAmendable;

                            
                        }
                        PageHeaderHelper.PopulatePageHeaderInformation(result, currentAssessmentToolWorkflowInstance);
                        return await Task.FromResult(result);
                    }

                }
                _logger.LogError($"Failed to load Confirmation Screen, response from elsa server client is null. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}");
                throw new ApplicationException("Failed to load Confirmation Screen activity.");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                _logger.LogError($"Failed to load Confirmation Screen. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}");
                throw new ApplicationException("Failed to load Confirmation Screen activity.");
            }
        }

        private AssessmentToolInstanceNextWorkflow AssessmentToolInstanceNextWorkflow(AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance, string workflowDefinitionId)
        {
            return new AssessmentToolInstanceNextWorkflow
            {
                AssessmentId = currentAssessmentToolWorkflowInstance.AssessmentId,
                AssessmentToolWorkflowInstanceId = currentAssessmentToolWorkflowInstance.Id,
                NextWorkflowDefinitionId = workflowDefinitionId,
                IsLast = currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsLast
            };
        }
    }
}
