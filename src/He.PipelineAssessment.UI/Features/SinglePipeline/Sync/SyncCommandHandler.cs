using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.SinglePipeline.Sync
{
    public class SyncCommandHandler : IRequestHandler<SyncCommand, SyncResponse>
    {
        private readonly IEsriSinglePipelineClient _esriSinglePipelineClient;
        private readonly IEsriSinglePipelineDataJsonHelper _jsonHelper;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IConfiguration _config;
        private readonly IAssessmentRepository _repo;

        public SyncCommandHandler(IEsriSinglePipelineClient esriSinglePipelineClient, IEsriSinglePipelineDataJsonHelper jsonHelper, IAssessmentRepository assessmentRepository, IConfiguration config, IAssessmentRepository repo)
        {
            _esriSinglePipelineClient = esriSinglePipelineClient;
            _jsonHelper = jsonHelper;
            _assessmentRepository = assessmentRepository;
            _config = config;
            _repo = repo;
        }

        public async Task<SyncResponse> Handle(SyncCommand request, CancellationToken cancellationToken)
        {
            var errorMessages = new List<string>();
            try
            {


                var data = await _esriSinglePipelineClient.GetSinglePipelineData();
                if (data != null)
                {
                    List<int> sourceAssessmentSpIds = data.Select(x => x.sp_id!.Value).ToList();

                    var destinationAssessments = await _assessmentRepository.GetAssessments();
                    var destinationAssessmentSpIds = destinationAssessments.Select(x => x.SpId).ToList();

                    var assessmentsToBeAdded = AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, data);
                    await _assessmentRepository.CreateAssessments(assessmentsToBeAdded);

                    var existingAssessments = destinationAssessmentSpIds.Intersect(sourceAssessmentSpIds).ToList();
                    await UpdateAssessments(destinationAssessments, existingAssessments, data);
                    await _assessmentRepository.SaveChanges();

                }
                else
                {
                    errorMessages.Add("Single Pipeline Response data returned null");
                }

            }
            catch (Exception e)
            {
                errorMessages.Add(e.Message);
            }

            return new SyncResponse()
            {
                ErrorMessages = errorMessages
            };
        }

        private Task UpdateAssessments(List<Models.Assessment> destinationAssessments, List<int> existingAssessments, List<SinglePipelineData> data)
        {
            foreach (var spId in existingAssessments)
            {
                var destination = destinationAssessments.FirstOrDefault(x => x.SpId == spId);
                var source = data.FirstOrDefault(x => x.sp_id == spId);

                if (destination != null && source != null)
                {
                    var fullName = source.he_advocate_f_name + " " + source.he_advocate_s_name;

                    if (!string.IsNullOrEmpty(source.applicant_1) && destination.Counterparty != source.applicant_1)
                    {
                        destination.Counterparty = source.applicant_1!;
                    }
                    if (!string.IsNullOrEmpty(source.internal_reference) && destination.Reference != source.internal_reference)
                    {
                        destination.Reference = source.internal_reference!;
                    }
                    if (!string.IsNullOrEmpty(source.pipeline_opportunity_site_name) && destination.SiteName != source.pipeline_opportunity_site_name)
                    {
                        destination.SiteName = source.pipeline_opportunity_site_name!;
                    }
                    if (!string.IsNullOrEmpty(source.he_advocate_f_name) && destination.ProjectManager != fullName)
                    {
                        destination.ProjectManager = fullName;
                    }
                    if (!string.IsNullOrEmpty(source.he_advocate_email) && destination.ProjectManagerEmail != source.he_advocate_email)
                    {
                        destination.ProjectManagerEmail = source.he_advocate_email!;
                    }
                    if (!string.IsNullOrEmpty(source.local_authority) && destination.LocalAuthority != source.local_authority)
                    {
                        destination.LocalAuthority = source.local_authority!;
                    }
                    if (source.funding_ask.HasValue && destination.FundingAsk != source.funding_ask)
                    {
                        destination.FundingAsk = source.funding_ask!;
                    }
                    if (source.units_or_homes.HasValue && destination.NumberOfHomes != source.units_or_homes)
                    {
                        destination.NumberOfHomes = source.units_or_homes!;
                    }
                }
            }

            return Task.CompletedTask;
        }

        private static List<Models.Assessment> AssessmentsToBeAdded(List<int> sourceAssessmentSpIds, List<int> destinationAssessmentSpIds, List<SinglePipelineData> dataResult)
        {
            //items in one list not in the other
            var assessmentSpIdsToAdd = sourceAssessmentSpIds.Where(s => !destinationAssessmentSpIds.Any(d => d == s)).ToList();
            var sourceAssessmentsToAdd = dataResult.Where(x => assessmentSpIdsToAdd.Contains(x.sp_id!.Value));

            var assessmentsToBeAdded = new List<Models.Assessment>();
            foreach (var item in sourceAssessmentsToAdd)
            {
                var fullName = item.he_advocate_f_name + " " + item.he_advocate_s_name;
                //Add to database
                var assessment = new Models.Assessment()
                {
                    Counterparty = string.IsNullOrEmpty(item.applicant_1) ? "-" : item.applicant_1,
                    Reference = string.IsNullOrEmpty(item.internal_reference) ? "-" : item.internal_reference,
                    SiteName = string.IsNullOrEmpty(item.pipeline_opportunity_site_name)
                        ? "-"
                        : item.pipeline_opportunity_site_name,
                    SpId = item.sp_id.HasValue ? item.sp_id.Value : 999,
                    Status = "New",
                    ProjectManager = string.IsNullOrEmpty(item.he_advocate_f_name)
                        ? "-" : fullName,
                    ProjectManagerEmail = string.IsNullOrEmpty(item.he_advocate_email)
                        ? "-" : item.he_advocate_email,
                    LocalAuthority = string.IsNullOrEmpty(item.local_authority)
                        ? "-" : item.local_authority,
                    FundingAsk = item.funding_ask,
                    NumberOfHomes = item.units_or_homes,
                };
                assessmentsToBeAdded.Add(assessment);
            }

            return assessmentsToBeAdded;
        }
    }
}
