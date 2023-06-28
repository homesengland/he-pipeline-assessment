﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Error
{
    public class ErrorController : Controller
    {
        private readonly IErrorHelper _errorHelper;

        public ErrorController(IErrorHelper errorHelper)
        {
            _errorHelper = errorHelper;
        }

        public IActionResult Index()
        {
            var exception = _errorHelper.ExceptionHandlerFeatureGetException(HttpContext);

            if (exception == null)
            {
                return NoContent();
            }

            var errorMessage = $"An error occurred while processing your request {exception.Message}";

            return View("Index", exception);
        }

        [Route("access-denied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }


    public interface IErrorHelper
    {
        Exception? ExceptionHandlerFeatureGetException(HttpContext httpContext);
    }
    public class ErrorHelper:IErrorHelper
    {

        public Exception? ExceptionHandlerFeatureGetException(HttpContext httpContext)
        {
            var exception = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (exception != null)
            {
                return exception.Error;
            }
            return null;
        }
    }
}
