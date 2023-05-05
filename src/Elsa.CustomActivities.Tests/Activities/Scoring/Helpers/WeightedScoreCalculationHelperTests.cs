using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Scoring.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Elsa.CustomModels;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.Scoring.Helpers
{
    public class WeightedScoreCalculationHelperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetTotalWeightedScoreValue_ReturnsDefaultResult_GivenWorkflowQuestionsReturnsEmptyList(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            WeightedScoreCalculationHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(new List<Question>());

            //Act
            var result = await sut.GetTotalWeightedScoreValue(workflowInstanceId);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalWeightedScoreValue_ReturnsCorrectCalculatedValue_GivenOneQuestionWithScore(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            WeightedScoreCalculationHelper sut)
        {
            //Arrange
            var questions = new List<Question>
            {
                new()
                {
                    Score = 23
                }
            };

            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

            //Act
            var result = await sut.GetTotalWeightedScoreValue(workflowInstanceId);

            //Assert
            Assert.Equal(23, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalWeightedScoreValue_ReturnsCorrectCalculatedValue_GivenMultipleQuestionsWithScores(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            WeightedScoreCalculationHelper sut)
        {
            //Arrange
            var questions = new List<Question>
            {
                new()
                {
                    Score = 29.5m
                },                
                new()
                {
                    Score = 39
                }

            };

            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

            //Act
            var result = await sut.GetTotalWeightedScoreValue(workflowInstanceId);

            //Assert
            Assert.Equal(68.5m, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalWeightedScoreValue_ReturnsCorrectCalculatedValue_GivenMultipleQuestionsWithScoresAndNoScores(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            WeightedScoreCalculationHelper sut)
        {
            //Arrange
            var questions = new List<Question>
            {
                new()
                {
                    Score = 10
                },
                new()
                {
                    Score = null
                },
                new()
                {
                    Score = 5.5m
                }
            };

            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

            //Act
            var result = await sut.GetTotalWeightedScoreValue(workflowInstanceId);

            //Assert
            Assert.Equal(15.5m, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalWeightedScoreValue_Rethrows_GivenDependencyThrows(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            WeightedScoreCalculationHelper sut)
        {
            //Arrange
            var exception = new ApplicationException("TestMessage");
            elsaCustomRepository
                .Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None))
                .ThrowsAsync(exception);

            //Assert
            var exceptionThrown = await Assert.ThrowsAsync<Exception>(() => sut.GetTotalWeightedScoreValue(workflowInstanceId));

            //Assert
            Assert.Equal(exception.Message, exceptionThrown.Message);
        }
        
        [Theory]
        [AutoMoqData]
        public async Task GetWeightedScoreCalculation_ReturnsDefaultResult_GivenNoWorkflowInstances(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            WeightedScoreCalculationHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None)).ReturnsAsync(new List<QuestionWorkflowInstance>());

            //Act
            var result = await sut.GetWeightedScoreCalculation(correlationId, name);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetWeightedScoreCalculation_ReturnsDefaultResult_GivenNoScoreSetOnWorkflowInstance(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            WeightedScoreCalculationHelper sut)
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
            var result = await sut.GetWeightedScoreCalculation(correlationId, name);

            //Assert
            Assert.Equal(0, result);
        }


        [Theory]
        [AutoMoqData]
        public async Task GetWeightedScoreCalculation_ReturnsDefaultResult_GivenScoreIsNotANumber(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            WeightedScoreCalculationHelper sut)
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
            var result = await sut.GetWeightedScoreCalculation(correlationId, name);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetWeightedScoreCalculation_ReturnsScoreFromLatestWorkflowInstance_GivenScoreIsANumber(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            WeightedScoreCalculationHelper sut)
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
            var result = await sut.GetWeightedScoreCalculation(correlationId, name);

            //Assert
            Assert.Equal(5678.9, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetWeightedScoreCalculation_Rethrows_GivenDependencyThrows(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            WeightedScoreCalculationHelper sut)
        {
            //Arrange
            var exception = new ApplicationException("TestMessage");
            elsaCustomRepository
                .Setup(x => x.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None))
                .ThrowsAsync(exception);

            //Assert
            var exceptionThrown = await Assert.ThrowsAsync<Exception>(() => sut.GetWeightedScoreCalculation(correlationId, name));

            //Assert
            Assert.Equal(exception.Message, exceptionThrown.Message);
        }
    }
}
