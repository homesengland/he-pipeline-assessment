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
        public async Task GetScore_ShouldReturnSumOfSelectedAnswers_GivenWeightedCheckboxStandardAnswersWithScores(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.MaxScore = null;
            questionDefinition.ScoreArray = null;
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100
                    }
                }
            };
            answeredQuestion.Weighting = null;

            //Act
            var result = await sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(334, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnMaxScore_GivenWeightedCheckboxStandardScoreIsHigherThanMaxScore(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.MaxScore = 300;
            questionDefinition.ScoreArray = null;
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100
                    }
                }
            };
            answeredQuestion.Weighting = null;

            //Act
            var result = await sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(300, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedSumOfSelectedAnswers_GivenWeightedCheckboxStandardScoreIsLowerThanMaxScore(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.MaxScore = 400;
            questionDefinition.ScoreArray = null;
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100
                    }
                }
            };
            answeredQuestion.Weighting = 0.1;

            //Act
            var result = await sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(33.4m, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedMaxScore_GivenWeightedCheckboxStandardScoreIsHigherThanMaxScore(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.MaxScore = 300;
            questionDefinition.ScoreArray = null;
            answeredQuestion.Answers = new List<Answer>
            {
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 234
                    }
                },
                new Answer()
                {
                    Choice = new QuestionChoice()
                    {
                        NumericScore = 100
                    }
                }
            };
            answeredQuestion.Weighting = 0.1;

            //Act
            var result = await sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(30, result);
        }

        [Theory]
        [InlineAutoMoqData(new [] { 2.5, 4, 8.1 }, 1, 2.5)]
        [InlineAutoMoqData(new [] { 2.5, 4, 8.1 }, 2, 4)]
        [InlineAutoMoqData(new [] { 2.5, 4, 8.1 }, 3, 8.1)]
        [InlineAutoMoqData(new [] { 2.5, 4, 8.1 }, 10, 8.1)]
        public async Task GetScore_ShouldReturnScoreFromArray_GivenWeightedCheckboxWithArrayScores(
            double[] scoreArray,
            int numberOfAnswers,
            decimal expectedResult,
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange
            questionDefinition.QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;
            questionDefinition.ScoreArray = scoreArray.Select(Convert.ToDecimal).ToList();
            answeredQuestion.Answers = new List<Answer>();
            for (int i = 0; i < numberOfAnswers; i++)
            {
                answeredQuestion.Answers.Add(new Answer());
            }

            answeredQuestion.Weighting = null;

            //Act
            var result = await sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        //[Theory]
        //[AutoMoqData]
        //public async Task GetScore_ShouldReturnWeightedScoreFromArray_GivenWeightedCheckboxWithArrayScoresAndQuestionWeighting()
        //{
        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[AutoMoqData]
        //public async Task GetScore_ShouldReturnSumOfSelectedAnswersFromMultipleGroups_GivenWeightedCheckboxStandardAnswersWithScoresInMultipleGroups()
        //{
        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[AutoMoqData]
        //public async Task GetScore_ShouldReturnMaxGroupScoreFromMultipleGroups_GivenWeightedCheckboxStandardScoreWithinGroupsIsHigherThanMaxGroupScore()
        //{
        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[AutoMoqData]
        //public async Task GetScore_ShouldReturnWeightedSumOfSelectedAnswersFromMultipleGroups_GivenWeightedCheckboxStandardAnswersWithScoresAndQuestionWeightingFromMultipleGroups()
        //{
        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[AutoMoqData]
        //public async Task GetScore_ShouldReturnWeightedMaxScoreMaxGroupScoreFromMultipleGroups_GivenWeightedCheckboxStandardScoreIsHigherThanMaxGroupScore()
        //{
        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[AutoMoqData]
        //public async Task GetScore_ShouldReturnScoreFromGroupArray_GivenWeightedCheckboxWithGroupArrayScoresInMultipleGroups()
        //{
        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[AutoMoqData]
        //public async Task GetScore_ShouldReturnWeightedScoreFromGroupArray_GivenWeightedCheckboxWithArrayScoresInMultipleGroupsAndQuestionWeighting()
        //{
        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[AutoMoqData]
        //public async Task GetScore_ShouldReturnScoreOverride_GivenWeightedCheckboxScoreOverrideIsPresent()
        //{
        //    throw new NotImplementedException();
        //}

        //[Theory]
        //[AutoMoqData]
        //public async Task GetScore_ShouldReturnWeightedScoreOverride_GivenWeightedCheckboxScoreOverrideIsPresentAndQuestionWeighting()
        //{
        //    throw new NotImplementedException();
        //}

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnSelectedAnswersScore_GivenWeightedRadioStandardAnswersWithScores(
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
            var result = await sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(234, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedSelectedAnswersScore_GivenWeightedRadioStandardAnswersWithScoresAndQuestionWeighting(
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
            var result = await sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(117, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnZero_GivenNoScoreQuestionType(
            CustomModels.Question answeredQuestion,
            Question questionDefinition,
            ScoreProvider sut)
        {
            //Arrange

            //Act
            var result = await sut.CalculateScore(answeredQuestion, questionDefinition);

            //Assert
            Assert.Equal(0, result);
        }
    }
}
