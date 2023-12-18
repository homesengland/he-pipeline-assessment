using System.Text.Json;
using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen.Helpers
{
    public class DataTableQuestionHelperTests
    {

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName,correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GivenQuestionRecordFirstAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0] = null!;

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsEmptyString_GivenDataTableAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName,correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, null);
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswer_ReturnsCorrectString_GivenDataTableAnswerIsPopulated(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            string answerText,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, answerText);
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetStringAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal(answerText, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsNull_GivenGetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsNull_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimal_ReturnsNull_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null( result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsEmptyString_GivenDataTableAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, null);
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null( result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsNull_GivenDataTableInputIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, null);
            datable.InputType = DataTableInputTypeConstants.DecimalDataTableInput;
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswer_ReturnsCorrectValue_GivenDataTableInputIsNotNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, "123.45");
            datable.InputType = DataTableInputTypeConstants.DecimalDataTableInput;
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, workflowName, activityName, questionId, tableCellIdentifier);

            //Assert
            Assert.Equal((decimal)123.45,result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswerArray_ReturnsEmptyString_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(new string[] { }, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswerArray_ReturnsEmptyString_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(new string[] { }, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswerArray_ReturnsEmptyString_GivenQuestionRecordFirstAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0] = null!;

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(new string[] { }, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswerArray_ReturnsEmptyString_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(new string[] { }, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStringAnswerArray_ReturnsCorrectString_GivenDataTableAnswerIsPopulated(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            string[] answerText,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;

            for (int i = 0; i < answerText.Length; i++)
            {
                TableInput input = new TableInput(i.ToString(), null, true, true, answerText[i]);
                datable.Inputs[i] = input;
            }
            
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(answerText, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswerArray_ReturnsEmpty_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(new List<decimal?>().ToArray(), result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswerArray_ReturnsEmpty_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(new List<decimal?>().ToArray(), result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswerArray_ReturnsEmpty_GivenQuestionRecordFirstAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0] = null!;

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(new List<decimal?>().ToArray(), result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswerArray_ReturnsEmpty_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(new List<decimal?>().ToArray(), result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDecimalAnswerArray_ReturnsCorrectArray_GivenDataTableAnswerIsPopulated(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            datable.InputType = DataTableInputTypeConstants.CurrencyDataTableInput;

            var answerText = new[] { (decimal?)null,Convert.ToDecimal(100), Convert.ToDecimal(200) };

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;

            for (int i = 0; i < answerText.Length; i++)
            {
                var value = answerText[i]?.ToString();
                TableInput input = new TableInput(i.ToString(), null, true, true, value);
                datable.Inputs[i] = input;
            }

            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(answerText[0], result[0]);
            Assert.Equal(answerText[1], result[1]);
            Assert.Equal(answerText[2], result[2]);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswer_ReturnsEmptyString_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetStringAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswer_ReturnsEmptyString_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetStringAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswer_ReturnsEmptyString_GivenQuestionRecordFirstAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0] = null!;

            //Act
            var result = await sut.GetStringAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswer_ReturnsEmptyString_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetStringAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswer_ReturnsEmptyString_GivenDataTableAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, null);
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetStringAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswer_ReturnsCorrectString_GivenDataTableAnswerIsPopulated(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            string answerText,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, answerText);
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetStringAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Equal(answerText, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswer_ReturnsNull_GivenGetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswer_ReturnsNull_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimal_ReturnsNull_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswer_ReturnsEmptyString_GivenDataTableAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, null);
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswer_ReturnsNull_GivenDataTableInputIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, null);
            datable.InputType = DataTableInputTypeConstants.DecimalDataTableInput;
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswer_ReturnsCorrectValue_GivenDataTableInputIsNotNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string tableCellIdentifier,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            TableInput input = new TableInput(tableCellIdentifier, null, true, true, "123.45");
            datable.InputType = DataTableInputTypeConstants.DecimalDataTableInput;
            datable.Inputs[0] = input;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswer(correlationId, dataDictionaryId, tableCellIdentifier);

            //Assert
            Assert.Equal((decimal)123.45, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswerArray_ReturnsEmptyString_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(new string[] { }, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswerArray_ReturnsEmptyString_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(new string[] { }, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswerArray_ReturnsEmptyString_GivenQuestionRecordFirstAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0] = null!;

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(new string[] { }, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswerArray_ReturnsEmptyString_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(new string[] { }, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetStringAnswerArray_ReturnsCorrectString_GivenDataTableAnswerIsPopulated(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            string[] answerText,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;

            for (int i = 0; i < answerText.Length; i++)
            {
                TableInput input = new TableInput(i.ToString(), null, true, true, answerText[i]);
                datable.Inputs[i] = input;
            }

            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetStringAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(answerText, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswerArray_ReturnsEmpty_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(new List<decimal?>().ToArray(), result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswerArray_ReturnsEmpty_GivenQuestionRecordAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers = null;

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(new List<decimal?>().ToArray(), result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswerArray_ReturnsEmpty_GivenQuestionRecordFirstAnswerIsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0] = null!;

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(new List<decimal?>().ToArray(), result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswerArray_ReturnsEmpty_GivenQuestionRecordAnswerDoesNotDeserialise(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            DataTableQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;
            question.Answers![0].AnswerText = JsonSerializer.Serialize(question.Answers[0].AnswerText);

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(new List<decimal?>().ToArray(), result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetDecimalAnswerArray_ReturnsCorrectArray_GivenDataTableAnswerIsPopulated(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            DataTable datable,
            DataTableQuestionHelper sut)
        {
            //Arrange
            datable.InputType = DataTableInputTypeConstants.CurrencyDataTableInput;

            var answerText = new[] { (decimal?)null, Convert.ToDecimal(100), Convert.ToDecimal(200) };

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            question.QuestionType = QuestionTypeConstants.DataTable;

            for (int i = 0; i < answerText.Length; i++)
            {
                var value = answerText[i]?.ToString();
                TableInput input = new TableInput(i.ToString(), null, true, true, value);
                datable.Inputs[i] = input;
            }

            question.Answers![0].AnswerText = JsonSerializer.Serialize(datable);

            //Act
            var result = await sut.GetDecimalAnswerArray(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(answerText[0], result[0]);
            Assert.Equal(answerText[1], result[1]);
            Assert.Equal(answerText[2], result[2]);
        }
    }
}
