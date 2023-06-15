using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Error
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exception?.Error == null)
            {
                return NoContent();
            }

            var errorMessage = $"An error occurred while processing your request {exception.Error.Message}";

            return View("Index", exception.Error);
        }

        [Route("access-denied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}
