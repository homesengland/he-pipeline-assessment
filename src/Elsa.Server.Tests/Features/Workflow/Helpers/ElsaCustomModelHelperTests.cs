﻿using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Providers;
using Elsa.Models;
using Elsa.Server.Helpers;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using Question = Elsa.CustomModels.Question;

namespace Elsa.Server.Tests.Features.Workflow.Helpers
{
    public class ElsaCustomModelHelperTests
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
        public void CreateQuestion_ShouldReturnQuestionForCheckbox(
                    [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
                    string nextActivityId,
                    string nextActivityType,
                    WorkflowInstance workflowInstance,
                    CustomActivities.Activities.QuestionScreen.Question question,
                    ElsaCustomModelHelper sut
                )
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.CreateQuestion(nextActivityId, nextActivityType, question, workflowInstance);

            //Assert
            Assert.IsType<Question>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(workflowInstance.Id, result.WorkflowInstanceId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
            Assert.Equal(question.Id, result.QuestionId);
            Assert.Equal(question.QuestionType, result.QuestionType);
            Assert.Equal(question.QuestionText, result.QuestionText);
            Assert.Equal(question.DataDictionary, result.QuestionDataDictionaryId);
            Assert.Equal(question.Checkbox.Choices, result.Choices!.Select(x => new CheckboxRecord(x.Identifier, x.Answer, x.IsSingle, x.IsPrePopulated)));
        }

        [Theory]
        [AutoMoqData]
        public void CreateQuestion_ShouldReturnQuestionForRadio(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
            CustomActivities.Activities.QuestionScreen.Question question,
            ElsaCustomModelHelper sut
        )
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.RadioQuestion;
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.CreateQuestion(nextActivityId, nextActivityType, question, workflowInstance);

            //Assert
            Assert.IsType<Question>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(workflowInstance.Id, result.WorkflowInstanceId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
            Assert.Equal(question.Id, result.QuestionId);
            Assert.Equal(question.QuestionType, result.QuestionType);
            Assert.Equal(question.QuestionText, result.QuestionText);
            Assert.Equal(question.DataDictionary, result.QuestionDataDictionaryId);
            Assert.Equal(question.Radio.Choices, result.Choices!.Select(x => new RadioRecord(x.Identifier, x.Answer, x.IsPrePopulated)));
        }

        [Theory]
        [AutoMoqData]
        public void CreateQuestion_ShouldReturnQuestionForStandardQuestions(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
            CustomActivities.Activities.QuestionScreen.Question question,
            ElsaCustomModelHelper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.CreateQuestion(nextActivityId, nextActivityType, question, workflowInstance);

            //Assert
            Assert.IsType<Question>(result);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.Equal(workflowInstance.Id, result.WorkflowInstanceId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
            Assert.Equal(question.Id, result.QuestionId);
            Assert.Equal(question.QuestionType, result.QuestionType);
            Assert.Equal(question.QuestionText, result.QuestionText);
            Assert.Equal(question.DataDictionary, result.QuestionDataDictionaryId);
            Assert.Null(result.Choices);
        }

        [Theory]
        [AutoMoqData]
        public void CreateQuestion_ShouldReturnNullQuestionDataDictionary_GivenTheValueIsZero(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string nextActivityId,
            string nextActivityType,
            WorkflowInstance workflowInstance,
            CustomActivities.Activities.QuestionScreen.Question question,
            ElsaCustomModelHelper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            question.DataDictionary = 0;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.CreateQuestion(nextActivityId, nextActivityType, question, workflowInstance);

            //Assert
            Assert.Null(result.QuestionDataDictionary);
        }

        [Theory]
        [AutoMoqData]
        public void CreateQuestions_ShouldReturnEmptyList(
            string activityId,
            WorkflowInstance workflowInstance,
            ElsaCustomModelHelper sut
        )
        {
            //Arrange

            //Act
            var result = sut.CreateQuestions(activityId, workflowInstance);

            //Assert
            Assert.IsType<List<Question>>(result);
            Assert.Empty(result);
        }

        [Theory]
        [AutoMoqData]
        public void CreateQuestions_ShouldReturnList(
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
            var result = sut.CreateQuestions(activityId, workflowInstance);

            //Assert
            Assert.IsType<List<Question>>(result);
            Assert.NotEmpty(result);
        }
    }
}
