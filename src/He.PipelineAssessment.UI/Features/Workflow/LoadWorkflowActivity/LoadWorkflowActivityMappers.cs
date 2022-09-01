using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity
{
    public interface ILoadWorkflowActivityMapper
    {
        SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto workflowActivityDataDto);
    }

    public class LoadWorkflowActivityMapper : ILoadWorkflowActivityMapper
    {
        public SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto workflowActivityDataDto)
        {
            var result = new SaveAndContinueCommand();

            result.IsValid = workflowActivityDataDto.IsValid;
            result.ValidationMessages = new List<string>();

            if (!workflowActivityDataDto.IsValid)
            {
                foreach (var message in workflowActivityDataDto.ValidationMessages)
                {
                    result.ValidationMessages.Add(message);
                }
            }

            result.Data = new SaveAndContinue.WorkflowActivityData();

            result.Data.WorkflowInstanceId = workflowActivityDataDto.Data.WorkflowInstanceId;
            result.Data.ActivityId = workflowActivityDataDto.Data.ActivityId;
            result.Data.ActivityType = workflowActivityDataDto.Data.ActivityType;
            result.Data.PreviousActivityId = workflowActivityDataDto.Data.PreviousActivityId;

            result.Data.MultipleChoiceQuestionActivityData = new SaveAndContinue.MultipleChoiceQuestionActivityData();

            if (result.Data.ActivityType == ActivityTypeConstants.MultipleChoiceQuestion)
            {
                result.Data.MultipleChoiceQuestionActivityData.Title = workflowActivityDataDto.Data.MultipleChoiceQuestionActivityData.Title;
                result.Data.MultipleChoiceQuestionActivityData.Question = workflowActivityDataDto.Data.MultipleChoiceQuestionActivityData.Question;
                result.Data.MultipleChoiceQuestionActivityData.QuestionHint = workflowActivityDataDto.Data.MultipleChoiceQuestionActivityData.QuestionHint;
                result.Data.MultipleChoiceQuestionActivityData.QuestionGuidance = workflowActivityDataDto.Data.MultipleChoiceQuestionActivityData.QuestionGuidance;
                result.Data.MultipleChoiceQuestionActivityData.Output = workflowActivityDataDto.Data.MultipleChoiceQuestionActivityData.Output;

                var mappedChoiceList = new List<SaveAndContinue.Choice>();
                foreach (var choice in workflowActivityDataDto.Data.MultipleChoiceQuestionActivityData.Choices)
                {
                    var mappedChoice = new SaveAndContinue.Choice()
                    {
                        Answer = choice.Answer,
                        IsSelected = choice.IsSelected,
                        IsSingle = choice.IsSingle
                    };
                    mappedChoiceList.Add(mappedChoice);
                }

                result.Data.MultipleChoiceQuestionActivityData.Choices = mappedChoiceList.ToArray();
            }
            result.Data.CurrencyQuestionActivityData = new SaveAndContinue.CurrencyQuestionActivityData();

            if (result.Data.ActivityType == ActivityTypeConstants.CurrencyQuestion)
            {
                result.Data.CurrencyQuestionActivityData.Title = workflowActivityDataDto.Data.CurrencyQuestionActivityData.Title;
                result.Data.CurrencyQuestionActivityData.Question = workflowActivityDataDto.Data.CurrencyQuestionActivityData.Question;
                result.Data.CurrencyQuestionActivityData.QuestionHint = workflowActivityDataDto.Data.CurrencyQuestionActivityData.QuestionHint;
                result.Data.CurrencyQuestionActivityData.QuestionGuidance = workflowActivityDataDto.Data.CurrencyQuestionActivityData.QuestionGuidance;
                result.Data.CurrencyQuestionActivityData.Output = workflowActivityDataDto.Data.CurrencyQuestionActivityData.Output;
                result.Data.CurrencyQuestionActivityData.Answer = workflowActivityDataDto.Data.CurrencyQuestionActivityData.Answer;
            }
            result.Data.TextQuestionActivityData = new SaveAndContinue.TextQuestionActivityData();

            if (result.Data.ActivityType == ActivityTypeConstants.TextQuestion)
            {
                result.Data.TextQuestionActivityData.Title = workflowActivityDataDto.Data.TextQuestionActivityData.Title;
                result.Data.TextQuestionActivityData.Question = workflowActivityDataDto.Data.TextQuestionActivityData.Question;
                result.Data.TextQuestionActivityData.QuestionHint = workflowActivityDataDto.Data.TextQuestionActivityData.QuestionHint;
                result.Data.TextQuestionActivityData.QuestionGuidance = workflowActivityDataDto.Data.TextQuestionActivityData.QuestionGuidance;
                result.Data.TextQuestionActivityData.Output = workflowActivityDataDto.Data.TextQuestionActivityData.Output;
                result.Data.TextQuestionActivityData.Answer = workflowActivityDataDto.Data.TextQuestionActivityData.Answer;
            }
            result.Data.DateQuestionActivityData = new SaveAndContinue.DateQuestionActivityData();

            if (result.Data.ActivityType == ActivityTypeConstants.DateQuestion)
            {
                result.Data.DateQuestionActivityData.Title = workflowActivityDataDto.Data.DateQuestionActivityData.Title;
                result.Data.DateQuestionActivityData.Question = workflowActivityDataDto.Data.DateQuestionActivityData.Question;
                result.Data.DateQuestionActivityData.QuestionHint = workflowActivityDataDto.Data.DateQuestionActivityData.QuestionHint;
                result.Data.DateQuestionActivityData.QuestionGuidance = workflowActivityDataDto.Data.DateQuestionActivityData.QuestionGuidance;
                result.Data.DateQuestionActivityData.Output = workflowActivityDataDto.Data.DateQuestionActivityData.Output;
                result.Data.DateQuestionActivityData.Answer = workflowActivityDataDto.Data.DateQuestionActivityData.Answer;
                result.Data.DateQuestionActivityData.Day = workflowActivityDataDto.Data.DateQuestionActivityData.DayFromDate();
                result.Data.DateQuestionActivityData.Month = workflowActivityDataDto.Data.DateQuestionActivityData.MonthFromDate();
                result.Data.DateQuestionActivityData.Year = workflowActivityDataDto.Data.DateQuestionActivityData.YearFromDate();

            }


            return result;
        }
    }
}
