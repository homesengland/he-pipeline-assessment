
namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {
        Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue.SaveAndContinueCommandDto SaveAndContinueCommandToMultipleChoiceSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);

        Elsa.CustomWorkflow.Sdk.Models.Currency.SaveAndContinue.SaveAndContinueCommandDto
            SaveAndContinueCommandToCurrencySaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {
        public Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue.SaveAndContinueCommandDto SaveAndContinueCommandToMultipleChoiceSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        {
            var choiceList = saveAndContinueCommand.Data.MultipleChoiceQuestionActivityData.Choices.Where(x => x.IsSelected).Select(choice => choice.Answer).ToList();

            return new Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue.SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answers = choiceList,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }

        public Elsa.CustomWorkflow.Sdk.Models.Currency.SaveAndContinue.SaveAndContinueCommandDto SaveAndContinueCommandToCurrencySaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        {

            return new Elsa.CustomWorkflow.Sdk.Models.Currency.SaveAndContinue.SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answer = saveAndContinueCommand.Data.CurrencyQuestionActivityData.Answer,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }

    }
}
