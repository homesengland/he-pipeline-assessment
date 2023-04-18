using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomActivities.Constants;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.Providers;
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
            Hint = "Set the formula for how to calculate a score",
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

                    var workflowInstance = await
                        _elsaCustomRepository.GetQuestionWorkflowInstanceByDefinitionId(context.WorkflowInstance.DefinitionId, context.WorkflowInstance.CorrelationId, CancellationToken.None);
                    if (workflowInstance == null)
                    {
                        var questionWorkflowInstance = new QuestionWorkflowInstance()
                        {
                            WorkflowInstanceId = context.WorkflowInstance.Id,
                            WorkflowDefinitionId = context.WorkflowInstance.DefinitionId,
                            CorrelationId = context.WorkflowInstance.CorrelationId,
                            WorkflowName = context.WorkflowExecutionContext.WorkflowBlueprint.Name ?? context.WorkflowInstance.DefinitionId,
                            Score = Calculation
                        };
                        await _elsaCustomRepository.CreateQuestionWorkflowInstance(questionWorkflowInstance, CancellationToken.None);
                    }
                    else
                    {
                        workflowInstance.WorkflowInstanceId = context.WorkflowInstance.Id;
                        workflowInstance.Score = Calculation;
                        workflowInstance.WorkflowName = context.WorkflowExecutionContext.WorkflowBlueprint.Name ??
                                                        context.WorkflowInstance.DefinitionId;
                        await _elsaCustomRepository.SaveChanges(CancellationToken.None);
                    }
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
