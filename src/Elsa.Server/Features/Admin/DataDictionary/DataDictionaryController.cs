using Elsa.Server.Features.Admin.DataDictionary.ArchiveDataDictionaryItem;
using Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryItem;
using Elsa.Server.Features.Admin.DataDictionary.UpdateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionary.UpdateDataDictionaryItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Admin.DataDictionary
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
    }
}
