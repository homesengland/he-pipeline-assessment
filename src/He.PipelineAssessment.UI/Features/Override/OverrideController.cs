using FluentValidation;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Override.CreateOverride;
using He.PipelineAssessment.UI.Features.Override.EditOverride;
using He.PipelineAssessment.UI.Features.Override.LoadOverrideCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Override.SubmitOverride;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Override
{
    [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
    public class OverrideController : Controller
    {
        private readonly ILogger<OverrideController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<AssessmentInterventionCommand> _validator;

        public OverrideController(ILogger<OverrideController> logger, IMediator mediator, IValidator<AssessmentInterventionCommand> validator)
        {
            _logger = logger;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<IActionResult> Override(string workflowInstanceId)
        {
            AssessmentInterventionDto dto = await _mediator.Send(new CreateOverrideRequest { WorkflowInstanceId = workflowInstanceId });
            return View("Override", dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOverride([FromForm] AssessmentInterventionDto dto)
        {

            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var createOverrideCommand = JsonConvert.DeserializeObject<CreateOverrideCommand>(serializedCommand);
            if (createOverrideCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(createOverrideCommand);
                if (validationResult.IsValid)
                {

                    int interventionId = await _mediator.Send(createOverrideCommand);
                    if (interventionId < 1)
                    {
                        return RedirectToAction("Index", "Error", new { message = "There has been an error whilst attempting to save this request.  Please try again." });
                    }
                    return RedirectToAction("CheckYourDetails", new { interventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("Override", dto);
                }
            }
            return View("Override", dto);
        }

        [HttpGet]
        public async Task<IActionResult> EditOverride(int interventionId)
        {

            AssessmentInterventionDto dto = await _mediator.Send(new EditOverrideRequest() { InterventionId = interventionId });
            if (dto.AssessmentInterventionCommand.Status == InterventionStatus.Pending)
            {
                return View("EditOverride", dto);
            }
            else
            {
                return RedirectToAction("CheckYourDetails", new { interventionId });
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditOverride([FromForm] AssessmentInterventionDto dto)
        {

            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var editOverrideCommand = JsonConvert.DeserializeObject<EditOverrideCommand>(serializedCommand);
            if (editOverrideCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(editOverrideCommand);
                if (validationResult.IsValid)
                {

                    var interventionId = await _mediator.Send(editOverrideCommand);

                    return RedirectToAction("CheckYourDetails", new { InterventionId = dto.AssessmentInterventionCommand.AssessmentInterventionId });

                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("EditOverride", dto);
                }
            }
            else
            {
                return RedirectToAction("Index", "Error", new { message = "There has been an error whilst attempting to save this request." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> CheckYourDetails(int interventionId)
        {

            SubmitOverrideCommand model = await _mediator.Send(new LoadOverrideCheckYourAnswersRequest() { InterventionId = interventionId });
            return View("OverrideCheckYourDetails", model);

        }

        [HttpPost]
        public async Task<IActionResult> SubmitOverride(SubmitOverrideCommand model, string submitButton)
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
                    model.Status = InterventionStatus.Pending;
                    break;
            }
            await _mediator.Send(model);
            return RedirectToAction("CheckYourDetails", new { InterventionId = model.AssessmentInterventionId });

        }

    }
}
