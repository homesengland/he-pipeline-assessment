using Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public static class SaveAndContinueMappers
    {
        public static SaveAndContinueCommandDto ToSaveAndContinueCommandDto(this SaveAndContinueCommand workflowNavigationViewModel)
        {
            return new SaveAndContinueCommandDto
            {
                Id = $"{workflowNavigationViewModel.Data.WorkflowInstanceId}-{workflowNavigationViewModel.Data.ActivityId}",
                Answer = string.Join(',', workflowNavigationViewModel.Data.ActivityData.Choices.Where(x => x is { IsSelected: true }).Select(x => x.Answer)),
                WorkflowInstanceId = workflowNavigationViewModel.Data.WorkflowInstanceId,
                ActivityId = workflowNavigationViewModel.Data.ActivityId
            };
        }
    }
}
