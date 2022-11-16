using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {
        SaveAndContinueCommandDto
            SaveAndContinueCommandToSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
        MultiSaveAndContinueCommandDto
            SaveAndContinueCommandToMultiSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {
        public SaveAndContinueCommandDto SaveAndContinueCommandToSaveAndContinueCommandDto(
            SaveAndContinueCommand saveAndContinueCommand)
        {
            return new SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answer = saveAndContinueCommand.Data.QuestionActivityData!.Answer,
                Comments = saveAndContinueCommand.Data.QuestionActivityData!.Comments,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }

        public MultiSaveAndContinueCommandDto SaveAndContinueCommandToMultiSaveAndContinueCommandDto(
    SaveAndContinueCommand saveAndContinueCommand)
        {
            return new MultiSaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answers = saveAndContinueCommand.Data.MultiQuestionActivityData?.Select(x => new Answer(x.QuestionId, x.Answer, x.Comments)).ToList(),
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }
    }
}
