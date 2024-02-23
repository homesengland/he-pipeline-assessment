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
    public class WeightedCheckboxQuestionHelperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, groupId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, groupId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, false)]
        public async Task AnswerEquals_ReturnsExpectedValue(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId}} }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, groupId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, "IncorrectGroup",  false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, "CorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, "CorrectGroup", false)]
        public async Task AnswerEquals_ReturnsExpectedValueFromCorrectGroup(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            string groupId,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, "CorrectGroup", choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerContains_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, groupId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerContains_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, groupId, stringAnswers);

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
            string workflowName,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, groupId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, "CorrectGroup", false)]

        public async Task AnswerContains_ReturnsExpectedValueFromCorrectGroup(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            string groupId,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, "CorrectGroup", choiceIdsToCheck);

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
            string workflowName,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, groupId, choiceIdsToCheck, true);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "D" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B", "C" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C", "D" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, "CorrectGroup", false)]

        public async Task AnswerContainsAny_ReturnsExpectedValueFromCorrectGroups(
           string[] expectedIdentifiers,
           string[] choiceIdsToCheck,
           string groupId,
           bool expectedResult,
           [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
           string workflowName,
           string activityName,
           string questionId,
           string correlationId,
           Question question,
           WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, "CorrectGroup", choiceIdsToCheck, true);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEquals_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string groupId,
            string correlationId,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(correlationId, dataDictionaryId, groupId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerEquals_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(correlationId, dataDictionaryId, groupId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, false)]
        public async Task DataDictionaryAnswerEquals_ReturnsExpectedValue(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEquals(correlationId, dataDictionaryId, groupId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, "CorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, "CorrectGroup", false)]
        public async Task DataDictionaryAnswerEquals_ReturnsExpectedValueFromCorrectGroup(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            string groupId,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerEquals(correlationId, dataDictionaryId, "CorrectGroup", choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerContains_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string groupId,
            string correlationId,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync((Question?)null);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(correlationId, dataDictionaryId, groupId, stringAnswers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryAnswerContains_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            var stringAnswers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(correlationId, dataDictionaryId, groupId, stringAnswers);

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
            int dataDictionaryId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(correlationId, dataDictionaryId, groupId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, "CorrectGroup", false)]

        public async Task DataDictionaryAnswerContains_ReturnsExpectedValueFromCorrectGroup(
            string[] expectedIdentifiers,
            string[] choiceIdsToCheck,
            string groupId,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);


            //Act
            var result = await sut.AnswerContains(correlationId, dataDictionaryId, "CorrectGroup", choiceIdsToCheck);

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
            int dataDictionaryId,
            string groupId,
            string correlationId,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(correlationId, dataDictionaryId, groupId, choiceIdsToCheck, true);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "A" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "D" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B" }, new string[] { "A", "B", "C" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C" }, "CorrectGroup", true)]
        [InlineAutoMoqData(new string[] { "A", "B", "C" }, new string[] { "A", "B", "C", "D" }, "IncorrectGroup", false)]
        [InlineAutoMoqData(new string[] { "A" }, new string[] { "B", "C" }, "CorrectGroup", false)]

        public async Task DataDictionaryAnswerContainsAny_ReturnsExpectedValueFromCorrectGroups(
           string[] expectedIdentifiers,
           string[] choiceIdsToCheck,
           string groupId,
           bool expectedResult,
           [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
           int dataDictionaryId,
           string correlationId,
           Question question,
           WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.AnswerContains(correlationId, dataDictionaryId, "CorrectGroup", choiceIdsToCheck, true);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
