using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Admin.Controllers
{
    public class AssessmentToolController : Controller
    {
        private readonly ILogger<AssessmentToolController> _logger;
        private readonly IMediator _mediator;

        public AssessmentToolController(ILogger<AssessmentToolController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get all assessment-tools 
        [HttpGet]
        public async Task<IActionResult> AssessmentTool()
        {
            try
            {
                var assessmentTools = await _mediator.Send(new AssessmentToolRequest());
                return View("AssessmentTool", assessmentTools);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }

        }

        //Create an assessment tool
        [HttpPost]
        public async Task<IActionResult> CreateAssessmentTool(CreateAssessmentToolCommand createAssessmentToolCommand)
        {
            try
            {
                await _mediator.Send(createAssessmentToolCommand);
                return RedirectToAction("AssessmentTool");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        //update an assessment tool
        [HttpPost]
        public async Task<IActionResult> UpdateAssessmentTool(UpdateAssessmentToolCommand updateAssessmentToolCommand)
        {
            if (updateAssessmentToolCommand.Id == 0)
            {
                return RedirectToAction("Index", "Error", new { message = "Bad request. No Assessment Tool Id provided." });
            }

            try
            {
                await _mediator.Send(updateAssessmentToolCommand);

                return RedirectToAction("AssessmentTool");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }

        }

        //delete an assessment tool 
        [HttpPost]
        public async Task<IActionResult> DeleteAssessmentTool(int assessmentToolId)
        {
            try
            {
                await _mediator.Send(new DeleteAssessmentToolCommand(assessmentToolId));

                return RedirectToAction("AssessmentTool");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }
    }
}
