using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using MediatR;

namespace Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandHandler : IRequestHandler<CheckYourAnswersSaveAndContinueCommand,
        OperationResult<CheckYourAnswersSaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;
        private readonly INextActivityNavigationService _nextActivityNavigationService;
        private readonly ILogger<CheckYourAnswersSaveAndContinueCommandHandler> _logger;

        public CheckYourAnswersSaveAndContinueCommandHandler(
            IElsaCustomRepository elsaCustomRepository,
            IWorkflowNextActivityProvider workflowNextActivityProvider,
            IWorkflowInstanceProvider workflowInstanceProvider, 
            INextActivityNavigationService nextActivityNavigationService, ILogger<CheckYourAnswersSaveAndContinueCommandHandler> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _workflowInstanceProvider = workflowInstanceProvider;
            _nextActivityNavigationService = nextActivityNavigationService;
            _logger = logger;
        }

        public async Task<OperationResult<CheckYourAnswersSaveAndContinueResponse>> Handle(CheckYourAnswersSaveAndContinueCommand command,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<CheckYourAnswersSaveAndContinueResponse>();
            try
            {
                var workflowNextActivityModel = await _workflowNextActivityProvider.GetNextActivity(command.ActivityId, command.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen, cancellationToken);
                var workflowInstance =
                    await _workflowInstanceProvider.GetWorkflowInstance(command.WorkflowInstanceId,
                        cancellationToken);

                var nextActivityRecord =
                   await _elsaCustomRepository.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                       command.WorkflowInstanceId, cancellationToken);

                await _nextActivityNavigationService.CreateNextActivityNavigation(command.ActivityId, nextActivityRecord, workflowNextActivityModel.NextActivity, workflowInstance, cancellationToken);

                result.Data = new CheckYourAnswersSaveAndContinueResponse
                {
                    WorkflowInstanceId = command.WorkflowInstanceId,
                    NextActivityId = workflowNextActivityModel.NextActivity.Id,
                    ActivityType = workflowNextActivityModel.NextActivity.Type
                };
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