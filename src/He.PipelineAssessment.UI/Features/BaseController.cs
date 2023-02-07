using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features
{
    public abstract class BaseController<TController> : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger<TController> _logger;

        protected BaseController(IMediator mediator, ILogger<TController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
    }
}
