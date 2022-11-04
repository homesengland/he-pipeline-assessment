using AutoFixture.Xunit2;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.SaveAndContinue;
using Elsa.Server.Providers;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void SaveAndContinueCommandToNextAssessmentQuestion_ShouldReturnAssessmentQuestion(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            SaveAndContinueCommand saveAndContinueCommand,
            string nextActivityId,
            string nextActivityType,
            SaveAndContinueMapper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.SaveAndContinueCommandToNextAssessmentQuestion(saveAndContinueCommand, nextActivityId, nextActivityType);

            //Assert
            Assert.IsType<AssessmentQuestion>(result);
            Assert.Equal(
                $"{saveAndContinueCommand.WorkflowInstanceId}-{nextActivityId}",
                result.Id);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(nextActivityType, result!.ActivityType);
            Assert.False(result.FinishWorkflow);
            Assert.False(result.NavigateBack);
            Assert.Null(result.Answer);
            Assert.Equal(saveAndContinueCommand.WorkflowInstanceId, result.WorkflowInstanceId);
            Assert.Equal(saveAndContinueCommand.ActivityId, result.PreviousActivityId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }
    }
}
