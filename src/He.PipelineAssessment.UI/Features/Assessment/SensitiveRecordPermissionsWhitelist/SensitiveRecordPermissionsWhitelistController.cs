using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.AddPermission;
using He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.RemovePermission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist
{
    [Authorize]
    public class SensitiveRecordPermissionsWhitelistController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;
        private readonly ILogger<SensitiveRecordPermissionsWhitelistController> _logger;

        public SensitiveRecordPermissionsWhitelistController(
            IMediator mediator, 
            IUserProvider userProvider, 
            ILogger<SensitiveRecordPermissionsWhitelistController> logger)
        {
            _mediator = mediator;
            _userProvider = userProvider;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> Add(int assessmentid, int correlationId, string email)
        {
            var sanitizedEmail = SanitizeForLogging(email);
            _logger.LogInformation("Add permission requested for AssessmentId={AssessmentId}, Email={Email}", assessmentid, sanitizedEmail);

                var command = new AddSensitiveRecordPermissionCommand
            {
                AssessmentId = assessmentid,
                Email = email ?? string.Empty,
                CurrentUsername = _userProvider.UserName() ?? string.Empty,
                IsAdmin = _userProvider.IsAdmin()
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ValidationMessage != null)
                {
                    TempData["ErrorMessage"] = result.ErrorMessage;
                    TempData["EmailValidationError"] = result.ValidationMessage;
                    TempData["EmailValue"] = result.EmailValue;
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage;
                }
            }
            else
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
            }

            return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
        }

        [HttpPost]
        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> Remove(int id, int assessmentid, int correlationId)
        {
            _logger.LogInformation("Remove permission requested for Id={Id}, AssessmentId={AssessmentId}", id, assessmentid);

            var command = new RemoveSensitiveRecordPermissionCommand
            {
                Id = id,
                AssessmentId = assessmentid,
                CurrentUsername = _userProvider.UserName() ?? string.Empty,
                IsAdmin = _userProvider.IsAdmin()
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
            }

            return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
        }

        /// <summary>
        /// Sanitizes email for logging by removing newlines and control characters to prevent log injection
        /// </summary>
        private static string SanitizeForLogging(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "[empty]";
            }
            input = input.Trim();
            return Regex.Replace(input, @"[\r\n\t\f\v\u0000-\u001F\u007F-\u009F]", "", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        }
    }
}