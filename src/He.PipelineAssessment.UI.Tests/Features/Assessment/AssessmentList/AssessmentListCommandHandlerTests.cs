using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using Microsoft.AspNetCore.Http;
using Moq;
using NuGet.Frameworks;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Assessment.AssessmentList
{
    public class AssessmentListCommandHandlerTests
    {

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsError_GivenRepoThrowsError(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            AssessmentListRequest assessmentListRequest,
            Exception exception,
            AssessmentListRequestHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessments()).Throws(exception);

            //Act
            var result = await Assert.ThrowsAsync<ApplicationException>(()=>sut.Handle(assessmentListRequest, CancellationToken.None));

            //Assert
            Assert.Equal("Unable to get list of assessments.", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsLAssessmentListData_GivenNoErrorsEncountered(
            [Frozen] Mock<IStoredProcedureRepository> repo,
             AssessmentListRequest assessmentListRequest,
            List<AssessmentDataViewModel> assessments,
            AssessmentListRequestHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessments())
                .ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(assessments.Count(), result.Count());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsFilteredAssessmentListData_GivenNoSensitiveGroupPresent(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            AssessmentListRequest assessmentListRequest,
            List<AssessmentDataViewModel> assessments,
            AssessmentListRequestHandler sut
        )
        {
            //Arrange
            assessmentListRequest.CanViewSensitiveRecords = false;
            assessments.First().SensitiveStatus = "Sensitive - NDA in place";
            repo.Setup(x => x.GetAssessments())
                .ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(assessments.Count() - 1, result.Count());
            Assert.Empty(result.Where(x => x.IsSensitiveRecord()));
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsFullAssessmentListData_GivenSensitiveGroupPresent(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            AssessmentListRequest assessmentListRequest,
            List<AssessmentDataViewModel> assessments,
            AssessmentListRequestHandler sut
        )
        {
            //Arrange
            assessmentListRequest.CanViewSensitiveRecords = true;
            assessments.First().SensitiveStatus = "Sensitive - NDA in place";
            repo.Setup(x => x.GetAssessments())
                .ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(assessments.Count(), result.Count());
            Assert.Equal(1, result.Count(x => x.IsSensitiveRecord()));
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsFullAssessmentListData_GivenNoSensitiveGroupPresentButProjectManagerMatches(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            AssessmentListRequest assessmentListRequest,
            List<AssessmentDataViewModel> assessments,
            string? projectManager,
            AssessmentListRequestHandler sut
        )
        {
            //Arrange
            assessmentListRequest.CanViewSensitiveRecords = false;
            assessmentListRequest.Username = projectManager;
            assessments.First().SensitiveStatus = "Sensitive - NDA in place";
            assessments.First().ProjectManager = projectManager;
            repo.Setup(x => x.GetAssessments())
                .ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(assessments.Count(), result.Count());
            Assert.Equal(1, result.Count(x => x.IsSensitiveRecord()));
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsWhitelistedSensitiveRecords_GivenUserIsWhitelisted(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            [Frozen] Mock<IAssessmentRepository> assessmentRepo,
            [Frozen] Mock<IUserProvider> userProvider,
            AssessmentListRequest assessmentListRequest,
            List<AssessmentDataViewModel> assessments,
            string userEmail,
            AssessmentListRequestHandler sut
        )
        {
            //Arrange
            assessmentListRequest.CanViewSensitiveRecords = false;
            assessmentListRequest.Username = "different.user@example.com";

            var sensitiveAssessment = assessments.First();
            sensitiveAssessment.SensitiveStatus = SensitivityStatus.SensitiveNDA;
            sensitiveAssessment.ProjectManager = "other.manager@example.com";
            sensitiveAssessment.Id = 1;

            var whitelist = new List<int>
            {
                1
            };

            userProvider.Setup(x => x.Email()).Returns(userEmail);
            assessmentRepo.Setup(x => x.GetAllWhitelistedAssessmentIdsByEmail(userEmail))
                .ReturnsAsync(whitelist);
            repo.Setup(x => x.GetAssessments()).ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(assessments.Count, result.Count);
            Assert.Equal(1, assessments.Count(x => x.IsSensitiveRecord()));
            Assert.Equal(1, result.Count(x => x.IsSensitiveRecord()));
            Assert.Contains(result, x => x.Id == sensitiveAssessment.Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_FiltersOutSensitiveRecords_GivenUserIsNotWhitelisted(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            [Frozen] Mock<IAssessmentRepository> assessmentRepo,
            [Frozen] Mock<IUserProvider> userProvider,
            AssessmentListRequest assessmentListRequest,
            List<AssessmentDataViewModel> assessments,
            string userEmail,
            AssessmentListRequestHandler sut
        )
        {
            //Arrange
            assessmentListRequest.CanViewSensitiveRecords = false;
            assessmentListRequest.Username = "regular.user@example.com";

            assessments.First().SensitiveStatus = "Sensitive - NDA in place";
            assessments.First().ProjectManager = "other.manager@example.com";

            var emptyWhitelist = new List<int>();

            userProvider.Setup(x => x.Email()).Returns(userEmail);
            assessmentRepo.Setup(x => x.GetAllWhitelistedAssessmentIdsByEmail(userEmail))
                .ReturnsAsync(emptyWhitelist);
            repo.Setup(x => x.GetAssessments()).ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(assessments.Count - 1, result.Count);
            Assert.Empty(result.Where(x => x.IsSensitiveRecord()));
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_FiltersOutSensitiveRecords_GivenUserEmailIsNull(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            [Frozen] Mock<IAssessmentRepository> assessmentRepo,
            [Frozen] Mock<IUserProvider> userProvider,
            AssessmentListRequest assessmentListRequest,
            List<AssessmentDataViewModel> assessments,
            AssessmentListRequestHandler sut
        )
        {
            //Arrange
            assessmentListRequest.CanViewSensitiveRecords = false;
            assessmentListRequest.Username = "regular.user@example.com";

            assessments.First().SensitiveStatus = "Sensitive - NDA in place";
            assessments.First().ProjectManager = "other.manager@example.com";

            userProvider.Setup(x => x.Email()).Returns((string?)null);
            repo.Setup(x => x.GetAssessments()).ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(assessments.Count - 1, result.Count);
            Assert.Empty(result.Where(x => x.IsSensitiveRecord()));
            assessmentRepo.Verify(x => x.GetAllSensitiveRecordWhitelistsByEmail(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsPartialSensitiveRecords_GivenUserWhitelistedForSomeRecords(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            [Frozen] Mock<IAssessmentRepository> assessmentRepo,
            [Frozen] Mock<IUserProvider> userProvider,
            AssessmentListRequest assessmentListRequest,
            List<AssessmentDataViewModel> assessments,
            string userEmail,
            AssessmentListRequestHandler sut
        )
        {
            //Arrange
            assessmentListRequest.CanViewSensitiveRecords = false;
            assessmentListRequest.Username = "regular.user@example.com";

            // Make first two assessments sensitive
            assessments[0].SensitiveStatus = "Sensitive - NDA in place";
            assessments[0].ProjectManager = "other.manager@example.com";
            assessments[0].Id = 1;

            assessments[1].SensitiveStatus = "Sensitive - PLC involved in delivery";
            assessments[1].ProjectManager = "other.manager@example.com";
            assessments[1].Id = 2;

            // User is only whitelisted for the first one
            var whitelist = new List<int>
            {
                1
            };

            userProvider.Setup(x => x.Email()).Returns(userEmail);
            assessmentRepo.Setup(x => x.GetAllWhitelistedAssessmentIdsByEmail(userEmail))
                .ReturnsAsync(whitelist);
            repo.Setup(x => x.GetAssessments()).ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(assessments.Count - 1, result.Count); // One sensitive record filtered out
            Assert.Equal(1, result.Count(x => x.IsSensitiveRecord()));
            Assert.Contains(result, x => x.Id == 1); // Whitelisted one is included
            Assert.DoesNotContain(result, x => x.Id == 2); // Non-whitelisted one is excluded
        }
    }
}
