using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository
{

    public interface IAdminAssessmentToolRepository
    {
        Task<IEnumerable<AssessmentTool>> GetAssessmentTools();
        Task<AssessmentTool?> GetAssessmentToolById(int assessmentToolId);
        Task<AssessmentToolWorkflow?> GetAssessmentToolByWorkflowDefinitionId(string workflowDefinitionId);
        Task<int> CreateAssessmentTool(AssessmentTool assessmentTool);

        Task<int> UpdateAssessmentTool(AssessmentTool assessmentTool);

        Task<int> DeleteAssessmentTool(AssessmentTool assessmentTool);
        Task<int> SaveChanges();
    }
    public class AdminAssessmentToolRepository : IAdminAssessmentToolRepository
    {
        private readonly PipelineAssessmentContext _context;

        public AdminAssessmentToolRepository(PipelineAssessmentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssessmentTool>> GetAssessmentTools()
        {
            // comment: The lines below fetch a list of assessment tools from the database that are not marked as deleted,
            // along with their associated workflows that are also not marked as deleted. The results are then ordered by a specific property
            // (Order) and returned as a list.
            return await _context.Set<AssessmentTool>().Where(x => x.Status != AssessmentToolStatus.Deleted)
                .Include(a => a.AssessmentToolWorkflows!.Where(w => w.Status != AssessmentToolStatus.Deleted)).OrderBy(x => x.Order).ToListAsync();
        }

        public async Task<AssessmentTool?> GetAssessmentToolById(int assessmentToolId)
        {
            return await _context.Set<AssessmentTool>().Include(x => x.AssessmentToolWorkflows).FirstOrDefaultAsync(x => x.Id == assessmentToolId);
        }

        public async Task<int> CreateAssessmentTool(AssessmentTool assessmentTool)
        {
            await _context.Set<AssessmentTool>().AddAsync(assessmentTool);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAssessmentTool(AssessmentTool assessmentTool)
        {
            _context.Set<AssessmentTool>().Update(assessmentTool);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAssessmentTool(AssessmentTool assessmentTool)
        {
            _context.Set<AssessmentTool>().Remove(assessmentTool);
            return await _context.SaveChangesAsync();
        }

        public async Task<AssessmentToolWorkflow?> GetAssessmentToolByWorkflowDefinitionId(string workflowDefinitionId)
        {
            return await _context.Set<AssessmentToolWorkflow>()
                .Include(x => x.AssessmentTool)
                .FirstOrDefaultAsync(x => x.WorkflowDefinitionId == workflowDefinitionId && x.Status != AssessmentToolStatus.Deleted);
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
