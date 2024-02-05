using Elsa.CustomModels;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public interface IElsaCustomRepository
    {
        Task<CustomActivityNavigation?> GetCustomActivityNavigation(string activityId,
            string workflowInstanceId,
            CancellationToken cancellationToken = default);

        Task<CustomActivityNavigation?> GetLatestCustomActivityNavigation(
            string workflowInstanceId,
            CancellationToken cancellationToken = default);

        ValueTask<int?> CreateCustomActivityNavigationAsync(CustomActivityNavigation model,
            CancellationToken cancellationToken = default);

        Task<Question?> UpdateQuestion(Question model, CancellationToken cancellationToken = default);

        Task<CustomActivityNavigation?> UpdateCustomActivityNavigation(CustomActivityNavigation model,
            CancellationToken cancellationToken = default);
        Task CreateQuestionsAsync(List<Question> assessments, CancellationToken cancellationToken);
        Task<List<Question>> GetActivityQuestions(string activityId, string workflowInstanceId, CancellationToken cancellationToken);
        Task<Question?> GetQuestionByWorkflowAndActivityName (string activityName, string workflowName, string correlationId, string questionID, CancellationToken cancellationToken);
        Task<Question?> GetQuestionByDataDictionary(string correlationId, int dataDictionaryId,
            CancellationToken cancellationToken);

        Task<Question?> GetQuestionById(int id);
        Task SaveChanges(CancellationToken cancellationToken);
        Task<List<Question>> GetWorkflowInstanceQuestions(string workflowInstanceId, CancellationToken cancellationToken);
        Task<CustomActivityNavigation?> GetChangedPathNavigation(string workflowInstanceId, string currentActivityId,
            string nextActivityId, CancellationToken cancellationToken);
        Task DeleteCustomNavigations(List<string> previousPathActivities, string workflowInstanceId, CancellationToken cancellationToken);
        Task DeleteQuestions(string workflowInstanceId, List<string> previousPathActivities, CancellationToken cancellationToken);

        Task<List<PotScoreOption>> GetPotScoreOptionsAsync(CancellationToken cancellationToken = default);
        Task<List<DataDictionary>> GetDataDictionaryListAsync(CancellationToken cancellationToken = default);
        Task<List<DataDictionaryGroup>> GetDataDictionaryGroupsAsync(bool includeArchived = false, CancellationToken cancellationToken = default);
        Task CreateQuestionWorkflowInstance(QuestionWorkflowInstance questionWorkflowInstance, CancellationToken cancellationToken = default);
        Task<QuestionWorkflowInstance?> GetQuestionWorkflowInstance(string workflowInstanceId, CancellationToken cancellationToken = default);
        Task<List<QuestionWorkflowInstance>> GetQuestionWorkflowInstancesByName(string correlationId, string name, CancellationToken cancellationToken = default);
        Task<QuestionWorkflowInstance?> GetQuestionWorkflowInstanceByDefinitionId(string workflowInstanceDefinitionId, string correlationId, CancellationToken cancellationToken = default);
        Task SetWorkflowInstanceResult(string workflowInstanceId, string result, CancellationToken cancellationToken = default);
        Task SetWorkflowInstanceScore(string workflowInstanceId, string score, CancellationToken cancellationToken = default);
        Task ArchiveQuestions(string[] requestWorkflowInstanceIds, CancellationToken cancellationToken = default);
        Task<int> CreateDataDictionaryGroup(DataDictionaryGroup group, CancellationToken cancellationToken);
        Task<int> CreateDataDictionaryItem(DataDictionary item, CancellationToken cancellationToken);
        Task UpdateDataDictionaryGroup(DataDictionaryGroup group, CancellationToken cancellationToken);
        Task UpdateDataDictionaryItem(DataDictionary item, CancellationToken cancellationToken);
        Task ArchiveDataDictionaryItem(int id, bool isArchived, CancellationToken cancellationToken);
        Task ArchiveDataDictionaryGroup(int id, bool isArchived, CancellationToken cancellationToken);
    }
}
