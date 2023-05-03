using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomActivities.Tests.Providers
{
    public class ScoreProviderTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnSumOfSelectedAnswers_GivenWeightedCheckboxStandardAnswersWithScores()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnMaxScore_GivenWeightedCheckboxStandardScoreIsHigherThanMaxScore()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedSumOfSelectedAnswers_GivenWeightedCheckboxStandardAnswersWithScoresAndQuestionWeighting()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedMaxScore_GivenWeightedCheckboxStandardScoreIsHigherThanMaxScore()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnScoreFromArray_GivenWeightedCheckboxWithArrayScores()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedScoreFromArray_GivenWeightedCheckboxWithArrayScoresAndQuestionWeighting()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnSumOfSelectedAnswersFromMultipleGroups_GivenWeightedCheckboxStandardAnswersWithScoresInMultipleGroups()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnMaxGroupScoreFromMultipleGroups_GivenWeightedCheckboxStandardScoreWithinGroupsIsHigherThanMaxGroupScore()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedSumOfSelectedAnswersFromMultipleGroups_GivenWeightedCheckboxStandardAnswersWithScoresAndQuestionWeightingFromMultipleGroups()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedMaxScoreMaxGroupScoreFromMultipleGroups_GivenWeightedCheckboxStandardScoreIsHigherThanMaxGroupScore()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnScoreFromGroupArray_GivenWeightedCheckboxWithGroupArrayScoresInMultipleGroups()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedScoreFromGroupArray_GivenWeightedCheckboxWithArrayScoresInMultipleGroupsAndQuestionWeighting()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnScoreOverride_GivenWeightedCheckboxScoreOverrideIsPresent()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedScoreOverride_GivenWeightedCheckboxScoreOverrideIsPresentAndQuestionWeighting()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnSelectedAnswersScore_GivenWeightedRadioStandardAnswersWithScores()
        {

        }

        [Theory]
        [AutoMoqData]
        public async Task GetScore_ShouldReturnWeightedSelectedAnswersScore_GivenWeightedRadioStandardAnswersWithScoresAndQuestionWeighting()
        {

        }
    }
}
