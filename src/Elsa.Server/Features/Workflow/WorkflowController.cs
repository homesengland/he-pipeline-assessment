using Elsa.Server.Features.Workflow.LoadWorkflow;
using Elsa.Server.Features.Workflow.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Workflow
{
    public class WorkflowController : Controller
    {
        private readonly IMediator _mediator;

        public WorkflowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkflow([FromBody] StartWorkflowCommand command)
        {
            try
            {
                var result = await this._mediator.Send(command);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet("LoadWorkflow")]
        public async Task<IActionResult> LoadWorkflow(string workflowInstanceId, string activityId)
        {
            var request = new LoadWorkflowRequest
            {
                WorkflowInstanceId = workflowInstanceId,
                ActivityId = activityId
            };

            try
            {
                var result = await this._mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(string.Join(',', result.ErrorMessages));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
    }
}
