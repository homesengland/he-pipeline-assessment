using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Providers;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class MutliSaveAndContinueMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void QuestionScreenSaveAndContinueCommandToCustomActivityNavigation_ShouldReturnAssessmentQuestion(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            string nextActivityId,
            string nextActivityType,
            QuestionScreenSaveAndContinueMapper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.saveAndContinueCommandToNextCustomActivityNavigation(saveAndContinueCommand, nextActivityId, nextActivityType);

            //Assert
            Assert.IsType<CustomActivityNavigation>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(nextActivityType, result!.ActivityType);
            Assert.Equal(saveAndContinueCommand.WorkflowInstanceId, result.WorkflowInstanceId);
            Assert.Equal(saveAndContinueCommand.ActivityId, result.PreviousActivityId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }

        [Theory]
        [AutoMoqData]
        public void QuestionScreenSaveAndContinueCommandToQuestionScreenQuestion_WithQuestionParameter_ShouldReturnAssessmentQuestion(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            string nextActivityId,
            string nextActivityType,
            Question question,
            QuestionScreenSaveAndContinueMapper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.SaveAndContinueCommandToQuestionScreenAnswer(saveAndContinueCommand, nextActivityId, nextActivityType, question);

            //Assert
            Assert.IsType<QuestionScreenAnswer>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(saveAndContinueCommand.WorkflowInstanceId, result.WorkflowInstanceId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
            Assert.Equal(question.Id, result.QuestionId);
            Assert.Equal(question.QuestionType, result.QuestionType);

        }
    }
}
