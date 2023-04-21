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

        Task<Question?> UpdateQuestion(Question model, CancellationToken cancellationToken = default);

        Task<CustomActivityNavigation?> UpdateCustomActivityNavigation(CustomActivityNavigation model,
            CancellationToken cancellationToken = default);
        Task CreateQuestionsAsync(List<Question> assessments, CancellationToken cancellationToken);
        Task<List<Question>> GetActivityQuestions(string activityId, string workflowInstanceId, CancellationToken cancellationToken);
        Task<Question?> GetQuestionByCorrelationId(string activityId, string correlationId, string questionID, CancellationToken cancellationToken);
        Task SaveChanges(CancellationToken cancellationToken);
        Task<List<Question>> GetWorkflowInstanceQuestions(string workflowInstanceId, CancellationToken cancellationToken);
        Task<CustomActivityNavigation?> GetChangedPathNavigation(string workflowInstanceId, string currentActivityId,
            string nextActivityId, CancellationToken cancellationToken);
        Task DeleteCustomNavigations(List<string> previousPathActivities, string workflowInstanceId, CancellationToken cancellationToken);
        Task DeleteQuestions(string workflowInstanceId, List<string> previousPathActivities, CancellationToken cancellationToken);

        Task<List<PotScoreOption>> GetPotScoreOptionsAsync(CancellationToken cancellationToken = default);
        Task<List<QuestionDataDictionary>> GetQuestionDataDictionaryListAsync(CancellationToken cancellationToken = default);
        Task<List<QuestionDataDictionaryGroup>> GetQuestionDataDictionaryGroupsAsync(CancellationToken cancellationToken = default);
        Task CreateQuestionWorkflowInstance(QuestionWorkflowInstance questionWorkflowInstance, CancellationToken cancellationToken = default);
        Task<QuestionWorkflowInstance?> GetQuestionWorkflowInstance(string workflowInstanceId, CancellationToken cancellationToken = default);
        Task<List<QuestionWorkflowInstance>> GetQuestionWorkflowInstancesByName(string correlationId, string name, CancellationToken cancellationToken = default);
        Task<QuestionWorkflowInstance?> GetQuestionWorkflowInstanceByDefinitionId(string workflowInstanceDefinitionId, string correlationId, CancellationToken cancellationToken = default);
        Task SetWorkflowInstanceResult(string workflowInstanceId, string result, CancellationToken cancellationToken = default);
        Task SetWorkflowInstanceScore(string workflowInstanceId, string score, CancellationToken cancellationToken = default);

    }
}
