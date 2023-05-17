using He.PipelineAssessment.UI.Features.Economist.AssessmentToolManagement.Queries.GetAssessmentTools;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Economist.Controllers;
public class EconomistController : BaseController<EconomistController>
{
    public EconomistController(IMediator mediator, ILogger<EconomistController> logger) : base(mediator, logger)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AssessmentTool()
    {
        try
        {

            var assessmentTools = await _mediator.Send(new AssessmentToolQuery());

            return View("AssessmentTool", assessmentTools);

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return RedirectToAction("Index", "Error", new { message = e.Message });
        }

    }
}
