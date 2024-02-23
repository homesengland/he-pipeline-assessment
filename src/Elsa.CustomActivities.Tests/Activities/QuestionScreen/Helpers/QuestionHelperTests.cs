using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
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
            string activityName,
            string questionId,
            string correlationId,
            QuestionHelper sut)
        {
            //Arrange
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
            string activityName,
            string questionId,
            string correlationId,
            Question question,
            QuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionByWorkflowAndActivityName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(question);

            //Act
            var result = await sut.GetAnswer(correlationId, workflowName, activityName, questionId);

            //Assert

            Assert.Equal(string.Join(",",question.Answers!.Select(x => x.AnswerText)), result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetAnswer_ReturnsEmptyString_GetQuestionRecordReturnsNull(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            QuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync((Question?)null);

            //Act
            var result = await sut.GetAnswer(correlationId, dataDictionaryId);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DataDictionaryGetAnswer_ReturnsValue(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            int dataDictionaryId,
            string correlationId,
            Question question,
            QuestionHelper sut)
        {
            //Arrange
            elsaCustomRepository
                .Setup(x => x.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None))
                .ReturnsAsync(question);

            //Act
            var result = await sut.GetAnswer(correlationId, dataDictionaryId);

            //Assert

            Assert.Equal(string.Join(",", question.Answers!.Select(x => x.AnswerText)), result);
        }
    }
}
