using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen.Helpers
{
    public class RadioQuestionHelperTests
    {

        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            RadioQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, "A");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GivenAnswersAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            Question question,
            RadioQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.RadioQuestion;
            question.Answers = null;

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);


            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, "A");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("A", "A", true)]
        [InlineAutoMoqData("A", "B", false)]
        public async Task AnswerEquals_ReturnsExpectedValue(
            string expectedIdentifier,
            string choiceIdToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            Question question,
            RadioQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.RadioQuestion;
            question.Answers = new List<Answer> { new Answer() { Choice = new QuestionChoice() { Identifier = expectedIdentifier } }}.ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);


            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, choiceIdToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }
        

        [Theory]
        [AutoMoqData]
        public async Task AnswerIn_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            RadioQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);


            var stringAnswers = new string[] { "A" };

            //Act
            var result = await sut.AnswerIn(correlationId, workflowName, activityName, questionId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerIn_ReturnsFalse_GivenAnswersAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            RadioQuestionHelper sut)
        {
            //Arrange
            question.Answers = null;

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);


            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerIn(correlationId, workflowName, activityName, questionId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("A", new string[] { "A", "B" }, true)]
        [InlineAutoMoqData("A", new string[] { "B", "C" }, false)]
        public async Task AnswerIn_ReturnsExpectedValue(
            string answer,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            RadioQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.RadioQuestion;

            question.Answers = new List<Answer> { new Answer() { Choice = new QuestionChoice() { Identifier = answer } } }.ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);


            //Act
            var result = await sut.AnswerIn(correlationId, workflowName, activityName, questionId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEquals_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            RadioQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, dataDictionaryId, "A");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEquals_ReturnsFalse_GivenAnswersAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            Question question,
            RadioQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.RadioQuestion;
            question.Answers = null;

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, dataDictionaryId, "A");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("A", "A", true)]
        [InlineAutoMoqData("A", "B", false)]
        public async Task DataDictionaryAnswerEquals_ReturnsExpectedValue(
            string expectedIdentifier,
            string choiceIdToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            Question question,
            RadioQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.RadioQuestion;
            question.Answers = new List<Answer> { new Answer() { Choice = new QuestionChoice() { Identifier = expectedIdentifier } } }.ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, dataDictionaryId, choiceIdToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }


        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerIn_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            RadioQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync((Question?)null);

            var stringAnswers = new string[] { "A" };

            //Act
            var result = await sut.AnswerIn(activityExecutionContext.CorrelationId, dataDictionaryId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerIn_ReturnsFalse_GivenAnswersAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            Question question,
            RadioQuestionHelper sut)
        {
            //Arrange
            question.Answers = null;

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync(question);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerIn(activityExecutionContext.CorrelationId, dataDictionaryId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("A", new string[] { "A", "B" }, true)]
        [InlineAutoMoqData("A", new string[] { "B", "C" }, false)]
        public async Task DataDictionaryAnswerIn_ReturnsExpectedValue(
            string answer,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            Question question,
            RadioQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.RadioQuestion;

            question.Answers = new List<Answer> { new Answer() { Choice = new QuestionChoice() { Identifier = answer } } }.ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerIn(activityExecutionContext.CorrelationId, dataDictionaryId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
