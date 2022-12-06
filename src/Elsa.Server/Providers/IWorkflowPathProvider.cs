using Elsa.CustomModels;

namespace Elsa.Server.Providers
{
    public interface IWorkflowPathProvider
    {
        Task<CustomActivityNavigation?> GetChangedPathCustomNavigation(string commandWorkflowInstanceId,
            string currentActivityId, string nextActivityId, CancellationToken cancellationToken);
        Task<List<string>> GetPreviousPathActivities(string workflowInstanceDefinitionId, string changedPathActivityId,
            CancellationToken cancellationToken);
    }
}
