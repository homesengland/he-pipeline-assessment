using AutoFixture.Xunit2;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Economist;
using He.PipelineAssessment.UI.Features.Economist.EconomistAssessmentList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Economist;
public class EconomistControllerTests
{

    [Theory]
    [AutoMoqData]
    public async Task GetEconomistList_ShouldThrowException_GivenExceptionThrownByMediator(
       [Frozen] Mock<IMediator> mediator,
       Exception exception,
       EconomistController sut)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<EconomistAssessmentListRequest>(), CancellationToken.None)).Throws(exception);

        //Assert
        await Assert.ThrowsAsync<Exception>(()=> sut.GetEconomistList());

    }

    [Theory]
    [AutoMoqData]
    public async Task GetEconomistList_ShouldRedirectToAction_GivenNoExceptionsThrow(
          [Frozen] Mock<IMediator> mediator,
          EconomistAssessmentListRequest request,
          List<AssessmentDataViewModel> response,
          EconomistController sut)
    {
        //Arrange
        mediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);

        //Act
        var result = await sut.GetEconomistList();

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);

    }
}
