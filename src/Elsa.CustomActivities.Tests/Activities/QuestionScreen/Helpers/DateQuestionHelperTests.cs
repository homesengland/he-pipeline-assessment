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
    public class DateQuestionHelperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task AnswerEqualToOrGreaterThan_ReturnsFalse_GivenWorkflowFindByNameAsyncReturnsNull(
       [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
       [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
       string workflowName,
       string activityName,
       string questionId,
       string workflowInstanceId,
       DateQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(workflowInstanceId, workflowName, activityName, questionId, 1, 2, 1900);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEqualToOrGreaterThan_ReturnsFalse_WorkflowActivitesReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            DateQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(workflowInstanceId, workflowName, activityName, questionId, 1, 2, 1900);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEqualToOrGreaterThan_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            DateQuestionHelper sut)
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
            var result = await sut.AnswerEqualToOrGreaterThan(workflowInstanceId, workflowName, activityName, questionId, 1, 2, 1900);

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
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            QuestionScreenAnswer questionScreenAnswer,
            DateQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            questionScreenAnswer.QuestionType = QuestionTypeConstants.DateQuestion;
            questionScreenAnswer.Answer = $"{answerYear}-{answerMonth}-{answerDay}";

            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(questionScreenAnswer);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerEqualToOrGreaterThan(workflowInstanceId, workflowName, activityName, questionId, answerToCheckDay, answerToCheckMonth, answerToCheckYear);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEqualToOrLessThan_ReturnsFalse_GivenWorkflowFindByNameAsyncReturnsNull(
             [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
             [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
             string workflowName,
             string activityName,
             string questionId,
             string workflowInstanceId,
             DateQuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync((WorkflowBlueprint?)null);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(workflowInstanceId, workflowName, activityName, questionId, 1, 2, 1900);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEqualToOrLessThan_ReturnsFalse_WorkflowActivitesReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            DateQuestionHelper sut)
        {

            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((QuestionScreenAnswer?)null);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(workflowInstanceId, workflowName, activityName, questionId, 1, 2, 1900);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AnswerEqualToOrLessThan_ReturnsFalse_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            DateQuestionHelper sut)
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
            var result = await sut.AnswerEqualToOrLessThan(workflowInstanceId, workflowName, activityName, questionId, 1, 2, 1900);

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
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string workflowInstanceId,
            WorkflowBlueprint workflowBlueprint,
            QuestionScreenAnswer questionScreenAnswer,
            DateQuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });
            questionScreenAnswer.QuestionType = QuestionTypeConstants.DateQuestion;
            questionScreenAnswer.Answer = $"{answerYear}-{answerMonth}-{answerDay}";

            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswer(activityId, workflowInstanceId, questionId, CancellationToken.None)).ReturnsAsync(questionScreenAnswer);

            workflowRegistry.Setup(x => x.FindByNameAsync(workflowName!, VersionOptions.Published, null, default)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.AnswerEqualToOrLessThan(workflowInstanceId, workflowName, activityName, questionId, answerToCheckDay, answerToCheckMonth, answerToCheckYear);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
