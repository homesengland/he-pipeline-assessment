using He.PipelineAssessment.Data.SinglePipeline;

namespace He.PipelineAssessment.UI.Features.SinglePipeline.Sync
{
    public interface ISyncCommandHandlerHelper
    {
        List<Models.Assessment> AssessmentsToBeAdded(List<int> sourceAssessmentSpIds, List<int> destinationAssessmentSpIds, List<SinglePipelineData> sourcSinglePipelineData);
        List<Models.Assessment> UpdateAssessments(List<Models.Assessment> destinationAssessments, List<int> existingAssessments, List<SinglePipelineData> data);
    }

    public class SyncCommandHandlerHelper : ISyncCommandHandlerHelper
    {

        public SyncCommandHandlerHelper()
        {
        }

        public List<Models.Assessment> AssessmentsToBeAdded(List<int> sourceAssessmentSpIds, List<int> destinationAssessmentSpIds, List<SinglePipelineData> sourceSinglePipelineData)
        {

            var assessmentSpIdsToAdd = sourceAssessmentSpIds.Where(p => destinationAssessmentSpIds.All(p2 => p2 != p)).ToList();
            var sourceAssessmentsToAdd = sourceSinglePipelineData.Where(x => assessmentSpIdsToAdd.Contains(x.sp_id!.Value));

            var assessmentsToBeAdded = new List<Models.Assessment>();
            foreach (var item in sourceAssessmentsToAdd)
            {
                var assessment = new Models.Assessment()
                {
                    Counterparty = string.IsNullOrEmpty(item.applicant_1) ? "-" : item.applicant_1,
                    Reference = string.IsNullOrEmpty(item.internal_reference) ? "-" : item.internal_reference,
                    SiteName = string.IsNullOrEmpty(item.pipeline_opportunity_site_name)
                        ? "-"
                        : item.pipeline_opportunity_site_name,
                    SpId = item.sp_id.HasValue ? item.sp_id.Value : 999,
                    Status = "New",
                    ProjectManager = string.IsNullOrEmpty(item.project_owner)
                        ? "-" : item.project_owner,
                    ProjectManagerEmail = string.IsNullOrEmpty(item.project_owner_email)
                        ? "-" : item.project_owner_email,
                    LocalAuthority = string.IsNullOrEmpty(item.local_authority)
                        ? "-" : item.local_authority,
                    FundingAsk = item.funding_ask,
                    NumberOfHomes = item.units_or_homes,
                    BusinessArea = string.IsNullOrEmpty(item.sp_business_area)
                         ? "-" : item.sp_business_area,
                };

                assessmentsToBeAdded.Add(assessment);
            }

            return assessmentsToBeAdded;
        }
        public List<Models.Assessment> UpdateAssessments(List<Models.Assessment> destinationAssessments, List<int> existingAssessments, List<SinglePipelineData> data)
        {
            foreach (var spId in existingAssessments)
            {
                var destination = destinationAssessments.FirstOrDefault(x => x.SpId == spId);
                var source = data.FirstOrDefault(x => x.sp_id == spId);

                if (destination != null && source != null)
                {
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
                    if (!string.IsNullOrEmpty(source.project_owner) && destination.ProjectManager != source.project_owner)
                    {
                        destination.ProjectManager = source.project_owner;
                    }
                    if (!string.IsNullOrEmpty(source.project_owner_email) && destination.ProjectManagerEmail != source.project_owner_email)
                    {
                        destination.ProjectManagerEmail = source.project_owner_email!;
                    }
                    if (!string.IsNullOrEmpty(source.local_authority) && destination.LocalAuthority != source.local_authority)
                    {
                        destination.LocalAuthority = source.local_authority!;
                    }
                    if (!string.IsNullOrEmpty(source.sp_business_area) && destination.BusinessArea != source.sp_business_area)
                    {
                        destination.BusinessArea = source.sp_business_area!;
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

            return destinationAssessments;
        }
    }
}
