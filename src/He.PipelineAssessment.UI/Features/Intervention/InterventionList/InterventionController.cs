using He.PipelineAssessment.Infrastructure;
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
        private readonly IUserProvider _userProvider;

        public InterventionController(IMediator mediator, ILogger<InterventionController> logger, IUserProvider userProvider)
        {
            _mediator = mediator;
            _logger = logger;
            _userProvider = userProvider;
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var username = _userProvider.UserName();
                var canViewSensitiveRecords = _userProvider.IsAdmin();

                var listModel = await _mediator.Send(new InterventionListRequest()
                {
                    CanViewSensitiveRecords = canViewSensitiveRecords,
                    Username = username
                });
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
