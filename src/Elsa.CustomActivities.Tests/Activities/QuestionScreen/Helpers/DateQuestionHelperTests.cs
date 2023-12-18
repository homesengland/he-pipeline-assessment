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
    public class DateQuestionHelperTests
    {

        [Theory]
        [AutoMoqData]
        public async Task AnswerEqualToOrGreaterThan_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            DateQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(correlationId, workflowName, activityName, questionId, 1, 2, 1900);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(1, 1, 1900, 1, 1, 1900, true)]
        [InlineAutoMoqData(1, 1, 1901, 1, 1, 1900, true)]
        [InlineAutoMoqData(1, 1, 1889, 1, 1, 1900, false)]
        public async Task AnswerEqualToOrGreaterThan_ReturnsExpectedValue(
            int answerDay,
            int answerMonth,
            int answerYear,
            int answerToCheckDay,
            int answerToCheckMonth,
            int answerToCheckYear,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            DateQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.DateQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = $"{answerYear}-{answerMonth}-{answerDay}" } };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(correlationId, workflowName, activityName, questionId, answerToCheckDay, answerToCheckMonth, answerToCheckYear);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEqualToOrLessThan_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            DateQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(correlationId, workflowName, activityName, questionId, 1, 2, 1900);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(1, 1, 1900, 1, 1, 1900, true)]
        [InlineAutoMoqData(1, 1, 1901, 1, 1, 1900, false)]
        [InlineAutoMoqData(1, 1, 1889, 1, 1, 1900, true)]
        public async Task AnswerEqualToOrLessThan_ReturnsExpectedValue(
            int answerDay,
            int answerMonth,
            int answerYear,
            int answerToCheckDay,
            int answerToCheckMonth,
            int answerToCheckYear,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            DateQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.DateQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = $"{answerYear}-{answerMonth}-{answerDay}" } };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(correlationId, workflowName, activityName, questionId, answerToCheckDay, answerToCheckMonth, answerToCheckYear);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEqualToOrGreaterThan_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            DateQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(correlationId, dataDictionaryId, 1, 2, 1900);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(1, 1, 1900, 1, 1, 1900, true)]
        [InlineAutoMoqData(1, 1, 1901, 1, 1, 1900, true)]
        [InlineAutoMoqData(1, 1, 1889, 1, 1, 1900, false)]
        public async Task DataDictionaryAnswerEqualToOrGreaterThan_ReturnsExpectedValue(
            int answerDay,
            int answerMonth,
            int answerYear,
            int answerToCheckDay,
            int answerToCheckMonth,
            int answerToCheckYear,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            DateQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.DateQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = $"{answerYear}-{answerMonth}-{answerDay}" } };

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(correlationId, dataDictionaryId, answerToCheckDay, answerToCheckMonth, answerToCheckYear);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEqualToOrLessThan_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            DateQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(correlationId, dataDictionaryId, 1, 2, 1900);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(1, 1, 1900, 1, 1, 1900, true)]
        [InlineAutoMoqData(1, 1, 1901, 1, 1, 1900, false)]
        [InlineAutoMoqData(1, 1, 1889, 1, 1, 1900, true)]
        public async Task DataDictionaryAnswerEqualToOrLessThan_ReturnsExpectedValue(
            int answerDay,
            int answerMonth,
            int answerYear,
            int answerToCheckDay,
            int answerToCheckMonth,
            int answerToCheckYear,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            DateQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.DateQuestion;
            question.Answers = new List<Answer> { new() { AnswerText = $"{answerYear}-{answerMonth}-{answerDay}" } };

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(correlationId, dataDictionaryId, answerToCheckDay, answerToCheckMonth, answerToCheckYear);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
