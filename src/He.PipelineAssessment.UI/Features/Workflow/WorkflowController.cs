using FluentValidation;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System;

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
                            ActivityId = result?.ActivityId
                        });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        public async Task<IActionResult> LoadWorkflowActivity(LoadWorkflowActivityRequest request)
        {
            try
            {
                var result = await this._mediator.Send(request);
                return View("SaveAndContinue", result);
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
                        ActivityId = result?.ActivityId
                    });
                }
                else
                {
                    command.ValidationMessages = validationResult.Errors.Select(x=>x.ErrorMessage).ToList();
              
                    return View("SaveAndContinue", command);
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
