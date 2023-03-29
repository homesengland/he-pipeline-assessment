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

        Task<Question?> UpdateQuestionScreenQuestion(Question model, CancellationToken cancellationToken = default);

        Task<CustomActivityNavigation?> UpdateCustomActivityNavigation(CustomActivityNavigation model,
            CancellationToken cancellationToken = default);
        Task CreateQuestionScreenQuestionsAsync(List<Question> assessments, CancellationToken cancellationToken);
        Task<List<Question>> GetQuestions(string activityId, string workflowInstanceId, CancellationToken cancellationToken);
        Task<Question?> GetQuestionScreenQuestion(string activityId, string workflowInstanceId, string questionID, CancellationToken cancellationToken);
        Task SaveChanges(CancellationToken cancellationToken);
        Task<List<Question>> GetQuestions(string workflowInstanceId, CancellationToken cancellationToken);
        Task<CustomActivityNavigation?> GetChangedPathNavigation(string workflowInstanceId, string currentActivityId,
            string nextActivityId, CancellationToken cancellationToken);
        Task DeleteCustomNavigations(List<string> previousPathActivities, string workflowInstanceId, CancellationToken cancellationToken);
        Task DeleteQuestionScreenQuestions(string workflowInstanceId, List<string> previousPathActivities, CancellationToken cancellationToken);

        Task<List<PotScoreOption>> GetPotScoreOptionsAsync(CancellationToken cancellationToken = default);
        Task CreateQuestionWorkflowInstance(QuestionWorkflowInstance questionWorkflowInstance, CancellationToken cancellationToken = default);
    }
}
