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
    public class WeightedCheckboxQuestionHelperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GivenWorkflowFindByNameAsyncReturnsNull(
         [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
         [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
         string workflowName,
         string activityName,
         string questionId,
         string groupId,
         string correlationId,
         WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, groupId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_WorkflowActivitesReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            WeightedCheckboxQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, groupId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, groupId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, groupId, stringAnwers);

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
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId}} }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

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
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerEquals(correlationId, workflowName, activityName, questionId, "CorrectGroup", choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerContains_ReturnsFalse_GivenWorkflowFindByNameAsyncReturnsNull(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
        string workflowName,
        string activityName,
        string questionId,
        string groupId,
        string correlationId,
        WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, groupId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerContains_ReturnsFalse_WorkflowActivitesReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            WeightedCheckboxQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, groupId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerContains_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, groupId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerContains_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, groupId, stringAnwers);

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
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

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
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

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
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string groupId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

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
           [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
           string workflowName,
           string activityId,
           string activityName,
           string questionId,
           string correlationId,
           WorkflowBlueprint workflowBlueprint,
           Question question,
           WeightedCheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x, QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = groupId } } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestionByCorrelationId(activityId, correlationId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerContains(correlationId, workflowName, activityName, questionId, "CorrectGroup", choiceIdsToCheck, true);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
