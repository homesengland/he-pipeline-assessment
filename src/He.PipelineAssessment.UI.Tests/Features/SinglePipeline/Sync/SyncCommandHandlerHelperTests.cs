using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.SinglePipeline.Sync
{
    public class SyncCommandHandlerHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void AssessmentsToBeAdded_ShouldRetrunEmptyList_GivenListofSourceandDestinationSpIdsIsTheSame(
         List<Models.Assessment> assessmentList,
         List<SinglePipelineData> singlePipelineData,
        SyncCommandHandlerHelper sut)
        {
            //Arrange
            var sourceAssessmentSpIds = new List<int>
            {
             2,6,8
            };

            assessmentList[0].SpId = 2;
            assessmentList[1].SpId = 6;
            assessmentList[2].SpId = 8;

            singlePipelineData[0].sp_id = 2;
            singlePipelineData[1].sp_id = 6;
            singlePipelineData[2].sp_id = 8;

            var destinationAssessmentSpIds = assessmentList.Select(x => x.SpId).ToList();

            //Act
            var result = sut.AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, singlePipelineData);

            //Assert
            Assert.Empty(result);
        }


        [Theory]
        [AutoMoqData]
        public void AssessmentsToBeAdded_ShouldRetrunListOFSinglePipelineData_GivenListofSourceandDestinationSpIdsIsDifferent(
            List<Models.Assessment> assessmentList,
            List<SinglePipelineData> singlePipelineData,
            SyncCommandHandlerHelper sut)
        {

            //Arrange
            var sourceAssessmentSpIds = new List<int>
            {
                2,6,8
            };

            singlePipelineData[0].sp_id = 2;
            singlePipelineData[1].sp_id = 6;
            singlePipelineData[2].sp_id = 8;

            assessmentList[0].SpId = 200;
            assessmentList[1].SpId = 600;
            assessmentList[2].SpId = 800;

            var destinationAssessmentSpIds = assessmentList.Select(x => x.SpId).ToList();

            //Act
            var result = sut.AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, singlePipelineData);

            //Assert
            Assert.NotEmpty(result);
        }

        [Theory]
        [AutoMoqData]
        public void UpdateAssessments_ShouldReturnSameListOfAssessmentModel_GivenNoChangesIdentified(
            Models.Assessment destinationAssessment,
            SinglePipelineData singlePipelineData,
            SyncCommandHandlerHelper sut)
        {
            var existingAssessments = new List<int> { 2 };

            destinationAssessment.SpId = 2;
            destinationAssessment.Counterparty = "Counterparty";
            destinationAssessment.Reference = "Reference";
            destinationAssessment.SiteName = "SiteName";
            destinationAssessment.ProjectManager = "Project Manager";
            destinationAssessment.ProjectManagerEmail = "ProjectManagerEmail";
            destinationAssessment.LocalAuthority = "LocalAuthority";
            destinationAssessment.FundingAsk = 123;
            destinationAssessment.NumberOfHomes = 345;
            destinationAssessment.BusinessArea = "Land";

            var destinationAssessments = new List<Models.Assessment>();
            destinationAssessments.Add(destinationAssessment);

            singlePipelineData.sp_id = 2;
            singlePipelineData.applicant_1 = "Counterparty";
            singlePipelineData.internal_reference = "Reference";
            singlePipelineData.pipeline_opportunity_site_name = "SiteName";
            singlePipelineData.project_owner = "Project Manager";
            singlePipelineData.project_owner_email = "ProjectManagerEmail";
            singlePipelineData.local_authority = "LocalAuthority";
            singlePipelineData.funding_ask = 123;
            singlePipelineData.units_or_homes = 345;
            singlePipelineData.sp_business_area = "Land";

            var singlePipelineDataList = new List<SinglePipelineData>();
            singlePipelineDataList.Add(singlePipelineData);

            //Arrange

            //Act
            var result = sut.UpdateAssessments(destinationAssessments, existingAssessments, singlePipelineDataList);

            //Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [AutoMoqData]
        public void UpdateAssessments_ShouldReturnUpdatedListOfAssessmentModel_GivenChangesIdentified(
           Models.Assessment destinationAssessment,
           SinglePipelineData singlePipelineData,
           SyncCommandHandlerHelper sut)
        {
            var existingAssessments = new List<int> { 2 };

            destinationAssessment.SpId = 2;
            destinationAssessment.Counterparty = "Counterparty";
            destinationAssessment.Reference = "Reference";
            destinationAssessment.SiteName = "SiteName";
            destinationAssessment.ProjectManager = "Project Manager";
            destinationAssessment.ProjectManagerEmail = "ProjectManagerEmail";
            destinationAssessment.LocalAuthority = "LocalAuthority";
            destinationAssessment.FundingAsk = 123;
            destinationAssessment.NumberOfHomes = 345;
            destinationAssessment.BusinessArea = "Land";


            var destinationAssessments = new List<Models.Assessment>();
            destinationAssessments.Add(destinationAssessment);

            singlePipelineData.sp_id = 2;
            singlePipelineData.applicant_1 = "applicant_1";
            singlePipelineData.internal_reference = "internal_reference";
            singlePipelineData.pipeline_opportunity_site_name = "pipeline_opportunity_site_name";
            singlePipelineData.project_owner = "project_owner";
            singlePipelineData.project_owner_email = "project_owner_email";
            singlePipelineData.local_authority = "local_authority";
            singlePipelineData.funding_ask = 555;
            singlePipelineData.units_or_homes = 667;
            singlePipelineData.sp_business_area = "Investment";

           var singlePipelineDataList = new List<SinglePipelineData>();
            singlePipelineDataList.Add(singlePipelineData);

            //Arrange

            //Act
            var result = sut.UpdateAssessments(destinationAssessments, existingAssessments, singlePipelineDataList);

            //Assert
            Assert.Equal(1, result);
        }
    }
}
