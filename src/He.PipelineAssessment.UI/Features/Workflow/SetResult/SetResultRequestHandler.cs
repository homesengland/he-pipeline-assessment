using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.SetResult;
using MediatR;
using System.Text.Json;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen
{
    public class SetResultRequestHandler : IRequestHandler<SetResultRequest, QuestionScreenSaveAndContinueCommandResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;

        public SetResultRequestHandler(IElsaServerHttpClient elsaServerHttpClient, IAssessmentRepository assessmentRepository)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
        }

        public async Task<QuestionScreenSaveAndContinueCommandResponse?> Handle(SetResultRequest request, CancellationToken cancellationToken)
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
                SetResultResponse? result = JsonSerializer.Deserialize<SetResultResponse>(jsonResponse);

                //var currentAssessmentToolWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(response.Data.WorkflowInstanceId);
                //if (currentAssessmentToolWorkflowInstance != null && result != null)
                //{
                //    var data = response.Data.ConfirmationTitle;
                //    currentAssessmentToolWorkflowInstance.Result = data;
                //    await _assessmentRepository.SaveChanges();


                //    if (!string.IsNullOrEmpty(response.Data.NextWorkflowDefinitionIds))
                //    {
                //        var nextWorkflows = new List<AssessmentToolInstanceNextWorkflow>();
                //        var workflowDefinitionIds = response.Data.NextWorkflowDefinitionIds.Split(',', StringSplitOptions.TrimEntries);
                //        foreach (var workflowDefinitionId in workflowDefinitionIds)
                //        {
                //            var nextWorkflow =
                //                await _assessmentRepository.GetAssessmentToolInstanceNextWorkflow(currentAssessmentToolWorkflowInstance.Id,
                //                    workflowDefinitionId);

                //            if (nextWorkflow == null)
                //            {
                //                var assessmentToolInstanceNextWorkflow =
                //                    AssessmentToolInstanceNextWorkflow(currentAssessmentToolWorkflowInstance.AssessmentId,
                //                        currentAssessmentToolWorkflowInstance.Id, workflowDefinitionId);
                //                nextWorkflows.Add(assessmentToolInstanceNextWorkflow);
                //            }
                //        }

                //        if (nextWorkflows.Any())
                //            await _assessmentRepository.CreateAssessmentToolInstanceNextWorkflows(nextWorkflows);
                //    }
                //    result.CorrelationId = currentAssessmentToolWorkflowInstance.Assessment.SpId.ToString();
                //    result.AssessmentId = currentAssessmentToolWorkflowInstance.AssessmentId;
                    QuestionScreenSaveAndContinueCommandResponse nextActivityResponse = new QuestionScreenSaveAndContinueCommandResponse();
                nextActivityResponse.ActivityId = result.Data.ActivityId;
                nextActivityResponse.ActivityType = result.Data.ActivityType;
                nextActivityResponse.WorkflowInstanceId = result.Data.WorkflowInstanceId;
                    return await Task.FromResult(nextActivityResponse);
                }
            return null;
            }
        }

        //private AssessmentToolInstanceNextWorkflow AssessmentToolInstanceNextWorkflow(int assessmentId, int assessmentToolWorkflowInstanceId, string workflowDefinitionId)
        //{
        //    return new AssessmentToolInstanceNextWorkflow
        //    {
        //        AssessmentId = assessmentId,
        //        AssessmentToolWorkflowInstanceId = assessmentToolWorkflowInstanceId,
        //        NextWorkflowDefinitionId = workflowDefinitionId,
        //        IsStarted = false
        //    };
        //}
    }
