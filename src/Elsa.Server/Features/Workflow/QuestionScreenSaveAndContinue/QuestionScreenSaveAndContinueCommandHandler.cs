using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Features.Workflow.Helpers;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using MediatR;

namespace Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandHandler : IRequestHandler<QuestionScreenSaveAndContinueCommand,
        OperationResult<QuestionScreenSaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IQuestionInvoker _invoker;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ISaveAndContinueHelper _saveAndContinueHelper;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;


        public QuestionScreenSaveAndContinueCommandHandler(IQuestionInvoker invoker,
            IElsaCustomRepository elsaCustomRepository,
            ISaveAndContinueHelper saveAndContinueHelper,
            IWorkflowNextActivityProvider workflowNextActivityProvider,
            IWorkflowInstanceProvider workflowInstanceProvider,
            IDateTimeProvider dateTimeProvider)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _saveAndContinueHelper = saveAndContinueHelper;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _workflowInstanceProvider = workflowInstanceProvider;
            _dateTimeProvider = dateTimeProvider;
            _invoker = invoker;
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

                    var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(command.ActivityId,
                        ActivityTypeConstants.QuestionScreen,
                        command.WorkflowInstanceId, dbAssessmentQuestionList, cancellationToken);

                    //we need to refresh the workflowInstance, as calling the invoker will change it
                    workflowInstance =
                        await _workflowInstanceProvider.GetWorkflowInstance(command.WorkflowInstanceId,
                            cancellationToken);

                    if (!collectedWorkflows.Any())
                    {
                        throw new Exception($"Unable to progress workflow. Workflow status is: {workflowInstance.WorkflowStatus}");
                    }

                    var nextActivity =
                        await _workflowNextActivityProvider.GetNextActivity(workflowInstance, cancellationToken);

                    var nextActivityRecord =
                        await _elsaCustomRepository.GetCustomActivityNavigation(nextActivity.Id,
                            command.WorkflowInstanceId, cancellationToken);

                    if (nextActivityRecord == null)
                    {
                        var customActivityNavigation =
                            _saveAndContinueHelper.CreateNextCustomActivityNavigation(command.ActivityId,
                                ActivityTypeConstants.QuestionScreen, nextActivity.Id, nextActivity.Type,
                                workflowInstance);
                        await _elsaCustomRepository.CreateCustomActivityNavigationAsync(customActivityNavigation,
                            cancellationToken);

                        if (customActivityNavigation.ActivityType == ActivityTypeConstants.QuestionScreen)
                        {
                            var questions =
                                _saveAndContinueHelper.CreateQuestionScreenAnswers(nextActivity.Id, workflowInstance);
                            await _elsaCustomRepository.CreateQuestionScreenAnswersAsync(questions, cancellationToken);
                        }
                    }

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
