using Elsa.Models;
using Elsa.Services.Models;

namespace Elsa.Server.Providers
{
    public interface IWorkflowRegistryProvider
    {
        public Task<IActivityBlueprint> GetNextActivity(WorkflowInstance workflowInstance, CancellationToken cancellationToken);
    }
}
