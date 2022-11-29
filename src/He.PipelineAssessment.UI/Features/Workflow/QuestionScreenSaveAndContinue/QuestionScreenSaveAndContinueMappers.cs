using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public interface IQuestionScreenSaveAndContinueMapper
    {

        QuestionScreenSaveAndContinueCommandDto
            SaveAndContinueCommandToMultiSaveAndContinueCommandDto(QuestionScreenSaveAndContinueCommand saveAndContinueCommand);
    }

    public class QuestionScreenSaveAndContinueMapper : IQuestionScreenSaveAndContinueMapper
    {


        public QuestionScreenSaveAndContinueCommandDto SaveAndContinueCommandToMultiSaveAndContinueCommandDto(
    QuestionScreenSaveAndContinueCommand saveAndContinueCommand)
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
