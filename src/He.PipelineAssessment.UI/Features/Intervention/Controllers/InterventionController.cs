using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.Commands.CreateAssessmentIntervention;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Intervention.Controllers
{
    public class InterventionController : Controller
    {
        private readonly ILogger<InterventionController> _logger;
        private readonly IMediator _mediator;

        public InterventionController(ILogger<InterventionController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Override(string workflowInstanceId)
        {
            var dto = await _mediator.Send(new CreateAssessmentInterventionRequest { WorkflowInstanceId = workflowInstanceId});
            
            return View(dto);
        }

        public IActionResult Rollback(int assessmentWorkflowInstanceId)
        {
            var dto = new CreateAssessmentInterventionDto();
            // populate info from the db - mediator
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssessmentIntervention([FromForm] CreateAssessmentInterventionDto createAssessmentInterventionDto)
        {
            try
            {
                //mediator send

                //redirect somewhere
                return View("Override", createAssessmentInterventionDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }
    }
}
