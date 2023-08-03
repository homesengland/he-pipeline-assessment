using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.SinglePipeline
{
    [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
    public class SinglePipelineController : Controller
    {
        private readonly ILogger<SinglePipelineController> _logger;
        private readonly IMediator _mediator;

        public SinglePipelineController(ILogger<SinglePipelineController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            var model = new SyncModel();
            return View("Sync", model);
        }

        public async Task<IActionResult> Sync()
        {
            var model = await _mediator.Send(new SyncCommand());
            return View("Sync", model);
        }
    }
}
