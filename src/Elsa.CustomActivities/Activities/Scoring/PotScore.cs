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
    [Trigger(
        Category = "Pipeline Assessment Scoring",
        Description = "Set the Formula to calculate Pot Score Outcomes",
        Outcomes = new[] { OutcomeNames.Done },
        DisplayName = "Pot Score"
        )]
    public class PotScore : Activity
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        public PotScore(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }


        [ActivityInput(
            Hint = "Set the formula for how to calculate the pot-score outcome of this stage",
            UIHint = ActivityInputUIHints.MultiLine, 
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript }, 
            DefaultSyntax = ScoringSyntaxNames.PotScore,
            IsDesignerCritical = true)]
        public string Calculation { get; set; } = null!;

        [ActivityInput(Label = "Scoring outcome conditions", Hint = "The conditions to evaluate.", UIHint = CustomActivityUIHints.CustomSwitch, DefaultSyntax = "Switch", IsDesignerCritical = true)]
        public ICollection<SwitchCase> Cases { get; set; } = new List<SwitchCase>();

        [ActivityInput(
            Hint = "The switch mode determines whether the first match should be scheduled, or all matches.",
            DefaultValue = SwitchMode.MatchFirst,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public SwitchMode Mode { get; set; } = SwitchMode.MatchFirst;

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
