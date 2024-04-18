using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using Moq;
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
    }
}
