using FluentValidation;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateRollback;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
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
            return View("~/Features/Intervention/Views/Rollback.cshtml", dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRollback([FromForm] AssessmentInterventionDto dto)
        {
            try
            {
                var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
                var createRollbackCommand = JsonConvert.DeserializeObject<CreateRollbackCommand>(serializedCommand);
                if(createRollbackCommand != null)
                {
                    var validationResult = await _validator.ValidateAsync(createRollbackCommand);
                    if (validationResult.IsValid)
                    {

                        int interventionId = await _mediator.Send(createRollbackCommand);
                        if (interventionId < 1)
                        {
                            return RedirectToAction("Index", "Error", new { message = "There has been an error whilst attempting to save this request.  Please try again." });
                        }
                        return RedirectToAction("CheckYourDetails", new { interventionId });
                    }
                    else
                    {
                        dto.ValidationResult = validationResult;
                        return View("~/Features/Intervention/Views/Rollback.cshtml", dto);
                    }
                }
                return View("~/Features/Intervention/Views/Rollback.cshtml", dto);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRollback(int interventionId)
        {
            throw new NotImplementedException();
            //try
            //{
            //    AssessmentInterventionDto dto = await _mediator.Send(new EditOverrideRequest() { InterventionId = interventionId });
            //    if (dto.AssessmentInterventionCommand.Status == InterventionStatus.NotSubmitted)
            //    {
            //        return View("~/Features/Intervention/Views/EditOverride.cshtml", dto);
            //    }
            //    else
            //    {
            //        return RedirectToAction("CheckYourDetails", new { interventionId });
            //    }
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e.Message);
            //    return RedirectToAction("Index", "Error", new { message = e.Message });
            //}
        }

        [HttpPost]
        public async Task<IActionResult> EditRollback([FromForm] AssessmentInterventionDto dto)
        {
            throw new NotImplementedException();
            //try
            //{
            //    var serializedCommand = JsonConvert.SerializeObject(dto.AssessmentInterventionCommand);
            //    var editOverrideCommand = JsonConvert.DeserializeObject<EditOverrideCommand>(serializedCommand);
            //    if(editOverrideCommand != null)
            //    {
            //        var validationResult = await _validator.ValidateAsync(editOverrideCommand);
            //        if (validationResult.IsValid)
            //        {

            //            var interventionId = await _mediator.Send(editOverrideCommand);
            //            if (interventionId > 0)
            //            {
            //                return RedirectToAction("CheckYourDetails", new { interventionId });
            //            }
            //            else
            //            {
            //                return View("~/Features/Intervention/Views/EditOverride.cshtml", dto);
            //            }

            //        }
            //        else
            //        {
            //            dto.ValidationResult = validationResult;
            //            return View("~/Features/Intervention/Views/EditOverride.cshtml", dto);
            //        }
            //    }
            //    else
            //    {
            //        return View("~/Features/Intervention/Views/EditOverride.cshtml", dto);
            //    }
               
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e.Message);
            //    return RedirectToAction("Index", "Error", new { message = e.Message });
            //}
        }


        [HttpGet]
        public async Task<IActionResult> CheckYourDetails(int interventionId)
        {
            throw new NotImplementedException();
            try
            {
                SubmitOverrideCommand model = await _mediator.Send(new LoadOverrideCheckYourAnswersRequest() { InterventionId = interventionId });
                if(model == null)
                {
                    return RedirectToAction("EditOverride", new { interventionId });
                }
                return View("~/Features/Intervention/Views/OverrideCheckYourDetails.cshtml", model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRollback(SubmitOverrideCommand model, string submitButton)
        {
            throw new NotImplementedException();
            //try
            //{
            //    switch (submitButton)
            //    {
            //        case "Submit":
            //            model.Status = InterventionStatus.Approved;
            //            break;
            //        case "Reject":
            //            model.Status = InterventionStatus.Rejected;
            //            break;
            //        default:
            //            model.Status = InterventionStatus.NotSubmitted;
            //            break;
            //    }
            //    var result = await _mediator.Send(model);
            //    //redirect to some other view, which lists all interventions
            //    return RedirectToAction("CheckYourDetails", new { InterventionId = model.AssessmentInterventionId });
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e.Message);
            //    return RedirectToAction("Index", "Error", new { message = e.Message });
            //}
        }

    }
}
