using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.MultipleChoice
{
    public class MultipleChoiceQuestionInvoker : IMultipleChoiceQuestionInvoker
    {
        private readonly IWorkflowLaunchpad _workflowLaunchpad;

        public MultipleChoiceQuestionInvoker(IWorkflowLaunchpad workflowLaunchpad)
        {
            _workflowLaunchpad = workflowLaunchpad;
        }

        public async Task<IEnumerable<CollectedWorkflow>> DispatchWorkflowsAsync(string ActivityId, string WorkflowInstanceId, MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            var context = new WorkflowsQuery(nameof(MultipleChoiceQuestion), new MultipleChoiceQuestionBookmark() { ActivityID = ActivityId.ToLowerInvariant() }, null, WorkflowInstanceId);
            return await _workflowLaunchpad.CollectAndDispatchWorkflowsAsync(context, new WorkflowInput(model), cancellationToken);
        }

        public async Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync(string ActivityId, string WorkflowInstanceId, MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            var context = new WorkflowsQuery(nameof(MultipleChoiceQuestion), new MultipleChoiceQuestionBookmark() { ActivityID = ActivityId.ToLowerInvariant() }, null, WorkflowInstanceId);
            return await _workflowLaunchpad.CollectAndExecuteWorkflowsAsync(context, new WorkflowInput(model), cancellationToken);
        }

        public async Task<IEnumerable<CollectedWorkflow>> FindWorkflowsAsync(string ActivityId, string WorkflowInstanceId, MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            var context = new WorkflowsQuery(nameof(MultipleChoiceQuestion), new MultipleChoiceQuestionBookmark() { ActivityID = ActivityId.ToLowerInvariant() }, null, WorkflowInstanceId);
            return await _workflowLaunchpad.FindWorkflowsAsync(context, cancellationToken);
        }

    }
}