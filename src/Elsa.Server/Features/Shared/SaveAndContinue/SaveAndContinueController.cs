using Elsa.Server.Features.MultipleChoice.SaveAndContinue;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Shared.SaveAndContinue
{
    [Route("save-and-continue")]
    [ApiController]
    public class SaveAndContinueController : ControllerBase
    {

        private readonly IMediator _mediator;

        public SaveAndContinueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SaveAndContinue")]
        public async Task<IActionResult> SaveAndContinue(MultipleChoiceSaveAndContinueCommand model)
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

