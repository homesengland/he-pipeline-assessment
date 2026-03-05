
using Elsa.Server.Features.Admin.GlobalVariableHandler.CreateGlobalVariableGroup;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler
{
    [Route("globalvariable")]
    public class GlobalVariableController : Controller
    {
        private readonly IMediator _mediator;

        public GlobalVariableController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateGlobalVariableGroup")]
        public async Task<ActionResult> CreateGlobalVariableGroup([FromBody] CreateGlobalVariableGroupCommand command)
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


    }
}
