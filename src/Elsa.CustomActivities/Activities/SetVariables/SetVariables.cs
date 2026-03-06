using Auth0.ManagementApi.Models;
using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomActivities.Constants;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elsa.CustomActivities.Activities.SetVariables
{
    [Action(
Category = "Utilities",
Description = "Set Variables in the Database for future use",
Outcomes = new[] { OutcomeNames.Done }
)]
    public class SetVariables : Activity
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        public SetVariables(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        [ActivityInput(Label = "Variables to set", 
            Hint = "Specify the variables to set in the database, using the Variable Intellisense", 
            UIHint = HePropertyUIHints.KeyPair)]
        public ICollection<GlobalVariable> Variables { get; set; } = new List<GlobalVariable>();

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            return await Task.FromResult(Suspend());
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            string correlationId = context.WorkflowInstance.CorrelationId;
            if (!int.TryParse(correlationId, out int parsedSpId))
            {
                throw new ArgumentException($"Invalid SpId format: {correlationId}", nameof(correlationId));
            }


            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
