using FluentValidation;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using He.PipelineAssessment.UI.Features.Rollback.DeleteRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor;
using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Rollback
{
    [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
    public class RollbackController : Controller
    {
        private readonly ILogger<RollbackController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<AssessmentInterventionCommand> _validator;

        public RollbackController(ILogger<RollbackController> logger, IMediator mediator, IValidator<AssessmentInterventionCommand> validator)
        {
            _logger = logger;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<IActionResult> Rollback(string workflowInstanceId)
        {
            var dto = await _mediator.Send(new CreateRollbackRequest() { WorkflowInstanceId = workflowInstanceId });
            return View("Rollback", dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRollback([FromForm] AssessmentInterventionDto dto)
        {

            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var createRollbackCommand = JsonConvert.DeserializeObject<CreateRollbackCommand>(serializedCommand);
            if (createRollbackCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(createRollbackCommand);
                if (validationResult.IsValid)
                {
                    int interventionId = await _mediator.Send(createRollbackCommand);
                    
                    return RedirectToAction("CheckYourDetailsAssessor", new { interventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("Rollback", dto);
                }
            }
            return View("Rollback", dto);
        }

        [HttpGet]
        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> EditRollback(int interventionId)
        {
            AssessmentInterventionDto dto = await _mediator.Send(new EditRollbackRequest() { InterventionId = interventionId });
            if (dto.AssessmentInterventionCommand.Status == InterventionStatus.Pending)
            {
                return View("EditRollback", dto);
            }
            else
            {
                return RedirectToAction("CheckYourDetails", new { interventionId });
            }
        }

        [HttpPost]
        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> EditRollback([FromForm] AssessmentInterventionDto dto)
        {
            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var editRollbackCommand = JsonConvert.DeserializeObject<EditRollbackCommand>(serializedCommand);
            if (editRollbackCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(editRollbackCommand);
                if (validationResult.IsValid)
                {

                    var interventionId = await _mediator.Send(editRollbackCommand);

                    return RedirectToAction("CheckYourDetails", new { InterventionId = dto.AssessmentInterventionCommand.AssessmentInterventionId });

                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("EditRollback", dto);
                }
            }
            else
            {
                return RedirectToAction("Index", "Error", new { message = "There has been an error whilst attempting to save this request." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRollbackAssessor(int interventionId)
        {
            AssessmentInterventionDto dto = await _mediator.Send(new EditRollbackAssessorRequest() { InterventionId = interventionId });
            if (dto.AssessmentInterventionCommand.Status == InterventionStatus.Draft)
            {
                return View("EditRollbackAssessor", dto);
            }
            else
            {
                return RedirectToAction("CheckYourDetailsAssessor", new { interventionId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRollbackAssessor([FromForm] AssessmentInterventionDto dto)
        {
            var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            var editRollbackAssessorCommand = JsonConvert.DeserializeObject<EditRollbackAssessorCommand>(serializedCommand);
            if (editRollbackAssessorCommand != null)
            {
                var validationResult = await _validator.ValidateAsync(editRollbackAssessorCommand);
                if (validationResult.IsValid)
                {

                    var interventionId = await _mediator.Send(editRollbackAssessorCommand);

                    return RedirectToAction("CheckYourDetailsAssessor", new { InterventionId = dto.AssessmentInterventionCommand.AssessmentInterventionId });
                }
                else
                {
                    dto.ValidationResult = validationResult;
                    return View("EditRollbackAssessor", dto);
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
            ConfirmRollbackCommand model = await _mediator.Send(new LoadRollbackCheckYourAnswersAssessorRequest() { InterventionId = interventionId });
            if (model == null)
            {
                return RedirectToAction("EditRollbackAssessor", new { interventionId });
            }
            return View("RollbackCheckYourDetailsAssessor", model);
        }

        [HttpGet]
        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> CheckYourDetails(int interventionId)
        {

            SubmitRollbackCommand model = await _mediator.Send(new LoadRollbackCheckYourAnswersRequest() { InterventionId = interventionId });
            return View("RollbackCheckYourDetails", model);

        }

        [HttpPost]
        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
        public async Task<IActionResult> SubmitRollback(SubmitRollbackCommand model, string submitButton)
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
            var result = await _mediator.Send(model);
            return RedirectToAction("CheckYourDetails", new { InterventionId = model.AssessmentInterventionId });

        }

        [HttpPost]
        public async Task<IActionResult> ConfirmRollback(ConfirmRollbackCommand model, string submitButton)
        {

            switch (submitButton)
            {
                case "Submit":
                    model.Status = InterventionStatus.Pending;
                    break;
                case "Cancel":
                    await _mediator.Send(new DeleteRollbackCommand
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
