using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public class OverrideController : Controller
    {
        private readonly ILogger<OverrideController> _logger;
        private readonly IMediator _mediator;

        public OverrideController(ILogger<OverrideController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Override(string workflowInstanceId)
        {
            var dto = await _mediator.Send(new CreateOverrideRequest { WorkflowInstanceId = workflowInstanceId });
            return View("~/Features/Intervention/Views/Override.cshtml", dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOverride([FromForm] CreateAssessmentInterventionDto createAssessmentInterventionDto)
        {
            try
            {
                //do some validation of the command
                var interventionId = await _mediator.Send(new CreateOverrideCommand(createAssessmentInterventionDto.CreateAssessmentInterventionCommand));
                return View("OverrideCheckYourAnswers", interventionId);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }
    }
}
