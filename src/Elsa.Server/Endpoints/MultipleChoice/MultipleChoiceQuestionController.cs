using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Endpoints.MultipleChoice
{
    [Route("multiple-choice")]
    [ApiController]
    public class MultipleChoiceQuestionController : ControllerBase
    {
        private readonly IMultipleChoiceQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;

        public MultipleChoiceQuestionController(IMultipleChoiceQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore)
        {
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
        }

        [HttpPost]
        public async Task<IActionResult> Handle(MultipleChoiceQuestionModel model)
        {
            string nextActivityId;
            WorkflowInstance workflowInstance;
            if (model.NavigateBack)
            {
                var workflow = await _invoker.FindWorkflowsAsync(model);
                workflowInstance = await _workflowInstanceStore.FindByIdAsync(workflow.First().WorkflowInstanceId);
                var activityDataIrdered = workflowInstance.ActivityData.OrderByDescending(x => x);
                var previousBlockingActivity = workflowInstance.BlockingActivities.OrderByDescending(x => x)
                    .FirstOrDefault(y => y.ActivityId != workflow.First().ActivityId);
                nextActivityId = previousBlockingActivity.ActivityId;
            }
            else
            {
                var collectedWorkflows = new List<CollectedWorkflow>();

                collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(model).ToList();
                workflowInstance = await _workflowInstanceStore.FindByIdAsync(collectedWorkflows.First().WorkflowInstanceId);
                nextActivityId = collectedWorkflows.First().WorkflowInstanceId;
                //_workflowInstanceStore.SaveAsync(workflowInstance);
            }


            var nextActivity =
                workflowInstance.ActivityData.FirstOrDefault(a => a.Key == workflowInstance.Output.ActivityId);
            return Ok(nextActivity.Value);
        }
    }
}

