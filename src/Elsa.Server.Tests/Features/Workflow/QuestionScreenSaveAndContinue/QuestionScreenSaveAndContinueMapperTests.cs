using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
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
        public void QuestionScreenSaveAndContinueCommandToCustomActivityNavigation_ShouldReturnCustomActivityNavigation(
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
            Assert.Equal(workflowInstance.CorrelationId, result.CorrelationId);
            Assert.Equal(saveAndContinueCommand.ActivityId, result.PreviousActivityId);
            Assert.Equal(ActivityTypeConstants.QuestionScreen, result.PreviousActivityType);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }

        [Theory]
        [AutoMoqData]
        public void QuestionScreenSaveAndContinueCommandToQuestionScreenQuestion_WithQuestionParameter_ShouldReturnQuestionScreenAnswerForCheckbox(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
            Question question,
            QuestionScreenSaveAndContinueMapper sut
        )
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
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
            Assert.Equal(question.QuestionText, result.Question);
            Assert.Equal(question.Checkbox.Choices, result.Choices!.Select( x => new CheckboxRecord(x.Identifier, x.Answer, x.IsSingle)));
        }

        [Theory]
        [AutoMoqData]
        public void QuestionScreenSaveAndContinueCommandToQuestionScreenQuestion_WithQuestionParameter_ShouldReturnQuestionScreenAnswerForRadio(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
            Question question,
            QuestionScreenSaveAndContinueMapper sut
        )
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.RadioQuestion;
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
            Assert.Equal(question.QuestionText, result.Question);
            Assert.Equal(question.Radio.Choices, result.Choices!.Select(x => new RadioRecord(x.Identifier, x.Answer)));
        }

        [Theory]
        [InlineAutoMoqData(QuestionTypeConstants.TextQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.CurrencyQuestion)]
        [InlineAutoMoqData(QuestionTypeConstants.DateQuestion)]
        public void QuestionScreenSaveAndContinueCommandToQuestionScreenQuestion_WithQuestionParameter_ShouldReturnQuestionScreenAnswerWithNoChoicesForOtherQuestionTypes(
            string questionType,
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
            Question question,
            QuestionScreenSaveAndContinueMapper sut
        )
        {
            //Arrange
            question.QuestionType = questionType;
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
            Assert.Equal(question.QuestionText, result.Question);
            Assert.Null(result.Choices);
        }
    }
}
