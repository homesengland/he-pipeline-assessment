using Elsa.Server.Features.Workflow.GetSinglePipelineData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Datasources
{
    [Route("datasources")]
    public class DatasourcesController : Controller
    {
        private readonly IMediator _mediator;

        public DatasourcesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetSinglePipelineData")]
        public async Task<IActionResult> GetSinglePipelineData(string workflowInstanceId, string spid, string outFields)

        {
            var request = new GetSinglePipelineDataRequest()
            {
                WorkflowInstanceId = workflowInstanceId,
                SpId = spid
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
