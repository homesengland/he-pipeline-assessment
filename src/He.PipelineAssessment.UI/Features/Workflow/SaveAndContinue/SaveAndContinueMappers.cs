using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {

        MultiSaveAndContinueCommandDto
            SaveAndContinueCommandToMultiSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {


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
