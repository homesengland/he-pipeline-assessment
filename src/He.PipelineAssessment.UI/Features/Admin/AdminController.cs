using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries;
using He.PipelineAssessment.UI.Features.Assessments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Admin
{
    public class AdminController : Controller
    {
        private readonly ILogger<AssessmentController> _logger;
        private readonly IMediator _mediator;

        public AdminController(ILogger<AssessmentController> logger, IMediator mediator)
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
            var createAssessmentToolData = await _mediator.Send(createAssessmentToolCommand);
            return RedirectToAction("AssessmentTool");
        }

        //update an assessment tool
        [HttpPost]
        public async Task<IActionResult> UpdateAssessmentTool(UpdateAssessmentToolCommand updateAssessmentToolCommand)
        {
            if (updateAssessmentToolCommand.Id == 0)
            {
                return RedirectToAction("Index", "Error", new { message = "Bad request. No Assessment Tool Id provided." });
            }

            await _mediator.Send(updateAssessmentToolCommand);

            return RedirectToAction("AssessmentTool");
        }

        //delete an assessment tool 
        [HttpPost]
        public async Task<IActionResult> DeleteAssessmentTool(int assessmentToolId)
        {
            //TODO: implement mediator
            return RedirectToAction("AssessmentTool");
        }
    }
}
