using Elsa.Server.Features.Text.SaveAndContinue;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Text
{

    [Route("text")]
    [ApiController]
    public class TextQuestionController : ControllerBase
    {

        private readonly IMediator _mediator;

        public TextQuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SaveAndContinue")]
        public async Task<IActionResult> SaveAndContinue(TextSaveAndContinueCommand model)
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
