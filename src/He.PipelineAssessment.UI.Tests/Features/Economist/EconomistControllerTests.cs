using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
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

    [Theory]
    [AutoMoqData]
    public async Task GetEconomistList_ShouldCheckPipelineEconomistRole_GivenUserIsEconomist(
    [Frozen] Mock<IMediator> mediator,
    [Frozen] Mock<IUserProvider> userProvider,
    List<AssessmentDataViewModel> response,
    EconomistController sut)
    {
        //Arrange
        userProvider.Setup(x => x.UserName()).Returns("economist@test.com");
        userProvider.Setup(x => x.IsEconomist()).Returns(true);

        mediator.Setup(x => x.Send(It.IsAny<EconomistAssessmentListRequest>(), CancellationToken.None))
            .ReturnsAsync(response);

        //Act
        var result = await sut.GetEconomistList();

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        userProvider.Verify(x => x.IsEconomist(), Times.Once);
        mediator.Verify(x => x.Send(It.Is<EconomistAssessmentListRequest>(r =>
            r.CanViewSensitiveRecords == true &&
            r.Username == "economist@test.com"),
            CancellationToken.None), Times.Once);
    }
}
