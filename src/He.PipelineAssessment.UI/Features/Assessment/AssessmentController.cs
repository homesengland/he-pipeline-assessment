using He.PipelineAssessment.UI.Features.Assessments.AssessmentList;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Assessments
{
    public class AssessmentController : Controller
    {
        private readonly ILogger<AssessmentController> _logger;
        private readonly IMediator _mediator;


        public AssessmentController(ILogger<AssessmentController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var listModel = await _mediator.Send(new AssessmentListCommand());
            return View("Index", listModel);
        }

        public async Task<IActionResult> Summary(string assessmentId)
        {
            var overviewModel = await _mediator.Send(new AssessmentSummaryCommand(assessmentId));
            return View("Summary", overviewModel);
        }
    }
}
