using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using MediatR;

namespace Elsa.Server.Features.Workflow.SetWorkflowResult
{
    public class SetWorkflowResultHandler : IRequestHandler<SetWorkflowResultCommand, OperationResult<SetWorkflowResultResponse>>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;
        private readonly INextActivityNavigationService _nextActivityNavigationService;

        public SetWorkflowResultHandler(
            IElsaCustomRepository elsaCustomRepository,
            IWorkflowNextActivityProvider workflowNextActivityProvider,
            IWorkflowInstanceProvider workflowInstanceProvider,
            INextActivityNavigationService nextActivityNavigationService)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _workflowInstanceProvider = workflowInstanceProvider;
            _nextActivityNavigationService = nextActivityNavigationService;
        }

        public async Task<OperationResult<SetWorkflowResultResponse>> Handle(SetWorkflowResultCommand command, CancellationToken cancellationToken)
        {
            var result = new OperationResult<SetWorkflowResultResponse>();
            try
            {
                var workflowNextActivityModel = await _workflowNextActivityProvider.GetNextActivity(command.ActivityId, command.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen, cancellationToken);

                var nextActivityRecord =
                   await _elsaCustomRepository.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                       command.WorkflowInstanceId, cancellationToken);

                await _nextActivityNavigationService.CreateNextActivityNavigation(command.ActivityId, nextActivityRecord, workflowNextActivityModel.NextActivity, workflowNextActivityModel.WorkflowInstance!, cancellationToken);

                result.Data = new SetWorkflowResultResponse
                {
                    WorkflowInstanceId = command.WorkflowInstanceId,
                    NextActivityId = workflowNextActivityModel.NextActivity.Id,
                    ActivityType = workflowNextActivityModel.NextActivity.Type
                };
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }
    }
}
