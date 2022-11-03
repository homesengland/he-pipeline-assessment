﻿using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.SinglePipeline
{
    public class SinglePipelineController : Controller
    {
        private readonly ILogger<SinglePipelineController> _logger;
        private readonly IMediator _mediator;

        public SinglePipelineController(ILogger<SinglePipelineController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Sync()
        {
            await _mediator.Send(new SyncCommand());
            return RedirectToAction("Index", "Assessment");
        }
    }
}
