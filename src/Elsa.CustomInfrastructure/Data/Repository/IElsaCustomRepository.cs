using Elsa.CustomModels;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public interface IElsaCustomRepository
    {
        Task<CustomActivityNavigation?> GetCustomActivityNavigation(string activityId,
            string workflowInstanceId,
            CancellationToken cancellationToken = default);

        ValueTask<int?> CreateCustomActivityNavigationAsync(CustomActivityNavigation model,
            CancellationToken cancellationToken = default);

        Task<QuestionScreenAnswer?> UpdateQuestionScreenAnswer(QuestionScreenAnswer model, CancellationToken cancellationToken = default);
        Task CreateQuestionScreenAnswersAsync(List<QuestionScreenAnswer> assessments, CancellationToken cancellationToken);
        Task<List<QuestionScreenAnswer>> GetQuestionScreenAnswers(string activityId, string workflowInstanceId, CancellationToken cancellationToken);
        Task<QuestionScreenAnswer?> GetQuestionScreenAnswer(string activityId, string workflowInstanceId, string questionID, CancellationToken cancellationToken);
        Task SaveChanges(CancellationToken cancellationToken);
        Task<List<QuestionScreenAnswer>> GetQuestionScreenAnswers(string workflowInstanceId, CancellationToken cancellationToken);
    }
}
