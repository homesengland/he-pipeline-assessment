using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity
{
    public static class LoadWorkflowActivityMappers
    {
        public static SaveAndContinueCommand ToSaveAndContinueCommand(this WorkflowActivityDataDto workflowActivityDataDto)
        {
            var result = new SaveAndContinueCommand();

            result.IsValid = workflowActivityDataDto.IsValid;
            result.ValidationMessages = new List<string>();

            if (!workflowActivityDataDto.IsValid)
            {
                foreach (var message in workflowActivityDataDto.ValidationMessages)
                {
                    result.ValidationMessages.Add(message);
                }
            }

            result.Data = new SaveAndContinue.WorkflowActivityData();

            result.Data.WorkflowInstanceId = workflowActivityDataDto.Data.WorkflowInstanceId;
            result.Data.ActivityId = workflowActivityDataDto.Data.ActivityId;
            result.Data.PreviousActivityId = workflowActivityDataDto.Data.PreviousActivityId;

            result.Data.ActivityData = new SaveAndContinue.ActivityData();

            result.Data.ActivityData.Title = workflowActivityDataDto.Data.ActivityData.Title;
            result.Data.ActivityData.Question = workflowActivityDataDto.Data.ActivityData.Question;
            result.Data.ActivityData.Output = workflowActivityDataDto.Data.ActivityData.Output;

            var mappedChoiceList = new List<SaveAndContinue.Choice>();
            foreach (var choice in workflowActivityDataDto.Data.ActivityData.Choices)
            {
                var mappedChoice = new SaveAndContinue.Choice()
                {
                    Answer = choice.Answer,
                    IsSelected = choice.IsSelected,
                    IsSingle = choice.IsSingle
                };
                mappedChoiceList.Add(mappedChoice);
            }

            result.Data.ActivityData.Choices = mappedChoiceList.ToArray();

            return result;
        }
    }
}
