using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;
using System.Text.Json;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Helper;

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

        public LoadConfirmationScreenRequestHandler(IElsaServerHttpClient elsaServerHttpClient, IAssessmentRepository assessmentRepository, IUserProvider userProvider, IDateTimeProvider dateTimeProvider, IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers, ILogger<LoadConfirmationScreenRequestHandler> logger)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _dateTimeProvider = dateTimeProvider;
            _assessmentToolWorkflowInstanceHelpers = assessmentToolWorkflowInstanceHelpers;
            _logger = logger;
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
                        if (currentAssessmentToolWorkflowInstance.Status == AssessmentToolWorkflowInstanceConstants.Draft)
                        {
                            var data = response.Data.ConfirmationTitle;
                            currentAssessmentToolWorkflowInstance.Result = data;
                            var submittedTime = _dateTimeProvider.UtcNow();
                            currentAssessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Submitted;
                            currentAssessmentToolWorkflowInstance.SubmittedDateTime = submittedTime;
                            currentAssessmentToolWorkflowInstance.SubmittedBy = _userProvider.GetUserName();
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

                                    if (nextWorkflow == null)
                                    {
                                        var assessmentToolInstanceNextWorkflow =
                                            AssessmentToolInstanceNextWorkflow(currentAssessmentToolWorkflowInstance.AssessmentId,
                                                currentAssessmentToolWorkflowInstance.Id, workflowDefinitionId);
                                        nextWorkflows.Add(assessmentToolInstanceNextWorkflow);
                                    }
                                }

                                if (nextWorkflows.Any())
                                    await _assessmentRepository.CreateAssessmentToolInstanceNextWorkflows(nextWorkflows);
                            }
                        }
                        result.CorrelationId = currentAssessmentToolWorkflowInstance.Assessment.SpId;
                        result.AssessmentId = currentAssessmentToolWorkflowInstance.AssessmentId;
                        result.IsLatestSubmittedWorkflow = _assessmentToolWorkflowInstanceHelpers.IsLatestSubmittedWorkflow(currentAssessmentToolWorkflowInstance);
                        result.IsVariationAllowed = await _assessmentToolWorkflowInstanceHelpers.IsVariationAllowed(currentAssessmentToolWorkflowInstance);
                        PageHeaderHelper.PopulatePageHeaderInformation(result, currentAssessmentToolWorkflowInstance);
                        return await Task.FromResult(result);
                    }

                }
                _logger.LogError($"Failed to load Confirmation Screen, response from elsa server client is null. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}");
                throw new ApplicationException("Failed to load Confirmation Screen activity.");
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                _logger.LogError($"Failed to load Confirmation Screen. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}");
                throw new ApplicationException("Failed to load Confirmation Screen activity.");
            }
        }

        private AssessmentToolInstanceNextWorkflow AssessmentToolInstanceNextWorkflow(int assessmentId, int assessmentToolWorkflowInstanceId, string workflowDefinitionId)
        {
            return new AssessmentToolInstanceNextWorkflow
            {
                AssessmentId = assessmentId,
                AssessmentToolWorkflowInstanceId = assessmentToolWorkflowInstanceId,
                NextWorkflowDefinitionId = workflowDefinitionId
            };
        }
    }
}
