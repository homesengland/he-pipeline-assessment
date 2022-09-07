using Elsa.Server.Features.Workflow.LoadWorkflowActivity;
using Elsa.Server.Features.Workflow.SaveAndContinue;
using Elsa.Server.Features.Workflow.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Workflow
{
    [Route("workflow")]
    public class WorkflowController : Controller
    {
        private readonly IMediator _mediator;

        public WorkflowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("StartWorkflow")]
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

        [HttpGet("LoadWorkflowActivity")]
        public async Task<IActionResult> LoadWorkflowActivity(string workflowInstanceId, string activityId)
        {
            var request = new LoadWorkflowActivityRequest
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

        [HttpPost("SaveAndContinue")]
        public async Task<IActionResult> SaveAndContinue([FromBody] SaveAndContinueCommand model)
        {
            try
            {
                var result = await this._mediator.Send(model);

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
