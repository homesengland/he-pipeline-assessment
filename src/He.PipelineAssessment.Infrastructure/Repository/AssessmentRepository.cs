using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IAssessmentRepository
    {
        Task<List<Assessment>> GetAssessments();
        Task<List<AssessmentStage>> GetAssessmentStages(int assessmentId);
        Task<int> CreateAssessments(List<Assessment> assessments);
        Task<int> CreateAssessmentStage(AssessmentStage assessmentStage);
        Task<Assessment?> GetAssessment(int assessmentId);
        Task<AssessmentStage?> GetAssessmentStage(string workflowInstance);
        Task<int> SaveChanges();
    }

    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly PipelineAssessmentContext context;

        public AssessmentRepository(PipelineAssessmentContext context)
        {
            this.context = context;
        }
        public async Task<List<Assessment>> GetAssessments()
        {
            return await context.Set<Assessment>().ToListAsync();
        }

        public async Task<List<AssessmentStage>> GetAssessmentStages(int assessmentId)
        {
            return await context.Set<AssessmentStage>().Where(x => x.AssessmentId == assessmentId).ToListAsync();
        }

        public async Task<int> CreateAssessments(List<Assessment> assessments)
        {
            await context.Set<Assessment>().AddRangeAsync(assessments);
            return await context.SaveChangesAsync();
        }

        public async Task<int> CreateAssessmentStage(AssessmentStage assessmentStage)
        {
            await context.Set<AssessmentStage>().AddAsync(assessmentStage);
            await context.SaveChangesAsync();

            return await context.SaveChangesAsync();
        }

        public async Task<Assessment?> GetAssessment(int assessmentId)
        {
            return await context.Set<Assessment>().FirstOrDefaultAsync(x => x.Id == assessmentId);
        }

        public async Task<AssessmentStage?> GetAssessmentStage(string workflowInstance)
        {
            return await context.Set<AssessmentStage>().Include(x => x.Assessment).FirstOrDefaultAsync(x => x.WorkflowInstanceId == workflowInstance);
        }

        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }
    }
}
