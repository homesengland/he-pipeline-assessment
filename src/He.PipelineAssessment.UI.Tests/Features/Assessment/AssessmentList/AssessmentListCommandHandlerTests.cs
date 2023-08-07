using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using Moq;
using Xunit;
using He.PipelineAssessment.Models;
using System.Collections.Generic;

namespace He.PipelineAssessment.UI.Tests.Features.Assessment.AssessmentList
{
    public class AssessmentListCommandHandlerTests
    {

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsError_GivenRepoThrowsError(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            AssessmentListCommand assessmentListCommand,
            Exception exception,
            AssessmentListCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessments()).Throws(exception);

            //Act
            var result = await Assert.ThrowsAsync<ApplicationException>(()=>sut.Handle(assessmentListCommand, CancellationToken.None));

            //Assert
            Assert.Equal("Unable to get list of assessments.", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsLAssessmentListData_GivenNoErrorsEncountered(
            [Frozen] Mock<IStoredProcedureRepository> repo,
             AssessmentListCommand assessmentListCommand,
            List<AssessmentDataViewModel> assessments,
            AssessmentListCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessments())
                .ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListCommand, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(assessments.Count(), result.Count());
        }
    }
}
