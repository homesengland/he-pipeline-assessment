using Elsa.CustomWorkflow.Sdk.Models;
using He.PipelineAssessment.UI.Models;

namespace He.PipelineAssessment.UI
{
    public static class Mappers
    {
        public static SaveAndContinueCommandDto ToSaveAndContinueCommandDto(this WorkflowNavigationViewModel workflowNavigationViewModel)
        {
            return new SaveAndContinueCommandDto
            {
                Id = $"{workflowNavigationViewModel.WorkflowInstanceId}-{workflowNavigationViewModel.ActivityId}",
                Answer = string.Join(',', workflowNavigationViewModel.ActivityData.Choices.Where(x => x is { IsSelected: true }).Select(x => x.Answer)),
                WorkflowInstanceId = workflowNavigationViewModel.WorkflowInstanceId,
                PreviousActivityId = "",
                ActivityId = workflowNavigationViewModel.ActivityId
            };
        }
    }
}
