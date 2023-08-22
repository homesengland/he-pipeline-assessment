using System.Globalization;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomActivities.Constants;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.Providers;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.PCSProfile;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

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
        private readonly IDateTimeProvider _dateTimeProvider;

        public ScoringCalculation(IElsaCustomRepository elsaCustomRepository, IDateTimeProvider dateTimeProvider)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _dateTimeProvider = dateTimeProvider;
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
                    context.JournalData.Add("Output", Output);

                    var workflowInstance = await
                        _elsaCustomRepository.GetQuestionWorkflowInstanceByDefinitionId(context.WorkflowInstance.DefinitionId, context.WorkflowInstance.CorrelationId, CancellationToken.None);
                    var utcNow = _dateTimeProvider.UtcNow();
                    if (workflowInstance == null)
                    {
                        var questionWorkflowInstance = new QuestionWorkflowInstance()
                        {
                            WorkflowInstanceId = context.WorkflowInstance.Id,
                            WorkflowDefinitionId = context.WorkflowInstance.DefinitionId,
                            CorrelationId = context.WorkflowInstance.CorrelationId,
                            WorkflowName = context.WorkflowExecutionContext.WorkflowBlueprint.Name ?? context.WorkflowInstance.DefinitionId,
                            Score = Calculation,
                            CreatedDateTime = utcNow
                        };
                        await _elsaCustomRepository.CreateQuestionWorkflowInstance(questionWorkflowInstance, CancellationToken.None);
                    }
                    else
                    {
                        workflowInstance.WorkflowInstanceId = context.WorkflowInstance.Id;
                        workflowInstance.Score = Calculation;
                        workflowInstance.WorkflowName = context.WorkflowExecutionContext.WorkflowBlueprint.Name ??
                                                        context.WorkflowInstance.DefinitionId;
                        workflowInstance.LastModifiedDateTime = utcNow;
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
