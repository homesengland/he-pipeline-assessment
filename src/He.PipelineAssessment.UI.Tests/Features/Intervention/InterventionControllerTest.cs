using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Intervention.InterventionList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention;
public class InterventionControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<InterventionController>> _loggerMock;
    public InterventionControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<InterventionController>>();

    }

    [Theory]
    [AutoMoqData]
    public async Task Override_ShouldRedirectToView_WhenGivenNoExceptionsThrow(AssessmentInterventionViewModel assessmentInterventionViewModel)
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<AssessmentInterventionViewModel>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionViewModel);

        // Act
        var interventionController = new InterventionController(_mediatorMock.Object, _loggerMock.Object);
        var actionResult = await interventionController.Index();

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ViewResult>(actionResult);
    }

    [Fact]
    public async Task Override_ShouldRedirectToView_AfterThrowingException()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<InterventionListRequest>(), CancellationToken.None)).ThrowsAsync(new Exception());

        // Act
        var interventionController = new InterventionController(_mediatorMock.Object, _loggerMock.Object);
        var actionResult = await interventionController.Index();

        // Assert
        Assert.IsType<RedirectToActionResult>(actionResult);
        var redirectToActionResult = (RedirectToActionResult)actionResult;
        Assert.Equal("Error", redirectToActionResult.ControllerName);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }
}
