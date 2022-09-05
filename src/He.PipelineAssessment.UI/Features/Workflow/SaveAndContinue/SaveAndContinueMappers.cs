using MultipleChoice = Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue;
using Currency = Elsa.CustomWorkflow.Sdk.Models.Currency.SaveAndContinue;
using Date = Elsa.CustomWorkflow.Sdk.Models.Date.SaveAndContinue;
using Text = Elsa.CustomWorkflow.Sdk.Models.Text.SaveAndContinue;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {
        MultipleChoice.SaveAndContinueCommandDto SaveAndContinueCommandToMultipleChoiceSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);

        Currency.SaveAndContinueCommandDto
            SaveAndContinueCommandToCurrencySaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);

        Date.SaveAndContinueCommandDto
            SaveAndContinueCommandToDateSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);

        Text.SaveAndContinueCommandDto
            SaveAndContinueCommandToTextSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {
        public MultipleChoice.SaveAndContinueCommandDto SaveAndContinueCommandToMultipleChoiceSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        {
            var choiceList = saveAndContinueCommand.Data.MultipleChoiceQuestionActivityData!.Choices.Where(x => x.IsSelected).Select(choice => choice.Answer).ToList();

            return new MultipleChoice.SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answers = choiceList,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }

        public Currency.SaveAndContinueCommandDto SaveAndContinueCommandToCurrencySaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        {

            return new Currency.SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answer = saveAndContinueCommand.Data.CurrencyQuestionActivityData!.Answer,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }

        public Date.SaveAndContinueCommandDto SaveAndContinueCommandToDateSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        {
            return new Date.SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answer = saveAndContinueCommand.Data.DateQuestionActivityData!.Answer,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }

        public Text.SaveAndContinueCommandDto SaveAndContinueCommandToTextSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        {
            return new Text.SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answer = saveAndContinueCommand.Data.TextQuestionActivityData!.Answer,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }
    }
}
