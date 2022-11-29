using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public interface ISaveAndContinueMapper
    {

        QuestionScreenSaveAndContinueCommandDto
            SaveAndContinueCommandToMultiSaveAndContinueCommandDto(CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {


        public QuestionScreenSaveAndContinueCommandDto SaveAndContinueCommandToMultiSaveAndContinueCommandDto(
    CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand)
        {
            return new QuestionScreenSaveAndContinueCommandDto
            {
                Answers = saveAndContinueCommand.Data.QuestionScreenAnswers?.Select(x => new Answer(x.QuestionId, x.Answer, x.Comments)).ToList(),
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }
    }
}
