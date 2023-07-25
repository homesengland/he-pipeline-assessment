﻿using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, LoadQuestionScreenRequest?>
    {

        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<StartWorkflowCommandHandler> _logger;
        public StartWorkflowCommandHandler(IElsaServerHttpClient elsaServerHttpClient, IAssessmentRepository assessmentRepository, IRoleValidation roleValidation, ILogger<StartWorkflowCommandHandler> logger)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
            _logger = logger;
        }

        public async Task<LoadQuestionScreenRequest?> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _roleValidation.ValidateRole(request.AssessmentId, request.WorkflowDefinitionId))
                {
                    return new LoadQuestionScreenRequest()
                    {
                        IsAuthorised = false
                    };
                }

                var dto = new StartWorkflowCommandDto()
                {
                    WorkflowDefinitionId = request.WorkflowDefinitionId,
                    CorrelationId = request.CorrelationId
                };
                var response = await _elsaServerHttpClient.PostStartWorkflow(dto);

                if (response != null)
                {
                    var result = new LoadQuestionScreenRequest()
                    {
                        ActivityId = response.Data.NextActivityId,
                        WorkflowInstanceId = response.Data.WorkflowInstanceId,
                        ActivityType = response.Data.ActivityType,
                        IsAuthorised = true
                    };

                    var assessmentToolWorkflowInstance = AssessmentToolWorkflowInstance(request, response);

                    await _assessmentRepository.CreateAssessmentToolWorkflowInstance(assessmentToolWorkflowInstance);

                    //if there is a next workflow record for the current set it to started
                    var nextWorkflow =
                        await _assessmentRepository.GetAssessmentToolInstanceNextWorkflowByAssessmentId(request.AssessmentId,
                            request.WorkflowDefinitionId);

                    if (nextWorkflow!=null)
                    {
                        await _assessmentRepository.DeleteNextWorkflow(nextWorkflow);
                    }

                    return await Task.FromResult(result);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }

        }

        private static AssessmentToolWorkflowInstance AssessmentToolWorkflowInstance(StartWorkflowCommand request, WorkflowNextActivityDataDto response)
        {
            var assessmentToolWorkflowInstance = new AssessmentToolWorkflowInstance();
            assessmentToolWorkflowInstance.WorkflowInstanceId = response.Data.WorkflowInstanceId;
            assessmentToolWorkflowInstance.AssessmentId = request.AssessmentId;
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            assessmentToolWorkflowInstance.WorkflowName = response.Data.WorkflowName;
            assessmentToolWorkflowInstance.WorkflowDefinitionId = request.WorkflowDefinitionId;
            assessmentToolWorkflowInstance.CurrentActivityId = response.Data.NextActivityId;
            assessmentToolWorkflowInstance.CurrentActivityType = response.Data.ActivityType;
            assessmentToolWorkflowInstance.FirstActivityId = response.Data.FirstActivityId;
            assessmentToolWorkflowInstance.FirstActivityType = response.Data.FirstActivityType;
            assessmentToolWorkflowInstance.AssessmentToolWorkflowId = request.AssessmentToolWorkflowId;
            return assessmentToolWorkflowInstance;
        }
    }
}
