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

        public Task<int> DeleteAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow)
        {
            _context.Set<AssessmentToolWorkflow>().Update(assessmentToolWorkflow);
            return await _context.SaveChangesAsync();
        }
    }
}
