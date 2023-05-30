using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Infrastructure.Migrations;
using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IAssessmentRepository
    {
        Task<Assessment?> GetAssessment(int assessmentId);
        Task<List<Assessment>> GetAssessments();
        Task<AssessmentToolWorkflowInstance?> GetAssessmentToolWorkflowInstance(string workflowInstance);
       
        Task<AssessmentToolInstanceNextWorkflow?> GetAssessmentToolInstanceNextWorkflow(int assessmentToolWorkflowInstanceId, string workflowDefinitionId);
        Task<AssessmentToolInstanceNextWorkflow?> GetNonStartedAssessmentToolInstanceNextWorkflow(int assessmentToolWorkflowInstanceId, string workflowDefinitionId);
        Task<AssessmentToolInstanceNextWorkflow?> GetNonStartedAssessmentToolInstanceNextWorkflowByAssessmentId(int assessmentId, string workflowDefinitionId);
        Task<IEnumerable<AssessmentToolWorkflowInstance>> GetAssessmentToolWorkflowInstances(int assessmentId);

        Task<int> CreateAssessments(List<Assessment> assessments);
        Task<int> CreateAssessmentToolWorkflowInstance(AssessmentToolWorkflowInstance assessmentStage);
        Task CreateAssessmentToolInstanceNextWorkflows(List<AssessmentToolInstanceNextWorkflow> nextWorkflows);
        Task<int> CreateAssessmentIntervention(AssessmentIntervention assessmentIntervention);
        Task<AssessmentIntervention?> GetAssessmentIntervention(int interventionId);
        Task<int> UpdateAssessmentIntervention(AssessmentIntervention assessmentIntervention);

        Task<int> SaveChanges();

    }

    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly PipelineAssessmentContext context;
      
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

        public async Task<AssessmentToolWorkflowInstance?> GetAssessmentToolWorkflowInstance(string workflowInstance)
        {
            return await context.Set<AssessmentToolWorkflowInstance>().Include(x => x.Assessment).FirstOrDefaultAsync(x => x.WorkflowInstanceId == workflowInstance);
        }

       

        public async Task<AssessmentToolInstanceNextWorkflow?> GetAssessmentToolInstanceNextWorkflow(int assessmentToolWorkflowInstanceId, string workflowDefinitionId)
        {
            return await context.Set<AssessmentToolInstanceNextWorkflow>().FirstOrDefaultAsync(x =>
                x.AssessmentToolWorkflowInstanceId == assessmentToolWorkflowInstanceId &&
                x.NextWorkflowDefinitionId == workflowDefinitionId);
        }

        public async Task<AssessmentToolInstanceNextWorkflow?> GetNonStartedAssessmentToolInstanceNextWorkflow(int assessmentToolWorkflowInstanceId, string workflowDefinitionId)
        {
            return await context.Set<AssessmentToolInstanceNextWorkflow>().FirstOrDefaultAsync(x =>
                x.AssessmentToolWorkflowInstanceId == assessmentToolWorkflowInstanceId &&
                x.NextWorkflowDefinitionId == workflowDefinitionId &&
                x.IsStarted == false);
        }

        public async Task<AssessmentToolInstanceNextWorkflow?> GetNonStartedAssessmentToolInstanceNextWorkflowByAssessmentId(int assessmentId, string workflowDefinitionId)
        {
            return await context.Set<AssessmentToolInstanceNextWorkflow>().FirstOrDefaultAsync(x =>
                x.AssessmentId == assessmentId &&
                x.NextWorkflowDefinitionId == workflowDefinitionId &&
                x.IsStarted == false);
        }

        public async Task<IEnumerable<AssessmentToolWorkflowInstance>> GetAssessmentToolWorkflowInstances(int assessmentId)
        {
            return await context.Set<AssessmentToolWorkflowInstance>().Where(x => x.AssessmentId == assessmentId).ToListAsync();
        }

        public async Task<int> CreateAssessments(List<Assessment> assessments)
        {
            await context.Set<Assessment>().AddRangeAsync(assessments);
            return await context.SaveChangesAsync();
        }

        public async Task<int> CreateAssessmentToolWorkflowInstance(AssessmentToolWorkflowInstance assessmentStage)
        {
            await context.Set<AssessmentToolWorkflowInstance>().AddAsync(assessmentStage);

            return await context.SaveChangesAsync();
        }

        public async Task CreateAssessmentToolInstanceNextWorkflows(List<AssessmentToolInstanceNextWorkflow> nextWorkflows)
        {
            await context.Set<AssessmentToolInstanceNextWorkflow>().AddRangeAsync(nextWorkflows);
            await context.SaveChangesAsync();
        }

        public async Task<int> CreateAssessmentIntervention(AssessmentIntervention assessmentIntervention)
        {
            await context.Set<AssessmentIntervention>().AddAsync(assessmentIntervention);

            return await context.SaveChangesAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }

        public async Task<AssessmentIntervention?> GetAssessmentIntervention(int interventionId)
        {
            return await context.Set<AssessmentIntervention>().Include(x => x.AssessmentToolWorkflowInstance.Assessment)
                .Include(x => x.TargetAssessmentToolWorkflow).FirstOrDefaultAsync(x => x.Id == interventionId);
        }

        public async Task<int> UpdateAssessmentIntervention(AssessmentIntervention assessmentIntervention)
        {
            context.Set<AssessmentIntervention>().Update(assessmentIntervention);
            return await context.SaveChangesAsync();
            
        }
    }
}
