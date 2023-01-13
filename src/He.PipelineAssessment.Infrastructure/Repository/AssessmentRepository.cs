using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Models.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IAssessmentRepository
    {
        Task<Assessment?> GetAssessment(int assessmentId);
        Task<List<Assessment>> GetAssessments();
        Task<AssessmentToolWorkflowInstance?> GetAssessmentStage(string workflowInstance);
        Task<List<AssessmentStageViewModel>> GetAssessmentStages(int assessmentId);
        Task<List<StartableToolViewModel>> GetStartableTools(int assessmentId);
        Task<AssessmentToolInstanceNextWorkflow?> GetAssessmentToolInstanceNextWorkflow(int assessmentToolWorkflowInstanceId, string workflowDefinitionId);

        Task<int> CreateAssessments(List<Assessment> assessments);
        Task<int> CreateAssessmentStage(AssessmentToolWorkflowInstance assessmentStage);
        Task CreateAssessmentToolInstanceNextWorkflows(List<AssessmentToolInstanceNextWorkflow> nextWorkflows);

        Task<int> SaveChanges();

    }

    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly PipelineAssessmentContext context;
        private readonly string sp_GetAssessmentStagesByAssessmentId = @"exec GetAssessmentStagesByAssessmentId @assessmentId";
        private readonly string sp_GetStartableToolsByAssessmentId = @"exec GetStartableToolsByAssessmentId @assessmentId";

        public AssessmentRepository(PipelineAssessmentContext context)
        {
            this.context = context;
        }

        public async Task<Assessment?> GetAssessment(int assessmentId)
        {
            return await context.Set<Assessment>().FirstOrDefaultAsync(x => x.Id == assessmentId);
        }

        public async Task<List<Assessment>> GetAssessments()
        {
            return await context.Set<Assessment>().ToListAsync();
        }

        public async Task<AssessmentToolWorkflowInstance?> GetAssessmentStage(string workflowInstance)
        {
            return await context.Set<AssessmentToolWorkflowInstance>().Include(x => x.Assessment).FirstOrDefaultAsync(x => x.WorkflowInstanceId == workflowInstance);
        }

        public async Task<List<AssessmentStageViewModel>> GetAssessmentStages(int assessmentId)
        {
            var assessmentIdParameter = new SqlParameter("@assessmentId", assessmentId);
            var stages = await context.AssessmentStageViewModel
                .FromSqlRaw(sp_GetAssessmentStagesByAssessmentId, assessmentIdParameter).ToListAsync();
            return new List<AssessmentStageViewModel>(stages);
        }

        public async Task<List<StartableToolViewModel>> GetStartableTools(int assessmentId)
        {
            var assessmentIdParameter = new SqlParameter("@assessmentId", assessmentId);
            var startableTools = await context.StartableToolViewModel
                .FromSqlRaw(sp_GetStartableToolsByAssessmentId, assessmentIdParameter).ToListAsync();
            return new List<StartableToolViewModel>(startableTools);
        }

        public async Task<AssessmentToolInstanceNextWorkflow?> GetAssessmentToolInstanceNextWorkflow(int assessmentToolWorkflowInstanceId, string workflowDefinitionId)
        {
            return await context.Set<AssessmentToolInstanceNextWorkflow>().FirstOrDefaultAsync(x =>
                x.AssessmentToolWorkflowInstanceId == assessmentToolWorkflowInstanceId &&
                x.NextWorkflowDefinitionId == workflowDefinitionId);
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

        public async Task CreateAssessmentToolInstanceNextWorkflows(List<AssessmentToolInstanceNextWorkflow> nextWorkflows)
        {
            await context.Set<AssessmentToolInstanceNextWorkflow>().AddRangeAsync(nextWorkflows);
            await context.SaveChangesAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }
    }
}
