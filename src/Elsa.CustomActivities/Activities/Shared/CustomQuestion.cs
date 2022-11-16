//using Elsa.Activities.ControlFlow;
//using Elsa.ActivityResults;
//using Elsa.Attributes;
//using Elsa.CustomModels;
//using Elsa.Design;
//using Elsa.Expressions;
//using Elsa.Services;
//using Elsa.Services.Models;

//namespace Elsa.CustomActivities.Activities.Shared
//{
//    public class CustomQuestion : Activity
//    {

//        [ActivityInput(Hint = "Section title")]
//        public string Title { get; set; } = null!;

//        [ActivityInput(
//            Hint = "Question to ask",
//            UIHint = ActivityInputUIHints.SingleLine,
//            DefaultSyntax = SyntaxNames.Literal,
//            SupportedSyntaxes = new[] { SyntaxNames.Literal })]
//        public string Question { get; set; } = null!;

//        [ActivityInput(Hint = "Question hint", UIHint = ActivityInputUIHints.SingleLine)]
//        public string QuestionHint { get; set; } = null!;

//        [ActivityInput(Hint = "Question guidance", UIHint = ActivityInputUIHints.MultiLine)]
//        public string QuestionGuidance { get; set; } = null!;

//        [ActivityInput(Hint = "Include comments box", UIHint = ActivityInputUIHints.Checkbox)]
//        public bool DisplayComments { get; set; }
//        public string Comments { get; set; } = null!;

//        [ActivityInput(Label = "Assessment outcome conditions", Hint = "The conditions to evaluate.", UIHint = "switch-case-builder", DefaultSyntax = "Switch", IsDesignerCritical = true)]
//        public ICollection<SwitchCase> Cases { get; set; } = new List<SwitchCase>();

//        [ActivityInput(
//            Hint = "The switch mode determines whether the first match should be scheduled, or all matches.",
//            DefaultValue = SwitchMode.MatchFirst,
//            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid }
//        )]
//        public SwitchMode Mode { get; set; } = SwitchMode.MatchFirst;

//        [ActivityOutput] public AssessmentQuestion? Output { get; set; }

//        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
//        {
//            return Suspend();
//        }

//        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
//        {
//            var response = context.GetInput<AssessmentQuestion>();
//            Output = response;
//            var matches = Cases.Where(x => x.Condition).Select(x => x.Name).ToList();
//            var hasAnyMatches = matches.Any();
//            var results = Mode == SwitchMode.MatchFirst ? hasAnyMatches ? new[] { matches.First() } : Array.Empty<string>() : matches.ToArray();
//            var outcomes = hasAnyMatches ? results : new[] { OutcomeNames.Default };
//            context.JournalData.Add("Matches", matches);

//            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
//            {
//                Outcomes(outcomes),
//                new SuspendResult()
//            }));
//        }

//    }
//}
