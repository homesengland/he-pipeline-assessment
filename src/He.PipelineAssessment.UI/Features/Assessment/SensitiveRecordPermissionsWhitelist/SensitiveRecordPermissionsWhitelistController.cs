using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

                // Validate email is not empty
                if (string.IsNullOrWhiteSpace(email))
                {
                    _logger.LogWarning("Add permission failed: missing email. AssessmentId={AssessmentId}", assessmentid);
                    TempData["ErrorMessage"] = "Enter an email address";
                    TempData["EmailValidationError"] = "Enter an email address";
                    TempData["EmailValue"] = email;
                    return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
                }

                // Validate email format
                var emailAttribute = new EmailAddressAttribute();
                if (!emailAttribute.IsValid(email))
                {
                    _logger.LogWarning("Add permission failed: invalid email format {Email} for AssessmentId={AssessmentId}", email, assessmentid);
                    TempData["ErrorMessage"] = "Inavlid email address format";
                    TempData["EmailValidationError"] = "Enter an email address in the correct format such as name@homesengland.gov.uk";
                    TempData["EmailValue"] = email;
                    return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
                }

                // Check if permission already exists
                var existingPermissions = await _assessmentRepository.GetSensitiveRecordWhitelist(assessmentid);
                if (existingPermissions.Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.LogWarning("Add permission failed: duplicate email {Email} for AssessmentId={AssessmentId}", email, assessmentid);
                    TempData["ErrorMessage"] = $"{email} is already on the permissions list for this assessment";
                    TempData["EmailValidationError"] = $"{email} is already on the permissions list for this assessment";
                    TempData["EmailValue"] = email;
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

                TempData["SuccessMessage"] = $"Permission for {whitelist.Email} added successfully";
                return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding permission for AssessmentId={AssessmentId}", assessmentid);
                TempData["ErrorMessage"] = $"Failed to add permission: {ex.Message}";
                return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
            }
        }

        [HttpPost]
        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> Remove(int id, int assessmentid, int correlationId)
        {
            _logger.LogInformation("Remove permission requested for Id={Id}, AssessmentId={AssessmentId}", id, assessmentid);

            try
            {
                var assessmentSummary = await _mediator.Send(new SensitiveRecordPermissionsWhitelistRequest(assessmentid));
                var isAdmin = _userProvider.CheckUserRole(Constants.AppRole.PipelineAdminOperations);
                var currentUsername = _userProvider.GetUserName();
                var isProjectManager = assessmentSummary.AssessmentSummary.ProjectManager == currentUsername;

                if (!isAdmin && !isProjectManager)
                {
                    _logger.LogWarning("Unauthorized remove attempt by User={User} for AssessmentId={AssessmentId}", currentUsername, assessmentid);
                    return RedirectToAction("AccessDenied", "Error");
                }

                // Get the whitelist entry
                var whitelist = await _assessmentRepository.GetSensitiveRecordWhitelistById(id);
                if (whitelist == null)
                {
                    _logger.LogWarning("Remove permission failed: whitelist entry not found. Id={Id}", id);
                    TempData["ErrorMessage"] = "Permission not found.";
                    return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
                }

                // Verify the whitelist entry belongs to the correct assessment
                if (whitelist.AssessmentId != assessmentid)
                {
                    _logger.LogWarning("Remove permission failed: whitelist entry {Id} does not belong to AssessmentId={AssessmentId}", id, assessmentid);
                    TempData["ErrorMessage"] = "Invalid permission reference.";
                    return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
                }

                // Remove permission
                var result = await _assessmentRepository.DeleteSensitiveRecordWhitelist(whitelist);
                _logger.LogInformation("Permission removed successfully. Id={Id}, AssessmentId={AssessmentId}, Email={Email}, Result={Result}", id, assessmentid, whitelist.Email, result);

                TempData["SuccessMessage"] = $"Permission for {whitelist.Email} removed successfully.";
                return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing permission for Id={Id}, AssessmentId={AssessmentId}", id, assessmentid);
                TempData["ErrorMessage"] = $"Failed to remove permission: {ex.Message}";
                return RedirectToAction("Summary", "Assessment", new { assessmentid, correlationId }, "permissions");
            }
        }


    }
}
