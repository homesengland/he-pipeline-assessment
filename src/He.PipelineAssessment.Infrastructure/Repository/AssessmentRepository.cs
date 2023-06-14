using System.Collections;
using System.Collections.Generic;
using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IAssessmentRepository
    {
        Task<Assessment?> GetAssessment(int assessmentId);
        Task<List<Assessment>> GetAssessments();
        Task<AssessmentToolWorkflowInstance?> GetAssessmentToolWorkflowInstance(string workflowInstance);
        Task<List<AssessmentToolWorkflowInstance>?> GetPreviousAssessmentToolWorkflowInstances(
            AssessmentToolWorkflowInstance instance);
        Task<AssessmentToolInstanceNextWorkflow?> GetAssessmentToolInstanceNextWorkflow(int assessmentToolWorkflowInstanceId, string workflowDefinitionId);
        Task<AssessmentToolInstanceNextWorkflow?> GetAssessmentToolInstanceNextWorkflowByAssessmentId(int assessmentId, string workflowDefinitionId);
        Task<IEnumerable<AssessmentToolWorkflowInstance>> GetAssessmentToolWorkflowInstances(int assessmentId);

        Task<int> CreateAssessments(List<Assessment> assessments);
        Task<int> CreateAssessmentToolWorkflowInstance(AssessmentToolWorkflowInstance assessmentStage);
        Task CreateAssessmentToolInstanceNextWorkflows(List<AssessmentToolInstanceNextWorkflow> nextWorkflows);
        Task<int> CreateAssessmentIntervention(AssessmentIntervention assessmentIntervention);
        Task<AssessmentIntervention?> GetAssessmentIntervention(int interventionId);
        Task<int> UpdateAssessmentIntervention(AssessmentIntervention assessmentIntervention);

        Task<List<AssessmentToolWorkflowInstance>> GetSubsequentWorkflowInstancesForOverride(string workflowInstanceId);
        Task<List<AssessmentToolWorkflowInstance>> GetWorkflowInstancesToDeleteForRollback(
            int assessmentId,int assessmentToolOrder);

        Task DeleteSubsequentNextWorkflows(AssessmentToolInstanceNextWorkflow? nextWorkflow);
        Task DeleteAllNextWorkflows(int assessmentId);
        Task<int> DeleteNextWorkflow(AssessmentToolInstanceNextWorkflow nextWorkflow);

        Task<int> SaveChanges();

        Task<int> DeleteIntervention(AssessmentIntervention intervention);
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

        public async Task<List<AssessmentToolWorkflowInstance>?> GetPreviousAssessmentToolWorkflowInstances(
            AssessmentToolWorkflowInstance instance)
        {
            List<AssessmentToolWorkflowInstance> previousAssessmentToolInstances = new List<AssessmentToolWorkflowInstance>();
  
            previousAssessmentToolInstances = await context.Set<AssessmentToolWorkflowInstance>()
                .Include(x => x.Assessment)
                .Where(x => x.CreatedDateTime < instance.CreatedDateTime
                && x.Assessment.Id == instance.AssessmentId
                && x.Assessment.SpId == instance.Assessment.SpId
                && x.Status != AssessmentToolWorkflowInstanceConstants.Deleted
                ).ToListAsync();

            return previousAssessmentToolInstances;

        }

        public async Task<AssessmentToolInstanceNextWorkflow?> GetAssessmentToolInstanceNextWorkflow(int assessmentToolWorkflowInstanceId, string workflowDefinitionId)
        {
            return await context.Set<AssessmentToolInstanceNextWorkflow>().FirstOrDefaultAsync(x =>
                x.AssessmentToolWorkflowInstanceId == assessmentToolWorkflowInstanceId &&
                x.NextWorkflowDefinitionId == workflowDefinitionId);
        }

        public async Task<AssessmentToolInstanceNextWorkflow?> GetAssessmentToolInstanceNextWorkflowByAssessmentId(int assessmentId, string workflowDefinitionId)
        {
            return await context.Set<AssessmentToolInstanceNextWorkflow>().FirstOrDefaultAsync(x =>
                x.AssessmentId == assessmentId &&
                x.NextWorkflowDefinitionId == workflowDefinitionId);
        }

        public async Task<IEnumerable<AssessmentToolWorkflowInstance>> GetAssessmentToolWorkflowInstances(int assessmentId)
        {
            return await context.Set<AssessmentToolWorkflowInstance>().Where(x =>
                    x.AssessmentId == assessmentId && x.Status != AssessmentToolWorkflowInstanceConstants.Deleted)
                .ToListAsync();
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


        public async Task<int> DeleteNextWorkflow(AssessmentToolInstanceNextWorkflow nextWorkflow)
        {
            context.Remove(nextWorkflow);
            return await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteIntervention(AssessmentIntervention intervention)
        {
            context.Remove(intervention);
            return await SaveChanges();
        }

        public async Task<AssessmentIntervention?> GetAssessmentIntervention(int interventionId)
        {
            return await context.Set<AssessmentIntervention>()
                .Include(x => x.AssessmentToolWorkflowInstance.Assessment)
                .Include(x => x.TargetAssessmentToolWorkflow)
                .ThenInclude(x=>x!.AssessmentTool)
                .FirstOrDefaultAsync(x => x.Id == interventionId);
        }

        public async Task<int> UpdateAssessmentIntervention(AssessmentIntervention assessmentIntervention)
        {
            context.Set<AssessmentIntervention>().Update(assessmentIntervention);
            return await context.SaveChangesAsync();
            
        }

        public async Task<List<AssessmentToolWorkflowInstance>> GetSubsequentWorkflowInstancesForOverride(string workflowInstanceId)
        {
            List<AssessmentToolWorkflowInstance> workflowsToRemove = new List<AssessmentToolWorkflowInstance>();
            AssessmentToolWorkflowInstance? workflow = await context.Set<AssessmentToolWorkflowInstance>()
                .Include(x => x.Assessment)
                .FirstOrDefaultAsync(x => x.WorkflowInstanceId == workflowInstanceId);
            if(workflow != null)
            {
                workflowsToRemove = await context.Set<AssessmentToolWorkflowInstance>()
                    .Where(x => x.CreatedDateTime > workflow.CreatedDateTime 
                    && x.Assessment.SpId == workflow.Assessment.SpId
                    && x.Assessment.Id == workflow.Assessment.Id
                    && x.Status != AssessmentToolWorkflowInstanceConstants.Deleted).ToListAsync();
            }
            return workflowsToRemove;
        }

        public async Task<List<AssessmentToolWorkflowInstance>> GetWorkflowInstancesToDeleteForRollback(
            int assessmentId, int assessmentToolOrder)
        {
            List<AssessmentToolWorkflowInstance> workflowsToRemove = await context.Set<AssessmentToolWorkflowInstance>()
                .Where(x =>
                            x.Assessment.Id == assessmentId
                            && x.Status != AssessmentToolWorkflowInstanceConstants.Deleted
                            && x.AssessmentToolWorkflow.AssessmentTool.Order >= assessmentToolOrder).ToListAsync();

            return workflowsToRemove;
        }

        public async Task DeleteSubsequentNextWorkflows(AssessmentToolInstanceNextWorkflow? nextWorkflow)
        {
            List<AssessmentToolInstanceNextWorkflow> nextWorkflows = await context.Set<AssessmentToolInstanceNextWorkflow>()
                .Where(x => x.CreatedDateTime > nextWorkflow!.CreatedDateTime
                && x.AssessmentId == nextWorkflow.AssessmentId).ToListAsync();

            context.Set<AssessmentToolInstanceNextWorkflow>().RemoveRange(nextWorkflows);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAllNextWorkflows(int assessmentId)
        {
            List<AssessmentToolInstanceNextWorkflow> nextWorkflows = await context.Set<AssessmentToolInstanceNextWorkflow>()
                .Where(x => x.AssessmentId == assessmentId).ToListAsync();

            context.Set<AssessmentToolInstanceNextWorkflow>().RemoveRange(nextWorkflows);

            await context.SaveChangesAsync();
        }
    }
}
