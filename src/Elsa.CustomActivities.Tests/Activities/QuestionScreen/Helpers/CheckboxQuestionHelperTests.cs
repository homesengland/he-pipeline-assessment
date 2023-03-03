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
using System.Text.Json;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen.Helpers
{
    public class CheckboxQuestionHelperTests
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
         CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

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
            CheckboxQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

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
            CheckboxQuestionHelper sut)
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
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

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
            CheckboxQuestionHelper sut)
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
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "Answer 1" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "Answer 1" }, new string[] { "B", "C" }, false)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2" }, new string[] { "A" }, false)]
        public async Task AnswerEquals_ReturnsExpectedValue(
            string[] answers,
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
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            var jsonAnswer = JsonSerializer.Serialize(answers);

            questionScreenAnswer.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            questionScreenAnswer.Answer = jsonAnswer;
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
            var result = await sut.AnswerEquals(workflowInstanceId, workflowName, activityName, questionId, choiceIdsToCheck);

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
        string workflowInstanceId,
        CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

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
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            CheckboxQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

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
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            CheckboxQuestionHelper sut)
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
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

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
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            QuestionScreenAnswer questionScreenAnswer,
            CheckboxQuestionHelper sut)
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
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "Answer 1" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2" }, new string[] { "A" }, true)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2", "Answer 3" }, new string[] { "A", "B" }, true)]
        [InlineAutoMoqData(new string[] { "Answer 1" }, new string[] { "B", "C" }, false)]

        public async Task AnswerContains_ReturnsExpectedValue(
            string[] answers,
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
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            var jsonAnswer = JsonSerializer.Serialize(answers);

            questionScreenAnswer.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            questionScreenAnswer.Answer = jsonAnswer;
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
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, choiceIdsToCheck);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAnswer_ReturnsEmptyString_GivenWorkflowFindByNameAsyncReturnsNull(
         [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
         [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
         string workflowName,
         string activityName,
         string questionId,
         string workflowInstanceId,
         CheckboxQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            //Act
            var result = await sut.GetAnswer(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(String.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAnswer_ReturnsFalse_WorkflowActivitesReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            CheckboxQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.GetAnswer(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(String.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAnswer_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            CheckboxQuestionHelper sut)
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
            var result = await sut.GetAnswer(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(String.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAnswer_ReturnsExpectedValue(
        string[] answers,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
        string workflowName,
        string activityId,
        string activityName,
        string questionId,
        string workflowInstanceId,
        WorkflowBlueprint workflowBlueprint,
        QuestionScreenAnswer questionScreenAnswer,
        CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            var jsonAnswer = JsonSerializer.Serialize(answers);

            questionScreenAnswer.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            questionScreenAnswer.Answer = jsonAnswer;


            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(questionScreenAnswer);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var expectedResult = string.Join(", ", answers);

            //Act
            var result = await sut.GetAnswer(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(expectedResult, result);
        }

    }
}
