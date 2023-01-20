using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository.StoreProc
{

    public interface IStoreProcRepository
    {
        Task<List<AssessmentStageViewModel>> GetAssessmentStages(int assessmentId);
        Task<List<StartableToolViewModel>> GetStartableTools(int assessmentId);

    }
    public class StoreProcRepository : IStoreProcRepository
    {
        private readonly PipelineAssessmentStoreProcContext _storeProcContext;
        private readonly string sp_GetAssessmentStagesByAssessmentId = @"exec GetAssessmentStagesByAssessmentId @assessmentId";
        private readonly string sp_GetStartableToolsByAssessmentId = @"exec GetStartableToolsByAssessmentId @assessmentId";

        public StoreProcRepository(PipelineAssessmentStoreProcContext storeProcContext)
        {
            _storeProcContext = storeProcContext;
        }
        public async Task<List<AssessmentStageViewModel>> GetAssessmentStages(int assessmentId)
        {

            var assessmentIdParameter = new SqlParameter("@assessmentId", assessmentId);
            var stages = await _storeProcContext.AssessmentStageViewModel
                .FromSqlRaw(sp_GetAssessmentStagesByAssessmentId, assessmentIdParameter).ToListAsync();
            return new List<AssessmentStageViewModel>(stages);
        }

        public async Task<List<StartableToolViewModel>> GetStartableTools(int assessmentId)
        {
            var assessmentIdParameter = new SqlParameter("@assessmentId", assessmentId);
            var startableTools = await _storeProcContext.StartableToolViewModel
                .FromSqlRaw(sp_GetStartableToolsByAssessmentId, assessmentIdParameter).ToListAsync();
            return new List<StartableToolViewModel>(startableTools);
        }

    }
}
