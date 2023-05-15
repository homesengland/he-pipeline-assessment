using Elsa.CustomActivities.Describers;
using Elsa.Server.Features.Activities.CustomActivityProperties;
using MediatR;
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
    }
}
