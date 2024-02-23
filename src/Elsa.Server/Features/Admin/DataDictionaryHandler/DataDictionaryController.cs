using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryRecord;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryRecord;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryRecord;
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

        [HttpPost("CreateDataDictionaryRecord")]
        public async Task<ActionResult> CreateDataDictionaryRecord([FromBody] CreateDataDictionaryRecordCommand command)
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

        [HttpPost("UpdateDataDictionaryRecord")]
        public async Task<ActionResult> UpdateDataDictionaryRecord([FromBody] UpdateDataDictionaryRecordCommand command)
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

        [HttpPost("ArchiveDataDictionaryRecord")]
        public async Task<ActionResult> ArchiveDataDictionaryRecord([FromBody] ArchiveDataDictionaryRecordCommand command)
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
