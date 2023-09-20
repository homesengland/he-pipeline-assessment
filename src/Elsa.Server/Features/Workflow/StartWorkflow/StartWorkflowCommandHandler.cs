using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.Providers;
using Elsa.Models;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Services;
using MediatR;


namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, OperationResult<StartWorkflowResponse>>
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IStartsWorkflow _startsWorkflow;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly INextActivityNavigationService _nextActivityNavigationService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger <StartWorkflowCommandHandler> _logger;


        public StartWorkflowCommandHandler(IWorkflowRegistry workflowRegistry,
            IStartsWorkflow startsWorkflow,
            IElsaCustomRepository elsaCustomRepository,
            IWorkflowNextActivityProvider workflowNextActivityProvider,
            INextActivityNavigationService nextActivityNavigationService,
            IDateTimeProvider dateTimeProvider, ILogger<StartWorkflowCommandHandler> logger)
        {
            _workflowRegistry = workflowRegistry;
            _startsWorkflow = startsWorkflow;
            _elsaCustomRepository = elsaCustomRepository;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _nextActivityNavigationService = nextActivityNavigationService;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<OperationResult<StartWorkflowResponse>> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<StartWorkflowResponse>();
            try
            {

                VersionOptions options = VersionOptions.Published;

                var workflow =
                    await _workflowRegistry.FindAsync(request.WorkflowDefinitionId, options, cancellationToken: cancellationToken);

                var workflowName = workflow!.Name != null ? workflow.Name : "undefined workflow";
                var runWorkflowResult = await _startsWorkflow.StartWorkflowAsync(workflow, null, null, request.CorrelationId, cancellationToken: cancellationToken);

                if (runWorkflowResult.WorkflowInstance != null)
                {
                    var workflowInstance = runWorkflowResult.WorkflowInstance;

                    var questionWorkflowInstance = new QuestionWorkflowInstance()
                    {
                        WorkflowInstanceId = workflowInstance.Id,
                        WorkflowDefinitionId = workflowInstance.DefinitionId,
                        CorrelationId = request.CorrelationId,
                        WorkflowName = workflowName,
                        CreatedDateTime = _dateTimeProvider.UtcNow()
                    };

                    await _elsaCustomRepository.CreateQuestionWorkflowInstance(questionWorkflowInstance, cancellationToken);

                    var activity = workflow.Activities.FirstOrDefault(x =>
                        x.Id == runWorkflowResult.WorkflowInstance.LastExecutedActivityId);

                    if (activity != null)
                    {
                        var workflowNextActivityModel = await _workflowNextActivityProvider.GetStartWorkflowNextActivity(activity, runWorkflowResult.WorkflowInstance.Id, cancellationToken);

                        if (workflowNextActivityModel.WorkflowInstance != null)
                            workflowInstance = workflowNextActivityModel.WorkflowInstance;

                        var nextActivityRecord = await _elsaCustomRepository.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id, workflowInstance.Id, cancellationToken);

                        await _nextActivityNavigationService.CreateNextActivityNavigation(workflowNextActivityModel.NextActivity.Id, nextActivityRecord, workflowNextActivityModel.NextActivity, workflowInstance, workflowName, cancellationToken);

                        result.Data = new StartWorkflowResponse
                        {
                            WorkflowInstanceId = workflowInstance.Id,
                            NextActivityId = workflowNextActivityModel.NextActivity.Id,
                            ActivityType = workflowNextActivityModel.NextActivity.Type,
                            WorkflowName = workflowName,
                            FirstActivityId = workflowInstance.BlockingActivities.First().ActivityId,//Unsure if this will always be the first item executed
                            FirstActivityType = workflowInstance.BlockingActivities.First().ActivityType
                        };
                    }
                    else
                    {
                        _logger.LogError($"Failed to get last executed activity. CorrelationId: {request.CorrelationId} WorkflowDefinitionId: {request.WorkflowDefinitionId}");
                        result.ErrorMessages.Add("Failed to get activity");
                    }
                }
                else
                {
                    _logger.LogError($"Workflow instance is null. CorrelationId: {request.CorrelationId} WorkflowDefinitionId: {request.WorkflowDefinitionId}");
                    result.ErrorMessages.Add("Workflow instance is null");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }
    }
}