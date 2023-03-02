using AutoFixture.Xunit2;
using FluentValidation;
using FluentValidation.Results;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;
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
            var result = await sut.DeleteAssessmentToolWorkflow(assessmentToolWorkflowId, assessmentToolId);

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
            var result = await sut.DeleteAssessmentToolWorkflow(assessmentToolWorkflowId, assessmentToolId);

            //Assert           
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentTool_ShouldRedirectToAssessmentTool_GivenValidationResultIsValid(
        [Frozen] Mock<IValidator<CreateAssessmentToolCommand>> validator,
        ValidationResult validationResult,
        AdminController sut)
        {
            //Arrange
            validationResult.Errors = new List<ValidationFailure>();
            var createAssessmentToolDto = new CreateAssessmentToolDto
            {
                ValidationResult = null
            };

            validator.Setup(x => x.ValidateAsync(createAssessmentToolDto.CreateAssessmentToolCommand, CancellationToken.None)).ReturnsAsync(validationResult);

            //Act
            var result = await sut.CreateAssessmentTool(createAssessmentToolDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("AssessmentTool", redirectToActionResult.ActionName);
            Assert.Null(createAssessmentToolDto.ValidationResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentTool_ShouldReturnLoadAssessmentToolViewWithValidationResult_GivenValidationResultIsInvalid(
        [Frozen] Mock<IValidator<CreateAssessmentToolCommand>> validator,
        CreateAssessmentToolDto createAssessmentToolDto,
        ValidationResult validationResult,
        AdminController sut)
        {
            //Arrange
            validator.Setup(x => x.ValidateAsync(createAssessmentToolDto.CreateAssessmentToolCommand, CancellationToken.None)).ReturnsAsync(validationResult);

            //Act
            var result = await sut.CreateAssessmentTool(createAssessmentToolDto);

            //Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("LoadAssessmentTool", viewResult.ViewName);
            Assert.Equal(validationResult, createAssessmentToolDto.ValidationResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentToolWorkflow_ShouldRedirectToAssessmentToolWorkflow_GivenValidationResultIsValid(
        [Frozen] Mock<IValidator<CreateAssessmentToolWorkflowCommand>> validator,
        ValidationResult validationResult,
        AdminController sut)
        {
            //Arrange
            validationResult.Errors = new List<ValidationFailure>();
            var createAssessmentToolWorkflowDto = new CreateAssessmentToolWorkflowDto()
            {
                ValidationResult = null
            };

            validator.Setup(x =>
                x.ValidateAsync(createAssessmentToolWorkflowDto.CreateAssessmentToolWorkflowCommand,
                CancellationToken.None))
                .ReturnsAsync(validationResult);

            //Act
            var result = await sut.CreateAssessmentToolWorkflow(createAssessmentToolWorkflowDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("AssessmentToolWorkflow", redirectToActionResult.ActionName);
            Assert.Null(createAssessmentToolWorkflowDto.ValidationResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentToolWorkflow_ShouldReturnLoadAssessmentToolWorkflowViewWithValidationResult_GivenValidationResultIsInvalid(
        [Frozen] Mock<IValidator<CreateAssessmentToolWorkflowCommand>> validator,
        CreateAssessmentToolWorkflowDto createAssessmentToolWorkflowDto,
        ValidationResult validationResult,
        AdminController sut)
        {
            //Arrange
            validator.Setup(x =>
                x.ValidateAsync(createAssessmentToolWorkflowDto.CreateAssessmentToolWorkflowCommand, CancellationToken.None))
                .ReturnsAsync(validationResult);

            //Act
            var result = await sut.CreateAssessmentToolWorkflow(createAssessmentToolWorkflowDto);

            //Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("LoadAssessmentToolWorkflow", viewResult.ViewName);
            Assert.Equal(validationResult, createAssessmentToolWorkflowDto.ValidationResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateAssessmentTool_ShouldRedirectToErrorPage_GivenAssessmentToolIdNotProvided(
        UpdateAssessmentToolDto updateAssessmentToolDto,
        AdminController sut)
        {
            //Arrange
            updateAssessmentToolDto.UpdateAssessmentToolCommand.Id = 0;

            //Act
            var result = await sut.UpdateAssessmentTool(updateAssessmentToolDto);

            //Assert           
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Bad request. No Assessment Tool Id provided.", redirectToActionResult.RouteValues!["message"]);
        }


        [Theory]
        [AutoMoqData]
        public async Task UpdateAssessmentTool_ShouldRedirectToAssessmentTool_GivenValidationResultIsValid(
        [Frozen] Mock<IValidator<UpdateAssessmentToolCommand>> validator,
        UpdateAssessmentToolDto updateAssessmentToolDto,
        ValidationResult validationResult,
        AdminController sut)
        {
            //Arrange
            validationResult.Errors = new List<ValidationFailure>();
            validator.Setup(x =>
                x.ValidateAsync(updateAssessmentToolDto.UpdateAssessmentToolCommand, CancellationToken.None))
                .ReturnsAsync(validationResult);

            //Act
            var result = await sut.UpdateAssessmentTool(updateAssessmentToolDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("AssessmentTool", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateAssessmentTool_ShouldReturnLoadAssessmentToolViewWithValidationResultWithValidatedAssessmentTool_GivenValidationResultIsInvalid(
        [Frozen] Mock<IValidator<UpdateAssessmentToolCommand>> validator,
        [Frozen] Mock<IMediator> mediator,
        UpdateAssessmentToolDto updateAssessmentToolDto,
        ValidationResult validationResult,
        AssessmentToolListData assessmentToolListData,
        AdminController sut)
        {
            //Arrange
            validator.Setup(x =>
                x.ValidateAsync(updateAssessmentToolDto.UpdateAssessmentToolCommand, CancellationToken.None))
                .ReturnsAsync(validationResult);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentToolQuery>(), CancellationToken.None))
                .ReturnsAsync(assessmentToolListData);

            //Act
            var result = await sut.UpdateAssessmentTool(updateAssessmentToolDto);

            //Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("AssessmentTool", viewResult.ViewName);
            var model = (AssessmentToolListData)viewResult.Model!;
            Assert.Equal(assessmentToolListData, model);
            Assert.DoesNotContain(validationResult,
                assessmentToolListData.AssessmentTools.Select(x => x.ValidationResult));
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateAssessmentTool_ShouldReturnLoadAssessmentToolViewWithValidationResultWithoutValidatedAssessmentTool_GivenValidationResultIsInvalid(
        [Frozen] Mock<IValidator<UpdateAssessmentToolCommand>> validator,
        [Frozen] Mock<IMediator> mediator,
        UpdateAssessmentToolDto updateAssessmentToolDto,
        ValidationResult validationResult,
        AssessmentToolListData assessmentToolListData,
        AdminController sut)
        {
            //Arrange
            var assessmentToolDto = new AssessmentToolDto
            {
                Id = updateAssessmentToolDto.UpdateAssessmentToolCommand.Id

            };
            assessmentToolListData.AssessmentTools.Add(assessmentToolDto);
            validator.Setup(x =>
                    x.ValidateAsync(updateAssessmentToolDto.UpdateAssessmentToolCommand, CancellationToken.None))
                .ReturnsAsync(validationResult);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentToolQuery>(), CancellationToken.None))
                .ReturnsAsync(assessmentToolListData);

            //Act
            var result = await sut.UpdateAssessmentTool(updateAssessmentToolDto);

            //Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("AssessmentTool", viewResult.ViewName);
            var model = (AssessmentToolListData)viewResult.Model!;
            Assert.Equal(assessmentToolListData, model);
            Assert.Equal(validationResult, assessmentToolDto.ValidationResult);
            Assert.Equal(updateAssessmentToolDto.UpdateAssessmentToolCommand.Name, assessmentToolDto.Name);
            Assert.Equal(updateAssessmentToolDto.UpdateAssessmentToolCommand.Order, assessmentToolDto.Order);
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateAssessmentToolWorkflow_ShouldRedirectToErrorPage_GivenAssessmentToolWorkflowIdNotProvided(
        UpdateAssessmentToolWorkflowDto updateAssessmentToolWorkflowDto,
        AdminController sut)
        {
            //Arrange
            updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.Id = 0;

            //Act
            var result = await sut.UpdateAssessmentToolWorkflow(updateAssessmentToolWorkflowDto);

            //Assert           
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Bad request. No Assessment Tool Workflow Id provided.", redirectToActionResult.RouteValues!["message"]);
        }


        [Theory]
        [AutoMoqData]
        public async Task UpdateAssessmentToolWorkflow_ShouldRedirectToAssessmentToolWorkflow_GivenValidationResultIsValid(
        [Frozen] Mock<IValidator<UpdateAssessmentToolWorkflowCommand>> validator,
        UpdateAssessmentToolWorkflowDto updateAssessmentToolWorkflowDto,
        ValidationResult validationResult,
        AdminController sut)
        {
            //Arrange
            validationResult.Errors = new List<ValidationFailure>();
            validator.Setup(x =>
                x.ValidateAsync(updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand, CancellationToken.None))
                .ReturnsAsync(validationResult);

            //Act
            var result = await sut.UpdateAssessmentToolWorkflow(updateAssessmentToolWorkflowDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("AssessmentToolWorkflow", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateAssessmentToolWorkflow_ShouldReturnLoadAssessmentToolWorkflowViewWithValidationResultWithValidatedAssessmentToolWorkflow_GivenValidationResultIsInvalid(
        [Frozen] Mock<IValidator<UpdateAssessmentToolWorkflowCommand>> validator,
        [Frozen] Mock<IMediator> mediator,
        UpdateAssessmentToolWorkflowDto updateAssessmentToolWorkflowDto,
        ValidationResult validationResult,
        AssessmentToolWorkflowListDto assessmentToolWorkflowListDto,
        AdminController sut)
        {
            //Arrange
            validator.Setup(x =>
                x.ValidateAsync(updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand, CancellationToken.None))
                .ReturnsAsync(validationResult);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentToolWorkflowQuery>(), CancellationToken.None))
                .ReturnsAsync(assessmentToolWorkflowListDto);

            //Act
            var result = await sut.UpdateAssessmentToolWorkflow(updateAssessmentToolWorkflowDto);

            //Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("AssessmentToolWorkflow", viewResult.ViewName);
            var model = (AssessmentToolWorkflowListDto)viewResult.Model!;
            Assert.Equal(assessmentToolWorkflowListDto, model);
            Assert.DoesNotContain(validationResult,
                assessmentToolWorkflowListDto.AssessmentToolWorkflowDtos.Select(x => x.ValidationResult));
        }

        [Theory]
        [AutoMoqData]
        public async Task UpdateAssessmentTool_ShouldReturnLoadAssessmentToolWorkflowViewWithValidationResultWithoutValidatedAssessmentToolWorkflow_GivenValidationResultIsInvalid(
        [Frozen] Mock<IValidator<UpdateAssessmentToolWorkflowCommand>> validator,
        [Frozen] Mock<IMediator> mediator,
        UpdateAssessmentToolWorkflowDto updateAssessmentToolWorkflowDto,
        ValidationResult validationResult,
        AssessmentToolWorkflowListDto assessmentToolWorkflowListDto,
        AdminController sut)
        {
            //Arrange
            var assessmentToolWorkflowDto = new UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto
            {
                Id = updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.Id

            };
            assessmentToolWorkflowListDto.AssessmentToolWorkflowDtos.Add(assessmentToolWorkflowDto);
            validator.Setup(x =>
                x.ValidateAsync(updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand, CancellationToken.None))
                .ReturnsAsync(validationResult);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentToolWorkflowQuery>(), CancellationToken.None))
                .ReturnsAsync(assessmentToolWorkflowListDto);

            //Act
            var result = await sut.UpdateAssessmentToolWorkflow(updateAssessmentToolWorkflowDto);

            //Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("AssessmentToolWorkflow", viewResult.ViewName);
            var model = (AssessmentToolWorkflowListDto)viewResult.Model!;
            Assert.Equal(assessmentToolWorkflowListDto, model);
            Assert.Equal(validationResult, assessmentToolWorkflowDto.ValidationResult);
            Assert.Equal(updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.Name, assessmentToolWorkflowDto.Name);
            Assert.Equal(updateAssessmentToolWorkflowDto.UpdateAssessmentToolWorkflowCommand.WorkflowDefinitionId, assessmentToolWorkflowDto.WorkflowDefinitionId);
        }
    }
}
