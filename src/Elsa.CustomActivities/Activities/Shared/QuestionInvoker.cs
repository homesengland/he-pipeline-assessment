using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.Shared
{
    public interface IQuestionInvoker
    {
        Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync(string activityId, string activityType, string workflowInstanceId, List<QuestionScreenAnswer>? model, CancellationToken cancellationToken = default);
    }

    public class QuestionInvoker : IQuestionInvoker
    {
        private readonly IWorkflowLaunchpad _workflowLaunchpad;

        public QuestionInvoker(IWorkflowLaunchpad workflowLaunchpad)
        {
            _workflowLaunchpad = workflowLaunchpad;
        }


        public async Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync(string activityId, string activityType, string workflowInstanceId, List<QuestionScreenAnswer>? model, CancellationToken cancellationToken = default)
        {
            var context = new WorkflowsQuery(activityType, new QuestionBookmark() { ActivityId = activityId.ToLowerInvariant() }, null, workflowInstanceId);
            var collectedWorkflows = await _workflowLaunchpad.CollectAndExecuteWorkflowsAsync(context, new WorkflowInput(model), cancellationToken);
            return collectedWorkflows;
        }
    }
}

