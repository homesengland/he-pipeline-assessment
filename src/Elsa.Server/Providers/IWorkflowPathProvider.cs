using Elsa.CustomModels;

namespace Elsa.Server.Providers
{
    public interface IWorkflowPathProvider
    {
        Task<CustomActivityNavigation?> GetChangedPathCustomNavigation(string workflowInstanceId,
            string currentActivityId, string nextActivityId, CancellationToken cancellationToken);
        Task<List<string>> GetPreviousPathActivities(string workflowDefinitionId, string changedPathActivityId,
            CancellationToken cancellationToken);
    }
}
