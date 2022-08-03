using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;
using MyActivityLibrary.Activities;

namespace Elsa.CustomActivities.Activites.MultipleChoice
{
    public class MultipleChoiceQuestionInvoker : IMultipleChoiceQuestionInvoker
    {
        private readonly IWorkflowLaunchpad _workflowLaunchpad;

        public MultipleChoiceQuestionInvoker(IWorkflowLaunchpad workflowLaunchpad)
        {
            _workflowLaunchpad = workflowLaunchpad;
        }

        public async Task<IEnumerable<CollectedWorkflow>> DispatchWorkflowsAsync(MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            var context = new WorkflowsQuery(nameof(MultipleChoiceQuestion), new MultipleChoiceQuestionBookmark() { QuestionID = model.QuestionID }, null, model.WorkflowInstanceID);
            return await _workflowLaunchpad.CollectAndDispatchWorkflowsAsync(context, new WorkflowInput(model), cancellationToken);
        }

        public async Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync(MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            var context = new WorkflowsQuery(nameof(MultipleChoiceQuestion), new MultipleChoiceQuestionBookmark() { QuestionID = model.QuestionID }, null, model.WorkflowInstanceID);
            return await _workflowLaunchpad.CollectAndExecuteWorkflowsAsync(context, new WorkflowInput(model), cancellationToken);
        }


    }
}