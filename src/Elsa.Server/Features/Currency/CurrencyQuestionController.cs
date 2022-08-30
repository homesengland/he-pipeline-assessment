using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Currency
{

    [Route("currency")]
    [ApiController]
    public class MultipleChoiceQuestionController : ControllerBase
    {

        private readonly IMediator _mediator;

        public MultipleChoiceQuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SaveAndContinue")]
        public async Task<IActionResult> SaveAndContinue(SaveAndContinueCommand model)
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
