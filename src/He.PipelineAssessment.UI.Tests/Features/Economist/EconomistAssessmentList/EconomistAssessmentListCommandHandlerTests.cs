using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Economist.EconomistAssessmentList;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Economist.EconomistAssessmentList;
public class EconomistAssessmentListCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsError_GivenRepoThrowsError(
           [Frozen] Mock<IStoredProcedureRepository> repo,
           EconomistAssessmentListRequest economistAssessmentListRequest,
           Exception exception,
           EconomistAssessmentListRequestHandler sut
       )
    {
        //Arrange
        repo.Setup(x => x.GetEconomistAssessments()).Throws(exception);

        //Assert
        await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(economistAssessmentListRequest, CancellationToken.None));
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsLAssessmentListData_GivenNoErrorsEncountered(
          [Frozen] Mock<IStoredProcedureRepository> repo,
           EconomistAssessmentListRequest economistAssessmentListRequest,
           List<AssessmentDataViewModel> assessments,
          EconomistAssessmentListRequestHandler sut
      )
    {
        //Arrange
        repo.Setup(x => x.GetEconomistAssessments())
            .ReturnsAsync(assessments);

        //Act
        var result = await sut.Handle(economistAssessmentListRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(assessments.Count(), result.Count());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsFilteredAssessmentListData_GivenAnAssessmentDoesNotHaveValidData(
      [Frozen] Mock<IStoredProcedureRepository> repo,
       EconomistAssessmentListRequest economistAssessmentListRequest,
       List<AssessmentDataViewModel> assessments,
       EconomistAssessmentListRequestHandler sut
  )
    {
        //Arrange
        repo.Setup(x => x.GetEconomistAssessments())
            .ReturnsAsync(assessments);
        assessments.First().ValidData = false;

        //Act
        var result = await sut.Handle(economistAssessmentListRequest, CancellationToken.None);
        

        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(assessments.Count() - 1, result.Count());
    }

}
