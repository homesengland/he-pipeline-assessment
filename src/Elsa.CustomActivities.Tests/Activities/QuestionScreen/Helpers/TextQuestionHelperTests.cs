using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen.Helpers
{
    public class TextQuestionHelperTests
    {

        [Theory]
        [AutoMoqData]
        public async Task AnswerContains_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            TextQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);


            //Act
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, "Test");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("Test", "Test", true)]
        [InlineAutoMoqData("Test", "st", true)]
        [InlineAutoMoqData("False", "Test", false)]
        public async Task AnswerContains_ForTextQuestion_ReturnsExepctedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            question.QuestionType = QuestionTypeConstants.TextQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = answer } };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);


            //Act
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData("Test", "Test", true)]
        [InlineAutoMoqData("Test", "st", true)]
        [InlineAutoMoqData("False", "Test", false)]
        public async Task AnswerContains_ForTextAreaQuestion_ReturnsExepctedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            question.QuestionType = QuestionTypeConstants.TextAreaQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = answer } };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);


            //Act
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            TextQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);


            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, "Test");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("Test", "Test", true)]
        [InlineAutoMoqData("False", "Test", false)]
        public async Task AnswerEquals_ForTextQuestion_ReturnsExpectedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            question.QuestionType = QuestionTypeConstants.TextQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = answer } };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);


            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData("Test", "Test", true)]
        [InlineAutoMoqData("False", "Test", false)]
        public async Task AnswerEquals_ForTextAreaQuestion_ReturnsExpectedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            question.QuestionType = QuestionTypeConstants.TextAreaQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = answer } };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);


            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
