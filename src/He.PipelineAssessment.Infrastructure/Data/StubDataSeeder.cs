using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Assessments;

namespace He.PipelineAssessment.Infrastructure.Data
{
    public  class StubDataSeeder
    {
        private IAssessmentRepository _repo;
        private AssessmentStubData _dataGenerator;
        public StubDataSeeder(IAssessmentRepository repo)
        {
            _repo = repo;
            _dataGenerator = new AssessmentStubData();

        }
        public async Task SeedData()
        {
            if (!_repo.GetAssessments().Result.Any())
            {
                await _repo.CreateAssessments(_dataGenerator.GetAssessments());
            }
        }
    }
}
