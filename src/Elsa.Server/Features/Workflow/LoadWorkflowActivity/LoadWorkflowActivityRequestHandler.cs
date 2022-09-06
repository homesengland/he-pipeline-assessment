using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Services.Models;
using MediatR;
using Constants = Elsa.CustomActivities.Activities.Constants;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandler : IRequestHandler<LoadWorkflowActivityRequest, OperationResult<LoadWorkflowActivityResponse>>
    {
        private readonly IQuestionInvoker _QuestionInvoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly ILoadWorkflowActivityJsonHelper _loadWorkflowActivityJsonHelper;

        public LoadWorkflowActivityRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository, IQuestionInvoker questionInvoker, ILoadWorkflowActivityJsonHelper loadWorkflowActivityJsonHelper)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _loadWorkflowActivityJsonHelper = loadWorkflowActivityJsonHelper;
            _QuestionInvoker = questionInvoker;
        }

        public async Task<OperationResult<LoadWorkflowActivityResponse>> Handle(LoadWorkflowActivityRequest activityRequest, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadWorkflowActivityResponse>
            {
                Data = new LoadWorkflowActivityResponse
                {
                    WorkflowInstanceId = activityRequest.WorkflowInstanceId,
                    ActivityId = activityRequest.ActivityId
                }
            };
            try
            {
                var dbAssessmentQuestion =
                    await _pipelineAssessmentRepository.GetAssessmentQuestion(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);

                if(dbAssessmentQuestion != null)
                {
                    IEnumerable<CollectedWorkflow> workflows = await _QuestionInvoker.FindWorkflowsAsync(activityRequest.ActivityId, dbAssessmentQuestion.ActivityType, activityRequest.WorkflowInstanceId, cancellationToken);

                    var collectedWorkflow = workflows.FirstOrDefault();
                    if (collectedWorkflow != null)
                    {
                        var workflowSpecification =
                            new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
                        var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken: cancellationToken);
                        if (workflowInstance != null)
                        {

                            if (!workflowInstance.ActivityData.ContainsKey(activityRequest.ActivityId))
                            {
                                result.ErrorMessages.Add(
                                    $"Cannot find activity Id {activityRequest.ActivityId} in the workflow activity data dictionary");
                            }
                            else
                            {
                                var activityDataDictionary =
                                    workflowInstance.ActivityData.FirstOrDefault(a => a.Key == activityRequest.ActivityId).Value;

                                result.Data.ActivityType = dbAssessmentQuestion.ActivityType;
                                result.Data.PreviousActivityId = dbAssessmentQuestion.PreviousActivityId;

                                AssignActivityData(dbAssessmentQuestion, activityDataDictionary, result);
                            }
                        }
                        else
                        {
                            result.ErrorMessages.Add(
                                $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} in Elsa database");
                        }
                    }
                    else
                    {
                        result.ErrorMessages.Add(
                            $"Unable to progress workflow instance Id {activityRequest.WorkflowInstanceId}. No collected workflows");
                    }
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId}");
                }
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private void AssignActivityData(AssessmentQuestion dbAssessmentQuestion, IDictionary<string, object?> activityDataDictionary,
            OperationResult<LoadWorkflowActivityResponse> result)
        {
            var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(activityDataDictionary);
            if (activityData != null)
            {
                result.Data!.QuestionActivityData = activityData;
                result.Data.QuestionActivityData.ActivityType = dbAssessmentQuestion.ActivityType;
                result.Data.QuestionActivityData.Answer = dbAssessmentQuestion.Answer;
            }
           
        }
    }
}
