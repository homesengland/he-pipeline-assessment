using Elsa.CustomActivities.Activities.QuestionScreen;

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
            throw new NotImplementedException();
        }
    }
}
