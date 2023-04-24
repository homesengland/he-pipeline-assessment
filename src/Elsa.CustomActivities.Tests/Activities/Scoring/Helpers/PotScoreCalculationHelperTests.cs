using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Scoring.Helpers;
using Elsa.CustomInfrastructure.Data.Repository;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Elsa.CustomModels;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.Scoring.Helpers
{
    public class PotScoreCalculationHelperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetTotalPotValue_ReturnsFailedResult_GivenWorkflowQuestionsReturnsEmptyList(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(new List<Question>());

            //Act
            var result = await sut.GetTotalPotValue(workflowInstanceId, "test");

            //Assert
            Assert.Equal(-1, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalPotValue_ReturnsZero_GivenNoAnswersForProvidedPot(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            string potScoreCategory,
            List<Question> questions,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            foreach (var question in questions)
            {
                foreach (var questionAnswer in question.Answers!)
                {
                    questionAnswer.Choice!.PotScoreCategory = "RandomPotScoreCategory";
                }
            }
            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

            //Act
            var result = await sut.GetTotalPotValue(workflowInstanceId, potScoreCategory);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalPotValue_ReturnsCorrectCalculatedValue_GivenOneAnswerForProvidedPot(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            string potScoreCategory,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            var questions = new List<Question>
            {
                new()
                {
                    Answers = new List<Answer>
                    {
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = potScoreCategory
                            }
                        }
                    },
                    Weighting = 3
                }
            };

            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

            //Act
            var result = await sut.GetTotalPotValue(workflowInstanceId, potScoreCategory);

            //Assert
            Assert.Equal(3, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalPotValue_ReturnsCorrectCalculatedValue_GivenMultipleAnswersForProvidedPot(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            string potScoreCategory,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            var questions = new List<Question>
            {
                new()
                {
                    Answers = new List<Answer>
                    {
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = potScoreCategory
                            }
                        },
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = potScoreCategory
                            }
                        }
                    },
                    Weighting = 3
                }
            };

            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

            //Act
            var result = await sut.GetTotalPotValue(workflowInstanceId, potScoreCategory);

            //Assert
            Assert.Equal(3, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalPotValue_ReturnsCorrectCalculatedValue_GivenMultipleQuestionAnswersForProvidedPot(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            string potScoreCategory,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            var questions = new List<Question>
            {
                new()
                {
                    Answers = new List<Answer>
                    {
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = potScoreCategory
                            }
                        }
                    },
                    Weighting = 3
                },
                new()
                {
                    Answers = new List<Answer>
                    {
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = potScoreCategory
                            }
                        }
                    },
                    Weighting = 5
                }
            };

            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

            //Act
            var result = await sut.GetTotalPotValue(workflowInstanceId, potScoreCategory);

            //Assert
            Assert.Equal(8, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalPotValue_ReturnsCorrectCalculatedValue_GivenMultipleQuestionAnswersForMixedPots(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            string potScoreCategory,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            var questions = new List<Question>
            {
                new()
                {
                    Answers = new List<Answer>
                    {
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = "IncorrectPot"
                            }
                        }
                    },
                    Weighting = 3
                },
                new()
                {
                    Answers = new List<Answer>
                    {
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = potScoreCategory
                            }
                        }
                    },
                    Weighting = 5
                }
            };

            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

            //Act
            var result = await sut.GetTotalPotValue(workflowInstanceId, potScoreCategory);

            //Assert
            Assert.Equal(5, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetTotalPotValue_IsNotCaseSensitive(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            PotScoreCalculationHelper sut)
        {
            var potScoreCategory = "MeDiUm";
            //Arrange
            var questions = new List<Question>
            {
                new()
                {
                    Answers = new List<Answer>
                    {
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = "medium"
                            }
                        }
                    },
                    Weighting = 3
                },
                new()
                {
                    Answers = new List<Answer>
                    {
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = "MEDIUM"
                            }
                        }
                    },
                    Weighting = 5
                },
                new()
                {
                    Answers = new List<Answer>
                    {
                        new()
                        {
                            Choice = new QuestionChoice
                            {
                                PotScoreCategory = "mEDIuM"
                            }
                        }
                    },
                    Weighting = 7
                }
            };

            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

            //Act
            var result = await sut.GetTotalPotValue(workflowInstanceId, potScoreCategory);

            //Assert
            Assert.Equal(15, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ReturnsFailedResult_GivenWorkflowQuestionsInstanceDoesNotExist(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance(workflowInstanceId, CancellationToken.None)).ReturnsAsync((QuestionWorkflowInstance?)null);

            //Act
            var result = await sut.GetWorkflowScore(workflowInstanceId);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ReturnsFailedResult_GivenWorkflowQuestionsInstanceDoesNotHaveScore(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            QuestionWorkflowInstance instance,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            instance.Score = null;
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance(workflowInstanceId, CancellationToken.None)).ReturnsAsync(instance);

            //Act
            var result = await sut.GetWorkflowScore(workflowInstanceId);

            //Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ReturnsCorrectScore_GivenWorkflowQuestionsInstanceHasScore(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            QuestionWorkflowInstance instance,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance(workflowInstanceId, CancellationToken.None)).ReturnsAsync(instance);

            //Act
            var result = await sut.GetWorkflowScore(workflowInstanceId);

            //Assert
            Assert.Equal(instance.Score, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_Rethrows_GivenDependencyThrows(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            string potScoreCategory,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            var exception = new ApplicationException("TestMessage");
            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None)).ThrowsAsync(exception);

            //Act
            var exceptionThrown = await Assert.ThrowsAsync<Exception>(() => sut.GetTotalPotValue(workflowInstanceId, potScoreCategory));

            //Assert
            Assert.Equal(exception.Message, exceptionThrown.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPotScoreCalculation_ReturnsDefaultResult_GivenNoWorkflowInstances(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None)).ReturnsAsync(new List<QuestionWorkflowInstance>());

            //Act
            var result = await sut.GetPotScoreCalculation(correlationId, name);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPotScoreCalculation_ReturnsDefaultResult_GivenNoScoreSetOnWorkflowInstance(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            PotScoreCalculationHelper sut)
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
            var result = await sut.GetPotScoreCalculation(correlationId, name);

            //Assert
            Assert.Equal(0, result);
        }


        [Theory]
        [AutoMoqData]
        public async Task GetPotScoreCalculation_ReturnsDefaultResult_GivenScoreIsNotANumber(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            PotScoreCalculationHelper sut)
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
            var result = await sut.GetPotScoreCalculation(correlationId, name);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPotScoreCalculation_ReturnsScoreFromLatestWorkflowInstance_GivenScoreIsANumber(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            PotScoreCalculationHelper sut)
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
            var result = await sut.GetPotScoreCalculation(correlationId, name);

            //Assert
            Assert.Equal(5678.9, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPotScoreCalculation_Rethrows_GivenDependencyThrows(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string name,
            string correlationId,
            PotScoreCalculationHelper sut)
        {
            //Arrange
            var exception = new ApplicationException("TestMessage");
            elsaCustomRepository
                .Setup(x => x.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None))
                .ThrowsAsync(exception);

            //Assert
            var exceptionThrown = await Assert.ThrowsAsync<Exception>(() => sut.GetPotScoreCalculation(correlationId, name));

            //Assert
            Assert.Equal(exception.Message, exceptionThrown.Message);
        }
    }
}
