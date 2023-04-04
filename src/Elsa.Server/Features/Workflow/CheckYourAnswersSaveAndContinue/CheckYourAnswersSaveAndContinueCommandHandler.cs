using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Helpers;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using MediatR;

namespace Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandHandler : IRequestHandler<CheckYourAnswersSaveAndContinueCommand,
        OperationResult<CheckYourAnswersSaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IElsaCustomModelHelper _elsaCustomModelHelper;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;

        public CheckYourAnswersSaveAndContinueCommandHandler(
            IElsaCustomRepository elsaCustomRepository,
            IElsaCustomModelHelper elsaCustomModelHelper,
            IWorkflowNextActivityProvider workflowNextActivityProvider,
            IWorkflowInstanceProvider workflowInstanceProvider)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _elsaCustomModelHelper = elsaCustomModelHelper;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _workflowInstanceProvider = workflowInstanceProvider;
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

                if (nextActivityRecord == null)
                {
                    var customActivityNavigation = _elsaCustomModelHelper.CreateNextCustomActivityNavigation(command.ActivityId, ActivityTypeConstants.CheckYourAnswersScreen, workflowNextActivityModel.NextActivity.Id, workflowNextActivityModel.NextActivity.Type, workflowInstance);
                    await _elsaCustomRepository.CreateCustomActivityNavigationAsync(customActivityNavigation, cancellationToken);

                    if (customActivityNavigation.ActivityType == ActivityTypeConstants.QuestionScreen)
                    {
                        var questions = _elsaCustomModelHelper.CreateQuestionScreenQuestions(workflowNextActivityModel.NextActivity.Id, workflowInstance);
                        await _elsaCustomRepository.CreateQuestionsAsync(questions, cancellationToken);
                    }
                }

                result.Data = new CheckYourAnswersSaveAndContinueResponse
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