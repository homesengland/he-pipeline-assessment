using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.SinglePipeline.Sync
{
    public class SyncCommandHandler : IRequestHandler<SyncCommand, SyncResponse>
    {
        private readonly IEsriSinglePipelineClient _esriSinglePipelineClient;
        private readonly IEsriSinglePipelineDataJsonHelper _jsonHelper;
        private readonly IAssessmentRepository _assessmentRepository;

        public SyncCommandHandler(IEsriSinglePipelineClient esriSinglePipelineClient, IEsriSinglePipelineDataJsonHelper jsonHelper, IAssessmentRepository assessmentRepository)
        {
            _esriSinglePipelineClient = esriSinglePipelineClient;
            _jsonHelper = jsonHelper;
            _assessmentRepository = assessmentRepository;
        }

        public async Task<SyncResponse> Handle(SyncCommand request, CancellationToken cancellationToken)
        {
            var data = await _esriSinglePipelineClient.GetSinglePipelineData();
            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToSinglePipelineDataList(data);
                List<int> sourceAssessmentSpIds = dataResult!.Select(x => x.sp_id!.Value).ToList();

                var destinationAssessments = await _assessmentRepository.GetAssessments();
                var destinationAssessmentSpIds = destinationAssessments.Select(x => x.SpId).ToList();

                var assessmentsToBeAdded = AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, dataResult);
                await _assessmentRepository.CreateAssessments(assessmentsToBeAdded);
            }

            return new SyncResponse()
            {
                ErrorMessages = new List<string>()
            };
        }

        private static List<Assessment> AssessmentsToBeAdded(List<int> sourceAssessmentSpIds, List<int> destinationAssessmentSpIds, List<SinglePipelineData>? dataResult)
        {
            //items in one list not in the other
            var assessmentSpIdsToAdd = sourceAssessmentSpIds.Where(s => !destinationAssessmentSpIds.Any(d => d == s)).ToList();
            var sourceAssessmentsToAdd = dataResult!.Where(x => assessmentSpIdsToAdd.Contains(x.sp_id!.Value));

            var assessmentsToBeAdded = new List<Assessment>();
            foreach (var item in sourceAssessmentsToAdd)
            {
                //Add to database
                var assessment = new Assessment()
                {
                    Counterparty = string.IsNullOrEmpty(item.applicant_1) ? "-" : item.applicant_1,
                    Reference = string.IsNullOrEmpty(item.internal_reference) ? "-" : item.internal_reference,
                    SiteName = string.IsNullOrEmpty(item.pipeline_opportunity_site_name)
                        ? "-"
                        : item.pipeline_opportunity_site_name,
                    SpId = item.sp_id.HasValue ? item.sp_id.Value : 999,
                    Status = "New"
                };
                assessmentsToBeAdded.Add(assessment);
            }

            return assessmentsToBeAdded;
        }
    }
}
