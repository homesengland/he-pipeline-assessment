using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using He.PipelineAssessment.UI.Features.Assessment.TestAssessmentSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace He.PipelineAssessment.UI.Features.Assessments
{
    [Authorize]
    public class AssessmentController : Controller
    {
        private readonly ILogger<AssessmentController> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IUserProvider _userProvider;


        public AssessmentController(ILogger<AssessmentController> logger, IMediator mediator, IConfiguration configuration, IUserProvider userProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
            _userProvider = userProvider;
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> Index()
        {
            var username = _userProvider.GetUserName();
            var canViewSensitiveRecords = _userProvider.CheckUserRole(Constants.AppRole.SensitiveRecordsViewer);
            var listModel = await _mediator.Send(new AssessmentListRequest()
            {
                Username = username,
                CanViewSensitiveRecords = canViewSensitiveRecords
            });
            return View("Index", listModel);
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Summary(int assessmentid, int correlationId)
        {
            var overviewModel = await _mediator.Send(new AssessmentSummaryRequest(assessmentid, correlationId));
            return View("Summary", overviewModel);
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> TestSummary(int assessmentid, int correlationId)
        {
            var enableTestSummaryPage = _configuration["Environment:EnableTestSummaryPage"];
            if (bool.Parse(enableTestSummaryPage ?? "false"))
            {
                var overviewModel = await _mediator.Send(new TestAssessmentSummaryRequest(assessmentid, correlationId));
                return View("TestSummary", overviewModel);
            }
            else
            {
                return RedirectToAction("Summary", new { assessmentid = assessmentid, correlationId = correlationId });
            }
        }
    }
}
