using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomModels;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.MultipleQuestionActivity
{
    public class MultipleQuestionActivity : Activity
    {
        [ActivityInput(Hint = "Page title")]
        public string PageTitle { get; set; } = null!;

        [ActivityInput(Label = "List of questions", Hint = "Questions to be displayed on this page.", UIHint = "question-builder", DefaultSyntax = "Json", IsDesignerCritical = true)]
        public List<Question>? Questions { get; set; }

        [ActivityInput(Label = "Assessment outcome conditions", Hint = "The conditions to evaluate.", UIHint = "switch-case-builder", DefaultSyntax = "Switch", IsDesignerCritical = true)]
        public ICollection<SwitchCase> Cases { get; set; } = new List<SwitchCase>();

        [ActivityInput(
            Hint = "The switch mode determines whether the first match should be scheduled, or all matches.",
            DefaultValue = SwitchMode.MatchFirst,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public SwitchMode Mode { get; set; } = SwitchMode.MatchFirst;

        [ActivityOutput] public List<AssessmentQuestion>? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            var response = context.GetInput<List<AssessmentQuestion>>();
            Output = response;
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
