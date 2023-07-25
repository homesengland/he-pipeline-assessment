using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Models;
using Elsa.Services.Models;

namespace Elsa.Server.Providers
{
    public interface IWorkflowNextActivityProvider
    {
        Task<WorkflowNextActivityModel> GetNextActivity(string commandActivityId, string workflowInstanceId,
            List<Question>? dbAssessmentQuestionList, string activityType,
            CancellationToken cancellationToken);
        Task<WorkflowNextActivityModel> GetStartWorkflowNextActivity(IActivityBlueprint activityBlueprint,
            string workflowInstanceId,
            CancellationToken cancellationToken);
    }

    public class WorkflowNextActivityProvider : IWorkflowNextActivityProvider
    {
        private readonly IQuestionInvoker _invoker;
        private readonly IWorkflowRegistryProvider _workflowRegistryProvider;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;
        private readonly IActivityDataProvider _activityDataProvider;
        private readonly ILogger<WorkflowNextActivityProvider> _logger;

        public WorkflowNextActivityProvider(
            IQuestionInvoker invoker,
            IWorkflowRegistryProvider workflowRegistryProvider,
            IWorkflowInstanceProvider workflowInstanceProvider,
            IActivityDataProvider activityDataProvider,
            ILogger<WorkflowNextActivityProvider> logger)
        {
            _invoker = invoker;
            _workflowRegistryProvider = workflowRegistryProvider;
            _workflowInstanceProvider = workflowInstanceProvider;
            _activityDataProvider = activityDataProvider;
            _logger = logger;
        }

        public async Task<WorkflowNextActivityModel> GetNextActivity(string commandActivityId, string workflowInstanceId,
            List<Question>? dbAssessmentQuestionList, string activityType,
            CancellationToken cancellationToken)
        {
            IActivityBlueprint? nextActivity = null;
            WorkflowInstance? workflowInstance = null;
            string activityId;
            do
            {
                activityId = nextActivity != null ? nextActivity.Id : commandActivityId;
                var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(activityId,
                    activityType,
                    workflowInstanceId, dbAssessmentQuestionList, cancellationToken);

                //we need to refresh the workflowInstance, as calling the invoker will change it
                workflowInstance =
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

            if (activityId == nextActivity.Id)
            {
                _logger.LogWarning("Cannot find next activity. Please check workflow definition setup.");
            }

            return new WorkflowNextActivityModel
            {
                NextActivity = nextActivity,
                WorkflowInstance = workflowInstance
            };
        }

        public async Task<WorkflowNextActivityModel> GetStartWorkflowNextActivity(IActivityBlueprint activity,
            string workflowInstanceId,
            CancellationToken cancellationToken)
        {
            IActivityBlueprint nextActivity = activity;
            WorkflowInstance? workflowInstance = null;
            string nextActivityId;
            do
            {
                nextActivityId = nextActivity.Id;
                var nextActivityData =
                    await _activityDataProvider.GetActivityData(workflowInstanceId, nextActivityId, cancellationToken);
                if (nextActivityData != null && nextActivityData.HasKey("Condition"))
                {
                    bool? condition = (bool?)nextActivityData["Condition"];
                    if (condition.HasValue && condition.Value)
                    {
                        break;
                    }
                }

                var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(nextActivityId,
                    nextActivity.Type,
                    workflowInstanceId, null, cancellationToken);

                //we need to refresh the workflowInstance, as calling the invoker will change it
                workflowInstance =
                    await _workflowInstanceProvider.GetWorkflowInstance(workflowInstanceId,
                        cancellationToken);

                if (!collectedWorkflows.Any())
                {
                    throw new Exception(
                        $"Unable to progress workflow. Workflow status is: {workflowInstance.WorkflowStatus}");
                }

                nextActivity =
                    await _workflowRegistryProvider.GetNextActivity(workflowInstance, cancellationToken);

            } while (nextActivity.Type == ActivityTypeConstants.QuestionScreen && nextActivityId != nextActivity.Id);

            return new WorkflowNextActivityModel
            {
                NextActivity = nextActivity,
                WorkflowInstance = workflowInstance
            };
        }
    }
}
