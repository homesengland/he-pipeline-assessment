using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomActivities.Constants;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.Scoring
{
    [Action(
        Category = "Pipeline Assessment Scoring",
        Description = "Set the Formula to calculate Score Outcomes",
        Outcomes = new[] { OutcomeNames.Done },
        DisplayName = "Calculation"
        )]
    public class ScoringCalculation : Activity
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        public ScoringCalculation(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        [ActivityInput(
            Hint = "Set the formula for how to calculate the pot-score outcome of this stage",
            UIHint = ActivityInputUIHints.MultiLine, 
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript }, 
            DefaultSyntax = ScoringSyntaxNames.ScoringCalculation,
            IsDesignerCritical = true)]
        public string Calculation { get; set; } = null!;

        [ActivityOutput] public string? Output { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            try
            {
                if(Calculation == string.Empty)
                {
                    context.JournalData.Add("Error", "Unable to parse Calculation");
                    return new SuspendResult();
                }
                else
                {
                    Output = Calculation;

                    if (context.WorkflowExecutionContext.Input != null)
                    {
                        var workflowInstance = await
                            _elsaCustomRepository.GetQuestionWorkflowInstance(context.WorkflowInstance.Id);
                        if (workflowInstance == null)
                        {
                            var questionWorkflowInstance = new QuestionWorkflowInstance()
                            {
                                WorkflowInstanceId = context.WorkflowInstance.Id,
                                WorkflowDefinitionId = context.WorkflowInstance.DefinitionId,
                                CorrelationId = context.WorkflowInstance.CorrelationId,
                                WorkflowName = context.WorkflowExecutionContext.WorkflowBlueprint.Name ?? context.WorkflowInstance.DefinitionId
                            };
                            await _elsaCustomRepository.CreateQuestionWorkflowInstance(questionWorkflowInstance, CancellationToken.None);
                        }
                    }

                    await _elsaCustomRepository.SetWorkflowInstanceScore(context.WorkflowInstance.Id, Calculation);
                }
            }
            catch(Exception)
            {
                context.JournalData.Add("Error", $"Error occurred whilst updating workflow with {Calculation} score.");
                return new SuspendResult();
            }

            return Done();
        }
    }
}
