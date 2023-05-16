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
        Task<IEnumerable<AssessmentToolWorkflow>> GetAssessmentToolWorkflows(int assessmentToolId);

        Task<int> CreateAssessmentTool(AssessmentTool assessmentTool);

        Task<int> UpdateAssessmentTool(AssessmentTool assessmentTool);

        Task<int> DeleteAssessmentTool(AssessmentTool assessmentTool);
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
            return await _context.Set<AssessmentTool>().Include(a => a.AssessmentToolWorkflows).OrderBy(x => x.Order).ToListAsync();
        }

        public async Task<AssessmentTool?> GetAssessmentToolById(int assessmentToolId)
        {
            return await _context.Set<AssessmentTool>().Include(x => x.AssessmentToolWorkflows).FirstOrDefaultAsync(x => x.Id == assessmentToolId);
        }

        public async Task<IEnumerable<AssessmentToolWorkflow>> GetAssessmentToolWorkflows(int assessmentToolId)
        {
            return await _context.Set<AssessmentToolWorkflow>().Include(x => x.AssessmentTool).Where(x => x.AssessmentToolId == assessmentToolId)
                .ToListAsync();
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
            return await _context.Set<AssessmentToolWorkflow>().Include(x => x.AssessmentTool).FirstOrDefaultAsync(x => x.WorkflowDefinitionId == workflowDefinitionId);
        }
    }
}
