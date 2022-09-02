using Elsa.Server.Features.Currency.SaveAndContinue;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elsa.Server.Features.Currency.LoadWorkflowActivity;

namespace Elsa.Server.Features.Currency
{

    [Route("currency")]
    [ApiController]
    public class CurrencyQuestionController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CurrencyQuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SaveAndContinue")]
        public async Task<IActionResult> SaveAndContinue(CurrencySaveAndContinueCommand model)
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

        [HttpGet("LoadWorkflowActivity")]
        public async Task<IActionResult> LoadWorkflowActivity(string workflowInstanceId, string activityId)
        {
            var request = new LoadCurrencyActivityRequest
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
