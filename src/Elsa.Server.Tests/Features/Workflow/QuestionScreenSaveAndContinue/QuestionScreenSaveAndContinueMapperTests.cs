using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Providers;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class MultiSaveAndContinueMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void QuestionScreenSaveAndContinueCommandToCustomActivityNavigation_ShouldReturnAssessmentQuestion(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
                QuestionScreenSaveAndContinueMapper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.saveAndContinueCommandToNextCustomActivityNavigation(saveAndContinueCommand, nextActivityId, nextActivityType, workflowInstance);

            //Assert
            Assert.IsType<CustomActivityNavigation>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(nextActivityType, result!.ActivityType);
            Assert.Equal(workflowInstance.Id, result.WorkflowInstanceId);
            Assert.Equal(saveAndContinueCommand.ActivityId, result.PreviousActivityId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }

        [Theory]
        [AutoMoqData]
        public void QuestionScreenSaveAndContinueCommandToQuestionScreenQuestion_WithQuestionParameter_ShouldReturnAssessmentQuestion(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
            Question question,
            QuestionScreenSaveAndContinueMapper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.SaveAndContinueCommandToQuestionScreenAnswer(nextActivityId, nextActivityType, question, workflowInstance);

            //Assert
            Assert.IsType<QuestionScreenAnswer>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(workflowInstance.Id, result.WorkflowInstanceId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
            Assert.Equal(question.Id, result.QuestionId);
            Assert.Equal(question.QuestionType, result.QuestionType);

        }
    }
}
