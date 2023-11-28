using Elsa.Client.Models;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Extensions;
using Elsa.Server.Features.Workflow.QuestionScreenValidateAndSave;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using Elsa.Services.Workflows;
using MediatR;
using VersionOptions = Elsa.Models.VersionOptions;

namespace Elsa.Server.Features.Workflow.ReturnToActivity
{
    public class ReturnToActivityCommandHandler : IRequestHandler<ReturnToActivityCommand,
        OperationResult<ReturnToActivityResponse>>
    {
        private readonly IActivityDataProvider _activityDataProvider;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ILogger<ReturnToActivityCommandHandler> _logger;

        public ReturnToActivityCommandHandler(IActivityDataProvider activityDataProvider, IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry, ILogger<ReturnToActivityCommandHandler> logger)
        {
            _activityDataProvider = activityDataProvider;
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
            _logger = logger;
        }

        public async Task<OperationResult<ReturnToActivityResponse>> Handle(ReturnToActivityCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<ReturnToActivityResponse>
            {
                Data = new ReturnToActivityResponse
                {
                    ActivityId = string.Empty,
                    ActivityType = string.Empty
                }
            };
            try
            {
                if (request.WorkflowInstanceId != null && request.ActivityId != null)
                {
                    var activityDataDictionary = await _activityDataProvider.GetActivityData(request.WorkflowInstanceId, request.ActivityId, cancellationToken);

                    var activityName = (string?)activityDataDictionary.GetData("ActivityName");

                    var workflowInstance = await _elsaCustomRepository.GetQuestionWorkflowInstance(request.WorkflowInstanceId);
                    if (workflowInstance != null)
                    {
                        var workflowDefinitionId = workflowInstance.WorkflowDefinitionId;

                        VersionOptions options = VersionOptions.Published;

                        var workflow =
                            await _workflowRegistry.FindAsync(workflowDefinitionId, options,
                            cancellationToken: cancellationToken);
                        if (workflow != null)
                        {
                            var activityBlueprint = workflow.Activities.Where(x => x.Name == activityName).FirstOrDefault();
                            if (activityBlueprint != null)
                            {
                                result.Data.ActivityId = activityBlueprint.Id;
                                result.Data.ActivityType = activityBlueprint.Type;
                            }
                        }
                        else
                        {
                            _logger.LogError($"Failed to get workflow. WorkflowDefinitionId: {workflowDefinitionId}");
                            result.ErrorMessages.Add("Failed to get workflow.");
                        }
                    }
                    else
                    {
                        _logger.LogError($"Failed to get workflow instance. WorkflowInstanceId: {request.WorkflowInstanceId}");
                        result.ErrorMessages.Add("Failed to get workflow instance");
                    }
                }
                else
                {
                    _logger.LogError($"Workflow instance id or activity id is null. WorkflowInstanceIde: {request.WorkflowInstanceId} WorkflowDefinitionId: {request.ActivityId}");
                    result.ErrorMessages.Add($"Workflow instance id or activity id is null. WorkflowInstanceIde: {request.WorkflowInstanceId} WorkflowDefinitionId: {request.ActivityId}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                result.ErrorMessages.Add(e.Message);
            }

            return result;
        }
    }
}
