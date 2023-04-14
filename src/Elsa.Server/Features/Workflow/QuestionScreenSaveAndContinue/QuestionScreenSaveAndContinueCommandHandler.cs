using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Providers;
using Elsa.Models;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using MediatR;

namespace Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandHandler : IRequestHandler<QuestionScreenSaveAndContinueCommand,
        OperationResult<QuestionScreenSaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly INextActivityNavigationService _nextActivityNavigationService;
        private readonly IDeleteChangedWorkflowPathService _deleteChangedWorkflowPathService;


        public QuestionScreenSaveAndContinueCommandHandler(
            IElsaCustomRepository elsaCustomRepository,
            IWorkflowInstanceProvider workflowInstanceProvider,
            IDateTimeProvider dateTimeProvider,
            IWorkflowNextActivityProvider workflowNextActivityProvider,
            INextActivityNavigationService nextActivityNavigationService,
            IDeleteChangedWorkflowPathService deleteChangedWorkflowPathService)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowInstanceProvider = workflowInstanceProvider;
            _dateTimeProvider = dateTimeProvider;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _nextActivityNavigationService = nextActivityNavigationService;
            _deleteChangedWorkflowPathService = deleteChangedWorkflowPathService;
        }

        public async Task<OperationResult<QuestionScreenSaveAndContinueResponse>> Handle(QuestionScreenSaveAndContinueCommand command,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<QuestionScreenSaveAndContinueResponse>();
            try
            {
                var dbAssessmentQuestionList =
                    await _elsaCustomRepository.GetQuestions(command.ActivityId, command.WorkflowInstanceId,
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

                    var workflowNextActivityModel = await _workflowNextActivityProvider.GetNextActivity(command.ActivityId, command.WorkflowInstanceId, dbAssessmentQuestionList, ActivityTypeConstants.QuestionScreen, cancellationToken);
                    if (workflowNextActivityModel.WorkflowInstance != null)
                        workflowInstance = workflowNextActivityModel.WorkflowInstance;
                    var nextActivityRecord = await _elsaCustomRepository.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id, command.WorkflowInstanceId, cancellationToken);

                    await _deleteChangedWorkflowPathService.DeleteChangedWorkflowPath(command.WorkflowInstanceId, command.ActivityId, workflowNextActivityModel.NextActivity, workflowInstance, cancellationToken);

                    await _nextActivityNavigationService.CreateNextActivityNavigation(command.ActivityId, nextActivityRecord, workflowNextActivityModel.NextActivity, workflowInstance, cancellationToken);

                    result.Data = new QuestionScreenSaveAndContinueResponse
                    {
                        WorkflowInstanceId = command.WorkflowInstanceId,
                        NextActivityId = workflowNextActivityModel.NextActivity.Id,
                        ActivityType = workflowNextActivityModel.NextActivity.Type
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

        private async Task SetAnswers(QuestionScreenSaveAndContinueCommand command, CancellationToken cancellationToken,
            List<Question> dbAssessmentQuestionList)
        {
            if (command.Answers != null && command.Answers.Any())
            {
                foreach (var question in dbAssessmentQuestionList)
                {
                    var answers = command.Answers.Where(x => x.WorkflowQuestionId == question.QuestionId);
                    var now = _dateTimeProvider.UtcNow();
                    if (answers.Any())
                    {
                        question.Answers = answers.Select(x => new CustomModels.Answer
                        {
                            AnswerText = x.AnswerText ?? "",
                            CreatedDateTime = now,
                            LastModifiedDateTime = now,
                            Choice = question.Choices?.FirstOrDefault(y => y.Id == x.ChoiceId)
                        }).ToList();
                        question.Comments = answers.First().Comments;
                        question.LastModifiedDateTime = now;
                    }
                }

                await _elsaCustomRepository.SaveChanges(cancellationToken);
            }
        }
    }
}
