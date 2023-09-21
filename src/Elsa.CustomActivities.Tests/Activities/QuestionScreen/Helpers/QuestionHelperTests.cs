using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen.Helpers
{
    public class QuestionHelperTests
    {

        [Theory]
        [AutoMoqData]
        public async Task GetAnswer_ReturnsEmptyString_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            QuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetAnswer(correlationId, workflowName, activityName, questionId);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAnswer_ReturnsValue(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowName,
            string activityId,
            string activityName,
            string questionId,
            string correlationId,
            WorkflowBlueprint workflowBlueprint,
            Question question,
            QuestionHelper sut)
        {
            //Arrange
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = activityId,
                Name = activityName
            });

            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.GetAnswer(correlationId, workflowName, activityName, questionId);

            //Assert

            Assert.Equal(string.Join(",",question.Answers!.Select(x => x.AnswerText)), result);
        }
    }
}
