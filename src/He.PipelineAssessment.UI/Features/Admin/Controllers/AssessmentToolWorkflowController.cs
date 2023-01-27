using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Admin.Controllers
{
    public class AssessmentToolWorkflowController : Controller
    {
        private readonly ILogger<AssessmentToolWorkflowController> _logger;
        private readonly IMediator _mediator;

        public AssessmentToolWorkflowController(ILogger<AssessmentToolWorkflowController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get all assessment-tool workflows
        [HttpGet]
        public async Task<IActionResult> AssessmentToolWorkflow(int assessmentToolId)
        {
            try
            {
                //var assessmentTools = await _mediator.Send(new AssessmentToolRequest());
                //return View("AssessmentTool", assessmentTools);
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }

        }

        //Create an assessment tool workflow
        [HttpPost]
        public async Task<IActionResult> CreateAssessmentTool(CreateAssessmentToolWorkflowCommand createAssessmentToolWorkflowCommand)
        {
            try
            {
                //await _mediator.Send(createAssessmentToolCommand);
                //return RedirectToAction("AssessmentTool");
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        //update an assessment tool workflow
        [HttpPost]
        public async Task<IActionResult> UpdateAssessmentTool(UpdateAssessmentToolWorkflowCommand updateAssessmentToolWorkflowCommand)
        {
            if (updateAssessmentToolWorkflowCommand.Id == 0)
            {
                return RedirectToAction("Index", "Error", new { message = "Bad request. No Assessment Tool Id provided." });
            }

            try
            {
                //await _mediator.Send(updateAssessmentToolCommand);

                //return RedirectToAction("AssessmentTool");
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }

        }

        //delete an assessment tool workflow
        [HttpPost]
        public async Task<IActionResult> DeleteAssessmentTool(int assessmentToolId)
        {
            try
            {
                //await _mediator.Send(new DeleteAssessmentToolCommand(assessmentToolId));

                //return RedirectToAction("AssessmentTool");
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }
    }
}
