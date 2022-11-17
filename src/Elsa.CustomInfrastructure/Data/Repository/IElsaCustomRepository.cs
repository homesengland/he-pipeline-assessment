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

        Task<QuestionScreenQuestion?> UpdateQuestionScreenQuestion(QuestionScreenQuestion model, CancellationToken cancellationToken = default);
        Task CreateQuestionScreenQuestionsAsync(List<QuestionScreenQuestion> assessments, CancellationToken cancellationToken);
        Task<List<QuestionScreenQuestion>> GetQuestionScreenQuestions(string commandActivityId, string commandWorkflowInstanceId, CancellationToken cancellationToken);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
