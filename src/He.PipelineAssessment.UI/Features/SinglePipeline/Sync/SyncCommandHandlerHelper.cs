﻿using He.PipelineAssessment.Data.SinglePipeline;

namespace He.PipelineAssessment.UI.Features.SinglePipeline.Sync
{
    public interface ISyncCommandHandlerHelper
    {
        List<Models.Assessment> AssessmentsToBeAdded(List<int> sourceAssessmentSpIds, List<int> destinationAssessmentSpIds, List<SinglePipelineData> sourcSinglePipelineData);
        int UpdateAssessments(List<Models.Assessment> destinationAssessments, List<int> existingAssessments, List<SinglePipelineData> data);
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
                    LocalAuthority = string.IsNullOrEmpty(item.local_authority)
                        ? "-" : item.local_authority,
                    FundingAsk = item.funding_ask,
                    NumberOfHomes = item.units_or_homes,
                    BusinessArea = string.IsNullOrEmpty(item.sp_business_area)
                         ? "-" : item.sp_business_area,
                    LandType = string.IsNullOrEmpty(item.land_type)
                        ? "-"
                        : item.land_type,
                };

                assessmentsToBeAdded.Add(assessment);
            }

            return assessmentsToBeAdded;
        }
        public int UpdateAssessments(List<Models.Assessment> destinationAssessments, List<int> existingAssessments, List<SinglePipelineData> data)
        {
            var count = 0;
            foreach (var spId in existingAssessments)
            {
                var destination = destinationAssessments.FirstOrDefault(x => x.SpId == spId);
                var source = data.FirstOrDefault(x => x.sp_id == spId);
                bool updateFlag = false;
                if (destination != null && source != null)
                {
                    if (!string.IsNullOrEmpty(source.applicant_1) && destination.Counterparty != source.applicant_1)
                    {
                        destination.Counterparty = source.applicant_1!;
                        updateFlag = true;
                    }
                    if (!string.IsNullOrEmpty(source.internal_reference) && destination.Reference != source.internal_reference)
                    {
                        destination.Reference = source.internal_reference!;
                        updateFlag = true;
                    }
                    if (!string.IsNullOrEmpty(source.pipeline_opportunity_site_name) && destination.SiteName != source.pipeline_opportunity_site_name)
                    {
                        destination.SiteName = source.pipeline_opportunity_site_name!;
                        updateFlag = true;
                    }
                    if (!string.IsNullOrEmpty(source.project_owner) && destination.ProjectManager != source.project_owner)
                    {
                        destination.ProjectManager = source.project_owner;
                        updateFlag = true;
                    }
                    if (!string.IsNullOrEmpty(source.local_authority) && destination.LocalAuthority != source.local_authority)
                    {
                        destination.LocalAuthority = source.local_authority!;
                        updateFlag = true;
                    }
                    if (!string.IsNullOrEmpty(source.sp_business_area) && destination.BusinessArea != source.sp_business_area)
                    {
                        destination.BusinessArea = source.sp_business_area!;
                        updateFlag = true;
                    }
                    if (source.funding_ask.HasValue && destination.FundingAsk != source.funding_ask)
                    {
                        destination.FundingAsk = source.funding_ask!;
                        updateFlag = true;
                    }
                    if (source.units_or_homes.HasValue && destination.NumberOfHomes != source.units_or_homes)
                    {
                        destination.NumberOfHomes = source.units_or_homes!;
                        updateFlag = true;
                    }
                    if (!string.IsNullOrEmpty(source.land_type) && destination.LandType != source.land_type)
                    {
                        destination.LandType = source.land_type!;
                        updateFlag = true;
                    }
                }
                if (updateFlag)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
