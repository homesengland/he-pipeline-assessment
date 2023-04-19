using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Scoring.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Elsa.CustomModels;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.Scoring.Helpers
{
    public class EconomicScoreCalculationHelperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetEconomicCalculation_ReturnsDefaultResult_GivenNoWorkflowInstances(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            EconomicScoreCalculationHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None)).ReturnsAsync(new List<QuestionWorkflowInstance>());

            //Act
            var result = await sut.GetEconomicCalculation(correlationId, name);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetEconomicCalculation_ReturnsDefaultResult_GivenNoScoreSetOnWorkflowInstance(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            EconomicScoreCalculationHelper sut)
        {
            //Arrange
            var workflowInstance = new QuestionWorkflowInstance
            {
                Score = null
            };
            elsaCustomRepository
                .Setup(x => x.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None))
                .ReturnsAsync(new List<QuestionWorkflowInstance>() { workflowInstance });

            //Act
            var result = await sut.GetEconomicCalculation(correlationId, name);

            //Assert
            Assert.Equal(0, result);
        }


        [Theory]
        [AutoMoqData]
        public async Task GetEconomicCalculation_ReturnsDefaultResult_GivenScoreIsNotANumber(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            EconomicScoreCalculationHelper sut)
        {
            //Arrange
            var workflowInstance = new QuestionWorkflowInstance
            {
                Score = "NotANumber"
            };
            elsaCustomRepository
                .Setup(x => x.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None))
                .ReturnsAsync(new List<QuestionWorkflowInstance>() { workflowInstance });

            //Act
            var result = await sut.GetEconomicCalculation(correlationId, name);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetEconomicCalculation_ReturnsScoreFromLatestWorkflowInstance_GivenScoreIsANumber(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            EconomicScoreCalculationHelper sut)
        {
            //Arrange
            var earlierWorkflowInstance = new QuestionWorkflowInstance
            {
                Score = "1234.5"
            };
            var laterWorkflowInstance = new QuestionWorkflowInstance
            {
                Score = "5678.9"
            };
            elsaCustomRepository
                .Setup(x => x.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None))
                .ReturnsAsync(new List<QuestionWorkflowInstance>() { laterWorkflowInstance, earlierWorkflowInstance });

            //Act
            var result = await sut.GetEconomicCalculation(correlationId, name);

            //Assert
            Assert.Equal(5678.9, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetEconomicCalculation_Rethrows_GivenDependencyThrows(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            EconomicScoreCalculationHelper sut)
        {
            //Arrange
            var exception = new ApplicationException("TestMessage");
            elsaCustomRepository
                .Setup(x => x.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None))
                .ThrowsAsync(exception);

            //Assert
            var exceptionThrown = await Assert.ThrowsAsync<Exception>(() => sut.GetEconomicCalculation(correlationId, name));

            //Assert
            Assert.Equal(exception.Message, exceptionThrown.Message);
        }
    }
}
