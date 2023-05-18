using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository.StoredProcedure
{

    public interface IStoredProcedureRepository
    {
        Task<List<AssessmentStageViewModel>> GetAssessmentStages(int assessmentId);
        Task<List<StartableToolViewModel>> GetStartableTools(int assessmentId);
        Task<List<AssessmentDataViewModel>> GetAssessments();
        Task<List<AssessmentDataViewModel>> GetEconomistAssessments();
    }
    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        private readonly PipelineAssessmentStoreProcContext _storeProcContext;
        private readonly string sp_GetAssessmentStagesByAssessmentId = @"exec GetAssessmentStagesByAssessmentId @assessmentId";
        private readonly string sp_GetStartableToolsByAssessmentId = @"exec GetStartableToolsByAssessmentId @assessmentId";
        private readonly string sp_AssessmentData = @"exec GetAssessments";
        private readonly string sp_EconomistAssessmentData = @"exec GetEconomistAssessments";
        public StoredProcedureRepository(PipelineAssessmentStoreProcContext storeProcContext)
        {
            _storeProcContext = storeProcContext;
        }

        public async Task<List<AssessmentDataViewModel>> GetAssessments()
        {
            var assessmentData = await _storeProcContext.AssessmentDataViewModel
                .FromSqlRaw(sp_AssessmentData).ToListAsync();
            return new List<AssessmentDataViewModel>(assessmentData);
        }

        public async Task<List<AssessmentStageViewModel>> GetAssessmentStages(int assessmentId)
        {

            var assessmentIdParameter = new SqlParameter("@assessmentId", assessmentId);
            var stages = await _storeProcContext.AssessmentStageViewModel
                .FromSqlRaw(sp_GetAssessmentStagesByAssessmentId, assessmentIdParameter).ToListAsync();
            return new List<AssessmentStageViewModel>(stages);
        }

        public async Task<List<AssessmentDataViewModel>> GetEconomistAssessments()
        {
            var assessmentData = await _storeProcContext.AssessmentDataViewModel
                .FromSqlRaw(sp_AssessmentData).ToListAsync();
            return new List<AssessmentDataViewModel>(assessmentData);
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
