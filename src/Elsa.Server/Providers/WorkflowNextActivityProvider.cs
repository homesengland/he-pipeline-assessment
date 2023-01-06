using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Services.Models;

namespace Elsa.Server.Providers
{
    public interface IWorkflowNextActivityProvider
    {
        Task<IActivityBlueprint> GetNextActivity(string commandActivityId, string workflowInstanceId,
            List<QuestionScreenAnswer>? dbAssessmentQuestionList, string activityType,
            CancellationToken cancellationToken);
        Task<IActivityBlueprint> GetStartWorkflowNextActivity(IActivityBlueprint activityBlueprint,
            string workflowInstanceId,
            CancellationToken cancellationToken);
    }

    public class WorkflowNextActivityProvider : IWorkflowNextActivityProvider
    {
        private readonly IQuestionInvoker _invoker;
        private readonly IWorkflowRegistryProvider _workflowRegistryProvider;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;
        private readonly IActivityDataProvider _activityDataProvider;

        public WorkflowNextActivityProvider(
            IQuestionInvoker invoker,
            IWorkflowRegistryProvider workflowRegistryProvider,
            IWorkflowInstanceProvider workflowInstanceProvider,
            IActivityDataProvider activityDataProvider)
        {
            _invoker = invoker;
            _workflowRegistryProvider = workflowRegistryProvider;
            _workflowInstanceProvider = workflowInstanceProvider;
            _activityDataProvider = activityDataProvider;
        }

        public async Task<IActivityBlueprint> GetNextActivity(string commandActivityId, string workflowInstanceId,
            List<QuestionScreenAnswer>? dbAssessmentQuestionList, string activityType,
            CancellationToken cancellationToken)
        {
            IActivityBlueprint? nextActivity = null;
            string activityId;
            do
            {
                activityId = nextActivity != null ? nextActivity.Id : commandActivityId;
                var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(activityId,
                    activityType,
                    workflowInstanceId, dbAssessmentQuestionList, cancellationToken);

                //we need to refresh the workflowInstance, as calling the invoker will change it
                var workflowInstance =
                    await _workflowInstanceProvider.GetWorkflowInstance(workflowInstanceId,
                        cancellationToken);

                if (!collectedWorkflows.Any())
                {
                    throw new Exception(
                        $"Unable to progress workflow. Workflow status is: {workflowInstance.WorkflowStatus}");
                }

                nextActivity =
                    await _workflowRegistryProvider.GetNextActivity(workflowInstance, cancellationToken);

                dbAssessmentQuestionList = null;
                activityType = nextActivity.Type;

                if (activityType != ActivityTypeConstants.QuestionScreen)
                    break;

                var nextActivityData =
                    await _activityDataProvider.GetActivityData(workflowInstance.Id, nextActivity.Id,
                        cancellationToken);
                if (nextActivityData != null)
                {
                    bool? condition = (bool?)nextActivityData["Condition"];
                    if (condition.HasValue && condition.Value) break;
                }
            } while (activityId != nextActivity.Id);

            return nextActivity;
        }

        public async Task<IActivityBlueprint> GetStartWorkflowNextActivity(IActivityBlueprint activity,
            string workflowInstanceId,
            CancellationToken cancellationToken)
        {
            IActivityBlueprint nextActivity = activity;
            while (true)
            {
                var nextActivityData = await _activityDataProvider.GetActivityData(workflowInstanceId, nextActivity.Id, cancellationToken);
                if (nextActivityData != null)
                {
                    bool? condition = (bool?)nextActivityData["Condition"];
                    if (condition.HasValue && condition.Value)
                    {
                        break;
                    }
                }

                var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(nextActivity.Id,
                    nextActivity.Type,
                    workflowInstanceId, null, cancellationToken);

                //we need to refresh the workflowInstance, as calling the invoker will change it
                var workflowInstance =
                    await _workflowInstanceProvider.GetWorkflowInstance(workflowInstanceId,
                        cancellationToken);

                if (!collectedWorkflows.Any())
                {
                    throw new Exception($"Unable to progress workflow. Workflow status is: {workflowInstance.WorkflowStatus}");
                }

                nextActivity =
                    await _workflowRegistryProvider.GetNextActivity(workflowInstance, cancellationToken);

                if (nextActivity.Type != ActivityTypeConstants.QuestionScreen)
                    break;
            }

            return nextActivity;
        }
    }
}
