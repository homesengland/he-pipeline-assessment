using Elsa.Activities.Workflows;
using Elsa.ActivityResults;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.Services.WorkflowStorage;

namespace Elsa.CustomActivities.Activities.RunEconomicCalculations
{
    public class RunEconomicCalculations : RunWorkflow
    {
        public RunEconomicCalculations(IStartsWorkflow startsWorkflow, IWorkflowRegistry workflowRegistry, IWorkflowStorageService workflowStorageService, IWorkflowReviver workflowReviver, IWorkflowInstanceStore workflowInstanceStore) : base(startsWorkflow, workflowRegistry, workflowStorageService, workflowReviver, workflowInstanceStore)
        {
        }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            if (string.IsNullOrEmpty(CorrelationId))
            {
                CorrelationId = context.CorrelationId;
            }

           

            var result = await base.OnExecuteAsync(context);

            return result;
        }
    }
}
