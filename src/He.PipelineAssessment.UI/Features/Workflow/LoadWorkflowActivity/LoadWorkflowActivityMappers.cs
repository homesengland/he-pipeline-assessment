using Elsa.CustomWorkflow.Sdk;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
//using CurrencyQuestionActivityData = Elsa.CustomWorkflow.Sdk.Models.Workflow.Archive.CurrencyQuestionActivityData;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Currency;
using Elsa.CustomWorkflow.Sdk.Models.Text;
using Elsa.CustomWorkflow.Sdk.Models.Date;
using Elsa.CustomWorkflow.Sdk.Models.MultipleChoice;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity
{
    public interface ILoadWorkflowActivityMapper
    {
        public SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto<CurrencyQuestionDataDto> workflowActivityDataDto);
        public SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto<TextQuestionDataDto> workflowActivityDataDto);
        public SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto<DateQuestionDataDto> workflowActivityDataDto);
        public SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto<MultipleChoiceQuestionDataDto> workflowActivityDataDto);
    }

    public class LoadWorkflowActivityMapper : ILoadWorkflowActivityMapper
    {
        public SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto<CurrencyQuestionDataDto> workflowActivityDataDto)
        {
            var result = SetupCommand<CurrencyQuestionDataDto, CurrencyQuestionActivityData>(workflowActivityDataDto);
           
            result.Data.ActivityData = new CurrencyQuestionActivityData();

            result.Data.ActivityData.Title = workflowActivityDataDto.Data.ActivityData.Title;
            result.Data.ActivityData.Question = workflowActivityDataDto.Data.ActivityData.Question;
            result.Data.ActivityData.QuestionHint = workflowActivityDataDto.Data.ActivityData.QuestionHint;
            result.Data.ActivityData.QuestionGuidance = workflowActivityDataDto.Data.ActivityData.QuestionGuidance;
            result.Data.ActivityData.Output = workflowActivityDataDto.Data.ActivityData.Output;
            result.Data.ActivityData.Answer = workflowActivityDataDto.Data.ActivityData.Answer;


            return result;
        }

        public SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto<TextQuestionDataDto> workflowActivityDataDto)
        {
            var result = SetupCommand<TextQuestionDataDto, TextQuestionActivityData>(workflowActivityDataDto);

            result.Data.ActivityData = new TextQuestionActivityData();

            result.Data.ActivityData.Title = workflowActivityDataDto.Data.ActivityData.Title;
            result.Data.ActivityData.Question = workflowActivityDataDto.Data.ActivityData.Question;
            result.Data.ActivityData.QuestionHint = workflowActivityDataDto.Data.ActivityData.QuestionHint;
            result.Data.ActivityData.QuestionGuidance = workflowActivityDataDto.Data.ActivityData.QuestionGuidance;
            result.Data.ActivityData.Output = workflowActivityDataDto.Data.ActivityData.Output;
            result.Data.ActivityData.Answer = workflowActivityDataDto.Data.ActivityData.Answer;


            return result;
        }

        public SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto<DateQuestionDataDto> workflowActivityDataDto)
        {
            var result = SetupCommand<DateQuestionDataDto, DateQuestionActivityData>(workflowActivityDataDto);

            result.Data.ActivityData = new DateQuestionActivityData();

            result.Data.ActivityData.Title = workflowActivityDataDto.Data.ActivityData.Title;
            result.Data.ActivityData.Question = workflowActivityDataDto.Data.ActivityData.Question;
            result.Data.ActivityData.QuestionHint = workflowActivityDataDto.Data.ActivityData.QuestionHint;
            result.Data.ActivityData.QuestionGuidance = workflowActivityDataDto.Data.ActivityData.QuestionGuidance;
            result.Data.ActivityData.Output = workflowActivityDataDto.Data.ActivityData.Output;
            result.Data.ActivityData.Answer = workflowActivityDataDto.Data.ActivityData.Answer;
            result.Data.ActivityData.Day = workflowActivityDataDto.Data.ActivityData.DayFromDate();
            result.Data.ActivityData.Month = workflowActivityDataDto.Data.ActivityData.MonthFromDate();
            result.Data.ActivityData.Year = workflowActivityDataDto.Data.ActivityData.YearFromDate();


            return result;
        }

        public SaveAndContinueCommand WorkflowActivityDataDtoToSaveAndContinueCommand(WorkflowActivityDataDto<MultipleChoiceQuestionDataDto> workflowActivityDataDto)
        {
            var result = SetupCommand<MultipleChoiceQuestionDataDto, MultipleChoiceQuestionActivityData>(workflowActivityDataDto);

            result.Data.ActivityData = new MultipleChoiceQuestionActivityData();

            result.Data.ActivityData.Title = workflowActivityDataDto.Data.ActivityData.Title;
            result.Data.ActivityData.Question = workflowActivityDataDto.Data.ActivityData.Question;
            result.Data.ActivityData.QuestionHint = workflowActivityDataDto.Data.ActivityData.QuestionHint;
            result.Data.ActivityData.QuestionGuidance = workflowActivityDataDto.Data.ActivityData.QuestionGuidance;
            result.Data.ActivityData.Output = workflowActivityDataDto.Data.ActivityData.Output;
            var mappedChoiceList = new List<SaveAndContinue.Choice>();
            foreach (var choice in workflowActivityDataDto.Data.ActivityData.Choices)
            {
                var mappedChoice = new SaveAndContinue.Choice()
                {
                    Answer = choice.Answer,
                    IsSelected = choice.IsSelected,
                    IsSingle = choice.IsSingle
                };
                mappedChoiceList.Add(mappedChoice);
            }

            result.Data.ActivityData.Choices = mappedChoiceList.ToArray();


            return result;
        }

        private SaveAndContinueCommand SetupCommand<T,Y>(WorkflowActivityDataDto<T> workflowActivityDataDto) where T: QuestionActivityDataDto where Y: QuestionActivityData
        {
            var result = new SaveAndContinueCommand<Y>();

            result.IsValid = workflowActivityDataDto.IsValid;
            result.ValidationMessages = new List<string>();

            if (!workflowActivityDataDto.IsValid)
            {
                foreach (var message in workflowActivityDataDto.ValidationMessages)
                {
                    result.ValidationMessages.Add(message);
                }
            }

            result.Data = new SaveAndContinue.WorkflowActivityData<Y>();

            result.Data.WorkflowInstanceId = workflowActivityDataDto.Data.WorkflowInstanceId;
            result.Data.ActivityId = workflowActivityDataDto.Data.ActivityId;
            result.Data.ActivityType = workflowActivityDataDto.Data.ActivityType;
            result.Data.PreviousActivityId = workflowActivityDataDto.Data.PreviousActivityId;

            return result;
        }
    }
}
