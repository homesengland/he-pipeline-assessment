using Elsa.CustomActivities.Activites.MultipleChoice;
using Elsa.CustomActivities.Activities.MultipleChoice;
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
            var collectedWorkflows = new List<CollectedWorkflow>();

            collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(model).ToList();
            var workflowInstance = await _workflowInstanceStore.FindByIdAsync(collectedWorkflows.First().WorkflowInstanceId);
            var nextActivity =
                workflowInstance.ActivityData.FirstOrDefault(a => a.Key == workflowInstance.Output.ActivityId);
            return Ok(nextActivity.Value);
        }
    }
}

