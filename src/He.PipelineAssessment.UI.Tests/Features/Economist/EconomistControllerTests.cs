using AutoFixture.Xunit2;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Economist.Controllers;
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
    public async Task GetEconomistList_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
           [Frozen] Mock<IMediator> mediator,
           Exception exception,
           EconomistController sut)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<EconomistAssessmentListCommand>(), CancellationToken.None)).Throws(exception);

        //Act
        var result = await sut.GetEconomistList();

        //Assert
        mediator.Verify(x => x.Send(It.IsAny<EconomistAssessmentListCommand>(), CancellationToken.None), Times.Once);
        await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.IsAny<EconomistAssessmentListCommand>()));

        Assert.IsType<RedirectToActionResult>(result);
        var redirectToActionResult = (RedirectToActionResult)result;
        Assert.Equal("Error", redirectToActionResult.ControllerName);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Theory]
    [AutoMoqData]
    public async Task GetEconomistList_ShouldRedirectToAction_GivenNoExceptionsThrow(
          [Frozen] Mock<IMediator> mediator,
          EconomistAssessmentListCommand command,
          List<AssessmentDataViewModel> response,
          EconomistController sut)
    {
        //Arrange
        mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

        //Act
        var result = await sut.GetEconomistList();

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);

    }
}
