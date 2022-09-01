﻿using Elsa.Server.Features.Date.SaveAndContinue;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elsa.Server.Features.Date
{

    [Route("date")]
    [ApiController]
    public class DateQuestionController : ControllerBase
    {

        private readonly IMediator _mediator;

        public DateQuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SaveAndContinue")]
        public async Task<IActionResult> SaveAndContinue(DateSaveAndContinueCommand model)
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
