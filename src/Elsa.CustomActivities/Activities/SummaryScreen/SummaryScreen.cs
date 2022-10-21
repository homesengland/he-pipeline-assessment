using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.SummaryScreen
{
    [Action(
        Category = "Homes England Activities",
        Description = "Get Assessment Stage Summary",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class SummaryScreen : Activity
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public SummaryScreen(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        [ActivityInput(Hint = "Section title")]
        public string Title { get; set; } = null!;
        [ActivityInput(Hint = "Footer title")]
        public string FooterTitle { get; set; } = null!;
        [ActivityInput(Hint = "Footer text")]
        public string FooterText { get; set; } = null!;
        [ActivityOutput] public ICollection<AssessmentQuestion>? Output { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(context.WorkflowInstance.DefinitionId), context.WorkflowInstance.DefinitionId);

            var assessmentQuestions = await _elsaCustomRepository.GetAssessmentQuestions(context.WorkflowInstance.DefinitionId, context.CorrelationId);
            if (assessmentQuestions != null) Output = assessmentQuestions.ToList();

            return Suspend();
        }
    }
}
