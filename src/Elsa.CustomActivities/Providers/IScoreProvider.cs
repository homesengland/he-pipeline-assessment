using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomWorkflow.Sdk;

namespace Elsa.CustomActivities.Providers
{
    public interface IScoreProvider
    {
        Task<decimal> CalculateScore(CustomModels.Question answeredQuestion, Question questionDefinition);
    }

    public class ScoreProvider : IScoreProvider
    {
        public async Task<decimal> CalculateScore(CustomModels.Question answeredQuestion, Question questionDefinition)
        {
            decimal score = 0;
            if (questionDefinition.QuestionType == QuestionTypeConstants.WeightedRadioQuestion)
            {
                var answer = answeredQuestion.Answers?.FirstOrDefault();
                if (answer != null && answer.Choice != null && answer.Choice.NumericScore.HasValue)
                {
                    score = answer.Choice.NumericScore.Value;
                }
            }

            if (questionDefinition.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion)
            {
                if (answeredQuestion.Answers != null)
                {
                    if (questionDefinition.ScoreArray != null && questionDefinition.ScoreArray.Any())
                    {
                        score = GetScoreFromScoreArray(answeredQuestion, questionDefinition);
                    }
                    else
                    {
                        foreach (var answer in answeredQuestion.Answers)
                        {
                            if (answer.Choice != null && answer.Choice.NumericScore.HasValue)
                            {
                                score += answer.Choice.NumericScore.Value;
                            }
                        }
                    }
                }
            }

            score = GetCappedScore(score, questionDefinition.MaxScore);
            score = GetWeightedScore(score, answeredQuestion.Weighting);
            return score;
        }

        private static decimal GetScoreFromScoreArray(CustomModels.Question answeredQuestion, Question questionDefinition)
        {
            var answerCount = answeredQuestion.Answers!.Count;
            var scoreArrayIndex = answerCount - 1;
            if (answerCount > questionDefinition.ScoreArray!.Count)
            {
                scoreArrayIndex = questionDefinition.ScoreArray.Count - 1;
            }

            var score = questionDefinition.ScoreArray[scoreArrayIndex];
            return score;
        }

        private decimal GetCappedScore(decimal score, int? maxScore)
        {
            if (maxScore.HasValue && maxScore.Value < score)
            {
                return Convert.ToDecimal(maxScore);
            }
            return score;
        }

        private decimal GetWeightedScore(decimal score, double? questionWeighting)
        {
            if (questionWeighting.HasValue && questionWeighting != 0)
            {
                return score * Convert.ToDecimal(questionWeighting);
            }

            return score;
        }
    }
}
