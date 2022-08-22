using System.Text.Json;
using Elsa.CustomModels;

namespace Elsa.Server.Features.MultipleChoice.SaveAndContinue
{
    public static class SaveAndContinueMappers
    {
        public static MultipleChoiceQuestionModel ToMultipleChoiceQuestionModel(this SaveAndContinueCommand command, string nextActivityId)
        {
            return new MultipleChoiceQuestionModel
            {
                Id = $"{command.WorkflowInstanceId}-{nextActivityId}",
                ActivityId = nextActivityId,
                Answer = command.Answer,
                FinishWorkflow = false,
                NavigateBack = false,
                WorkflowInstanceId = command.WorkflowInstanceId,
                PreviousActivityId = command.ActivityId
            };
        }

        public static MultipleChoiceQuestionModel ToMultipleChoiceQuestionModel(this SaveAndContinueCommand command)
        {
            return command.ToMultipleChoiceQuestionModel("");
        }
    }
}
