using Elsa.CustomActivities.Describers;
using Elsa.Server.Features.Activities.CustomActivityProperties;
using Elsa.Server.Features.Activities.DataDictionaryProvider;
using Elsa.Server.Features.Activities.GlobalVariableProvider;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Activities
{
    [Route("activities")]
    public class ActivitiesController : Controller
    {

        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("properties")]
        public async Task<IActionResult> GetCustomActivityProperties()
        {
            try
            {
                Dictionary<string, string> results = await _mediator.Send(new CustomPropertyCommand());
                return Ok(results);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet("dictionary")]
        public async Task<IActionResult> GetDataDictionary()
        {
            try
            {
                string results = await _mediator.Send(new DataDictionaryCommand() { IncludeArchived = false });
                return Ok(results);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet("dictionary/archived")]
        public async Task<IActionResult> GetArchivedDataDictionary()
        {
            try
            {
                string results = await _mediator.Send(new DataDictionaryCommand() { IncludeArchived = true });
                return Ok(results);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet("globalvariable")]
        public async Task<IActionResult> GetGlobalVariable()
        {
            try
            {
                string results = await _mediator.Send(new GlobalVariableCommand() { IncludeArchived = false });
                return Ok(results);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet("globalvariable/archived")]
        public async Task<IActionResult> GetArchivedGlobalVariable()
        {
            try
            {
                string results = await _mediator.Send(new GlobalVariableCommand() { IncludeArchived = true });
                return Ok(results);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
    }
}
