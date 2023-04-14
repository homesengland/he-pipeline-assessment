using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomActivities.Constants;
using Elsa.CustomInfrastructure.Data.Repository;
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
        Description = "Set the Formula to calculate Pot Score Outcomes",
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
                if(Calculation == null || Calculation == string.Empty)
                {
                    context.JournalData.Add("Error", "Unable to parse Calculation");
                    return new SuspendResult();
                }
                else
                {
                    Output = Calculation;
                    await _elsaCustomRepository.SetWorkflowInstanceScore(context.WorkflowInstance.Id, Calculation);
                }
            }
            catch(Exception)
            {
                context.JournalData.Add("Error", string.Format("Error occured whilst updating workflow with {0} score.", Calculation) );
                return new SuspendResult();
            }

            return Done();
        }
    }
}
