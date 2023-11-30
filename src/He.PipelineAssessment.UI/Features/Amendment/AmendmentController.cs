using FluentValidation;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using He.PipelineAssessment.UI.Features.Amendment.CreateAmendment;
using He.PipelineAssessment.UI.Features.Amendment.DeleteAmendment;
using He.PipelineAssessment.UI.Features.Amendment.EditAmendment;
using He.PipelineAssessment.UI.Features.Amendment.LoadAmendmentCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;

namespace He.PipelineAssessment.UI.Features.Amendment
{
    [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
    public class AmendmentController : Controller
    {
        private readonly ILogger<AmendmentController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<AssessmentInterventionCommand> _validator;

        public AmendmentController(ILogger<AmendmentController> logger, IMediator mediator, IValidator<AssessmentInterventionCommand> validator)
        {
            _logger = logger;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<IActionResult> Amendment(string workflowInstanceId)
        {
            var dto = await _mediator.Send(new CreateAmendmentRequest() { WorkflowInstanceId = workflowInstanceId });
            return View("Amendment", dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAmendment([FromForm] AssessmentInterventionDto dto)
        {

            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var createAmendmentCommand = JsonConvert.DeserializeObject<CreateAmendmentCommand>(serializedCommand);
            if (createAmendmentCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(createAmendmentCommand);
                if (validationResult.IsValid)
                {
                    int interventionId = await _mediator.Send(createAmendmentCommand);

                    return RedirectToAction("CheckYourDetails", new { interventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("Amendment", dto);
                }
            }
            return View("Amendment", dto);
        }

        [HttpGet]
        public async Task<IActionResult> CheckYourDetails(int interventionId)
        {
            SubmitAmendmentCommand model = await _mediator.Send(new LoadAmendmentCheckYourAnswersRequest() { InterventionId = interventionId });

            return View("AmendmentCheckYourDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAmendment(int interventionId)
        {
            AssessmentInterventionDto dto = await _mediator.Send(new EditAmendmentRequest() { InterventionId = interventionId });
            if (dto.AssessmentInterventionCommand.Status == InterventionStatus.Draft)
            {
                return View("EditAmendment", dto);
            }
            else
            {
                return RedirectToAction("CheckYourDetails", new { interventionId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAmendment([FromForm] AssessmentInterventionDto dto)
        {
            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var editAmendmentCommand = JsonConvert.DeserializeObject<EditAmendmentCommand>(serializedCommand);
            if (editAmendmentCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(editAmendmentCommand);
                if (validationResult.IsValid)
                {

                    var interventionId = await _mediator.Send(editAmendmentCommand);

                    return RedirectToAction("CheckYourDetails", new { InterventionId = dto.AssessmentInterventionCommand.AssessmentInterventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("EditAmendment", dto);
                }
            }
            else
            {
                return RedirectToAction("Index", "Error", new { message = "There has been an error whilst attempting to save this request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAmendment(SubmitAmendmentCommand model, string submitButton)
        {

            switch (submitButton)
            {
                case "Submit":
                    model.Status = InterventionStatus.Approved;
                    break;
                case "Cancel":
                    await _mediator.Send(new DeleteAmendmentCommand
                    {
                        AssessmentInterventionId = model.AssessmentInterventionId
                    });
                    return RedirectToAction("Summary", "Assessment", new { model.AssessmentId, model.CorrelationId });
                default:
                    model.Status = InterventionStatus.Draft;
                    break;
            }
            await _mediator.Send(model);
            return RedirectToAction("CheckYourDetails", new { InterventionId = model.AssessmentInterventionId });

        }

    }
}
