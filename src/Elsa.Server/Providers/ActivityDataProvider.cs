using Elsa.Models;

namespace Elsa.Server.Providers
{
    public interface IActivityDataProvider
    {
        public Task<IDictionary<string, object?>> GetActivityData(string workflowInstanceId, string activityId, CancellationToken cancellationToken);
        public IDictionary<string, object?> GetActivityData(WorkflowInstance workflowInstance, string activityId);
    }

    public class ActivityDataProvider : IActivityDataProvider
    {
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;

        public ActivityDataProvider(IWorkflowInstanceProvider workflowInstanceProvider)
        {
            _workflowInstanceProvider = workflowInstanceProvider;
        }

        public async Task<IDictionary<string, object?>> GetActivityData(string workflowInstanceId, string activityId, CancellationToken cancellationToken)
        {
            var workflowInstance = await _workflowInstanceProvider.GetWorkflowInstance(workflowInstanceId, cancellationToken);
            return GetActivityData(workflowInstance, activityId);
        }

        public IDictionary<string, object?> GetActivityData(WorkflowInstance workflowInstance, string activityId)
        {
            if (!workflowInstance.ActivityData.ContainsKey(activityId))
            {
                throw new Exception($"Cannot find activity data with id {activityId}.");
            }
            var activityDataDictionary =
                workflowInstance.ActivityData
                    .FirstOrDefault(a => a.Key == activityId).Value;
            return activityDataDictionary;
        }
    }
}
