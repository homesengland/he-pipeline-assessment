using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IAdminAssessmentToolWorkflowRepository
    {
        Task<AssessmentToolWorkflow?> GetAssessmentToolWorkflowById(int id);
        Task<int> CreateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow);

        Task<int> UpdateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow);

        Task<int> DeleteAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow);

        Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForOverride(int order);
        Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForRollback(int order);
        Task<List<AssessmentToolWorkflow>> GetAssessmentToolWorkflowsForVariation();
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
    }
}
