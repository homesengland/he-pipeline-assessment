﻿using FluentValidation;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Admin
{
    [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired)]
    public class AdminController : BaseController<AdminController>
    {
        private readonly IValidator<CreateAssessmentToolCommand> _createAssessmentToolCommandValidator;
        private readonly IValidator<UpdateAssessmentToolCommand> _updateAssessmentToolCommandValidator;
        private readonly IValidator<UpdateAssessmentToolWorkflowCommand> _updateAssessmentToolWorkflowCommandValidator;
        private readonly IValidator<CreateAssessmentToolWorkflowCommand> _createAssessmentToolWorkflowCommandValidator;

        public AdminController(
            IValidator<CreateAssessmentToolCommand> createAssessmentToolCommandValidator,
            IValidator<CreateAssessmentToolWorkflowCommand> createAssessmentToolWorkflowCommandValidator,
            IValidator<UpdateAssessmentToolCommand> updateAssessmentToolCommandValidator,
            IValidator<UpdateAssessmentToolWorkflowCommand> updateAssessmentToolWorkflowCommandValidator,
            IMediator mediator,
            ILogger<AdminController> logger) : base(mediator, logger)
        {
            _createAssessmentToolWorkflowCommandValidator = createAssessmentToolWorkflowCommandValidator;
            _updateAssessmentToolCommandValidator = updateAssessmentToolCommandValidator;
            _updateAssessmentToolWorkflowCommandValidator = updateAssessmentToolWorkflowCommandValidator;
            _createAssessmentToolCommandValidator = createAssessmentToolCommandValidator;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get all assessment-tools 
        [HttpGet]
        public async Task<IActionResult> AssessmentTool()
        {
            var assessmentTools = await _mediator.Send(new AssessmentToolQuery());

            return View("AssessmentTool", assessmentTools);
        }

        //Get all assessment-tools 
        [HttpGet]
        public async Task<IActionResult> AssessmentToolWorkflow(int assessmentToolId)
        {
            var assessmentToolsWorkflows = await _mediator.Send(new AssessmentToolWorkflowQuery(assessmentToolId));
            return View("AssessmentToolWorkflow", assessmentToolsWorkflows);
        }

        //get an assessment tool 
        [HttpGet]
        public async Task<IActionResult> GetAssessmentToolById(int assessmentToolId)
        {
            var assessmentToolsWorkflows = await _mediator.Send(new AssessmentToolWorkflowQuery(assessmentToolId));

            return View("DeleteAssessmentTool", assessmentToolsWorkflows);
        }

        [HttpGet]
        public Task<IActionResult> LoadAssessmentTool(CreateAssessmentToolDto createAssessmentToolDto)
        {
            return Task.FromResult<IActionResult>(View("LoadAssessmentTool", createAssessmentToolDto));
        }

        [HttpGet]
        public Task<IActionResult> LoadAssessmentToolWorkflow(CreateAssessmentToolWorkflowDto createAssessmentToolWorkflowDto)
        {
            return Task.FromResult<IActionResult>(View("LoadAssessmentToolWorkflow", createAssessmentToolWorkflowDto));
        }

        //Create an assessment tool
        [HttpPost]
        public async Task<IActionResult> CreateAssessmentTool(CreateAssessmentToolDto createAssessmentToolDto)
        {
            var validationResult = await _createAssessmentToolCommandValidator.ValidateAsync(createAssessmentToolDto.CreateAssessmentToolCommand);
            if (validationResult.IsValid)
            {
                await _mediator.Send(createAssessmentToolDto.CreateAssessmentToolCommand);
                return RedirectToAction("AssessmentTool");
            }
            else
            {
                createAssessmentToolDto.ValidationResult = validationResult;
                return View("LoadAssessmentTool", createAssessmentToolDto);
            }
        }

        //Create an assessment tool workflow
        [HttpPost]
        public async Task<IActionResult> CreateAssessmentToolWorkflow(CreateAssessmentToolWorkflowDto createAssessmentToolWorkflowDto)
        {
            var validationResult = await _createAssessmentToolWorkflowCommandValidator.ValidateAsync(createAssessmentToolWorkflowDto.CreateAssessmentToolWorkflowCommand);
            if (validationResult.IsValid)
            {
                await _mediator.Send(createAssessmentToolWorkflowDto.CreateAssessmentToolWorkflowCommand);
                return RedirectToAction("AssessmentToolWorkflow", new { createAssessmentToolWorkflowDto.AssessmentToolId });
            }
            else
            {
                createAssessmentToolWorkflowDto.ValidationResult = validationResult;
                return View("LoadAssessmentToolWorkflow", createAssessmentToolWorkflowDto);
            }
        }

        //update an assessment tool
        [HttpPost]
        public async Task<IActionResult> UpdateAssessmentTool(UpdateAssessmentToolDto updateAssessmentToolDto)
        {
            if (updateAssessmentToolDto.UpdateAssessmentToolCommand.Id == 0)
            {
                return RedirectToAction("Index", "Error", new { message = "Bad request. No Assessment Tool Id provided." });
            }


            var validationResult = await _updateAssessmentToolCommandValidator.ValidateAsync(updateAssessmentToolDto.UpdateAssessmentToolCommand);
            if (validationResult.IsValid)
            {
                await _mediator.Send(updateAssessmentToolDto.UpdateAssessmentToolCommand);
                return RedirectToAction("AssessmentTool");
            }
            else
            {
                var assessmentToolList = await _mediator.Send(new AssessmentToolQuery());
                var validatedAssessmentTool = assessmentToolList.AssessmentTools.FirstOrDefault(x =>
                    x.Id == updateAssessmentToolDto.UpdateAssessmentToolCommand.Id);
                if (validatedAssessmentTool != null)
                {
                    validatedAssessmentTool.ValidationResult = validationResult;
                    validatedAssessmentTool.Name = updateAssessmentToolDto.UpdateAssessmentToolCommand.Name;
                    validatedAssessmentTool.Order = updateAssessmentToolDto.UpdateAssessmentToolCommand.Order;
                }
                return View("AssessmentTool", assessmentToolList);
            }
        }

        //update an assessment tool workflow
        [HttpPost]
        public async Task<IActionResult> UpdateAssessmentToolWorkflow(UpdateAssessmentToolWorkflowDto updateAssessmentToolWorkflowDto)
        {
            if (updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.Id == 0)
            {
                return RedirectToAction("Index", "Error", new { message = "Bad request. No Assessment Tool Workflow Id provided." });
            }


            var validationResult = await _updateAssessmentToolWorkflowCommandValidator.ValidateAsync(updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand);
            if (validationResult.IsValid)
            {
                await _mediator.Send(updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand);

                return RedirectToAction("AssessmentToolWorkflow", new { assessmentToolId = updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.AssessmentToolId });
            }
            else
            {
                var assessmentToolWorkflowList = await _mediator.Send(new AssessmentToolWorkflowQuery(updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.AssessmentToolId));
                var validatedAssessmentToolWorkflow = assessmentToolWorkflowList.AssessmentToolWorkflowDtos.FirstOrDefault(x =>
                    x.Id == updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.Id);
                if (validatedAssessmentToolWorkflow != null)
                {
                    validatedAssessmentToolWorkflow.ValidationResult = validationResult;
                    validatedAssessmentToolWorkflow.Name = updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.Name;
                    validatedAssessmentToolWorkflow.WorkflowDefinitionId = updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.WorkflowDefinitionId;
                }
                return View("AssessmentToolWorkflow", assessmentToolWorkflowList);
            }

        }

        //delete an assessment tool 
        [HttpPost]
        public async Task<IActionResult> DeleteAssessmentTool(int assessmentToolId)
        {
            await _mediator.Send(new DeleteAssessmentToolCommand(assessmentToolId));

            return RedirectToAction("AssessmentTool");
        }

        //delete an assessment tool workflow
        [HttpPost]
        public async Task<IActionResult> DeleteAssessmentToolWorkflow(int assessmentToolWorkflowId, int assessmentToolId)
        {

            await _mediator.Send(new DeleteAssessmentToolWorkflowCommand(assessmentToolWorkflowId));

            return RedirectToAction("AssessmentToolWorkflow", new { assessmentToolId });
        }
    }
}
