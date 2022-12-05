using Elsa.Models;

namespace Elsa.Server.Providers
{
    public interface IWorkflowInstanceProvider
    {
        public Task<WorkflowInstance> GetWorkflowInstance(string workflowInstanceId, CancellationToken cancellationToken);
    }
}
