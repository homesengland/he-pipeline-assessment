using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryItem;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryItem;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler
{
    [Route("datadictionary")]
    public class DataDictionaryController : Controller
    {
        private readonly IMediator _mediator;

        public DataDictionaryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateDataDictionaryGroup")]
        public async Task<ActionResult> CreateDataDictionaryGroup([FromBody] CreateDataDictionaryGroupCommand command)
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

        [HttpPost("UpdateDataDictionaryGroup")]
        public async Task<ActionResult> UpdateDataDictionaryGroup([FromBody]UpdateDataDictionaryGroupCommand command)
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

        [HttpPost("CreateDataDictionaryItem")]
        public async Task<ActionResult> CreateDataDictionaryItem([FromBody] CreateDataDictionaryItemCommand command)
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

        [HttpPost("UpdateDataDictionaryItem")]
        public async Task<ActionResult> UpdateDataDictionaryItem([FromBody] UpdateDataDictionaryItemCommand command)
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

        [HttpPost("ArchiveDataDictionaryItem")]
        public async Task<ActionResult> ArchiveDataDictionaryItem([FromBody] ArchiveDataDictionaryItemCommand command)
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

        [HttpPost("ArchiveDataDictionaryGroup")]
        public async Task<ActionResult> ArchiveDataDictionaryGroup([FromBody] ArchiveDataDictionaryGroupCommand command)
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
