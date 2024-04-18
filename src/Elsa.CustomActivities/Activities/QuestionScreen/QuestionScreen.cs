using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomActivities.Constants;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.CustomActivities.Providers;
using Elsa.CustomInfrastructure.Data.Repository;
using System.Net.WebSockets;
using Elsa.CustomActivities.Activities.Common;

namespace Elsa.CustomActivities.Activities.QuestionScreen
{
    [Trigger(
    Category = "Homes England Activities",
    Description = "Set up a question screen",
    Outcomes = new[] { OutcomeNames.Done },
    DisplayName = "Question Screen"
    )]
    public class QuestionScreen : Activity
    {
        private readonly IScoreProvider _scoreProvider;
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public QuestionScreen(IScoreProvider scoreProvider, IElsaCustomRepository elsaCustomRepository)
        {
            _scoreProvider = scoreProvider;
            _elsaCustomRepository = elsaCustomRepository;
        }

        [ActivityInput]
        public string PageTitle { get; set; } = null!;

        [ActivityInput(Label = "List of questions",
            Hint = "Questions to be displayed on this page.",
            UIHint = CustomActivityUIHints.QuestionScreen,
            DefaultSyntax = CustomSyntaxNames.QuestionList,
            SupportedSyntaxes = new[] { SyntaxNames.Json },
            IsDesignerCritical = true)]
        public AssessmentQuestions Questions { get; set; } = new AssessmentQuestions();

        [ActivityInput(Hint = "Set the condition to state whether the question is displayed or not.", UIHint = ActivityInputUIHints.SingleLine, SupportedSyntaxes = new[] { SyntaxNames.JavaScript }, DefaultSyntax = SyntaxNames.JavaScript)]
        public bool Condition { get; set; } = true;

        [ActivityInput(Label = "Assessment outcome conditions", Hint = "The conditions to evaluate.", UIHint = CustomActivityUIHints.CustomSwitch, DefaultSyntax = "Switch", IsDesignerCritical = true)]
        public ICollection<SwitchCase> Cases { get; set; } = new List<SwitchCase>();

        [ActivityInput(
            Hint = "The switch mode determines whether the first match should be scheduled, or all matches.",
            DefaultValue = SwitchMode.MatchFirst,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public SwitchMode Mode { get; set; } = SwitchMode.MatchFirst;

        [ActivityOutput] public List<CustomModels.Question>? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            List<ValidationModel> validationModels = Questions.Questions.Select(x => x.Validations).ToList();
            List<Validation> invalidValidations = validationModels.SelectMany(x => x.Validations.Where(v => !v.IsValid)).ToList();
            if(invalidValidations.Any())
            {
                return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes(new [] {OutcomeNames.False}),
                new SuspendResult()
            }));
            }
            await UpdateQuestionScores(context);
            var response = context.GetInput<List<CustomModels.Question>>();
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

        private bool EvaluateValidations(ActivityExecutionContext context, out Dictionary<string, List<string?>> validationMap)
        {
            //TODO - tidy this up as I don't expect it will work - just need to map out intent.
            validationMap = new Dictionary<string, List<string?>>();
            foreach(var question in Questions.Questions)
            {
                if(question.Validations != null && question.Validations.Validations.Count > 0)
                {
                    var failedValidation = question.Validations.Validations.Where(x => !x.IsValid);
                    if(failedValidation.Any())
                    {
                        validationMap.Add(question.Id, failedValidation.Select(x => x.ValidationMessage).ToList());
                    } 
                }
            }
            return validationMap.Keys.Count > 0;
        }

        private async Task UpdateQuestionScores(ActivityExecutionContext context)
        {
            var questions = context.GetInput<List<CustomModels.Question>>();

            if (questions != null)
            {
                foreach (var answeredQuestion in questions)
                {
                    var questionDefinition = GetQuestionFromActivityData(context, answeredQuestion.QuestionId);
                    var score = CalculateQuestionScore(answeredQuestion, questionDefinition);

                    var dbQuestion = await _elsaCustomRepository.GetQuestionById(answeredQuestion.Id);
                    if (dbQuestion != null)
                    {
                        dbQuestion.Score = score;
                        await _elsaCustomRepository.UpdateQuestion(dbQuestion);
                    }
                }
            }

        }

        private Question? GetQuestionFromActivityData(ActivityExecutionContext context, string? questionId)
        {
            var dictionary = context.GetActivityData(context.ActivityId);
            if (dictionary.ContainsKey("Questions"))
            {
                var activityData = dictionary.FirstOrDefault(x => x.Key == "Questions");
                var assessmentQuestions = (AssessmentQuestions?)activityData.Value;
                if (assessmentQuestions != null)
                {
                    var questionDefinition = assessmentQuestions.Questions.FirstOrDefault(x => x.Id == questionId);
                    return questionDefinition;
                }

            }
            return null;
        }

        private decimal CalculateQuestionScore(CustomModels.Question answeredQuestion, Question? questionDefinition)
        {
            if (questionDefinition == null)
                return 0;

            var score = _scoreProvider.CalculateScore(answeredQuestion, questionDefinition);
            return score;
        }
    }

    public class AssessmentQuestions
    {
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
