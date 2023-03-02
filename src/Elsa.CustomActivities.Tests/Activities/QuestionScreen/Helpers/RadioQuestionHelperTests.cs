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
    public class RadioQuestionHelperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task AnswerEquals_ReturnsFalse_GivenWorkflowFindByNameAsyncReturnsNull(
          [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
          [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
          string workflowName,
          string activityName,
          string questionId,
          string workflowInstanceId,
          RadioQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            //Act
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, "A");

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
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            RadioQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, "A");

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
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            RadioQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(activityId, workflowInstanceId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, "A");

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
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            QuestionScreenAnswer questionScreenAnswer,
            RadioQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            questionScreenAnswer.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            questionScreenAnswer.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(questionScreenAnswer);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, "A");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("Answer 1", "A", true)]
        [InlineAutoMoqData("Answer 1", "B", false)]
        public async Task AnswerEquals_ReturnsExpectedValue(
            string answer,
            string choiceIdToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            QuestionScreenAnswer questionScreenAnswer,
            RadioQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            questionScreenAnswer.QuestionType = QuestionTypeConstants.RadioQuestion;
            questionScreenAnswer.Answer = answer;
            questionScreenAnswer.Choices = new List<QuestionScreenAnswer.Choice>()
            {
                new QuestionScreenAnswer.Choice()
                {
                    Answer = "Answer 1",
                    Identifier = "A"
                },
                new QuestionScreenAnswer.Choice()
                {
                    Answer = "Answer 2",
                    Identifier = "B"
                },
                new QuestionScreenAnswer.Choice()
                {
                    Answer = "Answer 3",
                    Identifier = "C"
                }
            };

            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(questionScreenAnswer);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, choiceIdToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerIn_ReturnsFalse_GivenWorkflowFindByNameAsyncReturnsNull(
          [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
          [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
          string workflowName,
          string activityName,
          string questionId,
          string workflowInstanceId,
          RadioQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            var stringAnwers = new string[] { "A" };

            //Act
            var result = await sut.AnswerIn(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerIn_ReturnsFalse_WorkflowActivitesReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            RadioQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };

            //Act
            var result = await sut.AnswerIn(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerIn_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            RadioQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(activityId, workflowInstanceId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };

            //Act
            var result = await sut.AnswerIn(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerIn_ReturnsFalse_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            QuestionScreenAnswer questionScreenAnswer,
            RadioQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            questionScreenAnswer.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            questionScreenAnswer.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(questionScreenAnswer);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerIn(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData("Answer 1", new string[] { "A", "B" }, true)]
        [InlineAutoMoqData("Answer 1", new string[] { "B", "C" }, false)]
        public async Task AnswerIn_ReturnsExpectedValue(
            string answer,
            string[] choiceIdsToCheck,
            bool expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            QuestionScreenAnswer questionScreenAnswer,
            RadioQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            questionScreenAnswer.QuestionType = QuestionTypeConstants.RadioQuestion;
            questionScreenAnswer.Answer = answer;
            questionScreenAnswer.Choices = new List<QuestionScreenAnswer.Choice>()
            {
                new QuestionScreenAnswer.Choice()
                {
                    Answer = "Answer 1",
                    Identifier = "A"
                },
                new QuestionScreenAnswer.Choice()
                {
                    Answer = "Answer 2",
                    Identifier = "B"
                },
                new QuestionScreenAnswer.Choice()
                {
                    Answer = "Answer 3",
                    Identifier = "C"
                }
            };

            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(questionScreenAnswer);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerIn(workflowInstanceId, workflowName, activityName, questionId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
