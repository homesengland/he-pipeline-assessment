﻿using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public class ElsaCustomRepository : IElsaCustomRepository
    {
        private readonly DbContext _dbContext;
        public ElsaCustomRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AssessmentQuestion?> GetAssessmentQuestion(string activityId, string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<AssessmentQuestion>().FirstOrDefaultAsync(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId, cancellationToken);
        }

        public async ValueTask<int?> CreateAssessmentQuestionAsync(AssessmentQuestion model, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model.Id;
        }

        public async Task<AssessmentQuestion?> UpdateAssessmentQuestion(AssessmentQuestion model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task CreateAssessmentQuestionAsync(List<AssessmentQuestion> assessments, CancellationToken cancellationToken)
        {
            await _dbContext.AddRangeAsync(assessments, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<AssessmentQuestion>> GetAssessmentQuestions(string activityId, string workflowInstanceId,
            CancellationToken cancellationToken)
        {
            var list = await _dbContext.Set<AssessmentQuestion>().Where(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId && x.QuestionId != null).ToListAsync();
            return list;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}