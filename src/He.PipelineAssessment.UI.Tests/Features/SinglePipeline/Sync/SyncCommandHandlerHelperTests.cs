using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.SinglePipeline.Sync
{
    public class SyncCommandHandlerHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void AssessmentsToBeAdded_ShouldRetrunEmptyList_GivenListofSourceandDestinationSpIdsIsTheSame(
         [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
         List<Models.Assessment> assessmentList,
         List<SinglePipelineData> singlePipelineData,
        SyncCommandHandlerHelper sut)
        {
            //Arrange
            dateTimeProvider.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);
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
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            List<Models.Assessment> assessmentList,
            List<SinglePipelineData> singlePipelineData,
            SyncCommandHandlerHelper sut)
        {

            //Arrange
            dateTimeProvider.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);
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
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
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

            var destinationAssessments = new List<Models.Assessment>();
            destinationAssessments.Add(destinationAssessment);

            singlePipelineData.sp_id = 2;
            singlePipelineData.applicant_1 = "Counterparty";
            singlePipelineData.internal_reference = "Reference";
            singlePipelineData.pipeline_opportunity_site_name = "SiteName";
            singlePipelineData.he_advocate_f_name = "Project";
            singlePipelineData.he_advocate_s_name = "Manager";
            singlePipelineData.he_advocate_email = "ProjectManagerEmail";
            singlePipelineData.local_authority = "LocalAuthority";
            singlePipelineData.funding_ask = 123;
            singlePipelineData.units_or_homes = 345;

            var singlePipelineDataList = new List<SinglePipelineData>();
            singlePipelineDataList.Add(singlePipelineData);

            //Arrange
            dateTimeProvider.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);

            //Act
            var result = sut.UpdateAssessments(destinationAssessments, existingAssessments, singlePipelineDataList);

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(singlePipelineData.applicant_1, result[0].Counterparty);
            Assert.Equal(singlePipelineData.internal_reference, result[0].Reference);
            Assert.Equal(singlePipelineData.pipeline_opportunity_site_name, result[0].SiteName);
            Assert.Equal(singlePipelineData.he_advocate_f_name + " " + singlePipelineData.he_advocate_s_name, result[0].ProjectManager);
            Assert.Equal(singlePipelineData.he_advocate_email, result[0].ProjectManagerEmail);
            Assert.Equal(singlePipelineData.local_authority, result[0].LocalAuthority);
            Assert.Equal(singlePipelineData.funding_ask, result[0].FundingAsk);
            Assert.Equal(singlePipelineData.units_or_homes, result[0].NumberOfHomes);
            dateTimeProvider.Verify(x => x.UtcNow(), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public void UpdateAssessments_ShouldReturnUpdatedListOfAssessmentModel_GivenChangesIdentified(
           [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
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

            var destinationAssessments = new List<Models.Assessment>();
            destinationAssessments.Add(destinationAssessment);

            singlePipelineData.sp_id = 2;
            singlePipelineData.applicant_1 = "applicant_1";
            singlePipelineData.internal_reference = "internal_reference";
            singlePipelineData.pipeline_opportunity_site_name = "pipeline_opportunity_site_name";
            singlePipelineData.he_advocate_f_name = "he_advocate_f_name";
            singlePipelineData.he_advocate_s_name = "he_advocate_s_name";
            singlePipelineData.he_advocate_email = "he_advocate_email";
            singlePipelineData.local_authority = "local_authority";
            singlePipelineData.funding_ask = 555;
            singlePipelineData.units_or_homes = 667;

            var singlePipelineDataList = new List<SinglePipelineData>();
            singlePipelineDataList.Add(singlePipelineData);

            //Arrange
            dateTimeProvider.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);

            //Act
            var result = sut.UpdateAssessments(destinationAssessments, existingAssessments, singlePipelineDataList);

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(singlePipelineData.applicant_1, result[0].Counterparty);
            Assert.Equal(singlePipelineData.internal_reference, result[0].Reference);
            Assert.Equal(singlePipelineData.pipeline_opportunity_site_name, result[0].SiteName);
            Assert.Equal(singlePipelineData.he_advocate_f_name + " " + singlePipelineData.he_advocate_s_name, result[0].ProjectManager);
            Assert.Equal(singlePipelineData.he_advocate_email, result[0].ProjectManagerEmail);
            Assert.Equal(singlePipelineData.local_authority, result[0].LocalAuthority);
            Assert.Equal(singlePipelineData.funding_ask, result[0].FundingAsk);
            Assert.Equal(singlePipelineData.units_or_homes, result[0].NumberOfHomes);
            dateTimeProvider.Verify(x => x.UtcNow(), Times.Once);
        }
    }
}
