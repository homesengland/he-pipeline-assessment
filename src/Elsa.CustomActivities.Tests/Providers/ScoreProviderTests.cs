using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Providers;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using He.PipelineAssessment.Tests.Common;
using Xunit;
using Question = Elsa.CustomActivities.Activities.QuestionScreen.Question;

namespace Elsa.CustomActivities.Tests.Providers
{
    public class ScoreProviderTests
    {
        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnSumOfSelectedAnswers_GivenWeightedCheckboxStandardAnswersWithScores(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = null;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(334, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnMaxScore_GivenWeightedCheckboxStandardScoreIsHigherThanMaxScore(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = 300;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} }
            };

            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = null;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(300, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnWeightedSumOfSelectedAnswers_GivenWeightedCheckboxStandardScoreIsLowerThanMaxScore(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = 400;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = 0.1;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(33.4m, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnWeightedMaxScore_GivenWeightedCheckboxStandardScoreIsHigherThanMaxScore(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = 300;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = 0.1;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(30, result);
        }

        [Theory]
        [InlineAutoMoqData(new [] { 2.5, 4, 8.1 }, 1, 2.5)]
        [InlineAutoMoqData(new [] { 2.5, 4, 8.1 }, 2, 4)]
        [InlineAutoMoqData(new [] { 2.5, 4, 8.1 }, 3, 8.1)]
        [InlineAutoMoqData(new [] { 2.5, 4, 8.1 }, 10, 8.1)]
        public void GetScore_ShouldReturnScoreFromArray_GivenWeightedCheckboxWithArrayScores(
            double[] scoreArray,
            int numberOfAnswers,
            decimal expectedResult,
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = scoreArray.Select(Convert.ToDecimal).ToList();
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>();
            for (int i = 0; i < numberOfAnswers; i++)
            {
                answeredQuestion.Answers.Add(new Answer()
                {
                    Choice = new QuestionChoice()
                        { QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = "Group A" } }
                });
            }

            answeredQuestion.Weighting = null;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineAutoMoqData(new[] { 2.5, 4, 8.1 }, 1, 2.5)]
        [InlineAutoMoqData(new[] { 2.5, 4, 8.1 }, 2, 4)]
        [InlineAutoMoqData(new[] { 2.5, 4, 8.1 }, 3, 8.1)]
        [InlineAutoMoqData(new[] { 2.5, 4, 8.1 }, 10, 8.1)]
        public void GetScore_ShouldReturnWeightedScoreFromArray_GivenWeightedCheckboxWithArrayScoresAndQuestionWeighting(
            double[] scoreArray,
            int numberOfAnswers,
            decimal expectedResult,
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = scoreArray.Select(Convert.ToDecimal).ToList();
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>();
            for (int i = 0; i < numberOfAnswers; i++)
            {
                answeredQuestion.Answers.Add(new Answer()
                {
                    Choice = new QuestionChoice()
                        { QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = "Group A" } }
                });
            }

            answeredQuestion.Weighting = 2;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(expectedResult*2, result);
        }

        [Theory]
        [InlineAutoMoqData(new[] { 2.5, 4, 8.1 }, 1, 2, 2)]
        [InlineAutoMoqData(new[] { 2.5, 4, 8.1 }, 2, 2, 2)]
        [InlineAutoMoqData(new[] { 2.5, 4, 8.1 }, 3, 2, 2)]
        [InlineAutoMoqData(new[] { 2.5, 4, 8.1 }, 10, 2, 2)]
        public void GetScore_ShouldReturnWeightedMaxScore_GivenWeightedCheckboxWithArrayScoresIsHigherThanMaxScore(
            double[] scoreArray,
            int numberOfAnswers,
            decimal maxScore,
            decimal expectedResult,
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = maxScore;
            questionDefinition.ScoreArray = scoreArray.Select(Convert.ToDecimal).ToList();
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>();
            for (int i = 0; i < numberOfAnswers; i++)
            {
                answeredQuestion.Answers.Add(new Answer()
                {
                    Choice = new QuestionChoice()
                        { QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = "Group A" } }
                });
            }

            answeredQuestion.Weighting = 3;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(expectedResult*3, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnSumOfSelectedAnswersFromMultipleGroups_GivenWeightedCheckboxStandardAnswersWithScoresInMultipleGroups(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} },
                { "Group B", new WeightedCheckboxGroup() { GroupIdentifier = "Group B", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group B"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = null;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(334, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnMaxGroupScoreFromMultipleGroups_GivenWeightedCheckboxStandardScoreWithinGroupsIsHigherThanMaxGroupScore(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = 250} },
                { "Group B", new WeightedCheckboxGroup() { GroupIdentifier = "Group B"} }
            };
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = null;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(250, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnWeightedSumOfSelectedAnswersFromMultipleGroups_GivenWeightedCheckboxStandardAnswersWithScoresAndQuestionWeightingFromMultipleGroups(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} },
                { "Group B", new WeightedCheckboxGroup() { GroupIdentifier = "Group B", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 500,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group B"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = 10;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(8340, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnWeightedMaxGroupScoreFromMultipleGroups_GivenWeightedCheckboxStandardScoreIsHigherThanMaxGroupScore(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = 250} },
                { "Group B", new WeightedCheckboxGroup() { GroupIdentifier = "Group B", GroupArrayScore = null, MaxGroupScore = 300} }
            };
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 500,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group B"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = 2;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(1100, result);
        }

        [Theory]
        [InlineAutoMoqData(new[] { 3.5, 10, 28.1, 50 }, 1, 3.5)]
        [InlineAutoMoqData(new[] { 3.5, 10, 28.1, 50 }, 2, 10)]
        [InlineAutoMoqData(new[] { 3.5, 10, 28.1, 50 }, 3, 28.1)]
        [InlineAutoMoqData(new[] { 3.5, 10, 28.1, 50 }, 10, 50)]
        public void GetScore_ShouldReturnScoreFromGroupArray_GivenWeightedCheckboxWithGroupArrayScoresInMultipleGroups(
            double[] groupScoreArray,
            int numberOfAnswers,
            decimal groupExpectedResult,
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = groupScoreArray.Select(Convert.ToDecimal).ToList(), MaxGroupScore = null} },
                { "Group B", new WeightedCheckboxGroup() { GroupIdentifier = "Group B", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>()
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 500,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group B"
                        }
                    }
                }
            };
            for (int i = 0; i < numberOfAnswers; i++)
            {
                answeredQuestion.Answers.Add(new Answer()
                {
                    Choice = new QuestionChoice()
                        { QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = "Group A" } }
                });
            }
            
            answeredQuestion.Weighting = null;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(groupExpectedResult + 500, result);
        }

        [Theory]
        [InlineAutoMoqData(new[] { 3.5, 10, 28.1, 50 }, 1, 3, 3)]
        [InlineAutoMoqData(new[] { 3.5, 10, 28.1, 50 }, 2, 9, 9)]
        [InlineAutoMoqData(new[] { 3.5, 10, 28.1, 50 }, 3, 25, 25)]
        [InlineAutoMoqData(new[] { 3.5, 10, 28.1, 50 }, 10, 40, 40)]
        public void GetScore_ShouldReturnWeightedScoreFromGroupArray_GivenWeightedCheckboxWithArrayScoresInMultipleGroupsAndQuestionWeighting(
            double[] groupScoreArray,
            int numberOfAnswers,
            decimal maxGroupScore,
            decimal groupExpectedResult,
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.Score = null;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = groupScoreArray.Select(Convert.ToDecimal).ToList(), MaxGroupScore = maxGroupScore} },
                { "Group B", new WeightedCheckboxGroup() { GroupIdentifier = "Group B", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>()
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 500,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group B"
                        }
                    }
                }
            };
            for (int i = 0; i < numberOfAnswers; i++)
            {
                answeredQuestion.Answers.Add(new Answer()
                {
                    Choice = new QuestionChoice()
                        { QuestionChoiceGroup = new QuestionChoiceGroup() { GroupIdentifier = "Group A" } }
                });
            }

            answeredQuestion.Weighting = 10;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal((groupExpectedResult + 500)*10, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnScoreOverride_GivenWeightedCheckboxScoreOverrideIsPresent(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            questionDefinition.Score = 5000;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = null;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(5000, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnWeightedScoreOverride_GivenWeightedCheckboxScoreOverrideIsPresentAndQuestionWeighting(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            questionDefinition.Score = 5000;
            questionDefinition.WeightedCheckbox.Groups = new Dictionary<string, WeightedCheckboxGroup>()
            {
                { "Group A", new WeightedCheckboxGroup() { GroupIdentifier = "Group A", GroupArrayScore = null, MaxGroupScore = null} }
            };
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100,
                        QuestionChoiceGroup = new QuestionChoiceGroup
                        {
                            GroupIdentifier = "Group A"
                        }
                    }
                }
            };
            answeredQuestion.Weighting = 0.05;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(250, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnSelectedAnswersScore_GivenWeightedRadioStandardAnswersWithScores(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedRadioQuestion;
            questionDefinition.MaxScore = null;
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234
                    }
                }
            };
            answeredQuestion.Weighting = null;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(234, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnWeightedSelectedAnswersScore_GivenWeightedRadioStandardAnswersWithScoresAndQuestionWeighting(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedRadioQuestion;
            questionDefinition.MaxScore = null;
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234
                    }
                }
            };
            answeredQuestion.Weighting = 0.5;

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(117, result);
        }

        [Theory]
        [AutoMoqData]
        public void GetScore_ShouldReturnZero_GivenNoScoreQuestionType(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange

            //Act
            var result = sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(0, result);
        }
    }
}
