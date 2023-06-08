using He.PipelineAssessment.UI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionList
{
    public class InterventionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InterventionController> _logger;

        public InterventionController(IMediator mediator, ILogger<InterventionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var listModel = await _mediator.Send(new InterventionListRequest());
                return View("~/Features/Intervention/Views/InterventionList.cshtml", listModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }
    }
}
