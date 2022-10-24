using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {
        SaveAndContinueCommandDto
            SaveAndContinueCommandToSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {
        public SaveAndContinueCommandDto SaveAndContinueCommandToSaveAndContinueCommandDto(
            SaveAndContinueCommand saveAndContinueCommand)
        {
            return new SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Question = saveAndContinueCommand.Data.QuestionActivityData!.Question,
                Answer = saveAndContinueCommand.Data.QuestionActivityData!.Answer,
                Comments = saveAndContinueCommand.Data.QuestionActivityData!.Comments,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }
    }
}
