using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.LoadCheckYourAnswersScreen;
using He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Workflow
{
    [Authorize]
    public class WorkflowController : Controller
    {
        private readonly ILogger<WorkflowController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<QuestionScreenSaveAndContinueCommand> _validator;


        public WorkflowController(IValidator<QuestionScreenSaveAndContinueCommand> validator, ILogger<WorkflowController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _validator = validator;
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartWorkflow([FromForm] StartWorkflowCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (result.IsAuthorised)
                {

                    return RedirectToAction("LoadWorkflowActivity",
                        new
                        {
                            WorkflowInstanceId = result?.WorkflowInstanceId,
                            ActivityId = result?.ActivityId,
                            ActivityType = result?.ActivityType
                        });
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Error");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        public async Task<IActionResult> LoadWorkflowActivity(QuestionScreenSaveAndContinueCommandResponse request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ActivityType))
                {
                    //try to get activity type from the server?
                }
                switch (request.ActivityType)
                {
                    case ActivityTypeConstants.QuestionScreen:
                        {
                            var questionScreenRequest = new LoadQuestionScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId,
                                IsReadOnly = false
                            };
                            var result = await this._mediator.Send(questionScreenRequest);

                            if (result.IsAuthorised)
                            {
                                return View("SaveAndContinue", result);
                            }
                            else
                            {
                                return RedirectToAction("LoadReadOnlyWorkflowActivity", request);

                            }
                        }
                    case ActivityTypeConstants.CheckYourAnswersScreen:
                        {
                            var checkYourAnswersScreenRequest = new LoadCheckYourAnswersScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId,
                                IsReadOnly = false,
                            };

                            var result = await this._mediator.Send(checkYourAnswersScreenRequest);

                            if (result.IsAuthorised)
                            {
                                return View("CheckYourAnswers", result);
                            }
                            else
                            {

                                return RedirectToAction("LoadReadOnlyWorkflowActivity", request);

                            }

                        }
                    case ActivityTypeConstants.ConfirmationScreen:
                        {
                            var checkYourAnswersScreenRequest = new LoadConfirmationScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId
                            };

                            var result = await this._mediator.Send(checkYourAnswersScreenRequest);

                            if (result.IsAuthorised)
                            {
                                return View("Confirmation", result);
                            }
                            else
                            {
                                return RedirectToAction("LoadReadOnlyWorkflowActivity", request);
                            }
                        }
                    default:
                        throw new ApplicationException(
                            $"Attempted to load unsupported activity type: {request.ActivityType}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> LoadReadOnlyWorkflowActivity(QuestionScreenSaveAndContinueCommandResponse request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ActivityType))
                {
                    //try to get activity type from the server?
                }
                switch (request.ActivityType)
                {
                    case ActivityTypeConstants.ConfirmationScreen:
                        {
                            var checkYourAnswersScreenRequest = new LoadConfirmationScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId
                            };
                            var result = await this._mediator.Send(checkYourAnswersScreenRequest);

                            return View("Confirmation", result);
                        }

                    default:
                        {
                            var checkYourAnswersScreenRequest = new LoadCheckYourAnswersScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId,
                                IsReadOnly = true
                            };
                            var result = await this._mediator.Send(checkYourAnswersScreenRequest);

                            return View("CheckYourAnswersReadOnly", result);
                        }

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuestionScreenSaveAndContinue([FromForm] QuestionScreenSaveAndContinueCommand command)
        {
            try
            {
                var validationResult = _validator.Validate(command);
                if (validationResult.IsValid)
                {
                    var result = await this._mediator.Send(command);

                    if (result.IsAuthorised)
                    {

                        return RedirectToAction("LoadWorkflowActivity",
                        new
                        {
                            WorkflowInstanceId = result?.WorkflowInstanceId,
                            ActivityId = result?.ActivityId,
                            ActivityType = result?.ActivityType
                        });
                    }
                    else
                    {
                        return RedirectToAction("AccessDenied", "Error");
                    }

                }
                else
                {
                    command.ValidationMessages = validationResult;

                    return View("SaveAndContinue", command);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckYourAnswerScreenSaveAndContinue([FromForm] CheckYourAnswersSaveAndContinueCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (result.IsAuthorised)
                {

                    return RedirectToAction("LoadWorkflowActivity",
                        new
                        {
                            WorkflowInstanceId = result?.WorkflowInstanceId,
                            ActivityId = result?.ActivityId,
                            ActivityType = result?.ActivityType
                        });
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Error");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

    }
}
