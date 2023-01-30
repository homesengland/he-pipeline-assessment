using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IAdminAssessmentToolWorkflowRepository
    {
        Task<IEnumerable<AssessmentToolWorkflow>> GetAssessmentToolWorkflows(int assessmentToolId);

        Task<int> CreateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow);

        Task<int> UpdateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow);

        Task<int> DeleteAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow);
    }
    public class AdminAssessmentToolWorkflowRepository : IAdminAssessmentToolWorkflowRepository
    {
        private readonly PipelineAssessmentContext _context;

        public AdminAssessmentToolWorkflowRepository(PipelineAssessmentContext pipelineAssessmentContext)
        {
             _context  = pipelineAssessmentContext;
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

        public async Task<IEnumerable<AssessmentToolWorkflow>> GetAssessmentToolWorkflows(int assessmentToolId)
        {
            return await _context.Set<AssessmentToolWorkflow>().Include(x => x.AssessmentTool).Where(x => x.AssessmentToolId == assessmentToolId)
                .ToListAsync();
        }

        public Task<int> UpdateAssessmentToolWorkflow(AssessmentToolWorkflow assessmentToolWorkflow)
        {
            throw new NotImplementedException();
        }
    }
}
