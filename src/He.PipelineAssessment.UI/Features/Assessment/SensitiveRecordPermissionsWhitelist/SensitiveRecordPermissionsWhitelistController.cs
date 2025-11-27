using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist
{
    [Authorize]
    public class SensitiveRecordPermissionsWhitelistController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<SensitiveRecordPermissionsWhitelistController> _logger;


        public SensitiveRecordPermissionsWhitelistController(IMediator mediator, IUserProvider userProvider, IAssessmentRepository assessmentRepository, ILogger<SensitiveRecordPermissionsWhitelistController> logger)
        {
            _mediator = mediator;
            _userProvider = userProvider;
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> Add(int assessmentid, int correlationId, string email)
        {
            _logger.LogInformation("Add permission requested for AssessmentId={AssessmentId}, Email={Email}", assessmentid, email);

            try
            {
                var assessmentSummary = await _mediator.Send(new SensitiveRecordPermissionsWhitelistRequest(assessmentid));
                var isAdmin = _userProvider.CheckUserRole(Constants.AppRole.PipelineAdminOperations);
                var currentUsername = _userProvider.GetUserName();
                var isProjectManager = assessmentSummary.AssessmentSummary.ProjectManager == currentUsername;

                if (!isAdmin && !isProjectManager)
                {
                    _logger.LogWarning("Unauthorized add attempt by User={User} for AssessmentId={AssessmentId}", currentUsername, assessmentid);
                    return RedirectToAction("AccessDenied", "Error");
                }

                // Validate email
                if (string.IsNullOrWhiteSpace(email))
                {
                    _logger.LogWarning("Add permission failed: missing email. AssessmentId={AssessmentId}", assessmentid);
                    TempData["ErrorMessage"] = "Email address is required.";
                    return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
                }

                // Check if permission already exists
                var existingPermissions = await _assessmentRepository.GetSensitiveRecordWhitelist(assessmentid);
                if (existingPermissions.Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.LogWarning("Add permission failed: duplicate email {Email} for AssessmentId={AssessmentId}", email, assessmentid);
                    TempData["ErrorMessage"] = "This email address already has permission.";
                    return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
                }

                // Add permission
                var whitelist = new SensitiveRecordWhitelist
                {
                    AssessmentId = assessmentid,
                    Email = email.Trim()
                };

                var result = await _assessmentRepository.CreateSensitiveRecordWhitelist(whitelist);
                _logger.LogInformation("Permission added successfully. AssessmentId={AssessmentId}, Email={Email}, NewId={Id}, Result={Result}", assessmentid, whitelist.Email, whitelist.Id, result);

                TempData["SuccessMessage"] = "Permission added successfully.";
                return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding permission for AssessmentId={AssessmentId}", assessmentid);
                TempData["ErrorMessage"] = $"Failed to add permission: {ex.Message}";
                return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
            }
        }


    }
}
