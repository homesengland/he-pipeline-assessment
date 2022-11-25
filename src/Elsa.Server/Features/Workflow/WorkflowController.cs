using Elsa.Server.Features.Workflow.LoadQuestionScreen;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
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

        [HttpGet("LoadQuestionScreen")]
        public async Task<IActionResult> LoadWorkflowActivity(string workflowInstanceId, string activityId)
        {
            var request = new LoadQuestionScreenRequest
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

        [HttpPost("QuestionScreenSaveAndContinue")]
        public async Task<IActionResult> MultiSaveAndContinue([FromBody] QuestionScreenSaveAndContinueCommand model)
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
