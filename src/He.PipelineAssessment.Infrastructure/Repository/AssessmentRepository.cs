﻿using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IAssessmentRepository
    {
        Task<List<Assessment>> GetAssessments();
        Task<List<AssessmentStageViewModel>> GetAssessmentStages(int assessmentId);
        Task<int> CreateAssessments(List<Assessment> assessments);
        Task<int> CreateAssessmentStage(AssessmentToolWorkflowInstance assessmentStage);
        Task<Assessment?> GetAssessment(int assessmentId);
        Task<AssessmentToolWorkflowInstance?> GetAssessmentStage(string workflowInstance);
        Task<int> SaveChanges();
    }

    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly PipelineAssessmentContext context;
        private readonly string sp_GetAssessmentStagesByAssessmentId = @"exec GetAssessmentStagesByAssessmentId @assessmentId";

        public AssessmentRepository(PipelineAssessmentContext context)
        {
            this.context = context;
        }
        public async Task<List<Assessment>> GetAssessments()
        {
            return await context.Set<Assessment>().ToListAsync();
        }

        public async Task<List<AssessmentStageViewModel>> GetAssessmentStages(int assessmentId)
        {
            var stages = await context.AssessmentStageViewModel.FromSqlRaw(sp_GetAssessmentStagesByAssessmentId, assessmentId).ToListAsync();
            return new List<AssessmentStageViewModel>(stages);
        }

        public async Task<int> CreateAssessments(List<Assessment> assessments)
        {
            await context.Set<Assessment>().AddRangeAsync(assessments);
            return await context.SaveChangesAsync();
        }

        public async Task<int> CreateAssessmentStage(AssessmentToolWorkflowInstance assessmentStage)
        {
            await context.Set<AssessmentToolWorkflowInstance>().AddAsync(assessmentStage);
            await context.SaveChangesAsync();

            return await context.SaveChangesAsync();
        }

        public async Task<Assessment?> GetAssessment(int assessmentId)
        {
            return await context.Set<Assessment>().FirstOrDefaultAsync(x => x.Id == assessmentId);
        }

        public async Task<AssessmentToolWorkflowInstance?> GetAssessmentStage(string workflowInstance)
        {
            return await context.Set<AssessmentToolWorkflowInstance>().Include(x => x.Assessment).FirstOrDefaultAsync(x => x.WorkflowInstanceId == workflowInstance);
        }

        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }
    }
}
