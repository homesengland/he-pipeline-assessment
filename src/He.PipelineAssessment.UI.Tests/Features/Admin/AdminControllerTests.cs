using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows;
using He.PipelineAssessment.UI.Features.Admin.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin
{
    public class AdminControllerTests
    {
        [Theory]
        [AutoMoqData]
        public void Index_ShouldReturn(AdminController sut)
        {
            //Arrange

            //Act
            var result = sut.Index();

            //Assert
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AssessmentTool_ShouldRedirectToView_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            AssessmentToolQuery query,
            AssessmentToolListData assessmentToolListData,
            AdminController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(query, CancellationToken.None)).ReturnsAsync(assessmentToolListData);

            //Act
            var result = await sut.AssessmentTool();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var redirectToView = (ViewResult)result;
            Assert.Equal("AssessmentTool", redirectToView.ViewName);   
        }

        [Theory]
        [AutoMoqData]
        public async Task AssessmentTool_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
         [Frozen] Mock<IMediator> mediator,            
            Exception exception,            
            AdminController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<AssessmentToolQuery>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.AssessmentTool();

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<AssessmentToolQuery>(), CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.IsAny<AssessmentToolQuery>()));
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);

        }

        [Theory]
        [AutoMoqData]
        public async Task AssessmentToolWorkflow_ShouldRedirectToView_GivenNoExceptionsThrow(
           [Frozen] Mock<IMediator> mediator,
           AssessmentToolQuery query,
           AssessmentToolListData assessmentToolListData,         
           AdminController sut,
            int assessmentToolId)
        {
            //Arrange
            mediator.Setup(x => x.Send(query, CancellationToken.None)).ReturnsAsync(assessmentToolListData);

            //Act
            var result = await sut.AssessmentToolWorkflow(assessmentToolId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task AssessmentToolWorkflow_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
         [Frozen] Mock<IMediator> mediator,
         Exception exception,
         AdminController sut,
         int assessmentToolId)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<AssessmentToolWorkflowQuery>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.AssessmentToolWorkflow(assessmentToolId);

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<AssessmentToolWorkflowQuery>(), CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.IsAny<AssessmentToolWorkflowQuery>()));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
