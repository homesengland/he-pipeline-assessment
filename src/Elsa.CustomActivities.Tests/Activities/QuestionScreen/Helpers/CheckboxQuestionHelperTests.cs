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
    public class CheckboxQuestionHelperTests
    {

        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            string workflowName,
            string activityName,
            string questionId,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, activityExecutionContext.CorrelationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            string workflowName,
            string activityName,
            string questionId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, activityExecutionContext.CorrelationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, false)]
        [InlineAutoMoqData(new string[] { "A", "B", "E" }, new string[] { "A", "D", "E" }, false)]
        public async Task AnswerEquals_ReturnsExpectedValue(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            string workflowName,
            string activityName,
            string questionId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, activityExecutionContext.CorrelationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerContains_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            string workflowName,
            string activityName,
            string questionId,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, activityExecutionContext.CorrelationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(activityExecutionContext, workflowName, activityName, questionId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerContains_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            string workflowName,
            string activityName,
            string questionId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, activityExecutionContext.CorrelationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(activityExecutionContext, workflowName, activityName, questionId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, false)]

        public async Task AnswerContains_ReturnsExpectedValue(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            string workflowName,
            string activityName,
            string questionId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, activityExecutionContext.CorrelationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "D" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B", "C" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C", "D" }, true)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, false)]

        public async Task AnswerContainsAny_ReturnsExpectedValue(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            string workflowName,
            string activityName,
            string questionId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, activityExecutionContext.CorrelationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck, true);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAnswer_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetAnswer(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(String.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAnswer_ReturnsExpectedValue(
        List<Answer> answers,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        string workflowName,
        string activityName,
        string questionId,
        string correlationId,
        Question question,
        CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = answers;


            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            var expectedResult = string.Join(",", answers.Select(x => x.AnswerText));

            //Act
            var result = await sut.GetAnswer(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerCount_ReturnsDefaultValue_GivenGetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerCount(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerCount_ReturnsDefaultValue_GivenGetQuestionRecordReturnsNotACheckbox(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = "NotACheckbox";
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName,correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerCount(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerCount_ReturnsDefaultValue_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = null;

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerCount(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "Answer 1" }, 1)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2" }, 2)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2", "Answer 3" }, 3)]

        public async Task AnswerCount_ReturnsExpectedValue(
            string[] answers,
            int expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.Answers = new List<Answer>();
            foreach (var answer in answers)
            {
                question.Answers.Add(new Answer
                {
                    AnswerText = answer
                });
            }

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerCount(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEquals_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            string workflowName,
            string activityName,
            string questionId,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(activityName, workflowName, activityExecutionContext.CorrelationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEquals_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            int dataDictionaryId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, dataDictionaryId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, false)]
        [InlineAutoMoqData(new string[] { "A", "B", "E" }, new string[] { "A", "D", "E" }, false)]
        public async Task DataDictionaryAnswerEquals_ReturnsExpectedValue(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            int dataDictionaryId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x } }).ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEquals(activityExecutionContext, dataDictionaryId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerContains_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            int dataDictionaryId,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(activityExecutionContext, dataDictionaryId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerContains_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            int dataDictionaryId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(activityExecutionContext, dataDictionaryId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, false)]

        public async Task DataDictionaryAnswerContains_ReturnsExpectedValue(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            int dataDictionaryId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x } }).ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(activityExecutionContext, dataDictionaryId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "D" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B", "C" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C", "D" }, true)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, false)]

        public async Task DataDictionaryAnswerContainsAny_ReturnsExpectedValue(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] ActivityExecutionContext activityExecutionContext,
            int dataDictionaryId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x } }).ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(activityExecutionContext, dataDictionaryId, choiceIdsToCheck, true);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetAnswer_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetAnswer(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(String.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetAnswer_ReturnsExpectedValue(
        List<Answer> answers,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        int dataDictionaryId,
        string correlationId,
        Question question,
        CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = answers;


            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            var expectedResult = string.Join(",", answers.Select(x => x.AnswerText));

            //Act
            var result = await sut.GetAnswer(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerCount_ReturnsDefaultValue_GivenGetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.AnswerCount(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerCount_ReturnsDefaultValue_GivenGetQuestionRecordReturnsNotACheckbox(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = "NotACheckbox";
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerCount(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerCount_ReturnsDefaultValue_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = null;

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerCount(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "Answer 1" }, 1)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2" }, 2)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2", "Answer 3" }, 3)]

        public async Task DataDictionaryAnswerCount_ReturnsExpectedValue(
            string[] answers,
            int expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            question.Answers = new List<Answer>();
            foreach (var answer in answers)
            {
                question.Answers.Add(new Answer
                {
                    AnswerText = answer
                });
            }

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerCount(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
