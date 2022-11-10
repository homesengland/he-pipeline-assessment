using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Assessments;
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
                if (_config["Data:UseSeedData"].ToLower() == "true")
                {
                    var dataGenerator = new AssessmentStubData();
                    await _repo.CreateAssessments(dataGenerator.GetAssessments());
                }
                else
                {
                    var data = await _esriSinglePipelineClient.GetSinglePipelineData();
                    if (data != null)
                    {
                        var dataResult = _jsonHelper.JsonToSinglePipelineDataList(data);
                        if (dataResult != null)
                        {
                            List<int> sourceAssessmentSpIds = dataResult.Select(x => x.sp_id!.Value).ToList();

                            var destinationAssessments = await _assessmentRepository.GetAssessments();
                            var destinationAssessmentSpIds = destinationAssessments.Select(x => x.SpId).ToList();

                            var assessmentsToBeAdded = AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, dataResult);
                            await _assessmentRepository.CreateAssessments(assessmentsToBeAdded);
                        }
                        else
                        {
                            errorMessages.Add("Single Pipeline Response data failed to deserialize");
                        }

                    }
                    else
                    {
                        errorMessages.Add("Single Pipeline Response data returned null");
                    }
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



        private static List<Assessment> AssessmentsToBeAdded(List<int> sourceAssessmentSpIds, List<int> destinationAssessmentSpIds, List<SinglePipelineData> dataResult)
        {
            //items in one list not in the other
            var assessmentSpIdsToAdd = sourceAssessmentSpIds.Where(s => !destinationAssessmentSpIds.Any(d => d == s)).ToList();
            var sourceAssessmentsToAdd = dataResult.Where(x => assessmentSpIdsToAdd.Contains(x.sp_id!.Value));

            var assessmentsToBeAdded = new List<Assessment>();
            foreach (var item in sourceAssessmentsToAdd)
            {
                var fullName = item.he_advocate_f_name + " " + item.he_advocate_s_name;
                //Add to database
                var assessment = new Assessment()
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
                };
                assessmentsToBeAdded.Add(assessment);
            }

            return assessmentsToBeAdded;
        }
    }
}
