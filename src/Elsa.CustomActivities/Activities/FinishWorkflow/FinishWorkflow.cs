using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.FinishWorkflow
{
    [Activity(
        Category = "Workflows",
        Description = "Sets workflow status to Finished",
        Outcomes = new string[0]
    )]
    public class FinishWorkflow : Finish
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;

        public FinishWorkflow(IWorkflowInstanceStore workflowInstanceStore)
        {
            _workflowInstanceStore = workflowInstanceStore;
        }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            //Due to needing workflows to be ammendable - this should likely let us
            //Keep the flag as being 'Finished' without wiping out the Blueprints from the
            //child class.  
            //await base.OnExecuteAsync(context);

            var workflowInstance = context.WorkflowInstance;
            workflowInstance.WorkflowStatus = WorkflowStatus.Finished;
            await _workflowInstanceStore.UpdateAsync(workflowInstance);
            return Noop();
        }
    }
}
