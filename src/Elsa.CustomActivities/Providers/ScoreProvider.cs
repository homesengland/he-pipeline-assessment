using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Question = Elsa.CustomActivities.Activities.QuestionScreen.Question;

namespace Elsa.CustomActivities.Providers
{
    public interface IScoreProvider
    {
        decimal CalculateScore(CustomModels.Question answeredQuestion, Question questionDefinition);
    }

    public class ScoreProvider : IScoreProvider
    {
        public decimal CalculateScore(CustomModels.Question answeredQuestion, Question questionDefinition)
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
                    if (questionDefinition.Score != null && questionDefinition.Score.HasValue)
                    {
                        score = questionDefinition.Score.Value;
                    }
                    else
                    {
                        if (questionDefinition.ScoreArray != null && questionDefinition.ScoreArray.Any())
                        {
                            score = GetScoreFromScoreArray(answeredQuestion.Answers, questionDefinition.ScoreArray);
                        }
                        else
                        {
                            foreach (var group in questionDefinition.WeightedCheckbox.Groups)
                            {
                                decimal groupScore = 0;
                                var groupDefinition = group.Value;
                                var groupAnswers = answeredQuestion.Answers.Where(x =>
                                    x.Choice?.QuestionChoiceGroup?.GroupIdentifier == groupDefinition.GroupIdentifier).ToList();

                                if (groupDefinition.GroupArrayScore != null && groupDefinition.GroupArrayScore.Any())
                                {
                                    groupScore += GetScoreFromScoreArray(groupAnswers, groupDefinition.GroupArrayScore);
                                }

                                foreach (var answer in groupAnswers)
                                {
                                    if (answer.Choice != null && answer.Choice.NumericScore.HasValue)
                                    {
                                        groupScore += answer.Choice.NumericScore.Value;
                                    }
                                }

                                groupScore = GetCappedScore(groupScore, groupDefinition.MaxGroupScore);
                                score += groupScore;
                            }
                        }
                    }
                    
                }
            }

            score = GetCappedScore(score, questionDefinition.MaxScore);
            score = GetWeightedScore(score, answeredQuestion.Weighting);
            return score;
        }

        private static decimal GetScoreFromScoreArray(List<Answer> answers, List<decimal> scoreArray)
        {
            var answerCount = answers.Count;
            var scoreArrayIndex = answerCount - 1;
            if (answerCount > scoreArray.Count)
            {
                scoreArrayIndex = scoreArray.Count - 1;
            }

            var score = scoreArray[scoreArrayIndex];
            return score;
        }

        private decimal GetCappedScore(decimal score, decimal? maxScore)
        {
            if (maxScore.HasValue && maxScore.Value < score)
            {
                return maxScore.Value;
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
