using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using MediatR;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandHandler : IRequestHandler<CheckYourAnswersSaveAndContinueCommand,
        OperationResult<CheckYourAnswersSaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ICheckYourAnswersSaveAndContinueMapper _saveAndContinueMapper;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly IWorkflowInstanceProvider _workflowInstanceProvider;


        public CheckYourAnswersSaveAndContinueCommandHandler(IQuestionInvoker invoker,
            IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository,
            IDateTimeProvider dateTimeProvider, IWorkflowRegistry workflowRegistry,
            ICheckYourAnswersSaveAndContinueMapper saveAndContinueMapper,
            IWorkflowNextActivityProvider workflowNextActivityProvider,
            IWorkflowInstanceProvider workflowInstanceProvider)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _dateTimeProvider = dateTimeProvider;
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowRegistry = workflowRegistry;
            _saveAndContinueMapper = saveAndContinueMapper;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _workflowInstanceProvider = workflowInstanceProvider;
        }

        public async Task<OperationResult<CheckYourAnswersSaveAndContinueResponse>> Handle(CheckYourAnswersSaveAndContinueCommand command,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<CheckYourAnswersSaveAndContinueResponse>();
            try
            {
                var collectedWorkflow = await _invoker.ExecuteWorkflowsAsync(command.ActivityId, ActivityTypeConstants.CheckYourAnswersScreen,
                    command.WorkflowInstanceId, null, cancellationToken).FirstOrDefault();

                var workflowInstance = await _workflowInstanceProvider.GetWorkflowInstance(command.WorkflowInstanceId, cancellationToken);
                var nextActivity = await _workflowNextActivityProvider.GetNextActivity(workflowInstance, cancellationToken);

                var nextActivityRecord =
                   await _elsaCustomRepository.GetCustomActivityNavigation(nextActivity.Id,
                       command.WorkflowInstanceId, cancellationToken);

                if (nextActivityRecord == null)
                {
                    await CreateNextActivityRecord(command, nextActivity.Id, nextActivity.Type, workflowInstance);
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

        private async Task CreateNextActivityRecord(CheckYourAnswersSaveAndContinueCommand command, string nextActivityId, string nextActivityType, WorkflowInstance workflowInstance)
        {
            var assessmentQuestion = _saveAndContinueMapper.saveAndContinueCommandToNextCustomActivityNavigation(command, nextActivityId, nextActivityType, workflowInstance);
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
                                assessments.Add(_saveAndContinueMapper.SaveAndContinueCommandToQuestionScreenAnswer(nextActivityId, nextActivityType, item, workflowInstance));
                            }
                            await _elsaCustomRepository.CreateQuestionScreenAnswersAsync(assessments, CancellationToken.None);
                        }
                    }
                }
            }
        }

    }
}