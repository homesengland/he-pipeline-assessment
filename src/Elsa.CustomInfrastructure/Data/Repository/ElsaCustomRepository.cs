using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public class ElsaCustomRepository : IElsaCustomRepository
    {
        private readonly DbContext _dbContext;
        public ElsaCustomRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomActivityNavigation?> GetCustomActivityNavigation(string activityId, string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<CustomActivityNavigation>().FirstOrDefaultAsync(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId, cancellationToken);
        }

        public async ValueTask<int?> CreateCustomActivityNavigationAsync(CustomActivityNavigation model, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model.Id;
        }

        public async Task<Question?> UpdateQuestion(Question model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task<CustomActivityNavigation?> UpdateCustomActivityNavigation(CustomActivityNavigation model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task CreateQuestionsAsync(List<Question> assessments, CancellationToken cancellationToken)
        {
            await _dbContext.AddRangeAsync(assessments, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Question>> GetActivityQuestions(string activityId, string workflowInstanceId,
            CancellationToken cancellationToken)
        {
            var list = await _dbContext.Set<Question>()
                .Where(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId && x.QuestionId != null)
                .Include(x => x.Choices)!.ThenInclude(y => y.QuestionChoiceGroup)
                .Include(x => x.Answers)
                .ToListAsync(cancellationToken);
            return list;
        }

        public async Task<Question?> GetQuestionByWorkflowAndActivityName(string activityName, string workflowName, string correlationId, string questionID,
    CancellationToken cancellationToken)
        {
            var result = await _dbContext.Set<Question>()
                .Where(x =>
                    x.CorrelationId == correlationId &&
                    x.ActivityName == activityName &&
                    x.WorkflowName == workflowName &&
                    x.QuestionId == questionID &&
                    (!x.IsArchived.HasValue || !x.IsArchived.Value))
                .Include(x => x.Choices)!.ThenInclude(y => y.QuestionChoiceGroup)
                .Include(x => x.Answers)
                .OrderByDescending(x => x.CreatedDateTime)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<Question?> GetQuestionByDataDictionary(string correlationId, int dataDictionaryId,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Set<Question>()
                .Where(x =>
                    x.CorrelationId == correlationId &&
                    x.QuestionDataDictionaryId == dataDictionaryId &&
                    (!x.IsArchived.HasValue || !x.IsArchived.Value))
                .Include(x => x.Choices)!.ThenInclude(y => y.QuestionChoiceGroup)
                .Include(x => x.Answers)
                .OrderByDescending(x => x.CreatedDateTime)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return result;
        }

        public async Task<Question?> GetQuestionById(int id)
        {
            var result = await _dbContext.Set<Question>().FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Question>> GetWorkflowInstanceQuestions(string workflowInstanceId, CancellationToken cancellationToken)
        {
            var list = await _dbContext.Set<Question>()
                .Where(x => x.WorkflowInstanceId == workflowInstanceId)
                .Include(x => x.Choices)
                .Include(x => x.Answers)
                .ToListAsync(cancellationToken);
            return list;
        }

        public async Task<CustomActivityNavigation?> GetChangedPathNavigation(string workflowInstanceId,
            string currentActivityId, string nextActivityId, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<CustomActivityNavigation>()
                .Where(x => x.WorkflowInstanceId == workflowInstanceId && x.PreviousActivityId == currentActivityId && x.ActivityId != nextActivityId &&
                            x.ActivityId != currentActivityId).OrderByDescending(x => x.LastModifiedDateTime)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task DeleteCustomNavigations(List<string> previousPathActivities, string workflowInstanceId, CancellationToken cancellationToken)
        {
            var list = _dbContext.Set<CustomActivityNavigation>().Where(x => x.WorkflowInstanceId == workflowInstanceId && previousPathActivities.Contains(x.ActivityId));
            _dbContext.RemoveRange(list);
            await SaveChanges(cancellationToken);
        }

        public async Task DeleteQuestions(string workflowInstanceId, List<string> previousPathActivities, CancellationToken cancellationToken)
        {
            var list = _dbContext.Set<Question>().Where(x => x.WorkflowInstanceId == workflowInstanceId && previousPathActivities.Contains(x.ActivityId));
            _dbContext.RemoveRange(list);
            await SaveChanges(cancellationToken);
        }

        public async Task<List<PotScoreOption>> GetPotScoreOptionsAsync(CancellationToken cancellationToken) =>
            await _dbContext.Set<PotScoreOption>().Where(x => x.IsActive).ToListAsync(cancellationToken);

        public async Task<List<DataDictionary>> GetDataDictionaryListAsync(bool includeArchived = false, CancellationToken cancellationToken = default)
        {
            if (includeArchived)
            {
                var result = await _dbContext.Set<DataDictionary>().Include(x => x.Group).Where(x => !x.IsArchived).ToListAsync(cancellationToken);
                return result;
            }
            else
            {
                var result = await _dbContext.Set<DataDictionary>().Include(x => x.Group).ToListAsync(cancellationToken);
                return result;
            }

        }

        public async Task<List<DataDictionaryGroup>> GetDataDictionaryGroupsAsync(bool includeArchived = false, CancellationToken cancellationToken = default)
        {
            var result = new List<DataDictionaryGroup>();
            if (!includeArchived)
            {
                result = await _dbContext.Set<DataDictionaryGroup>().Include(x => x.DataDictionaryList.Where(x => !x.IsArchived)).ToListAsync(cancellationToken);
            }
            else
            {
                result = await _dbContext.Set<DataDictionaryGroup>().Include(x => x.DataDictionaryList).ToListAsync(cancellationToken);
            }
            return result;

        }

        public async Task CreateQuestionWorkflowInstance(QuestionWorkflowInstance questionWorkflowInstance, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(questionWorkflowInstance, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<QuestionWorkflowInstance?> GetQuestionWorkflowInstance(string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            QuestionWorkflowInstance? workflow = await _dbContext.Set<QuestionWorkflowInstance>().FirstOrDefaultAsync(x => x.WorkflowInstanceId == workflowInstanceId);
            return workflow;
        }

        public async Task<List<QuestionWorkflowInstance>> GetQuestionWorkflowInstancesByName(string correlationId, string name,
            CancellationToken cancellationToken = default)
        {
            var workflowInstances = await _dbContext.Set<QuestionWorkflowInstance>()
                .Where(x => x.CorrelationId == correlationId && x.WorkflowName == name).ToListAsync(cancellationToken);
            return workflowInstances;
        }

        public async Task<QuestionWorkflowInstance?> GetQuestionWorkflowInstanceByDefinitionId(string workflowDefinitionId, string correlationId,
            CancellationToken cancellationToken = default)
        {
            var workflowInstance = await _dbContext.Set<QuestionWorkflowInstance>()
                .FirstOrDefaultAsync(
                    x => x.WorkflowDefinitionId == workflowDefinitionId && x.CorrelationId == correlationId,
                    cancellationToken: cancellationToken);

            return workflowInstance;
        }

        public async Task SetWorkflowInstanceResult(string workflowInstanceId, string result, CancellationToken cancellationToken = default)
        {
            var workflowInstance = _dbContext.Set<QuestionWorkflowInstance>().Where(x => x.WorkflowInstanceId == workflowInstanceId).ToList();
            foreach(var workflow in workflowInstance)
            {
                workflow.Result = result;
            }
            _dbContext.UpdateRange(workflowInstance);
            await SaveChanges(cancellationToken);
        }

        public async Task SetWorkflowInstanceScore(string workflowInstanceId, string score, CancellationToken cancellationToken = default)
        {
            var workflowInstance = _dbContext.Set<QuestionWorkflowInstance>().Where(x => x.WorkflowInstanceId == workflowInstanceId).ToList();
            foreach (var workflow in workflowInstance)
            {
                workflow.Score = score;
            }
            _dbContext.UpdateRange(workflowInstance);
            await SaveChanges(cancellationToken);
        }

        public async Task ArchiveQuestions(string[] requestWorkflowInstanceIds, CancellationToken cancellationToken = default)
        {
            var questions = _dbContext.Set<Question>()
                .Where(x => requestWorkflowInstanceIds.Contains(x.WorkflowInstanceId));

            foreach (var question in questions)
            {
                question.IsArchived = true;
            }
            _dbContext.UpdateRange(questions);
            await SaveChanges(cancellationToken);
        }

        public async Task<CustomActivityNavigation?> GetLatestCustomActivityNavigation(string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<CustomActivityNavigation>()
                .OrderByDescending(x=>x.Id)
                .FirstOrDefaultAsync(x=>x.WorkflowInstanceId == workflowInstanceId, cancellationToken);
        }

        public async Task ArchiveDataDictionaryItem(int id, bool isArchived, CancellationToken cancellationToken)
        {
            var items = _dbContext.Set<DataDictionary>().Where(x => x.Id== id);
            foreach (var item in items)
            {
                item.IsArchived = isArchived;
            }
            _dbContext.UpdateRange(items);
            await SaveChanges(cancellationToken);
        }
        public async Task ArchiveDataDictionaryGroup(int id, bool isArchived,CancellationToken cancellationToken)
        {
            var items = _dbContext.Set<DataDictionaryGroup>().Where(x => x.Id == id);
            foreach (var item in items)
            {
                item.IsArchived = isArchived;
            }
            _dbContext.UpdateRange(items);

            var dictionaryItems = _dbContext.Set<DataDictionary>().Where(x => x.DataDictionaryGroupId == id);
            foreach (var item in dictionaryItems)
            {
                item.IsArchived = isArchived;
            }
            _dbContext.UpdateRange(dictionaryItems);

            await SaveChanges(cancellationToken);
        }

        public async Task<int> CreateDataDictionaryGroup(DataDictionaryGroup group, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(group, cancellationToken);
            await SaveChanges(cancellationToken);
            return group.Id;
        }

        public async Task<int> CreateDataDictionaryItem(DataDictionary item, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(item, cancellationToken);
            await SaveChanges(cancellationToken);
            return item.Id;
        }

        public async Task UpdateDataDictionaryGroup(DataDictionaryGroup group, CancellationToken cancellationToken)
        {
            _dbContext.Set<DataDictionaryGroup>().Update(group);
            await SaveChanges(cancellationToken);
        }


        public async Task UpdateDataDictionaryItem(DataDictionary item, CancellationToken cancellationToken)
        {
            var record = _dbContext.Set<DataDictionary>().Where(x => x.Id == item.Id).FirstOrDefault();
            if(record != null)
            {
                record.Name = item.Name ?? record.Name;
                record.LegacyName = item.LegacyName ?? record.LegacyName;
                record.CreatedDateTime = record!.CreatedDateTime;
                record.Type = item.Type ?? record!.Type;
                record.LastModifiedDateTime = DateTime.UtcNow;
                record.Description = item.Description ?? record!.Description;
                record.DataDictionaryGroupId = record.DataDictionaryGroupId;
                    record.IsArchived = item.IsArchived;
                _dbContext.Set<DataDictionary>().Update(record);
            }
            await SaveChanges(cancellationToken);
        }
    }
}
