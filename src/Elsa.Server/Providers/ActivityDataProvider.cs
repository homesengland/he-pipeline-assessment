using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;

namespace Elsa.Server.Providers
{
    public class ActivityDataProvider :IActivityDataProvider
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;

        public ActivityDataProvider(IWorkflowInstanceStore workflowInstanceStore)
        {
            _workflowInstanceStore = workflowInstanceStore;
        }

        public async Task<IDictionary<string, object?>?> GetActivityData(string workflowInstanceId, string activityId, CancellationToken cancellationToken)
        {
            var workflowSpecification = new WorkflowInstanceIdSpecification(workflowInstanceId);
            var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken: cancellationToken);
            if (workflowInstance != null)
            {
                if (workflowInstance.ActivityData.ContainsKey(activityId))
                {
                    var activityDataDictionary =
                        workflowInstance.ActivityData
                            .FirstOrDefault(a => a.Key == activityId).Value;
                    if (activityDataDictionary != null)
                    {
                        return activityDataDictionary;
                    }
                }
            }
            return null;
        }

    }
}
