using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.ExecuteWorkflow;
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


        public WorkflowController(ILogger<WorkflowController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartWorkflow([FromForm] StartWorkflowCommand command)
        {

            var result = await _mediator.Send(command);

            return RedirectToAction("LoadWorkflowActivity",
                new
                {
                    WorkflowInstanceId = result?.WorkflowInstanceId,
                    ActivityId = result?.ActivityId,
                    ActivityType = result?.ActivityType
                });
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        public async Task<IActionResult> LoadWorkflowActivity(QuestionScreenSaveAndContinueCommandResponse request)
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

                        if (result.IsAuthorised && !result.IsReadOnly)
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

                        if (result.IsAuthorised && !result.IsReadOnly)
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
                        var confirmationScreenRequest = new LoadConfirmationScreenRequest
                        {
                            WorkflowInstanceId = request.WorkflowInstanceId,
                            ActivityId = request.ActivityId
                        };

                        var result = await this._mediator.Send(confirmationScreenRequest);
                        return View("Confirmation", result);

                    }
                case ActivityTypeConstants.HousingNeedDataSource:
                case ActivityTypeConstants.PCSProfileDataSource:
                case ActivityTypeConstants.SinglePipelineDataSource:
                case ActivityTypeConstants.VFMDataSource:
                case ActivityTypeConstants.RegionalFigsDataSource:
                case ActivityTypeConstants.RegionalIPUDataSource:

                {
                        var executeWorkflowRequest = new ExecuteWorkflowCommand
                        {
                            WorkflowInstanceId = request.WorkflowInstanceId,
                            ActivityId = request.ActivityId,
                            ActivityType = request.ActivityType,
                        };

                        var result = await this._mediator.Send(executeWorkflowRequest);

                        return RedirectToAction("LoadWorkflowActivity",
                            new
                            {
                                WorkflowInstanceId = result?.WorkflowInstanceId,
                                ActivityId = result?.ActivityId,
                                ActivityType = result?.ActivityType
                            });

                    }
                default:
                    throw new ApplicationException(
                        $"Attempted to load unsupported activity type: {request.ActivityType}");
            }
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> LoadReadOnlyWorkflowActivity(QuestionScreenSaveAndContinueCommandResponse request)
        {

            if (string.IsNullOrEmpty(request.ActivityType))
            {
                //try to get activity type from the server?
            }
            switch (request.ActivityType)
            {

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

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuestionScreenSaveAndContinue([FromForm] QuestionScreenSaveAndContinueCommand command)
        {
            var result = await this._mediator.Send(command);
            if (result.IsValid)
            {
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
                command.ValidationMessages = result.ValidationMessages;

                return View("SaveAndContinue", command);
            }

        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckYourAnswerScreenSaveAndContinue([FromForm] CheckYourAnswersSaveAndContinueCommand command)
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

    }
}
