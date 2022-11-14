using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.MultiSaveAndContinue;
using Elsa.Server.Providers;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.MultiSaveAndContinue
{
    public class MutliSaveAndContinueMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void MultiSaveAndContinueCommandToNextAssessmentQuestion_ShouldReturnAssessmentQuestion(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            MultiSaveAndContinueCommand saveAndContinueCommand,
            string nextActivityId,
            string nextActivityType,
            MultiSaveAndContinueMapper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.SaveAndContinueCommandToNextAssessmentQuestion(saveAndContinueCommand, nextActivityId, nextActivityType);

            //Assert
            Assert.IsType<AssessmentQuestion>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(nextActivityType, result!.ActivityType);
            Assert.False(result.FinishWorkflow);
            Assert.False(result.NavigateBack);
            Assert.Null(result.Answer);
            Assert.Equal(saveAndContinueCommand.WorkflowInstanceId, result.WorkflowInstanceId);
            Assert.Equal(saveAndContinueCommand.ActivityId, result.PreviousActivityId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }

        [Theory]
        [AutoMoqData]
        public void MultiSaveAndContinueCommandToNextAssessmentQuestion_WithQuestionParameter_ShouldReturnAssessmentQuestion(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            MultiSaveAndContinueCommand saveAndContinueCommand,
            string nextActivityId,
            string nextActivityType,
            Question question,
            MultiSaveAndContinueMapper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.SaveAndContinueCommandToNextAssessmentQuestion(saveAndContinueCommand, nextActivityId, nextActivityType, question);

            //Assert
            Assert.IsType<AssessmentQuestion>(result);
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
