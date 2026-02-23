using Elsa.Activities.Workflows;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.Services.WorkflowStorage;

namespace Elsa.CustomActivities.Activities.RunEconomicCalculations
{
    public class RunEconomicCalculations : RunWorkflow
    {
        [ActivityOutput] public decimal? NumericScoreOutput { get; set; }
        [ActivityOutput] public string? StringScoreOutput { get; set; }

        public RunEconomicCalculations(IStartsWorkflow startsWorkflow, IWorkflowRegistry workflowRegistry, IWorkflowStorageService workflowStorageService, IWorkflowReviver workflowReviver, IWorkflowInstanceStore workflowInstanceStore, IWorkflowDefinitionDispatcher workflowDefinitionDispatcher) : base(startsWorkflow, workflowRegistry, workflowStorageService, workflowReviver, workflowInstanceStore, workflowDefinitionDispatcher)
        {
        }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            if (string.IsNullOrEmpty(CorrelationId))
            {
                CorrelationId = context.CorrelationId;
            }

            var result = await base.OnExecuteAsync(context);

            ProcessWorkflowOutput(context);




            return result;
        }

        public void ProcessWorkflowOutput(ActivityExecutionContext? context = null)
        {
            var outputWorkflow = Output?.WorkflowOutput;

            if (outputWorkflow != null)
            {
                StringScoreOutput = (string)outputWorkflow;
                context?.JournalData.Add("StringScoreOutput", StringScoreOutput);

                decimal numericScore = 0;
                var canParse = decimal.TryParse(StringScoreOutput, out numericScore);
                if (canParse)
                {
                    NumericScoreOutput = numericScore;
                    context?.JournalData.Add("NumericScoreOutput", NumericScoreOutput);
                }
            }
        }
    }
}