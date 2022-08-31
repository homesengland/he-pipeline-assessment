using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomModels;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.Currency
{

    [Trigger(
       Category = "Homes England Activities",
       Description = "Assessment screen currency question",
       Outcomes = new[] { OutcomeNames.Done }
   )]
    public class CurrencyQuestion : Activity
    {

        [ActivityInput(Hint = "Section title")]
        public string Title { get; set; } = null!;

        #region Input

        [ActivityInput(
            Hint = "Question to ask",
            UIHint = ActivityInputUIHints.SingleLine,
            DefaultSyntax = SyntaxNames.Literal,
            SupportedSyntaxes = new[] { SyntaxNames.Literal })]
        public string Question { get; set; } = null!;
        public DateTime LastUpdated => DateTime.Now;


        //[ActivityInput(
        //    Hint = "Answer",
        //    UIHint = ActivityInputUIHints.SingleLine,
        //    DefaultSyntax = SyntaxNames.Literal,

        //    SupportedSyntaxes = new[] { SyntaxNames.Literal })]
        //public string Answer { get; set; } = null!;

        #endregion


        #region Switch
        [ActivityInput(Label = "Assessment outcome conditions", Hint = "The conditions to evaluate.", UIHint = "switch-case-builder", DefaultSyntax = "Switch", IsDesignerCritical = true)]
        public ICollection<SwitchCase> Cases { get; set; } = new List<SwitchCase>();

        [ActivityInput(
            Hint = "The switch mode determines whether the first match should be scheduled, or all matches.",
            DefaultValue = SwitchMode.MatchFirst,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public SwitchMode Mode { get; set; } = SwitchMode.MatchFirst;

        #endregion

        #region Output
        [ActivityOutput] public MultipleChoiceQuestionModel? Output { get; set; }

        #endregion


        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            var response = context.GetInput<MultipleChoiceQuestionModel>();
            Output = response;
            var matches = Cases.Where(x => x.Condition).Select(x => x.Name).ToList();
            var hasAnyMatches = matches.Any();
            var results = Mode == SwitchMode.MatchFirst ? hasAnyMatches ? new[] { matches.First() } : Array.Empty<string>() : matches.ToArray();
            var outcomes = hasAnyMatches ? results : new[] { OutcomeNames.Default };
            context.JournalData.Add("Matches", matches);

            if (response != null && response.FinishWorkflow.HasValue && response.FinishWorkflow.Value)
            {
                return new CombinedResult(new List<IActivityExecutionResult>
                {
                    Outcomes(outcomes),
                    new DoneResult()
                });
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes(outcomes),
                new SuspendResult()
            }));

        }

    }
}

