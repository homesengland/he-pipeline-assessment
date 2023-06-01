using FluentValidation;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators;
using He.PipelineAssessment.UI.Features.Intervention.Constants;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
    public class OverrideController : Controller
    {
        private readonly ILogger<OverrideController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<CreateOverrideCommand> _createOverrideCommandValidator;

        public OverrideController(ILogger<OverrideController> logger, IMediator mediator, IValidator<CreateOverrideCommand> createOverrideCommandValidator)
        {
            _logger = logger;
            _mediator = mediator;
            _createOverrideCommandValidator = createOverrideCommandValidator;
        }

        public async Task<IActionResult> Override(string workflowInstanceId)
        {
            var dto = await _mediator.Send(new CreateOverrideRequest { WorkflowInstanceId = workflowInstanceId });
            return View("~/Features/Intervention/Views/Override.cshtml", dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOverride([FromForm] AssessmentInterventionDto dto)
        {
            try
            {
                var createOverrideCommand = new CreateOverrideCommand(dto.AssessmentInterventionCommand);
                var validationResult = await _createOverrideCommandValidator.ValidateAsync(createOverrideCommand);
                if (validationResult.IsValid)
                {
                    
                    var interventionId = await _mediator.Send(createOverrideCommand);
                    return RedirectToAction("CheckYourDetails", new { interventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("~/Features/Intervention/Views/Override.cshtml", dto);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditOverride(int interventionId)
        {
            try
            {
                AssessmentInterventionDto dto = await _mediator.Send(new EditOverrideRequest() { InterventionId = interventionId });
                return View("~/Features/Intervention/Views/EditOverride.cshtml", dto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> CheckYourDetails(int interventionId)
        {
            try
            {
                LoadOverrideCheckYourAnswersCommand model = await _mediator.Send(new LoadOverrideCheckYourAnswersRequest() { InterventionId = interventionId });
                return View("~/Features/Intervention/Views/OverrideCheckYourDetails.cshtml", model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOverride(SubmitOverrideCommand model, string submitButton)
        {
            try
            {
                switch (submitButton)
                {
                    case "Submit":
                        model.Status = InterventionStatus.Approved;
                        break;
                    case "Reject":
                        model.Status = InterventionStatus.Rejected;
                        break;
                    default:
                        model.Status = InterventionStatus.NotSubmitted;
                        break;
                }
                var result = await _mediator.Send(model);
                //redirect to some other view, which lists all interventions
                return RedirectToAction("CheckYourDetails", new { InterventionId = model.AssessmentInterventionId });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

    }
}
