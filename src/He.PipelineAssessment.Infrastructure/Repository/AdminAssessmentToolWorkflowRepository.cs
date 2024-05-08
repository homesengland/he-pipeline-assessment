using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IAdminAssessmentToolWorkflowRepository
    {
        Task<AssessmentToolWorkflow?> GetAssessmentToolWorkflowById(int id);
        Task<int> CreateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow);

        Task<int> UpdateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow);

        Task<int> DeleteAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow);

        Task<AssessmentToolWorkflow?> GetExistingAssessmentWorkFlow(int assessmentToolId , string category);

        Task<int> UpdateIsLatest(int assessmentToolId, string category, string oldWorkFlowDefinitionId);

        Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForOverride(int order);
        Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForRollback(int order);
        Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForVariation();
        Task UpdateWorkflowNameAndCategory(string oldCategoryName , string newCategoryName);

        Task<List<string>> GetWorkflowsOfSameCategory(int assessmentToolId, string category);
        Task<List<string>> GetNonLatestWorkflowsByAssessmentTool(int assessmentToolId);
        Task<AssessmentToolWorkflow?> GetLatestWorkflowDefinition(string category);
        Task<AssessmentToolWorkflow?> GetLatestWorkflowDefinitionByAssessmentToolId(int assessmentToolId, string category);
        Task<int> SaveChanges();

    }
    public class AdminAssessmentToolWorkflowRepository : IAdminAssessmentToolWorkflowRepository
    {
        private readonly PipelineAssessmentContext _context;

        public AdminAssessmentToolWorkflowRepository(PipelineAssessmentContext pipelineAssessmentContext)
        {
            _context = pipelineAssessmentContext;
        }

        public async Task<AssessmentToolWorkflow?> GetAssessmentToolWorkflowById(int id)
        {
            return await _context.Set<AssessmentToolWorkflow>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> CreateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow)
        {
            await _context.Set<AssessmentToolWorkflow>().AddAsync(assessmentToolWorkflow);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow)
        {
            _context.Set<AssessmentToolWorkflow>().Remove(assessmentToolWorkflow);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForOverride(int order)
        {
            return await _context.Set<AssessmentToolWorkflow>().Where(x => x.Status != AssessmentToolStatus.Deleted && x.AssessmentTool.Order > order)
                .OrderBy(x => x.AssessmentTool.Order).Include(x => x.AssessmentTool).ToListAsync();
        }

        public async Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForRollback(int order)
        {
            return await _context.Set<AssessmentToolWorkflow>()
                .Where(x => x.Status != AssessmentToolStatus.Deleted && 
                    x.AssessmentTool.Order <= order)
                .OrderBy(x => x.AssessmentTool.Order)
                .Include(x => x.AssessmentTool)
                .ToListAsync();
        }

        public async Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForVariation()
        {
            return await _context.Set<AssessmentToolWorkflow>()
                .Where(x => x.Status != AssessmentToolStatus.Deleted && x.IsVariation)
                .OrderBy(x => x.AssessmentTool.Order)
                .Include(x => x.AssessmentTool)
                .ToListAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow)
        {
            _context.Set<AssessmentToolWorkflow>().Update(assessmentToolWorkflow);
            return await _context.SaveChangesAsync();
        }

        public async Task<AssessmentToolWorkflow?> GetExistingAssessmentWorkFlow(int assessmentToolId, string category)
        {
            return await _context.Set<AssessmentToolWorkflow>()
                      .FirstOrDefaultAsync(x => x.Status != AssessmentToolStatus.Deleted && x.Category == category && x.AssessmentToolId == assessmentToolId && x.IsLatest);
        }

        public async Task<int> UpdateIsLatest(int assessmentToolId, string category , string oldWorkFlowDefinitionId)
        {
            var workflows = await _context.Set<AssessmentToolWorkflow>()
                      .Where(x => x.Status != AssessmentToolStatus.Deleted && x.Category == category && x.AssessmentToolId == assessmentToolId && x.WorkflowDefinitionId == oldWorkFlowDefinitionId).ToListAsync();
            workflows.ForEach(x => x.IsLatest = false);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetWorkflowsOfSameCategory(int assessmentToolId, string category)
        {
            return await _context.Set<AssessmentToolWorkflow>()
                .Where(x => x.Status != AssessmentToolStatus.Deleted && x.AssessmentToolId == assessmentToolId && x.Category == category)
                .Select(x => x.WorkflowDefinitionId)
                .ToListAsync();
        }

        public async Task<AssessmentToolWorkflow?> GetLatestWorkflowDefinition(string category)
        { 
            return await _context.Set<AssessmentToolWorkflow>()
               .FirstOrDefaultAsync(x => x.Status != AssessmentToolStatus.Deleted && x.Category == category && x.IsLatest);
        }

        public async Task<AssessmentToolWorkflow?> GetLatestWorkflowDefinitionByAssessmentToolId(int assessmentToolId ,string category)
        {
            return await _context.Set<AssessmentToolWorkflow>()
              .FirstOrDefaultAsync(x => x.Status != AssessmentToolStatus.Deleted && x.IsLatest && x.Category.Contains(category));
        }

        public async Task<List<string>> GetNonLatestWorkflowsByAssessmentTool(int assessmentToolId)
        {
            return await _context.Set<AssessmentToolWorkflow>()
                 .Where(x => x.Status != AssessmentToolStatus.Deleted && x.AssessmentToolId == assessmentToolId && !x.IsLatest)
                 .Select(x => x.WorkflowDefinitionId)
                 .ToListAsync();
        }

        public async Task UpdateWorkflowNameAndCategory(string oldCategoryName, string newCategoryName)
        {
            var assessmentWorflows = await _context.Set<AssessmentToolWorkflow>().Where(x => x.Name == oldCategoryName).ToListAsync();
            assessmentWorflows.ForEach(x => { x.Name = newCategoryName; x.Category = x.Category.Replace(oldCategoryName, newCategoryName);  });
            await _context.SaveChangesAsync();
        }
    }
}
