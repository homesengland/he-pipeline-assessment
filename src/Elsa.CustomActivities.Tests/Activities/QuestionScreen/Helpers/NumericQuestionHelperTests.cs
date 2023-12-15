using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System.Globalization;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen.Helpers
{
    public class NumericQuestionHelperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task AnswerEqualToOrGreaterThan_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            NumericQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(correlationId, workflowName, activityName, questionId, 100);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.CurrencyQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.CurrencyQuestion, true)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.CurrencyQuestion, false)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.PercentageQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.PercentageQuestion, true)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.PercentageQuestion, false)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.DecimalQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.DecimalQuestion, true)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.DecimalQuestion, false)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.IntegerQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.IntegerQuestion, true)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.IntegerQuestion, false)]
        public async Task AnswerEqualToOrGreaterThan_ReturnsExpectedValue(
            decimal answer,
            decimal answerToCheck,
            string questionType,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            NumericQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = questionType;
            question.Answers = new List<Answer> { new() { AnswerText = answer.ToString(CultureInfo.InvariantCulture) } };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(correlationId, workflowName, activityName, questionId, answerToCheck);

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
            NumericQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(correlationId, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(correlationId, workflowName, activityName, questionId, 100);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.CurrencyQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.CurrencyQuestion, false)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.CurrencyQuestion, true)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.PercentageQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.PercentageQuestion, false)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.PercentageQuestion, true)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.DecimalQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.DecimalQuestion, false)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.DecimalQuestion, true)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.IntegerQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.IntegerQuestion, false)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.IntegerQuestion, true)]
        public async Task AnswerEqualToOrLessThan_ReturnsExpectedValue(
            decimal answer,
            decimal answerToCheck,
            string questionType,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            NumericQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = questionType;
            question.Answers = new List<Answer> { new() { AnswerText = answer.ToString(CultureInfo.InvariantCulture) } };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(correlationId, workflowName, activityName, questionId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsEmptyString_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            NumericQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsValue(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            NumericQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.CurrencyQuestion;
            question.Answers![0].AnswerText = "123.345";

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal((decimal)123.345,result!.Value);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEqualToOrGreaterThan_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            NumericQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(correlationId, dataDictionaryId, 100);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.CurrencyQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.CurrencyQuestion, true)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.CurrencyQuestion, false)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.PercentageQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.PercentageQuestion, true)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.PercentageQuestion, false)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.DecimalQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.DecimalQuestion, true)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.DecimalQuestion, false)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.IntegerQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.IntegerQuestion, true)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.IntegerQuestion, false)]
        public async Task DataDictionaryAnswerEqualToOrGreaterThan_ReturnsExpectedValue(
            decimal answer,
            decimal answerToCheck,
            string questionType,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            NumericQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = questionType;
            question.Answers = new List<Answer> { new() { AnswerText = answer.ToString(CultureInfo.InvariantCulture) } };

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(correlationId, dataDictionaryId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEqualToOrLessThan_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            NumericQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(correlationId, dataDictionaryId, 100);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.CurrencyQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.CurrencyQuestion, false)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.CurrencyQuestion, true)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.PercentageQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.PercentageQuestion, false)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.PercentageQuestion, true)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.DecimalQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.DecimalQuestion, false)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.DecimalQuestion, true)]
        [InlineAutoMoqData(100, 100, QuestionTypeConstants.IntegerQuestion, true)]
        [InlineAutoMoqData(1000, 100, QuestionTypeConstants.IntegerQuestion, false)]
        [InlineAutoMoqData(100, 200, QuestionTypeConstants.IntegerQuestion, true)]
        public async Task DataDictionaryAnswerEqualToOrLessThan_ReturnsExpectedValue(
            decimal answer,
            decimal answerToCheck,
            string questionType,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            NumericQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = questionType;
            question.Answers = new List<Answer> { new() { AnswerText = answer.ToString(CultureInfo.InvariantCulture) } };

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(correlationId, dataDictionaryId, answerToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswer_ReturnsEmptyString_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            NumericQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, dataDictionaryId);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswer_ReturnsValue(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            NumericQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.CurrencyQuestion;
            question.Answers![0].AnswerText = "123.345";

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal((decimal)123.345, result!.Value);
        }

    }
}
