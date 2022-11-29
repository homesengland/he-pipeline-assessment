using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;

namespace Elsa.Server.Providers
{
    public class ActivityDataProvider : IActivityDataProvider
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;

        public ActivityDataProvider(IWorkflowInstanceStore workflowInstanceStore, IWorkflowInstanceProvider workflowInstanceProvider)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _workflowInstanceProvider = workflowInstanceProvider;
        }

        public async Task<IDictionary<string, object?>> GetActivityData(string workflowInstanceId, string activityId, CancellationToken cancellationToken)
        {
            var workflowInstance = await _workflowInstanceProvider.GetWorkflowInstance(workflowInstanceId, cancellationToken);
            if (!workflowInstance.ActivityData.ContainsKey(activityId))
            {
                throw new Exception($"Cannot find activity data with id {activityId}.");
            }
            var activityDataDictionary =
                workflowInstance.ActivityData
                    .FirstOrDefault(a => a.Key == activityId).Value;
            if(activityDataDictionary == null)
            {
                throw new Exception($"Cannot find activity data with id {activityId}.");
            }
            return activityDataDictionary;
        }

    }
}
