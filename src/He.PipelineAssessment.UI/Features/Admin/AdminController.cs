using He.PipelineAssessment.UI.Features.Assessments;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Admin
{
    public class AdminController : Controller
    {
        private readonly ILogger<AssessmentController> _logger;
        private readonly IMediator _mediator;

        public IActionResult Index()
        {
            return View();
        }

        //Get all assessment-tools 
        [HttpGet]
        public async Task<IActionResult> GetAssessmentTools()
        {
            try
            {
                var assessmentTools = await _mediator.Send(new AssessmentToolRequest());
                return View("AssessmentTools");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
           
        }

        //Create an assessment tool
        [HttpPost]
        public async Task<IActionResult> CreateAssessmentTool()
        {
            return View();
        }

        //update an assessment tool
        [HttpPut]
        public async Task<IActionResult> UpdateAssessmentTool()
        {
            return View();
        }

        //delete an assessment tool 
        [HttpDelete]
        public async Task<IActionResult> DeleteAssessmentTool()
        {
            return View();
        }
    }
}
