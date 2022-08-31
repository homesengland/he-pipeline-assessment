using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.Shared
{
    public interface IQuestionInvoker
    {
        Task<IEnumerable<CollectedWorkflow>> DispatchWorkflowsAsync<T>(string ActivityId, string WorkflowInstanceId, MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync<T>(string ActivityId, string WorkflowInstanceId, MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<CollectedWorkflow>> FindWorkflowsAsync<T>(string ActivityId, string WorkflowInstanceId, CancellationToken cancellationToken = default);
    }

    public class QuestionInvoker : IQuestionInvoker
    {
        private readonly IWorkflowLaunchpad _workflowLaunchpad;

        public QuestionInvoker(IWorkflowLaunchpad workflowLaunchpad)
        {
            _workflowLaunchpad = workflowLaunchpad;
        }

        public async Task<IEnumerable<CollectedWorkflow>> DispatchWorkflowsAsync<T>(string activityId, string workflowInstanceId, MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            var context = new WorkflowsQuery(typeof(T).Name!, new QuestionBookmark() { ActivityId = activityId.ToLowerInvariant() }, null, workflowInstanceId);
            return await _workflowLaunchpad.CollectAndDispatchWorkflowsAsync(context, new WorkflowInput(model), cancellationToken);
        }

        public async Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync<T>(string activityId, string workflowInstanceId, MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            var context = new WorkflowsQuery(typeof(T).Name!, new QuestionBookmark() { ActivityId = activityId.ToLowerInvariant() }, null, workflowInstanceId);
            return await _workflowLaunchpad.CollectAndExecuteWorkflowsAsync(context, new WorkflowInput(model), cancellationToken);
        }

        public async Task<IEnumerable<CollectedWorkflow>> FindWorkflowsAsync<T>(string activityId, string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            var context = new WorkflowsQuery(typeof(T).Name!, new QuestionBookmark() { ActivityId = activityId.ToLowerInvariant() }, null, workflowInstanceId);
            return await _workflowLaunchpad.FindWorkflowsAsync(context, cancellationToken);
        }

    }
}

