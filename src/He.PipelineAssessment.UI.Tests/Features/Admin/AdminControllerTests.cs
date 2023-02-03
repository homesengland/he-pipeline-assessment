using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentToolWorkflow;
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

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolById_ShouldRedirectToView_GivenNoExceptionsThrow(
         [Frozen] Mock<IMediator> mediator,
         AssessmentToolWorkflowQuery query,
         AssessmentToolWorkflowListDto assessmentToolListData,
         AdminController sut,
          int assessmentToolId)
        {
            //Arrange
            mediator.Setup(x => x.Send(query, CancellationToken.None)).ReturnsAsync(assessmentToolListData);

            //Act
            var result = await sut.GetAssessmentToolById(assessmentToolId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolById_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
         [Frozen] Mock<IMediator> mediator,
         Exception exception,
         AdminController sut,
         int assessmentToolId)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<AssessmentToolWorkflowQuery>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.GetAssessmentToolById(assessmentToolId);

            //Assert
            mediator.Verify(x => x.Send(It.IsAny<AssessmentToolWorkflowQuery>(), CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.IsAny<AssessmentToolWorkflowQuery>()));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadAssessmentTool_ShouldRedirectToView_GivenNoExceptionsThrow(              
        CreateAssessmentToolDto createAssessmentToolDto,
        AdminController sut)
        {
            //Arrange
          
            //Act
            var result = await sut.LoadAssessmentTool(createAssessmentToolDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        //[Theory]
        //[AutoMoqData]
        //public async Task LoadAssessmentTool_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
        // [Frozen] Mock<IMediator> mediator,
        // Exception exception,
        // CreateAssessmentToolDto createAssessmentToolDto,         
        // AdminController sut)
        //{
        //    //Arrange
        //    // mediator.Setup(x => x.Send(It.IsAny<CreateAssessmentToolDto>(), CancellationToken.None)).ReturnsAsync(null);

        //    //Act
        //    var result =  await Assert.ThrowsAnyAsync<Exception>(() => sut.LoadAssessmentTool(createAssessmentToolDto));
        //    // var result = await sut.LoadAssessmentTool(createAssessmentToolDto);

        //    //Assert
        //    // result.Verify(x => x.Send(It.IsAny<CreateAssessmentToolCommand>(), CancellationToken.None ), Times.Once);
        //    // await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.IsAny<CreateAssessmentToolCommand>()));

        //    Assert.Equal("Cannot read temperature before initializing.", result.Message);
        //    //Assert.Equal("Error", redirectToActionResult.ControllerName);
        //    //Assert.Equal("Index", redirectToActionResult.ActionName);
        //}


        [Theory]
        [AutoMoqData]
        public async Task LoadAssessmentToolWorkflow_ShouldRedirectToView_GivenNoExceptionsThrow(       
        CreateAssessmentToolWorkflowDto createAssessmentToolWorkflowDto,
        AdminController sut)
        {
            //Arrange

            //Act
            var result = await sut.LoadAssessmentToolWorkflow(createAssessmentToolWorkflowDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }


        [Theory]
        [AutoMoqData]
        public async Task DeleteAssessmentTool_ShouldRedirectToView_GivenNoExceptionsThrow(
           [Frozen] Mock<IMediator> mediator,
           DeleteAssessmentToolCommand command,        
           AdminController sut,
            int assessmentToolId)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None));

            //Act
            var result = await sut.DeleteAssessmentTool(assessmentToolId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }
        [Theory]
        [AutoMoqData]
        public async Task DeleteAssessmentTool_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
      [Frozen] Mock<IMediator> mediator,
      Exception exception,
      AdminController sut,
      int assessmentToolId)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<DeleteAssessmentToolCommand>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.DeleteAssessmentTool(assessmentToolId);

            //Assert           
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }


        [Theory]
        [AutoMoqData]
        public async Task DeleteAssessmentToolWorkflow_ShouldRedirectToView_GivenNoExceptionsThrow(
           [Frozen] Mock<IMediator> mediator,
           DeleteAssessmentToolCommand command,
           AdminController sut,
           int assessmentToolWorkflowId,
            int assessmentToolId)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None));

            //Act
            var result = await sut.DeleteAssessmentToolWorkflow(assessmentToolWorkflowId,assessmentToolId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteAssessmentToolWorkflow_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
        [Frozen] Mock<IMediator> mediator,
        Exception exception,
        AdminController sut,       
        int assessmentToolWorkflowId,
        int assessmentToolId)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<DeleteAssessmentToolWorkflowCommand>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.DeleteAssessmentToolWorkflow(assessmentToolWorkflowId,assessmentToolId);

            //Assert           
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

    }
}
