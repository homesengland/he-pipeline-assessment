using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.UI.Common.Utility;

namespace He.PipelineAssessment.UI.Features.SinglePipeline.Sync
{
    public interface ISyncCommandHandlerHelper
    {
        List<Models.Assessment> AssessmentsToBeAdded(List<int> sourceAssessmentSpIds, List<int> destinationAssessmentSpIds, List<SinglePipelineData> dataResult);
        List<Models.Assessment> UpdateAssessments(List<Models.Assessment> destinationAssessments, List<int> existingAssessments, List<SinglePipelineData> data);
    }

    public class SyncCommandHandlerHelper : ISyncCommandHandlerHelper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public SyncCommandHandlerHelper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public List<Models.Assessment> AssessmentsToBeAdded(List<int> sourceAssessmentSpIds, List<int> destinationAssessmentSpIds, List<SinglePipelineData> dataResult)
        {
            var assessmentSpIdsToAdd = sourceAssessmentSpIds.Where(s => destinationAssessmentSpIds.Any(d => d == s)).ToList();
            var sourceAssessmentsToAdd = dataResult.Where(x => assessmentSpIdsToAdd.Contains(x.sp_id!.Value));

            var assessmentsToBeAdded = new List<Models.Assessment>();
            foreach (var item in sourceAssessmentsToAdd)
            {
                var fullName = item.he_advocate_f_name + " " + item.he_advocate_s_name;
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
                    CreatedDateTime = _dateTimeProvider.UtcNow()
                };
                assessmentsToBeAdded.Add(assessment);
            }

            return assessmentsToBeAdded;
        }
        public List<Models.Assessment> UpdateAssessments(List<Models.Assessment> destinationAssessments, List<int> existingAssessments, List<SinglePipelineData> data)
        {
            foreach (var spId in existingAssessments)
            {
                bool isModified = false;
                var destination = destinationAssessments.FirstOrDefault(x => x.SpId == spId);
                var source = data.FirstOrDefault(x => x.sp_id == spId);

                if (destination != null && source != null)
                {
                    var fullName = source.he_advocate_f_name + " " + source.he_advocate_s_name;

                    if (!string.IsNullOrEmpty(source.applicant_1) && destination.Counterparty != source.applicant_1)
                    {
                        destination.Counterparty = source.applicant_1!;
                        isModified = true;
                    }
                    if (!string.IsNullOrEmpty(source.internal_reference) && destination.Reference != source.internal_reference)
                    {
                        destination.Reference = source.internal_reference!;
                        isModified = true;
                    }
                    if (!string.IsNullOrEmpty(source.pipeline_opportunity_site_name) && destination.SiteName != source.pipeline_opportunity_site_name)
                    {
                        destination.SiteName = source.pipeline_opportunity_site_name!;
                        isModified = true;
                    }
                    if (!string.IsNullOrEmpty(source.he_advocate_f_name) && destination.ProjectManager != fullName)
                    {
                        destination.ProjectManager = fullName;
                        isModified = true;
                    }
                    if (!string.IsNullOrEmpty(source.he_advocate_email) && destination.ProjectManagerEmail != source.he_advocate_email)
                    {
                        destination.ProjectManagerEmail = source.he_advocate_email!;
                        isModified = true;
                    }
                    if (!string.IsNullOrEmpty(source.local_authority) && destination.LocalAuthority != source.local_authority)
                    {
                        destination.LocalAuthority = source.local_authority!;
                        isModified = true;
                    }
                    if (source.funding_ask.HasValue && destination.FundingAsk != source.funding_ask)
                    {
                        destination.FundingAsk = source.funding_ask!;
                        isModified = true;
                    }
                    if (source.units_or_homes.HasValue && destination.NumberOfHomes != source.units_or_homes)
                    {
                        destination.NumberOfHomes = source.units_or_homes!;
                        isModified = true;
                    }
                    if (isModified)
                    {
                        destination.LastModifiedDateTime = _dateTimeProvider.UtcNow();
                    }
                }
            }

            return destinationAssessments;
        }
    }
}
