using Elsa.Activities.Workflows;
using Elsa.ActivityResults;
using Elsa.Persistence;
using Elsa.Services.Models;
using Elsa.Services.WorkflowStorage;
using Elsa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elsa.Attributes;

namespace Elsa.CustomActivities.Activities.ReturnToStartOfWorkflow
{
    [Action(
    Category = "Homes England Activities",
    Description = "Set up return to start of workflow activity",
    Outcomes = new[] { OutcomeNames.Done }
)]
    public class ReturnToStartOfWorkflow : Activity
    {
        [ActivityOutput] public string Output { get; set; } = null!;
        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(context.WorkflowInstance.DefinitionId), context.WorkflowInstance.DefinitionId);
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            return await Task.FromResult(Done());
        }

    }
}
