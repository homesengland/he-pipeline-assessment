using FluentValidation;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Variation.ConfirmVariation;
using He.PipelineAssessment.UI.Features.Variation.CreateVariation;
using He.PipelineAssessment.UI.Features.Variation.DeleteVariation;
using He.PipelineAssessment.UI.Features.Variation.EditVariation;
using He.PipelineAssessment.UI.Features.Variation.EditVariationAssessor;
using He.PipelineAssessment.UI.Features.Variation.LoadVariationCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Variation.LoadVariationCheckYourAnswersAssessor;
using He.PipelineAssessment.UI.Features.Variation.SubmitVariation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Variation
{
    [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
    public class VariationController : Controller
    {
        private readonly ILogger<VariationController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<AssessmentInterventionCommand> _validator;

        public VariationController(ILogger<VariationController> logger, IMediator mediator, IValidator<AssessmentInterventionCommand> validator)
        {
            _logger = logger;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<IActionResult> Variation(string workflowInstanceId)
        {
            var dto = await _mediator.Send(new CreateVariationRequest() { WorkflowInstanceId = workflowInstanceId });
            return View("Variation", dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariation([FromForm] AssessmentInterventionDto dto)
        {

            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var createVariationCommand = JsonConvert.DeserializeObject<CreateVariationCommand>(serializedCommand);
            if (createVariationCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(createVariationCommand);
                if (validationResult.IsValid)
                {
                    int interventionId = await _mediator.Send(createVariationCommand);
                    
                    return RedirectToAction("CheckYourDetailsAssessor", new { interventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("Variation", dto);
                }
            }
            return View("Variation", dto);
        }

        [HttpGet]
        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> EditVariation(int interventionId)
        {
           
                AssessmentInterventionDto dto = await _mediator.Send(new EditVariationRequest() { InterventionId = interventionId });
                if (dto.AssessmentInterventionCommand.Status == InterventionStatus.Pending)
                {
                    return View("EditVariation", dto);
                }
                if (dto.AssessmentInterventionCommand.Status == InterventionStatus.Draft)
                {
                    return RedirectToAction("CheckYourDetailsAssessor", new { interventionId }); 
                }
                else
                {
                    return RedirectToAction("CheckYourDetails", new { interventionId });
                }
        }

        [HttpPost]
        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> EditVariation([FromForm] AssessmentInterventionDto dto)
        {
            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var editVariationCommand = JsonConvert.DeserializeObject<EditVariationCommand>(serializedCommand);
            if (editVariationCommand != null)
            {
                editVariationCommand.TargetWorkflowDefinitions = dto.TargetWorkflowDefinitions;
                var validationResult = await _validator.ValidateAsync(editVariationCommand);
                if (validationResult.IsValid)
                {

                    var interventionId = await _mediator.Send(editVariationCommand);

                    return RedirectToAction("CheckYourDetails", new { InterventionId = dto.AssessmentInterventionCommand.AssessmentInterventionId });

                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("EditVariation", dto);
                }
            }
            else
            {
                return RedirectToAction("Index", "Error", new { message = "There has been an error whilst attempting to save this request." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditVariationAssessor(int interventionId)
        {
            AssessmentInterventionDto dto = await _mediator.Send(new EditVariationAssessorRequest() { InterventionId = interventionId });
            if (dto.AssessmentInterventionCommand.Status == InterventionStatus.Draft)
            {
                return View("EditVariationAssessor", dto);
            }
            else
            {
                return RedirectToAction("CheckYourDetailsAssessor", new { interventionId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditVariationAssessor([FromForm] AssessmentInterventionDto dto)
        {
            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var editVariationAssessorCommand = JsonConvert.DeserializeObject<EditVariationAssessorCommand>(serializedCommand);
            if (editVariationAssessorCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(editVariationAssessorCommand);
                if (validationResult.IsValid)
                {

                    var interventionId = await _mediator.Send(editVariationAssessorCommand);

                    return RedirectToAction("CheckYourDetailsAssessor", new { InterventionId = dto.AssessmentInterventionCommand.AssessmentInterventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("EditVariationAssessor", dto);
                }
            }
            else
            {
                return RedirectToAction("Index", "Error", new { message = "There has been an error whilst attempting to save this request." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> CheckYourDetailsAssessor(int interventionId)
        {
            ConfirmVariationCommand model = (ConfirmVariationCommand)await _mediator.Send(new LoadVariationCheckYourAnswersAssessorRequest() { InterventionId = interventionId });
            if (model == null)
            {
                return RedirectToAction("EditVariationAssessor", new { interventionId });
            }
            return View("VariationCheckYourDetailsAssessor", model);
        }

        [HttpGet]
        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> CheckYourDetails(int interventionId)
        {

            SubmitVariationCommand model = (SubmitVariationCommand)await _mediator.Send(new LoadVariationCheckYourAnswersRequest() { InterventionId = interventionId });
            return View("VariationCheckYourDetails", model);

        }

        [HttpPost]
        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> SubmitVariation(SubmitVariationCommand model, string submitButton)
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

        [HttpPost]
        public async Task<IActionResult> ConfirmVariation(ConfirmVariationCommand model, string submitButton)
        {

            switch (submitButton)
            {
                case "Submit":
                    model.Status = InterventionStatus.Pending;
                    break;
                case "Cancel":
                    await _mediator.Send(new DeleteVariationCommand
                    {
                        AssessmentInterventionId = model.AssessmentInterventionId
                    });
                    return RedirectToAction("Summary", "Assessment", new { model.AssessmentId, model.CorrelationId });
                default:
                    model.Status = InterventionStatus.Draft;
                    break;
            }
            await _mediator.Send(model);
            return RedirectToAction("CheckYourDetailsAssessor", new { InterventionId = model.AssessmentInterventionId });

        }
    }
}
