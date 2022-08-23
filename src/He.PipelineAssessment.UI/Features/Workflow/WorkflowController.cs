﻿using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Workflow
{
    public class WorkflowController : Controller
    {
        private readonly ILogger<WorkflowController> _logger;
        private readonly IElsaServerHttpClient _eslElsaServerHttpClient;
        private readonly IMediator _mediator;


        public WorkflowController(ILogger<WorkflowController> logger, IElsaServerHttpClient eslElsaServerHttpClient, IMediator mediator)
        {
            _logger = logger;
            _eslElsaServerHttpClient = eslElsaServerHttpClient;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View("Index", new StartWorkflowCommand() { WorkflowDefinitionId = "ef4c34589fa345d3be955dd6e0c2483f" });
        }

        [HttpPost]
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
                throw new Exception(e.Message);
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
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAndContinue([FromForm] SaveAndContinueCommand command)
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
                throw new Exception(e.Message);
            }
        }

    }
}
