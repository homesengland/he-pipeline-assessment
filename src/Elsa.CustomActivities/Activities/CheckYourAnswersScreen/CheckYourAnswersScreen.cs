using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.CheckYourAnswersScreen
{
    [Action(
    Category = "Homes England Activities",
    Description = "Set up a check your answers screen",
    Outcomes = new[] { OutcomeNames.Done }
    )]
    public class CheckYourAnswersScreen : Activity
    {
        [ActivityInput(Hint = "Section title")]
        public string Title { get; set; } = null!;
        [ActivityInput(Hint = "Footer title")]
        public string FooterTitle { get; set; } = null!;
        [ActivityInput(Hint = "Footer text")]
        public string FooterText { get; set; } = null!;


        [ActivityInput(Label = "Assessment outcome conditions", Hint = "The conditions to evaluate.", UIHint = "switch-case-builder", DefaultSyntax = "Switch", IsDesignerCritical = true)]
        public ICollection<SwitchCase> Cases { get; set; } = new List<SwitchCase>();

        [ActivityInput(
            Hint = "The switch mode determines whether the first match should be scheduled, or all matches.",
            DefaultValue = SwitchMode.MatchFirst,
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript }
        )]
        public SwitchMode Mode { get; set; } = SwitchMode.MatchFirst;

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(context.WorkflowInstance.DefinitionId), context.WorkflowInstance.DefinitionId);

            return await Task.FromResult(Suspend());
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            var matches = Cases.Where(x => x.Condition).Select(x => x.Name).ToList();
            var hasAnyMatches = matches.Any();
            var results = Mode == SwitchMode.MatchFirst ? hasAnyMatches ? new[] { matches.First() } : Array.Empty<string>() : matches.ToArray();
            var outcomes = hasAnyMatches ? results : new[] { OutcomeNames.Default };
            context.JournalData.Add("Matches", matches);

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes(outcomes),
                new SuspendResult()
            }));
        }
    }
}
