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


        public AssessmentController(ILogger<AssessmentController> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var listModel = await _mediator.Send(new AssessmentListCommand());
                return View("Index", listModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Summary(int assessmentid, int correlationId)
        {
            try
            {
                var overviewModel = await _mediator.Send(new AssessmentSummaryRequest(assessmentid, correlationId));
                return View("Summary", overviewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> TestSummary(int assessmentid, int correlationId)
        {
            var enableTestSummaryPage = _configuration["Environment:EnableTestSummaryPage"];
            if (bool.Parse(enableTestSummaryPage))
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
