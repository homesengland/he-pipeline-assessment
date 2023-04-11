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
            elsaCustomRepository.Setup(x => x.GetQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(new List<Question>());

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
            elsaCustomRepository.Setup(x => x.GetQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

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

            elsaCustomRepository.Setup(x => x.GetQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

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

            elsaCustomRepository.Setup(x => x.GetQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

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

            elsaCustomRepository.Setup(x => x.GetQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

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

            elsaCustomRepository.Setup(x => x.GetQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

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

            elsaCustomRepository.Setup(x => x.GetQuestions(workflowInstanceId, CancellationToken.None)).ReturnsAsync(questions);

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
            var result = await sut.GetPotScore(workflowInstanceId);

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
            var result = await sut.GetPotScore(workflowInstanceId);

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
            var result = await sut.GetPotScore(workflowInstanceId);

            //Assert
            Assert.Equal(instance.Score, result);
        }
    }
}
