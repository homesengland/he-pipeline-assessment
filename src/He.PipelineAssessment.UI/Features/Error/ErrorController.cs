﻿using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Error
{
    public class ErrorController : Controller
    {
        public IActionResult Index(string? message)
        {
            return View();
        }
    }
}
