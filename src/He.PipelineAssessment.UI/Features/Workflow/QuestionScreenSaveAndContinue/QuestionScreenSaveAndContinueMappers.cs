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
                Answers = saveAndContinueCommand.Data.Questions?.SelectMany(x => x.Answers.Select(y => new Answer(x.QuestionId, y.AnswerText, x.Comments,x.DocumentEvidenceLink, y.ChoiceId))).ToList(),
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }
    }
}
