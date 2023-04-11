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
            elsaCustomRepository.Setup(x => x.GetQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

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
            elsaCustomRepository.Setup(x => x.GetQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

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

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

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
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(question);

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
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = answers.Select(x => new Answer { AnswerText = x }).ToList();
            question.Choices = new List<QuestionChoice>()
            {
                new QuestionChoice()
                {
                    Answer = "Answer 1",
                    Identifier = "A"
                },
                new QuestionChoice()
                {
                    Answer = "Answer 2",
                    Identifier = "B"
                },
                new QuestionChoice()
                {
                    Answer = "Answer 3",
                    Identifier = "C"
                }
            };

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(question);

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
            elsaCustomRepository.Setup(x => x.GetQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

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
            elsaCustomRepository.Setup(x => x.GetQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

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

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

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
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Choices = null;

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var stringAnwers = new string[] { "A" };
            //Act
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, stringAnwers);

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
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, choiceIdsToCheck);

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
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = expectedIdentifiers
                .Select(x => new Answer() { Choice = new QuestionChoice() { Identifier = x } }).ToList();

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerContains(workflowInstanceId, workflowName, activityName, questionId, choiceIdsToCheck, true);

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
            elsaCustomRepository.Setup(x => x.GetQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

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
            elsaCustomRepository.Setup(x => x.GetQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

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

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.GetAnswer(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(String.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAnswer_ReturnsExpectedValue(
        List<Answer> answers,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
        string workflowName,
        string activityId,
        string activityName,
        string questionId,
        string workflowInstanceId,
        WorkflowBlueprint workflowBlueprint,
        Question question,
        CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = answers;


            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            var expectedResult = string.Join(",", answers.Select(x => x.AnswerText));

            //Act
            var result = await sut.GetAnswer(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerCount_ReturnsDefaultValue_GivenWorkflowFindByNameAsyncReturnsNull(
        [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
        string workflowName,
        string activityName,
        string questionId,
        string workflowInstanceId,
        CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            //Act
            var result = await sut.AnswerCount(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(-1, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerCount_ReturnsDefaultValue_GivenWorkflowActivitiesReturnsNull(
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
            elsaCustomRepository.Setup(x => x.GetQuestion(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerCount(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(-1, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerCount_ReturnsDefaultValue_GivenGetQuestionRecordReturnsNull(
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

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerCount(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(-1, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerCount_ReturnsDefaultValue_GivenGetQuestionRecordReturnsNotACheckbox(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            Question question,
            WorkflowBlueprint workflowBlueprint,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = "NotACheckbox";
            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerCount(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(-1, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerCount_ReturnsDefaultValue_GivenChoicesAreNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;
            question.Answers = null;

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerCount(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineAutoMoqData(new string[] { "Answer 1" }, 1)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2" }, 2)]
        [InlineAutoMoqData(new string[] { "Answer 1", "Answer 2", "Answer 3" }, 3)]

        public async Task AnswerCount_ReturnsExpectedValue(
            string[] answers,
            int expectedResult,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            CheckboxQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            question.Answers = new List<Answer>();
            foreach (var answer in answers)
            {
                question.Answers.Add(new Answer
                {
                    AnswerText = answer
                });
            }

            question.QuestionType = QuestionTypeConstants.CheckboxQuestion;

            elsaCustomRepository.Setup(x => x.GetQuestion(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(question);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerCount(workflowInstanceId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
