using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using MediatR;
using Open.Linq.AsyncExtensions;


namespace Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandHandler : IRequestHandler<QuestionScreenSaveAndContinueCommand,
        OperationResult<QuestionScreenSaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IQuestionScreenSaveAndContinueMapper _saveAndContinueMapper;


        public QuestionScreenSaveAndContinueCommandHandler(IQuestionInvoker invoker,
            IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository,
            IDateTimeProvider dateTimeProvider, IWorkflowRegistry workflowRegistry,
            IQuestionScreenSaveAndContinueMapper saveAndContinueMapper)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _dateTimeProvider = dateTimeProvider;
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowRegistry = workflowRegistry;
            _saveAndContinueMapper = saveAndContinueMapper;
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

                if (dbAssessmentQuestionList != null && dbAssessmentQuestionList.Any())
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

                    var collectedWorkflow = await _invoker.ExecuteWorkflowsAsync(command.ActivityId, ActivityTypeConstants.QuestionScreen,
                        command.WorkflowInstanceId, dbAssessmentQuestionList, cancellationToken).FirstOrDefault();

                    var workflowSpecification =
                        new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
                    var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken);

                    if (workflowInstance != null)
                    {
                        if (workflowInstance.Output != null)
                        {
                            var nextActivityId = workflowInstance.Output.ActivityId;

                            var workflow =
                                await _workflowRegistry.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published,
                                    cancellationToken: cancellationToken);

                            var nextActivity = workflow!.Activities.FirstOrDefault(x =>
                                x.Id == nextActivityId);

                            if (nextActivity != null)
                            {
                                var nextActivityRecord =
                                    await _elsaCustomRepository.GetCustomActivityNavigation(nextActivityId,
                                        command.WorkflowInstanceId, cancellationToken);

                                if (nextActivityRecord == null)
                                {
                                    await CreateNextActivityRecord(command, nextActivityId, nextActivity.Type, workflowInstance);
                                }

                                result.Data = new QuestionScreenSaveAndContinueResponse
                                {
                                    WorkflowInstanceId = command.WorkflowInstanceId,
                                    NextActivityId = nextActivityId,
                                    ActivityType = nextActivity.Type
                                };

                            }
                            else
                            {
                                result.ErrorMessages.Add(
                                    $"Unable to determine next activity ID");
                            }
                        }
                        else
                        {
                            result.ErrorMessages.Add(
                                $"Workflow instance output for workflow instance Id {command.WorkflowInstanceId} is not set. Unable to determine next activity");
                        }
                    }
                    else
                    {
                        result.ErrorMessages.Add(
                            $"Unable to find workflow instance with Id: {command.WorkflowInstanceId} in Elsa database");
                    }
                }
                else
                {
                    result.ErrorMessages.Add($"Unable to find any questions for Question Screen activity Workflow Instance Id: {command.WorkflowInstanceId} and Activity Id: {command.ActivityId} in custom database");
                }
            }


            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private async Task CreateNextActivityRecord(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string nextActivityType, WorkflowInstance workflowInstance)
        {
            var assessmentQuestion = _saveAndContinueMapper.SaveAndContinueCommandToNextCustomActivityNavigation(command, nextActivityId, nextActivityType);
            await _elsaCustomRepository.CreateCustomActivityNavigationAsync(assessmentQuestion);

            if (nextActivityType == ActivityTypeConstants.QuestionScreen)
            {
                //create one for each question
                var dictionList = workflowInstance.ActivityData
                    .FirstOrDefault(x => x.Key == nextActivityId).Value;

                if (dictionList != null)
                {
                    var dictionaryQuestions = (AssessmentQuestions?)dictionList.FirstOrDefault(x => x.Key == "Questions").Value;

                    if (dictionaryQuestions != null)
                    {
                        var questionList = (List<Question>)dictionaryQuestions.Questions;
                        if (questionList!.Any())
                        {
                            var assessments = new List<QuestionScreenAnswer>();

                            foreach (var item in questionList!)
                            {
                                assessments.Add(_saveAndContinueMapper.SaveAndContinueCommandToQuestionScreenAnswer(command, nextActivityId, nextActivityType, item));
                            }
                            await _elsaCustomRepository.CreateQuestionScreenAnswersAsync(assessments, CancellationToken.None);
                        }
                    }
                }
            }
        }

    }
}
