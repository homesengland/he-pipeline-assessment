using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using MediatR;

namespace Elsa.Server.Features.Workflow.ExecuteWorkflow
{
    public class ExecuteWorkflowCommandHandler : IRequestHandler<ExecuteWorkflowCommand, OperationResult<ExecuteWorkflowResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowNextActivityProvider _nextActivityProvider;
        private readonly INextActivityNavigationService _nextActivityNavigationService;
        private readonly IDeleteChangedWorkflowPathService _deleteChangedWorkflowPathService;

        public ExecuteWorkflowCommandHandler(IWorkflowNextActivityProvider nextActivityProvider, IDeleteChangedWorkflowPathService deleteChangedWorkflowPathService, INextActivityNavigationService nextActivityNavigationService, IElsaCustomRepository elsaCustomRepository)
        {
            _nextActivityProvider = nextActivityProvider;
            _deleteChangedWorkflowPathService = deleteChangedWorkflowPathService;
            _nextActivityNavigationService = nextActivityNavigationService;
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<OperationResult<ExecuteWorkflowResponse>> Handle(ExecuteWorkflowCommand command, CancellationToken cancellationToken)
        {
            var workflowNextActivityModel = await _nextActivityProvider.GetNextActivity(command.ActivityId, command.WorkflowInstanceId, null,
                command.ActivityType,cancellationToken);

            var nextActivityRecord = await _elsaCustomRepository.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id, command.WorkflowInstanceId, cancellationToken);

            await _deleteChangedWorkflowPathService.DeleteChangedWorkflowPath(command.WorkflowInstanceId, command.ActivityId, workflowNextActivityModel.NextActivity, workflowNextActivityModel.WorkflowInstance!, cancellationToken);

            var latestCustomActivityNavigation = await _elsaCustomRepository.GetLatestCustomActivityNavigation(command.WorkflowInstanceId, cancellationToken);

            if (latestCustomActivityNavigation !=null)
            {
                await _nextActivityNavigationService.CreateNextActivityNavigation(latestCustomActivityNavigation.ActivityId,
                    nextActivityRecord, workflowNextActivityModel.NextActivity,
                    workflowNextActivityModel.WorkflowInstance!, cancellationToken);
            }
            else
            {
                await _nextActivityNavigationService.CreateNextActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                    nextActivityRecord, workflowNextActivityModel.NextActivity,
                    workflowNextActivityModel.WorkflowInstance!, cancellationToken);
            }

            return new OperationResult<ExecuteWorkflowResponse>()
            {
                Data = new ExecuteWorkflowResponse()
                {
                    WorkflowInstanceId = command.WorkflowInstanceId,
                    ActivityType = workflowNextActivityModel.NextActivity.Type,
                    NextActivityId = workflowNextActivityModel.NextActivity.Id
                }
            };

        }
    }
}
