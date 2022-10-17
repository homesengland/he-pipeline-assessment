using Elsa.Activities.Workflows;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Providers.WorkflowStorage;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.Services.WorkflowStorage;

namespace Elsa.CustomActivities.Activities.LoadAssessmentStage
{
    [Activity(
        Category = "Homes England Activities",
        Description = "Runs a child workflow.",
        Outcomes = new[] { OutcomeNames.Done, "Not Found" }
    )]
    public class LoadAssessmentStage : Activity
    {
        private readonly IStartsWorkflow _startsWorkflow;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowStorageService _workflowStorageService;

        public LoadAssessmentStage(IStartsWorkflow startsWorkflow, IWorkflowRegistry workflowRegistry, IWorkflowStorageService workflowStorageService)
        {
            _startsWorkflow = startsWorkflow;
            _workflowRegistry = workflowRegistry;
            _workflowStorageService = workflowStorageService;
        }

        [ActivityInput(
            Label = "Workflow Definition",
            Hint = "The workflow definition ID to run.",
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public string? WorkflowDefinitionId { get; set; } = default!;

        [ActivityInput(
            Label = "Tenant ID",
            Hint = "The tenant ID to which the workflow to run belongs.",
            Category = PropertyCategories.Advanced,
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public string? TenantId { get; set; } = default!;

        [ActivityInput(Hint = "Optional input to send to the workflow to run.", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public object? Input { get; set; }

        [ActivityInput(
            Label = "Correlation ID",
            Hint = "The correlation ID to associate with the workflow to run.",
            Category = PropertyCategories.Advanced,
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public string? CorrelationId { get; set; }

        [ActivityInput(
            Label = "Context ID",
            Hint = "The context ID to associate with the workflow to run.",
            Category = PropertyCategories.Advanced,
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public string? ContextId { get; set; }

        [ActivityInput(
            Hint = "Fire And Forget: run the child workflow and continue the current one. Blocking: Run the child workflow and suspend the current one until the child workflow finishes.",
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public RunWorkflowMode Mode { get; set; }

        [ActivityOutput] public FinishedWorkflowModel? Output { get; set; }

        public string ChildWorkflowInstanceId
        {
            get => GetState<string>()!;
            set => SetState(value);
        }

        public Dictionary<string, string> AlreadyExecutedChildren
        {
            get => GetState<Dictionary<string, string>>()!;
            set => SetState(value);
        }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            var cancellationToken = context.CancellationToken;

            var workflowBlueprint = await FindWorkflowBlueprintAsync(cancellationToken);
            WorkflowStatus? childWorkflowStatus;
            WorkflowInstance? childWorkflowInstance;

            //Someway the initial input changes when retrying, so we hash the values
            //when retrying this activity if faulted, if there is only one with this activity id in the workflow, we dont need to use the hash because
            //ChildWorkflowInstanceId will have the id of the subworkflow but, if it has failed in a loop, as ChildWorkflowInstanceId is metadata, it will always
            //have stored just the last workflowinstance executed, but not the rest, so we need to discover what is the real workflowinstace using hashed input


            if (workflowBlueprint == null || workflowBlueprint.Id == context.WorkflowInstance.DefinitionId)
                return Outcome("Not Found");

            var result = await _startsWorkflow.StartWorkflowAsync(workflowBlueprint!, TenantId, new WorkflowInput(Input), CorrelationId, ContextId, cancellationToken: cancellationToken);
            childWorkflowInstance = result.WorkflowInstance!;
            childWorkflowStatus = childWorkflowInstance.WorkflowStatus;
            ChildWorkflowInstanceId = childWorkflowInstance.Id;

            context.JournalData.Add("Workflow Blueprint ID", workflowBlueprint.Id);
            context.JournalData.Add("Workflow Instance ID", childWorkflowInstance.Id);
            context.JournalData.Add("Workflow Instance Status", childWorkflowInstance.WorkflowStatus);


            return Suspend();
        }

        protected override IActivityExecutionResult OnResume(ActivityExecutionContext context)
        {
            var model = (FinishedWorkflowModel)context.WorkflowExecutionContext.Input!;
            return OnResumeInternal(context, model);
        }

        private async Task<IActivityExecutionResult> ResumeSynchronouslyAsync(ActivityExecutionContext context, WorkflowInstance childWorkflowInstance, CancellationToken cancellationToken)
        {
            var outputReference = childWorkflowInstance.Output;

            var output = outputReference != null
                ? await _workflowStorageService.LoadAsync(outputReference.ProviderName, new WorkflowStorageContext(childWorkflowInstance, outputReference.ActivityId), "Output", cancellationToken)
                : null;

            var model = new FinishedWorkflowModel
            {
                WorkflowOutput = output,
                WorkflowInstanceId = childWorkflowInstance.Id
            };

            context.LogOutputProperty(this, "Output", output);
            context.JournalData.Add("Child Workflow Instance ID", childWorkflowInstance.Id);

            return OnResumeInternal(context, model);
        }

        private IActivityExecutionResult OnResumeInternal(ActivityExecutionContext context, FinishedWorkflowModel output)
        {
            var results = new List<IActivityExecutionResult> { Done() };

            Output = output;

            if (output.WorkflowOutput is FinishOutput finishOutput)
            {
                // Deconstruct FinishOutput.
                Output = new FinishedWorkflowModel
                {
                    WorkflowOutput = finishOutput.Output,
                    WorkflowInstanceId = output.WorkflowInstanceId
                };

                var outcomeNames = finishOutput.Outcomes.Except(new[] { OutcomeNames.Done });
                results.Add(Outcomes(outcomeNames));
            }

            return Combine(results);
        }

        private async Task<IWorkflowBlueprint?> FindWorkflowBlueprintAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(WorkflowDefinitionId))
                return null;

            return await _workflowRegistry.FindAsync(WorkflowDefinitionId, VersionOptions.Published, TenantId, cancellationToken);
        }

        public enum RunWorkflowMode
        {
            /// <summary>
            /// Run the specified workflow and continue with the current one. 
            /// </summary>
            FireAndForget,

            /// <summary>
            /// Run the specified workflow and continue once the child workflow finishes. 
            /// </summary>
            Blocking
        }
    }
}
