using Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public static class SaveAndContinueMappers
    {
        public static SaveAndContinueCommandDto ToSaveAndContinueCommandDto(this SaveAndContinueCommand workflowNavigationViewModel)
        {
            var choiceList = workflowNavigationViewModel.Data.ActivityData.Choices.Where(x => x.IsSelected).Select(choice => choice.Answer).ToList();

            return new SaveAndContinueCommandDto
            {
                Id = $"{workflowNavigationViewModel.Data.WorkflowInstanceId}-{workflowNavigationViewModel.Data.ActivityId}",
                Answers = choiceList,
                WorkflowInstanceId = workflowNavigationViewModel.Data.WorkflowInstanceId,
                ActivityId = workflowNavigationViewModel.Data.ActivityId
            };
        }
    }
}
