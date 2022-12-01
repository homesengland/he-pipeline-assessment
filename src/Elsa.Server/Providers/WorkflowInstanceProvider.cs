using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Models;

namespace Elsa.Server.Providers
{
    public class WorkflowInstanceProvider : IWorkflowInstanceProvider
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;

        public WorkflowInstanceProvider(IWorkflowInstanceStore workflowInstanceStore)
        {
            _workflowInstanceStore = workflowInstanceStore;
        }
        public async Task<WorkflowInstance> GetWorkflowInstance(string workflowInstanceId, CancellationToken cancellationToken)
        {
            var workflowSpecification =
                    new WorkflowInstanceIdSpecification(workflowInstanceId);
            var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken);
            if(workflowInstance == null)
            {
                throw new Exception($"Cannot find workflow for workflowId {workflowInstanceId}.");
            }
            return workflowInstance;
        }

    }
}
