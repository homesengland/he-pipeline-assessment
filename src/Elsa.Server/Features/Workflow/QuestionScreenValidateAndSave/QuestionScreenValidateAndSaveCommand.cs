using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.QuestionScreenValidateAndSave
{
    public class QuestionScreenValidateAndSaveCommand : WorkflowActivityDataDto, IRequest<OperationResult<QuestionScreenValidateAndSaveResponse>>
    {

    }

    public static class QuestionScreenValidateAndSaveCommandExtensions
    {
        public static QuestionScreenSaveAndContinueCommand ToQuestionScreenSaveAndContinueCommand(this QuestionScreenValidateAndSaveCommand command)
        {
            return new QuestionScreenSaveAndContinueCommand
            {
                Answers = command.Data.Questions?.SelectMany(x => x.Answers.Select(y => new QuestionScreenSaveAndContinue.Answer(x.QuestionId, y.AnswerText, x.Comments, x.DocumentEvidenceLink, y.ChoiceId))).ToList(),
                WorkflowInstanceId = command.Data.WorkflowInstanceId,
                ActivityId = command.Data.ActivityId
            };
        }
    }
}
