﻿using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Features.Workflow.Helpers;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using MediatR;

namespace Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandHandler : IRequestHandler<CheckYourAnswersSaveAndContinueCommand,
        OperationResult<CheckYourAnswersSaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ISaveAndContinueHelper _saveAndContinueHelper;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;


        public CheckYourAnswersSaveAndContinueCommandHandler(
            IElsaCustomRepository elsaCustomRepository,
            ISaveAndContinueHelper saveAndContinueHelper,
            IWorkflowNextActivityProvider workflowNextActivityProvider,
            IWorkflowInstanceProvider workflowInstanceProvider)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _saveAndContinueHelper = saveAndContinueHelper;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _workflowInstanceProvider = workflowInstanceProvider;
        }

        public async Task<OperationResult<CheckYourAnswersSaveAndContinueResponse>> Handle(CheckYourAnswersSaveAndContinueCommand command,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<CheckYourAnswersSaveAndContinueResponse>();
            try
            {
                var nextActivity = await _workflowNextActivityProvider.GetNextActivity(command.ActivityId, command.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen, cancellationToken);
                var workflowInstance =
                    await _workflowInstanceProvider.GetWorkflowInstance(command.WorkflowInstanceId,
                        cancellationToken);

                var nextActivityRecord =
                   await _elsaCustomRepository.GetCustomActivityNavigation(nextActivity.Id,
                       command.WorkflowInstanceId, cancellationToken);

                if (nextActivityRecord == null)
                {
                    var customActivityNavigation = _saveAndContinueHelper.CreateNextCustomActivityNavigation(command.ActivityId, ActivityTypeConstants.CheckYourAnswersScreen, nextActivity.Id, nextActivity.Type, workflowInstance);
                    await _elsaCustomRepository.CreateCustomActivityNavigationAsync(customActivityNavigation, cancellationToken);

                    if (customActivityNavigation.ActivityType == ActivityTypeConstants.QuestionScreen)
                    {
                        var questions = _saveAndContinueHelper.CreateQuestionScreenAnswers(nextActivity.Id, workflowInstance);
                        await _elsaCustomRepository.CreateQuestionScreenAnswersAsync(questions, cancellationToken);
                    }
                }

                result.Data = new CheckYourAnswersSaveAndContinueResponse
                {
                    WorkflowInstanceId = command.WorkflowInstanceId,
                    NextActivityId = nextActivity.Id,
                    ActivityType = nextActivity.Type
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