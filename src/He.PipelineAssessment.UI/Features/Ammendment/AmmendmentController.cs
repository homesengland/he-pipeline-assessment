using FluentValidation;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Ammendment.ConfirmAmmendment;
using He.PipelineAssessment.UI.Features.Ammendment.CreateAmmendment;
using He.PipelineAssessment.UI.Features.Ammendment.DeleteAmmendment;
using He.PipelineAssessment.UI.Features.Ammendment.EditAmmendment;
using He.PipelineAssessment.UI.Features.Ammendment.LoadAmmendmentCheckYourAnswersAssessor;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using He.PipelineAssessment.UI.Features.Rollback.DeleteRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;

namespace He.PipelineAssessment.UI.Features.Ammendment
{
    [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
    public class AmmendmentController : Controller
    {
        private readonly ILogger<AmmendmentController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<AssessmentInterventionCommand> _validator;

        public AmmendmentController(ILogger<AmmendmentController> logger, IMediator mediator, IValidator<AssessmentInterventionCommand> validator)
        {
            _logger = logger;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<IActionResult> Ammendment(string workflowInstanceId)
        {
            var dto = await _mediator.Send(new CreateAmmendmentRequest() { WorkflowInstanceId = workflowInstanceId });
            return View("Ammendment", dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAmmendment([FromForm] AssessmentInterventionDto dto)
        {

            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var createAmmendmentCommand = JsonConvert.DeserializeObject<CreateAmmendmentCommand>(serializedCommand);
            if (createAmmendmentCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(createAmmendmentCommand);
                if (validationResult.IsValid)
                {
                    int interventionId = await _mediator.Send(createAmmendmentCommand);

                    return RedirectToAction("CheckYourDetails", new { interventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("Ammendment", dto);
                }
            }
            return View("Ammendment", dto);
        }

        [HttpGet]
        public async Task<IActionResult> CheckYourDetails(int interventionId)
        {
            ConfirmAmmendmentCommand model = await _mediator.Send(new LoadAmmendmentCheckYourAnswersAssessorRequest() { InterventionId = interventionId });

            return View("AmmendmentCheckYourDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAmmendment(int interventionId)
        {
            AssessmentInterventionDto dto = await _mediator.Send(new EditAmmendmentRequest() { InterventionId = interventionId });
            if (dto.AssessmentInterventionCommand.Status == InterventionStatus.Draft)
            {
                return View("EditAmmendment", dto);
            }
            else
            {
                return RedirectToAction("CheckYourDetails", new { interventionId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAmmendment([FromForm] AssessmentInterventionDto dto)
        {
            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var editAmmendmentCommand = JsonConvert.DeserializeObject<EditAmmendmentCommand>(serializedCommand);
            if (editAmmendmentCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(editAmmendmentCommand);
                if (validationResult.IsValid)
                {

                    var interventionId = await _mediator.Send(editAmmendmentCommand);

                    return RedirectToAction("CheckYourDetails", new { InterventionId = dto.AssessmentInterventionCommand.AssessmentInterventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("EditAmmendment", dto);
                }
            }
            else
            {
                return RedirectToAction("Index", "Error", new { message = "There has been an error whilst attempting to save this request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAmmendment(ConfirmAmmendmentCommand model, string submitButton)
        {

            switch (submitButton)
            {
                case "Submit":
                    model.Status = InterventionStatus.Approved;
                    break;
                case "Cancel":
                    await _mediator.Send(new DeleteAmmendmentCommand
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
