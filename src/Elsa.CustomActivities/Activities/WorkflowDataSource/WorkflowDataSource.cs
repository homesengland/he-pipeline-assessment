using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.WorkflowDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get Data from previous Workflow Instance",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class WorkflowDataSource : Activity
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public WorkflowDataSource(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        [ActivityInput(Hint = "Workflow Definition Id",
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string WorkflowDefinitionId { get; set; } = null!;

        [ActivityOutput] public List<AssessmentQuestion>? Output { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(WorkflowDefinitionId), WorkflowDefinitionId);

            var assessmentQuestions = await _elsaCustomRepository.GetAssessmentQuestions(WorkflowDefinitionId, context.CorrelationId);
            if (assessmentQuestions != null) Output = assessmentQuestions.ToList();
            // find latest instance id for the same correlation Id

            // load answers for all questions

            //set output

            return Done();
        }
    }
}
