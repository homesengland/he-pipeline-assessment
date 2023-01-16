using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.ConfirmationScreen
{
    [Action(
        Category = "Homes England Activities",
        Description = "Set up a confirmation screen",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class ConfirmationScreen : Activity
    {
        [ActivityInput(Hint = "Confirmation title")]
        public string ConfirmationTitle { get; set; } = null!;
        [ActivityInput(Hint = "Confirmation Text")]
        public string ConfirmationText { get; set; } = null!;
        [ActivityInput(Hint = "Footer title")]
        public string FooterTitle { get; set; } = null!;
        [ActivityInput(Hint = "Footer text")]
        public string FooterText { get; set; } = null!;
        [ActivityInput(Hint = "Next workflow to run")]
        public string NextWorkflowDefinitionIds { get; set; } = null!;
        [ActivityOutput] public string Output { get; set; } = null!;

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(context.WorkflowInstance.DefinitionId), context.WorkflowInstance.DefinitionId);

            return await Task.FromResult(Suspend());
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            return await Task.FromResult(Done());
        }
    }
}