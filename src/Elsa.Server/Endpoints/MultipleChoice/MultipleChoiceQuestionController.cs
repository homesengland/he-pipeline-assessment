using Elsa.CustomActivities.Activites.MultipleChoice;
using Elsa.CustomActivities.Activities.MultipleChoice;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Endpoints.MultipleChoice
{
    [Route("multiple-choice")]
    [ApiController]
    public class MultipleChoiceQuestionController : ControllerBase
    {
        private readonly IMultipleChoiceQuestionInvoker _invoker;
        public MultipleChoiceQuestionController(IMultipleChoiceQuestionInvoker invoker)
        {
            _invoker = invoker;
        }

        [HttpPost]
        public async Task<IActionResult> Handle(MultipleChoiceQuestionModel model)
        {
            var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(model);
            return Ok(collectedWorkflows.ToList());
        }
    }
}

