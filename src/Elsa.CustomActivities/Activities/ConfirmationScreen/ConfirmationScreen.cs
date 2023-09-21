using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Design;
using Elsa.Expressions;
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
        [ActivityInput(Hint = "Confirmation Title", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string ConfirmationTitle { get; set; } = null!;
        [ActivityInput(Hint = "Confirmation Text", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string ConfirmationText { get; set; } = null!;
        [ActivityInput(Hint = "Footer title", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string FooterTitle { get; set; } = null!;
        [ActivityInput(Hint = "Footer text", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string FooterText { get; set; } = null!;

        [HeActivityInput(Label = "Assessment Conditional Text",
            Hint = "Text to display on Outcome Screen.",
            UIHint = CustomActivityUIHints.TextGroupProperty,
            SupportedSyntaxes = new[] { SyntaxNames.Json, TextActivitySyntaxNames.TextGroup },
            DefaultSyntax = TextActivitySyntaxNames.TextGroup,
            IsDesignerCritical = true)]
        public GroupedTextModel Text { get; set; } = new GroupedTextModel();

        [ActivityInput(Hint = "Next workflow to run. Comma separate for multiple workflows.", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
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