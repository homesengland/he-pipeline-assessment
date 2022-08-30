using Elsa.CustomWorkflow.Sdk.Models.Workflow;
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

            if (result.Data.ActivityType == "MultpleChoiceQuestion")
            {
                result.Data.MultipleChoiceQuestionActivityData.Title = workflowActivityDataDto.Data.MultipleChoiceQuestionActivityData.Title;
                result.Data.MultipleChoiceQuestionActivityData.Question = workflowActivityDataDto.Data.MultipleChoiceQuestionActivityData.Question;
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

            if (result.Data.ActivityType == "CurrencyQuestion")
            {
                result.Data.CurrencyQuestionActivityData.Title = workflowActivityDataDto.Data.CurrencyQuestionActivityData.Title;
                result.Data.CurrencyQuestionActivityData.Question = workflowActivityDataDto.Data.CurrencyQuestionActivityData.Question;
                result.Data.CurrencyQuestionActivityData.Output = workflowActivityDataDto.Data.CurrencyQuestionActivityData.Output;
                result.Data.CurrencyQuestionActivityData.Answer = workflowActivityDataDto.Data.CurrencyQuestionActivityData.Answer;
            }


            return result;
        }
    }
}
