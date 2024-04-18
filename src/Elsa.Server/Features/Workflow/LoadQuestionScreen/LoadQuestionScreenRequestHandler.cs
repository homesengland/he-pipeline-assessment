using System.Text.Json;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Providers;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Mappers;
using Elsa.Server.Models;
using FluentValidation.Results;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenRequestHandler : IRequestHandler<LoadQuestionScreenRequest, OperationResult<LoadQuestionScreenResponse>>
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IQuestionInvoker _invoker;
        private readonly ITextGroupMapper _textGroupMapper;
        private readonly ILogger<LoadQuestionScreenRequestHandler> _logger;

        public LoadQuestionScreenRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository, IQuestionInvoker invoker, ILogger<LoadQuestionScreenRequestHandler> logger, ITextGroupMapper textGroupMapper)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _elsaCustomRepository = elsaCustomRepository;
            _invoker = invoker;
            _logger = logger;
            _textGroupMapper = textGroupMapper;
        }

        public async Task<OperationResult<LoadQuestionScreenResponse>> Handle(LoadQuestionScreenRequest activityRequest, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadQuestionScreenResponse>
            {
                Data = new LoadQuestionScreenResponse
                {
                    WorkflowInstanceId = activityRequest.WorkflowInstanceId,
                    ActivityId = activityRequest.ActivityId
                }
            };
            try
            {
                var customActivityNavigation =
                    await _elsaCustomRepository.GetCustomActivityNavigation(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);

                if (customActivityNavigation != null)
                {
                    if (customActivityNavigation.ActivityType != ActivityTypeConstants.QuestionScreen)
                    {
                        result.ErrorMessages.Add(
                            $"Attempted to load question screen with {customActivityNavigation.ActivityType} activity type");

                        _logger.LogError($"Attempted to load question screen with {customActivityNavigation.ActivityType} activity type");

                        return await Task.FromResult(result);
                    }
                    var workflowSpecification =
                        new WorkflowInstanceIdSpecification(activityRequest.WorkflowInstanceId);

                    var dbAssessmentQuestionList =
                        await _elsaCustomRepository.GetActivityQuestions(activityRequest.ActivityId, activityRequest.WorkflowInstanceId,
                            cancellationToken);
                    if (dbAssessmentQuestionList.SelectMany(x => x.Answers!).Any())
                    {
                        await _invoker.ExecuteWorkflowsAsync(activityRequest.ActivityId,
                            customActivityNavigation.ActivityType,
                            activityRequest.WorkflowInstanceId, dbAssessmentQuestionList, cancellationToken);
                    }

                    var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken: cancellationToken);
                    if (workflowInstance != null)
                    {
                        if (!workflowInstance.ActivityData.ContainsKey(activityRequest.ActivityId))
                        {
                            _logger.LogError($"Cannot find activity Id {activityRequest.ActivityId} in the workflow activity data dictionary");

                            result.ErrorMessages.Add(
                                $"Cannot find activity Id {activityRequest.ActivityId} in the workflow activity data dictionary");
                        }
                        else
                        {
                            var activityDataDictionary =
                                workflowInstance.ActivityData
                                    .FirstOrDefault(a => a.Key == activityRequest.ActivityId).Value;

                            result.Data.ActivityType = customActivityNavigation.ActivityType;
                            result.Data.PreviousActivityId = customActivityNavigation.PreviousActivityId;
                            result.Data.PreviousActivityType = customActivityNavigation.PreviousActivityType;

                            var title = (string?)activityDataDictionary.FirstOrDefault(x => x.Key == "PageTitle").Value;
                            result.Data.PageTitle = title;

                            var elsaActivityAssessmentQuestions =
                                (AssessmentQuestions?)activityDataDictionary
                                    .FirstOrDefault(x => x.Key == "Questions").Value;

                            if (elsaActivityAssessmentQuestions != null)
                            {
                                result.Data.Questions = new List<QuestionActivityData>();
                                result.Data.ActivityType = customActivityNavigation.ActivityType;

                                int questionIndex = 0;
                                foreach (var item in elsaActivityAssessmentQuestions.Questions)
                                {
                                    //get me the item
                                    var dbQuestion =
                                        dbAssessmentQuestionList.FirstOrDefault(x => x.QuestionId == item.Id);
                                    if (dbQuestion != null)
                                    {
                                        var questionActivityData = CreateQuestionActivityData(dbQuestion, item, _textGroupMapper, questionIndex);

                                        result.Data.Questions.Add(questionActivityData);
                                    }
                                    questionIndex++;
                                }
                                result.ValidationMessages = MapValidationModelToValidationResults(elsaActivityAssessmentQuestions.Questions);
                            }
                            else
                            {
                                _logger.LogError($"Failed to map activity data to Questions WorkflowInstanceId: {activityRequest.WorkflowInstanceId}");

                                result.ErrorMessages.Add(
                                    $"Failed to map activity data to Questions");
                            }
                        }
                    }
                    else
                    {
                        _logger.LogError($"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} in Elsa database");

                        result.ErrorMessages.Add(
                            $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} in Elsa database");
                    }
                }
                else
                {
                    _logger.LogError($"Unable to find activity navigation with Workflow Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId} in Elsa Custom database");

                    result.ErrorMessages.Add(
                        $"Unable to find activity navigation with Workflow Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId} in Elsa Custom database");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private static QuestionActivityData CreateQuestionActivityData(CustomModels.Question dbQuestion,
            CustomActivities.Activities.QuestionScreen.Question item, ITextGroupMapper textGroupMapper, int questionIndex)
        {
            var reevaluatePrepopulatedAnswers = item.ReevaluatePrePopulatedAnswers;

            List<Answer> answers = dbQuestion.Answers!.Any() && !reevaluatePrepopulatedAnswers
                ? dbQuestion.Answers!
                : new List<Answer> { new Answer { AnswerText = item.Answer ?? string.Empty } };
            //assign the values
            var questionActivityData = new QuestionActivityData();
            questionActivityData.ActivityId = dbQuestion.ActivityId;
            questionActivityData.Answers = answers;
            questionActivityData.Comments = dbQuestion.Comments;
            questionActivityData.DocumentEvidenceLink = dbQuestion.DocumentEvidenceLink;
            questionActivityData.QuestionId = dbQuestion.QuestionId;
            questionActivityData.QuestionType = dbQuestion.QuestionType;

            questionActivityData.Question = item.QuestionText;
            questionActivityData.DisplayComments = item.DisplayComments;
            questionActivityData.DisplayEvidenceBox = item.DisplayEvidenceBox;
            questionActivityData.QuestionHint = item.QuestionHint;
            questionActivityData.CharacterLimit = item.CharacterLimit;
            questionActivityData.IsReadOnly = item.IsReadOnly;
            questionActivityData.ReevaluatePrepopulatedAnswers = item.ReevaluatePrePopulatedAnswers;
            questionActivityData.HideQuestion = item.HideQuestion;
            
            if (item.QuestionType == QuestionTypeConstants.CheckboxQuestion ||
                item.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion)
            {
                if (dbQuestion.Choices != null)
                {
                    questionActivityData.Checkbox = new Checkbox
                    {
                        Choices = dbQuestion.Choices.Select(x => new QuestionChoice()
                        {
                            Answer = x.Answer, IsSingle = x.IsSingle, IsExclusiveToQuestion = x.IsExclusiveToQuestion,
                            Id = x.Id, QuestionChoiceGroup = x.QuestionChoiceGroup
                        }).ToArray()
                    };

                    List<int> answerList;
                    if (dbQuestion.Answers != null && dbQuestion.Answers.Any() && !reevaluatePrepopulatedAnswers)
                    {
                        answerList = dbQuestion.Answers.Select(x => x.Choice!.Id).ToList();
                    }
                    else
                    {
                        if (item.QuestionType == QuestionTypeConstants.CheckboxQuestion)
                        {
                            var prepopulatedAnswerListIdentifiers = item.Checkbox.Choices.Where(x => x.IsPrePopulated).Select(x => x.Identifier).ToList();
                            answerList = dbQuestion.Choices.Where(x => prepopulatedAnswerListIdentifiers.Contains(x.Identifier)).Select(x => x.Id).ToList();
                        }
                        else
                        {
                            var prepopulatedAnswerListIdentifiers = item.WeightedCheckbox.Groups.Values.SelectMany(x => x.Choices.Where(y => y.IsPrePopulated)
                            .Select(z => new { Id = z.Identifier, GroupId = x.GroupIdentifier})).ToList();
                            
                            var groups = dbQuestion.Choices.Select(x => x.QuestionChoiceGroup);
                            
                            answerList = dbQuestion.Choices.Where(x => prepopulatedAnswerListIdentifiers.Any(y => {
                                var g = groups.FirstOrDefault(z => z!.Id == x.QuestionChoiceGroupId);
                                return y.Id == x.Identifier && g != null && g.GroupIdentifier == y.GroupId;
                            })).Select(x => x.Id).ToList(); ;
                        } 
                    }

                    questionActivityData.Checkbox.SelectedChoices = answerList;
                }
            }

            if (item.QuestionType == QuestionTypeConstants.RadioQuestion ||
                item.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion ||
                item.QuestionType == QuestionTypeConstants.WeightedRadioQuestion)
            {
                if (dbQuestion.Choices != null)
                {
                    questionActivityData.Radio = new Radio
                    {
                        Choices = dbQuestion.Choices
                            .Select(x => new QuestionChoice() { Answer = x.Answer, Id = x.Id })
                            .ToArray()
                    };
                    if (dbQuestion.Answers != null && dbQuestion.Answers.Any() && !reevaluatePrepopulatedAnswers)
                    {
                        questionActivityData.Radio.SelectedAnswer = dbQuestion.Answers.First().Choice!.Id;
                    }
                    else
                    {
                        if (item.QuestionType == QuestionTypeConstants.RadioQuestion)
                        {
                            var prepopulatedAnswer = item.Radio.Choices.FirstOrDefault(x => x.IsPrePopulated);
                            if (prepopulatedAnswer != null)
                            {
                                var databaseChoiceAnswer = dbQuestion.Choices.Where(x => x.Identifier == prepopulatedAnswer.Identifier).FirstOrDefault();
                                if (databaseChoiceAnswer != null)
                                {
                                    questionActivityData.Radio.SelectedAnswer = databaseChoiceAnswer.Id;
                                }
                            }
                        }
                        else if (item.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion)
                        {
                            var prepopulatedAnswer = item.PotScoreRadio.Choices.FirstOrDefault(x => x.IsPrePopulated);
                            if (prepopulatedAnswer != null)
                            {
                                var databaseChoiceAnswer = dbQuestion.Choices.Where(x => x.Identifier == prepopulatedAnswer.Identifier).FirstOrDefault();
                                if (databaseChoiceAnswer != null)
                                {
                                    questionActivityData.Radio.SelectedAnswer = databaseChoiceAnswer.Id;
                                }
                            }
                        }
                        else if(item.QuestionType == QuestionTypeConstants.WeightedRadioQuestion)
                        {
                            var prepopulatedAnswer = item.WeightedRadio.Choices.FirstOrDefault(x => x.IsPrePopulated);
                            if (prepopulatedAnswer != null)
                            {
                                var databaseChoiceAnswer = dbQuestion.Choices.Where(x => x.Identifier == prepopulatedAnswer.Identifier).FirstOrDefault();
                                if (databaseChoiceAnswer != null)
                                {
                                    questionActivityData.Radio.SelectedAnswer = databaseChoiceAnswer.Id;
                                }
                            }
                        }
                    }
                }
            }

            if (item.QuestionType == QuestionTypeConstants.Information)
            {
                questionActivityData.Information = textGroupMapper.InformationTextGroupListFromTextGroupsForInformation(item.Text.TextGroups);
            }

            if (item.EnhancedGuidance.TextGroups.Any())
            {
                questionActivityData.EnhancedGuidance = textGroupMapper.InformationTextGroupListFromTextGroupsForGuidance(item.EnhancedGuidance.TextGroups);
            }
            else
            {
                #pragma warning disable 0612, 0618
                if (!string.IsNullOrEmpty(item.QuestionGuidance))
                {
                    questionActivityData.EnhancedGuidance = textGroupMapper.InformationTextGroupListFromGuidanceString(item.QuestionGuidance);
                }
                #pragma warning restore 0612, 0618
            }

            if (item.QuestionType == QuestionTypeConstants.DataTable)
            {
                questionActivityData.DataTable = new DataTableInput();
                DataTableInput? dataTableInput = null;
                if (dbQuestion.Answers != null && dbQuestion.Answers.Any() && !reevaluatePrepopulatedAnswers &&
                    dbQuestion.Answers.FirstOrDefault() != null)
                {
                    var dataTable = JsonSerializer.Deserialize<DataTable>(dbQuestion.Answers.First().AnswerText);
                    if (dataTable != null)
                    {
                        dataTableInput = new DataTableInput
                        {
                            Inputs = dataTable.Inputs.ToArray(),
                            InputType = dataTable.InputType,
                            DisplayGroupId = dataTable.DisplayGroupId
                        };
                    }
                }

                if (dataTableInput == null)
                {
                    dataTableInput = new DataTableInput
                    {
                        Inputs = item.DataTable.Inputs.ToArray(),
                        InputType = item.DataTable.InputType,
                        DisplayGroupId = item.DataTable.DisplayGroupId

                    };
                }

                questionActivityData.DataTable = dataTableInput;
            }

            return questionActivityData;
        }

        private ValidationResult MapValidationModelToValidationResults(List<CustomActivities.Activities.QuestionScreen.Question> items)
        {
            var validationFailures = new List<ValidationFailure>();
            for (int i = 0; i < items.Count; i++)
            {
                foreach (var validation in items[i].Validations.Validations)
                {

                    if (!validation.IsValid && !string.IsNullOrEmpty(validation.ValidationMessage))
                    {
                        var failure = new ValidationFailure()
                        {
                            PropertyName = ValidationPropertyNameProvider.GetPropertyName(items[i].QuestionType, i),
                            ErrorMessage = validation.ValidationMessage
                        };
                        validationFailures.Add(failure);
                    }
                }
            }
            var validationResult = new ValidationResult(validationFailures);
            return validationResult;
        }
    }
}
