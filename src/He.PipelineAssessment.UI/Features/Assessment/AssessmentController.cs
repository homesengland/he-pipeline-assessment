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

        public async Task<IActionResult> Summary(int id, int correlationId)
        {
            try
            {
                var overviewModel = await _mediator.Send(new AssessmentSummaryCommand(id, correlationId));
                return View("Summary", overviewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }
    }
}
