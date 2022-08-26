using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using System.Text.Json;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void WorkflowActivityDataDtoToSaveAndContinueCommand_Maps(
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadWorkflowActivityMapper sut)
        {
            //Arrange
            workflowActivityDataDto.IsValid = false;

            //Act
            var result = sut.WorkflowActivityDataDtoToSaveAndContinueCommand(workflowActivityDataDto);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(workflowActivityDataDto.Data.ActivityId, result.Data.ActivityId);
            Assert.Equal(workflowActivityDataDto.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Equal(workflowActivityDataDto.Data.PreviousActivityId, result.Data.PreviousActivityId);
            Assert.Equal(workflowActivityDataDto.Data.ActivityData.Title, result.Data.ActivityData.Title);
            Assert.Equal(workflowActivityDataDto.Data.ActivityData.Question, result.Data.ActivityData.Question);
            Assert.Equal(workflowActivityDataDto.Data.ActivityData.Output, result.Data.ActivityData.Output);

            var expectedChoices = JsonSerializer.Serialize(workflowActivityDataDto.Data.ActivityData.Choices);
            var actualChoices = JsonSerializer.Serialize(result.Data.ActivityData.Choices);
            Assert.Equal(expectedChoices, actualChoices);


            Assert.Equal(workflowActivityDataDto.ValidationMessages, result.ValidationMessages);

        }
    }
}
