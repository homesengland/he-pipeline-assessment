﻿using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Helpers;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandHandler : IRequestHandler<QuestionScreenSaveAndContinueCommand,
        OperationResult<QuestionScreenSaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IElsaCustomModelHelper _elsaCustomModelHelper;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;
        private readonly IWorkflowPathProvider _workflowPathProvider;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly INextActivityNavigationService _nextActivityNavigationService;


        public QuestionScreenSaveAndContinueCommandHandler(
            IElsaCustomRepository elsaCustomRepository,
            IElsaCustomModelHelper elsaCustomModelHelper,
            IWorkflowInstanceProvider workflowInstanceProvider,
            IDateTimeProvider dateTimeProvider,
            IWorkflowPathProvider workflowPathProvider,
            IWorkflowNextActivityProvider workflowNextActivityProvider, INextActivityNavigationService nextActivityNavigationService)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _elsaCustomModelHelper = elsaCustomModelHelper;
            _workflowInstanceProvider = workflowInstanceProvider;
            _dateTimeProvider = dateTimeProvider;
            _workflowPathProvider = workflowPathProvider;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _nextActivityNavigationService = nextActivityNavigationService;
        }

        public async Task<OperationResult<QuestionScreenSaveAndContinueResponse>> Handle(QuestionScreenSaveAndContinueCommand command,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<QuestionScreenSaveAndContinueResponse>();
            try
            {
                var dbAssessmentQuestionList =
                    await _elsaCustomRepository.GetQuestionScreenAnswers(command.ActivityId, command.WorkflowInstanceId,
                        cancellationToken);

                if (dbAssessmentQuestionList.Any())
                {
                    var workflowInstance =
                        await _workflowInstanceProvider.GetWorkflowInstance(command.WorkflowInstanceId,
                            cancellationToken);

                    if (workflowInstance.WorkflowStatus == WorkflowStatus.Finished)
                    {
                        throw new Exception($"Unable to save answers. Workflow status is: Finished");
                    }

                    await SetAnswers(command, cancellationToken, dbAssessmentQuestionList);

                    var nextActivity = await _workflowNextActivityProvider.GetNextActivity(command.ActivityId, command.WorkflowInstanceId, dbAssessmentQuestionList, ActivityTypeConstants.QuestionScreen, cancellationToken);

                    var nextActivityRecord = await _elsaCustomRepository.GetCustomActivityNavigation(nextActivity.Id, command.WorkflowInstanceId, cancellationToken);

                    await _nextActivityNavigationService.CreateNextActivityNavigation(command.ActivityId, cancellationToken, nextActivityRecord, nextActivity, workflowInstance);

                    await DeleteChangedWorkflowPath(command, cancellationToken, nextActivity, workflowInstance);

                    result.Data = new QuestionScreenSaveAndContinueResponse
                    {
                        WorkflowInstanceId = command.WorkflowInstanceId,
                        NextActivityId = nextActivity.Id,
                        ActivityType = nextActivity.Type
                    };
                }
                else
                {
                    throw new Exception($"Unable to find any questions for Question Screen activity Workflow Instance Id: {command.WorkflowInstanceId} and Activity Id: {command.ActivityId} in custom database");
                }
            }

            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private async Task DeleteChangedWorkflowPath(QuestionScreenSaveAndContinueCommand command,
            CancellationToken cancellationToken, IActivityBlueprint nextActivity, WorkflowInstance workflowInstance)
        {
            var changedPathCustomNavigation =
                await _workflowPathProvider.GetChangedPathCustomNavigation(command.WorkflowInstanceId, command.ActivityId,
                    nextActivity.Id, cancellationToken);

            if (changedPathCustomNavigation != null)
            {
                await _elsaCustomRepository.DeleteCustomNavigation(changedPathCustomNavigation, cancellationToken);
                var previousPathActivities =
                    await _workflowPathProvider.GetPreviousPathActivities(workflowInstance.DefinitionId,
                        changedPathCustomNavigation.ActivityId, cancellationToken);

                await _elsaCustomRepository.DeleteQuestionScreenAnswers(
                    changedPathCustomNavigation.WorkflowInstanceId, previousPathActivities, cancellationToken);
            }
        }



        private async Task SetAnswers(QuestionScreenSaveAndContinueCommand command, CancellationToken cancellationToken,
            List<QuestionScreenAnswer> dbAssessmentQuestionList)
        {
            if (command.Answers != null && command.Answers.Any())
            {
                foreach (var question in dbAssessmentQuestionList)
                {
                    var answer = command.Answers.FirstOrDefault(x => x.Id == question.QuestionId);

                    if (answer != null)
                    {
                        question.SetAnswer(answer.AnswerText, _dateTimeProvider.UtcNow());
                        question.Comments = answer.Comments;
                    }
                }

                await _elsaCustomRepository.SaveChanges(cancellationToken);
            }
        }
    }
}
