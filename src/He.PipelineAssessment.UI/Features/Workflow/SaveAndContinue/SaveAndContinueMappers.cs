using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {

        SaveAndContinueCommandDto
            SaveAndContinueCommandToMultiSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {


        public SaveAndContinueCommandDto SaveAndContinueCommandToMultiSaveAndContinueCommandDto(
    SaveAndContinueCommand saveAndContinueCommand)
        {
            return new SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answers = saveAndContinueCommand.Data.MultiQuestionActivityData?.Select(x => new Answer(x.QuestionId, x.Answer, x.Comments)).ToList(),
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }
    }
}
