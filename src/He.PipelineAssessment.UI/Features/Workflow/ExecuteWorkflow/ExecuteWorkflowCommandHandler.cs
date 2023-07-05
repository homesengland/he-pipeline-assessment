﻿using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.ExecuteWorkflow
{
    public class ExecuteWorkflowCommandHandler : IRequestHandler<ExecuteWorkflowCommand, LoadQuestionScreenRequest?>
    {
        private readonly IRoleValidation _roleValidation;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        public ExecuteWorkflowCommandHandler(IRoleValidation roleValidation, IAssessmentRepository assessmentRepository, IElsaServerHttpClient elsaServerHttpClient)
        {
            _roleValidation = roleValidation;
            _assessmentRepository = assessmentRepository;
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task<LoadQuestionScreenRequest?> Handle(ExecuteWorkflowCommand request, CancellationToken cancellationToken)
        {
            var assessmentWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);

            if (!await _roleValidation.ValidateRole(assessmentWorkflowInstance!.AssessmentId, assessmentWorkflowInstance!.WorkflowDefinitionId))
            {
                return new LoadQuestionScreenRequest()
                {
                    IsAuthorised = false
                };
            }

            var dto = new ExecuteWorkflowCommandDto()
            {
                WorkflowInstanceId = request.WorkflowInstanceId,
                ActivityType = request.ActivityType,
                ActivityId = request.ActivityId
            };

            var response = await _elsaServerHttpClient.PostExecuteWorkflow(dto);

            if (response != null)
            {
                var result = new LoadQuestionScreenRequest()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType,
                    IsAuthorised = true
                };

                return await Task.FromResult(result);
            }
            else
            {
                return null;
            }
        }
    }
}
