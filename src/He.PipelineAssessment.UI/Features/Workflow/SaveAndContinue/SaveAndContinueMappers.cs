using Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {
        SaveAndContinueCommandDto SaveAndContinueCommandToSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {
        public SaveAndContinueCommandDto SaveAndContinueCommandToSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        {
            var choiceList = saveAndContinueCommand.Data.ActivityData.Choices.Where(x => x.IsSelected).Select(choice => choice.Answer).ToList();

            return new SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answers = choiceList,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }
    }
}
