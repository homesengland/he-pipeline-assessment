using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
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
            string activityName,
            string questionId,
            string workflowInstanceId,
            TextQuestionHelper sut)
        {
            //Arrange
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
        public async Task AnswerContains_ForTextQuestion_ReturnsExpectedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string workflowInstanceId,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
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
        public async Task AnswerContains_ForTextAreaQuestion_ReturnsExpectedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string workflowInstanceId,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
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
            string activityName,
            string questionId,
            string correlationId,
            TextQuestionHelper sut)
        {
            //Arrange
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
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
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
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.TextAreaQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = answer } };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerContains_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string correlationId,
            int dataDictionaryId,
            TextQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerContains(correlationId, dataDictionaryId, "Test");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("Test", "Test", true)]
        [InlineAutoMoqData("Test", "st", true)]
        [InlineAutoMoqData("False", "Test", false)]
        public async Task DataDictionaryAnswerContains_ForTextQuestion_ReturnsExpectedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string correlationId,
            int dataDictionaryId,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.TextQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = answer } };

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(correlationId, dataDictionaryId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData("Test", "Test", true)]
        [InlineAutoMoqData("Test", "st", true)]
        [InlineAutoMoqData("False", "Test", false)]
        public async Task DataDictionaryAnswerContains_ForTextAreaQuestion_ReturnsExpectedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string correlationId,
            int dataDictionaryId,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.TextAreaQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = answer } };

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(correlationId, dataDictionaryId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEquals_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string correlationId,
            int dataDictionaryId,
            TextQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEquals(correlationId, dataDictionaryId, "Test");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("Test", "Test", true)]
        [InlineAutoMoqData("False", "Test", false)]
        public async Task DataDictionaryAnswerEquals_ForTextQuestion_ReturnsExpectedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string correlationId,
            int dataDictionaryId,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.TextQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = answer } };

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);


            //Act
            var result = await sut.AnswerEquals(correlationId, dataDictionaryId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData("Test", "Test", true)]
        [InlineAutoMoqData("False", "Test", false)]
        public async Task DataDictionaryAnswerEquals_ForTextAreaQuestion_ReturnsExpectedValue(
            string answer,
            string answerToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string correlationId,
            int dataDictionaryId,
            Question question,
            TextQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.TextAreaQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = answer } };

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);


            //Act
            var result = await sut.AnswerEquals(correlationId, dataDictionaryId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
