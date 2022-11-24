﻿using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using He.PipelineAssessment.UI.Features.Workflow.LoadCheckYourAnswersScreen;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Workflow
{
    public class WorkflowController : Controller
    {
        private readonly ILogger<WorkflowController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<SaveAndContinueCommand> _validator;


        public WorkflowController(IValidator<SaveAndContinueCommand> validator, ILogger<WorkflowController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _validator = validator;
        }

        public IActionResult Index()
        {
            return View("Index", new StartWorkflowCommand() { WorkflowDefinitionId = "e1ded93b0b4a432ebeb2b8e10bc1175a" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartWorkflow([FromForm] StartWorkflowCommand command)
        {
            try
            {
                var result = await this._mediator.Send(command);

                return RedirectToAction("LoadWorkflowActivity",
                    new
                    {
                        WorkflowInstanceId = result?.WorkflowInstanceId,
                        ActivityId = result?.ActivityId,
                        ActivityType = result?.ActivityType
                    });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        public async Task<IActionResult> LoadWorkflowActivity(SaveAndContinueCommandResponse request)
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
                                ActivityId = request.ActivityId
                            };
                            var result = await this._mediator.Send(questionScreenRequest);

                            return View("MultiSaveAndContinue", result);
                        }
                    case ActivityTypeConstants.CheckYourAnswersScreen:
                        {
                            var checkYourAnswersScreenRequest = new LoadCheckYourAnswersScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId
                            };
                            var result = await this._mediator.Send(checkYourAnswersScreenRequest);

                            return View("CheckYourAnswers", result);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAndContinue([FromForm] SaveAndContinueCommand command)
        {
            try
            {
                var validationResult = _validator.Validate(command);
                if (validationResult.IsValid)
                {
                    var result = await this._mediator.Send(command);

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
                    command.ValidationMessages = validationResult;

                    return View("MultiSaveAndContinue", command);
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
