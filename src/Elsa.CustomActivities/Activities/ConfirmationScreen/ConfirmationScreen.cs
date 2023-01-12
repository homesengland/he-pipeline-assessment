using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.Attributes;
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
        [ActivityInput(Hint = "Confirmation title")]
        public string ConfirmationTitle { get; set; } = null!;
        [ActivityInput(Hint = "Confirmation Text")]
        public string ConfirmationText { get; set; } = null!;
        [ActivityInput(Hint = "Footer title")]
        public string FooterTitle { get; set; } = null!;
        [ActivityInput(Hint = "Footer text")]
        public string FooterText { get; set; } = null!;

        //[ActivityInput(Hint = "The conditions to evaluate.", UIHint = "switch-case-builder", DefaultSyntax = SyntaxNames.JavaScript, IsDesignerCritical = true)]
        //public ICollection<SwitchCase> Test { get; set; } = null!;


        [ActivityInput(Hint = "The conditions to evaluate.", UIHint = ActivityInputUIHints.Dictionary, SupportedSyntaxes = new[] { SyntaxNames.Json, SyntaxNames.JavaScript }, DefaultSyntax = SyntaxNames.JavaScript, IsDesignerCritical = true)]
        public Dictionary<string, string> TestDict { get; set; } = null!;

        //[ActivityInput(Hint = "The conditions to evaluate.", UIHint = ActivityInputUIHints.MultiText, SupportedSyntaxes = new[] { SyntaxNames.Json, SyntaxNames.JavaScript }, DefaultSyntax = SyntaxNames.JavaScript, IsDesignerCritical = true)]
        //public IList<string> TestStrings { get; set; } = new List<string>();


        //[ActivityInput(Label = "Blocks of Outcome Text", Hint = "The Outcome to display to the end user.", UIHint = "outcome-builder", DefaultSyntax = SyntaxNames.Json, IsDesignerCritical = true)]
        //public ICollection<ConditionalText> OutcomeText { get; set; } = null!;

        [ActivityInput(Hint = "Next workflow to run")]
        public string NextWorkflowDefinitionId { get; set; } = null!;
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

        class Foo
        {
            readonly string _value;
            public Foo(string value)
            {
                this._value = value;
            }
            public static implicit operator string(Foo d)
            {
                return d._value;
            }
            public static implicit operator Foo(string d)
            {
                return new Foo(d);
            }
        }

        public class ConditionalText
        {
            [ActivityInput(Hint = "The condition to evaluate.", UIHint = ActivityInputUIHints.SingleLine, SupportedSyntaxes = new[] { SyntaxNames.JavaScript }, DefaultSyntax = SyntaxNames.JavaScript)]
            public bool Condition { get; set; }
            [ActivityInput(Hint = "The text to display.", UIHint = ActivityInputUIHints.SingleLine, SupportedSyntaxes = new[] { SyntaxNames.JavaScript }, DefaultSyntax = SyntaxNames.JavaScript)]
            public string Text { get; set; } = null!;
        }
    }
}