using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Helpers;
using Elsa.Server.Providers;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.Helpers
{
    public class SaveAndContinueHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void CreateNextCustomActivityNavigation_ShouldReturnCustomActivityNavigation(
                    [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
                    string previousActivityId,
                    string previousActivityType,
                    string nextActivityId,
                    string nextActivityType,
                    WorkflowInstance workflowInstance,
                    ElsaCustomModelHelper sut
                )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.CreateNextCustomActivityNavigation(previousActivityId, previousActivityType, nextActivityId, nextActivityType, workflowInstance);

            //Assert
            Assert.IsType<CustomActivityNavigation>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(nextActivityType, result!.ActivityType);
            Assert.Equal(workflowInstance.Id, result.WorkflowInstanceId);
            Assert.Equal(workflowInstance.CorrelationId, result.CorrelationId);
            Assert.Equal(previousActivityId, result.PreviousActivityId);
            Assert.Equal(previousActivityType, result.PreviousActivityType);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
            Assert.Equal(currentTimeUtc, result.LastModifiedDateTime);
        }

        [Theory]
        [AutoMoqData]
        public void CreateQuestionScreenAnswer_ShouldReturnQuestionScreenAnswerForCheckbox(
                    [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
                    string nextActivityId,
                    string nextActivityType,
                    WorkflowInstance workflowInstance,
                    Question question,
                    ElsaCustomModelHelper sut
                )
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.CreateQuestionScreenAnswer(nextActivityId, nextActivityType, question, workflowInstance);

            //Assert
            Assert.IsType<QuestionScreenAnswer>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(workflowInstance.Id, result.WorkflowInstanceId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
            Assert.Equal(question.Id, result.QuestionId);
            Assert.Equal(question.QuestionType, result.QuestionType);
            Assert.Equal(question.QuestionText, result.Question);
            Assert.Equal(question.Checkbox.Choices, result.Choices!.Select(x => new CheckboxRecord(x.Identifier, x.Answer, x.IsSingle)));
        }

        [Theory]
        [AutoMoqData]
        public void CreateQuestionScreenAnswer_ShouldReturnQuestionScreenAnswerForRadio(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
            Question question,
            ElsaCustomModelHelper sut
        )
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.RadioQuestion;
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.CreateQuestionScreenAnswer(nextActivityId, nextActivityType, question, workflowInstance);

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
        [AutoMoqData]
        public void CreateQuestionScreenAnswer_ShouldReturnQuestionScreenAnswerForStandardQuestions(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
            Question question,
            ElsaCustomModelHelper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.CreateQuestionScreenAnswer(nextActivityId, nextActivityType, question, workflowInstance);

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

        [Theory]
        [AutoMoqData]
        public void CreateQuestionScreenAnswers_ShouldReturnEmptyList(
            string activityId,
            WorkflowInstance workflowInstance,
            ElsaCustomModelHelper sut
        )
        {
            //Arrange

            //Act
            var result = sut.CreateQuestionScreenAnswers(activityId, workflowInstance);

            //Assert
            Assert.IsType<List<QuestionScreenAnswer>>(result);
            Assert.Empty(result);
        }

        [Theory]
        [AutoMoqData]
        public void CreateQuestionScreenAnswers_ShouldReturnList(
            string activityId,
            WorkflowInstance workflowInstance,
            AssessmentQuestions elsaAssessmentQuestions,
            ElsaCustomModelHelper sut
        )
        {
            //Arrange
            var assessmentQuestionsDictionary = new Dictionary<string, object?>();
            //AssessmentQuestions? elsaAssessmentQuestions = null;
            assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

            workflowInstance.ActivityData.Add(activityId, assessmentQuestionsDictionary);

            //Act
            var result = sut.CreateQuestionScreenAnswers(activityId, workflowInstance);

            //Assert
            Assert.IsType<List<QuestionScreenAnswer>>(result);
            Assert.NotEmpty(result);
        }
    }
}
