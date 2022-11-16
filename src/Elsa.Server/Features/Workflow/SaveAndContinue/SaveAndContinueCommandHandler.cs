using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, OperationResult<SaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ISaveAndContinueMapper _saveAndContinueMapper;


        public SaveAndContinueCommandHandler(IQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository, IDateTimeProvider dateTimeProvider, IWorkflowRegistry workflowRegistry, ISaveAndContinueMapper saveAndContinueMapper)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _dateTimeProvider = dateTimeProvider;
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowRegistry = workflowRegistry;
            _saveAndContinueMapper = saveAndContinueMapper;
        }

        public async Task<OperationResult<SaveAndContinueResponse>> Handle(SaveAndContinueCommand command, CancellationToken cancellationToken)
        {
            var result = new OperationResult<SaveAndContinueResponse>();
            try
            {
                var dbAssessmentActivityInstance =
                    await _elsaCustomRepository.GetAssessmentQuestion(command.ActivityId, command.WorkflowInstanceId, cancellationToken);
                if (dbAssessmentActivityInstance != null)
                {
                    dbAssessmentActivityInstance.Comments = command.Comments;

                    dbAssessmentActivityInstance.SetAnswer(command.Answer, _dateTimeProvider.UtcNow()); //use DateTimeProvider
                    await _elsaCustomRepository.UpdateAssessmentQuestion(dbAssessmentActivityInstance,
                        cancellationToken);

                    var collectedWorkflow = await _invoker.ExecuteWorkflowsAsync(command.ActivityId, dbAssessmentActivityInstance.ActivityType,
                    command.WorkflowInstanceId, dbAssessmentActivityInstance, cancellationToken).FirstOrDefault();

                    var workflowSpecification =
                        new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
                    var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken);

                    if (workflowInstance != null)
                    {
                        if (workflowInstance.Output != null)
                        {
                            var nextActivityId = workflowInstance.Output.ActivityId;

                            var workflow =
                                await _workflowRegistry.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);

                            var nextActivity = workflow!.Activities.FirstOrDefault(x =>
                                x.Id == nextActivityId);

                            if (nextActivity != null)
                            {
                                var nextActivityRecord =
                                    await _elsaCustomRepository.GetAssessmentQuestion(nextActivityId,
                                        command.WorkflowInstanceId, cancellationToken);

                                if (nextActivityRecord == null)
                                {
                                    await CreateNextActivityRecord(command, nextActivityId, nextActivity, workflowInstance);
                                }

                                result.Data = new SaveAndContinueResponse
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
                    result.ErrorMessages.Add(
                        $"Unable to find activity instance with Workflow Instance Id: {command.WorkflowInstanceId} and Activity Id: {command.ActivityId} in custom database");
                }
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);


        }
        private async Task CreateNextActivityRecord(SaveAndContinueCommand command, string nextActivityId, IActivityBlueprint nextActivityBlueprint, WorkflowInstance workflowInstance)
        {
            var assessmentQuestion = _saveAndContinueMapper.SaveAndContinueCommandToNextAssessmentQuestion(command, nextActivityId, nextActivityBlueprint.Type);
            await _elsaCustomRepository.CreateAssessmentQuestionAsync(assessmentQuestion);

            if (nextActivityBlueprint.Type == "QuestionScreen")
            {
                //create one for each question
                var dictionList = workflowInstance.ActivityData
                    .FirstOrDefault(x => x.Key == nextActivityId).Value;

                var dictionaryQuestions = (AssessmentQuestions?)dictionList.FirstOrDefault(x => x.Key == "Questions").Value;

                if (dictionaryQuestions != null)
                {
                    var questionList = (List<Question>)dictionaryQuestions.Questions;
                    if (questionList!.Any())
                    {
                        var assessments = new List<AssessmentQuestion>();

                        foreach (var item in questionList!)
                        {
                            assessments.Add(_saveAndContinueMapper.SaveAndContinueCommandToNextAssessmentQuestion(command, nextActivityId, nextActivityBlueprint.Type, item));
                        }
                        await _elsaCustomRepository.CreateAssessmentQuestionAsync(assessments, CancellationToken.None);
                    }
                }
            }
        }
    }
}
