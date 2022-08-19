using Elsa.CustomWorkflow.Sdk.Models;

namespace Elsa.CustomWorkflow.Sdk.Mappers
{
    public static class Mappers
    {
        public static MultipleChoiceQuestionDto ToMultipleChoiceQuestionDto(this WorkflowNavigationDto workflowNavigationDto, bool navigateBack)
        {
            return new MultipleChoiceQuestionDto
            {
                Id = $"{workflowNavigationDto.WorkflowInstanceId}-{workflowNavigationDto.ActivityId}",
                Answer = string.Join(',', workflowNavigationDto.ActivityData.Choices.Where(x => x is { IsSelected: true }).Select(x => x.Answer)),
                WorkflowInstanceId = workflowNavigationDto.WorkflowInstanceId,
                NavigateBack = navigateBack,
                FinishWorkflow = false,
                PreviousActivityId = "",
                ActivityId = workflowNavigationDto.ActivityId
            };
        }
    }
}
