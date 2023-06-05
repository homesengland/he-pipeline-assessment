﻿using FluentValidation;
using FluentValidation.Results;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention;
public class OverrideControllerTests
{
    private readonly Mock<ILogger<OverrideController>> _loggerMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IValidator<AssessmentInterventionCommand>> _validatorMock;

    public OverrideControllerTests()
    {
        _loggerMock = new Mock<ILogger<OverrideController>>();
        _mediatorMock = new Mock<IMediator>();
        _validatorMock = new Mock<IValidator<AssessmentInterventionCommand>>();
    }

    [Theory]
    [AutoMoqData]
    public async Task Override_ShouldRedirectToView_WhenGivenNoExceptionsThrow(
        AssessmentInterventionDto assessmentInterventionDto, string workflowInstanceId)
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOverrideRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.Override(workflowInstanceId);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ViewResult>(actionResult);
    }

    [Theory]
    [AutoMoqData]
    public async Task CreateOverride_ShouldRedirectToView_WhenGivenValidationResultIsValid(
     ValidationResult validationResult
    )
    {
        //Arrange
        validationResult.Errors = new List<ValidationFailure>();

        var assessmentInterventionDto = new AssessmentInterventionDto { ValidationResult = null };

        var serializedCommand = JsonConvert.SerializeObject(assessmentInterventionDto.AssessmentInterventionCommand);
        var createOverrideCommand = JsonConvert.DeserializeObject<CreateOverrideCommand>(serializedCommand);

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateOverrideCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.CreateOverride(assessmentInterventionDto);

        //Assert
        Assert.IsType<RedirectToActionResult>(actionResult);
        var redirectToActionResult = (RedirectToActionResult)actionResult;
        Assert.Equal("CheckYourDetails", redirectToActionResult.ActionName);
        Assert.Null(assessmentInterventionDto.ValidationResult);

    }

    //[Theory]
    //[AutoMoqData]
    //public async Task CreateOverride_ShouldRedirectToView_WhenInterventionIdIsGreaterThanZero(
    //     int interventionId,
    //    ValidationResult validationResult
    //   )
    //{
    //    //Arrange
    //    validationResult.Errors = new List<ValidationFailure>();

    //    var assessmentInterventionDto = new AssessmentInterventionDto { ValidationResult = null };

    //    var serializedCommand = JsonConvert.SerializeObject(assessmentInterventionDto.AssessmentInterventionCommand);
    //    var createOverrideCommand = JsonConvert.DeserializeObject<CreateOverrideCommand>(serializedCommand);

    //    _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateOverrideCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

    //    _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOverrideCommand>(), CancellationToken.None)).ReturnsAsync(interventionId);

    //    //Act
    //    var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
    //    var actionResult = await overrideController.CreateOverride(assessmentInterventionDto);

    //    //Assert
    //    Assert.IsType<RedirectToActionResult>(actionResult);
    //    var redirectToActionResult = (RedirectToActionResult)actionResult;
    //    Assert.Equal("Error", redirectToActionResult.ControllerName);
    //    Assert.Equal("Index", redirectToActionResult.ActionName);
    //}

    [Theory]
    [AutoMoqData]
    public async Task CreateOverride_ShouldRedirectToView_WhenGivenValidationResultIsInvalid(
      AssessmentInterventionDto assessmentInterventionDto,
      ValidationResult validationResult
    )
    {
        //Arrange
        var serializedCommand = JsonConvert.SerializeObject(assessmentInterventionDto.AssessmentInterventionCommand);
        var createOverrideCommand = JsonConvert.DeserializeObject<CreateOverrideCommand>(serializedCommand);

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateOverrideCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.CreateOverride(assessmentInterventionDto);

        //Assert
        Assert.IsType<ViewResult>(actionResult);
        var viewResult = (ViewResult)actionResult;
        Assert.Equal("~/Features/Intervention/Views/Override.cshtml", viewResult.ViewName);
        Assert.Equal(validationResult, assessmentInterventionDto.ValidationResult);

    }

    [Theory]
    [AutoMoqData]
    public async Task CreateOverride_ShouldRedirectToView_WhenGivenFakeDynamicResultISNull(
    AssessmentInterventionDto assessmentInterventionDto
    )
    {
        //Arrange
        assessmentInterventionDto.AssessmentInterventionCommand = null!;

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.CreateOverride(assessmentInterventionDto);

        //Assert
        Assert.IsType<ViewResult>(actionResult);
        var viewResult = (ViewResult)actionResult;
        Assert.Equal("~/Features/Intervention/Views/Override.cshtml", viewResult.ViewName);
    }

    [Theory]
    [AutoMoqData]
    public async Task CreateOverride_ShouldRedirectToView_WhenGivenFakeDynamicResult(
        int interventionId,
        AssessmentInterventionDto createAssessmentInterventionDto
        )
    {
        // Arrange

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOverrideCommand>(), CancellationToken.None)).ReturnsAsync(interventionId);

        // Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.CreateOverride(createAssessmentInterventionDto);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<RedirectToActionResult>(actionResult);
    }

    [Fact]
    public async Task CreateOverride_ShouldRedirectOverridView_AfterThrowingException()
    {
        // Arrange
        AssessmentInterventionDto createAssessmentInterventionDto = new AssessmentInterventionDto();

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOverrideCommand>(), CancellationToken.None)).Throws(new Exception());

        // Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.CreateOverride(createAssessmentInterventionDto);

        // Assert
        Assert.IsType<RedirectToActionResult>(actionResult);
        var redirectToActionResult = (RedirectToActionResult)actionResult;
        Assert.Equal("Error", redirectToActionResult.ControllerName);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Theory]
    [AutoMoqData]
    public async Task EditOverride_ShouldRedirectToView_WhenGivenStatusNotSubmitted(
     int interventionId,
     AssessmentInterventionDto assessmentInterventionDto
     )
        {
        //Arrange
        assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.NotSubmitted;

        _mediatorMock.Setup(x => x.Send(It.IsAny<EditOverrideRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.EditOverride(interventionId);

        //Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ViewResult>(actionResult);
        Assert.Equal(InterventionStatus.NotSubmitted, assessmentInterventionDto.AssessmentInterventionCommand.Status);

    }

    [Theory]
    [AutoMoqData]
    public async Task EditOverride_ShouldRedirectToView_WhenGivenStatusOtherThanNotSubmitted(
     int interventionId,
     AssessmentInterventionDto assessmentInterventionDto
     )
    {
        // Arrange
        assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Pending;

        _mediatorMock.Setup(x => x.Send(It.IsAny<EditOverrideRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

        // Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.EditOverride(interventionId);

        // Assert
        Assert.IsType<RedirectToActionResult>(actionResult);
        var redirectToActionResult = (RedirectToActionResult)actionResult;
        Assert.Equal("CheckYourDetails", redirectToActionResult.ActionName);

    }

    [Theory]
    [AutoMoqData]
    public async Task EditOverride_ShouldRedirectToView_AfterThrowingException(int interventionId)
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<EditOverrideRequest>(), CancellationToken.None)).Throws(new Exception());

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.EditOverride(interventionId);

        // Assert
        Assert.IsType<RedirectToActionResult>(actionResult);
        var redirectToActionResult = (RedirectToActionResult)actionResult;
        Assert.Equal("Error", redirectToActionResult.ControllerName);
        Assert.Equal("Index", redirectToActionResult.ActionName);

    }

    [Theory]
    [AutoMoqData]
    public async Task EditOverride_ShouldRedirectToView_WhenGivenValidationResultIsValidAndStatusIsPending(
      int interventionId,
      AssessmentInterventionDto assessmentInterventionDto,
      ValidationResult validationResult
    )
    {
        //Arrange
        assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Pending;
        validationResult.Errors = new List<ValidationFailure>();

        assessmentInterventionDto.ValidationResult = null;

        var serializedCommand = JsonConvert.SerializeObject(assessmentInterventionDto.AssessmentInterventionCommand);
        var editOverrideCommand = JsonConvert.DeserializeObject<EditOverrideCommand>(serializedCommand);

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditOverrideCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

        _mediatorMock.Setup(x => x.Send(It.IsAny<EditOverrideCommand>(), CancellationToken.None)).ReturnsAsync(interventionId);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.EditOverride(assessmentInterventionDto);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ViewResult>(actionResult);
        var viewResult = (ViewResult)actionResult;
        Assert.Equal("~/Features/Intervention/Views/EditOverride.cshtml", viewResult.ViewName);
    }

    [Theory]
    [AutoMoqData]
    public async Task EditOverride_ShouldRedirectToView_WhenGivenIsNUull(
      AssessmentInterventionDto assessmentInterventionDto
    )
    {
        //Arrange
        assessmentInterventionDto.AssessmentInterventionCommand = null!;

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.EditOverride(assessmentInterventionDto);

        // Assert
        Assert.IsType<ViewResult>(actionResult);
        var viewResult = (ViewResult)actionResult;
        Assert.Equal("~/Features/Intervention/Views/EditOverride.cshtml", viewResult.ViewName);

    }

    [Theory]
    [AutoMoqData]
    public async Task EditOverride_ShouldRedirectToView_WhenInterventionIdIsLessOrEqualToZero(
      int interventionId,
      AssessmentInterventionDto assessmentInterventionDto,
      ValidationResult validationResult
    )
    {
        //Arrange
        interventionId = 0;
        validationResult.Errors = new List<ValidationFailure>();

        assessmentInterventionDto.ValidationResult = null;

        var serializedCommand = JsonConvert.SerializeObject(assessmentInterventionDto.AssessmentInterventionCommand);
        var editOverrideCommand = JsonConvert.DeserializeObject<EditOverrideCommand>(serializedCommand);

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditOverrideCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

        _mediatorMock.Setup(x => x.Send(It.IsAny<EditOverrideCommand>(), CancellationToken.None)).ReturnsAsync(interventionId);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.EditOverride(assessmentInterventionDto);

        // Assert
        Assert.IsType<ViewResult>(actionResult);
        var viewResult = (ViewResult)actionResult;
        Assert.Equal("~/Features/Intervention/Views/EditOverride.cshtml", viewResult.ViewName);
    }

    [Theory]
    [AutoMoqData]
    public async Task EditOverride_ShouldRedirectToView_WhenGivenValidationResultIsInValid(
      AssessmentInterventionDto assessmentInterventionDto,
      ValidationResult validationResult
    )
    {
        //Arrang
        var serializedCommand = JsonConvert.SerializeObject(assessmentInterventionDto.AssessmentInterventionCommand);
        var editOverrideCommand = JsonConvert.DeserializeObject<EditOverrideCommand>(serializedCommand);

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditOverrideCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.EditOverride(assessmentInterventionDto);

        // Assert
        Assert.IsType<ViewResult>(actionResult);
        var viewResult = (ViewResult)actionResult;
        Assert.Equal("~/Features/Intervention/Views/EditOverride.cshtml", viewResult.ViewName);
        Assert.Equal(validationResult, assessmentInterventionDto.ValidationResult);

    }

    [Theory]
    [AutoMoqData]
    public async Task EditOverride_ShouldRedirectToView_AfterThrowException(AssessmentInterventionDto assessmentInterventionDto)
    {
        //Arrang
        var serializedCommand = JsonConvert.SerializeObject(assessmentInterventionDto.AssessmentInterventionCommand);
        var editOverrideCommand = JsonConvert.DeserializeObject<EditOverrideCommand>(serializedCommand);

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditOverrideCommand>(), CancellationToken.None)).ThrowsAsync(new Exception());

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.EditOverride(assessmentInterventionDto);

        // Assert
        Assert.IsType<RedirectToActionResult>(actionResult);
        var redirectToActionResult = (RedirectToActionResult)actionResult;
        Assert.Equal("Error", redirectToActionResult.ControllerName);
        Assert.Equal("Index", redirectToActionResult.ActionName);

    }


    [Theory]
    [AutoMoqData]
    public async Task CheckYourDetails_ShouldRedirectToView_WhenGivenFakeDynamicResult(
        int interventionId,
        SubmitOverrideCommand submitOverrideCommand
        
        )
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<LoadOverrideCheckYourAnswersRequest>(), CancellationToken.None)).ReturnsAsync(submitOverrideCommand);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.CheckYourDetails(interventionId);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<ViewResult>(actionResult);

    }

    [Theory]
    [AutoMoqData]
    public async Task CheckYourDetails_ShouldRedirectToErrorView_WhenRequestThrowsException(
      int interventionId,
      NotFoundException exception
      )
    {
        //Arrange

        _mediatorMock.Setup(x => x.Send(It.IsAny<LoadOverrideCheckYourAnswersRequest>(), CancellationToken.None)).ThrowsAsync(exception);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.CheckYourDetails(interventionId);

        // Assert
        Assert.IsType<RedirectToActionResult>(actionResult);
        var redirectToActionResult = (RedirectToActionResult)actionResult;
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal("Error", redirectToActionResult.ControllerName);

    }

    [Theory]
    [AutoMoqData]
    public async Task CheckYourDetails_ShouldRedirectToView_AfterThrowingException(int interventionId)
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<LoadOverrideCheckYourAnswersRequest>(), CancellationToken.None)).ThrowsAsync(new Exception());


        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.CheckYourDetails(interventionId);

        // Assert
        Assert.IsType<RedirectToActionResult>(actionResult);
        var redirectToActionResult = (RedirectToActionResult)actionResult;
        Assert.Equal("Error", redirectToActionResult.ControllerName);
        Assert.Equal("Index", redirectToActionResult.ActionName);

    }


    [Theory]
    [AutoMoqData]
    public async Task SubmitOverride_ShouldRedirectToView_WhenDefaultStatusIsGiven(
      SubmitOverrideCommand submitOverrideCommand, 
      string submitButton
      )
    {
        //Arrange
        Unit unit;
        _mediatorMock.Setup(x => x.Send(submitOverrideCommand, CancellationToken.None)).ReturnsAsync(unit);


        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.SubmitOverride(submitOverrideCommand, submitButton);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<RedirectToActionResult>(actionResult);

    }

    [Theory]
    [AutoMoqData]
    public async Task SubmitOverride_ShouldRedirectToView_WhenGivenStatusIsAprroved(
     SubmitOverrideCommand submitOverrideCommand
     )
    {
        //Arrange
        Unit unit;
        string submitButton = "Submit";
        //submitOverrideCommand.Status = InterventionStatus.Approved;

        _mediatorMock.Setup(x => x.Send(submitOverrideCommand, CancellationToken.None)).ReturnsAsync(unit);


        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.SubmitOverride(submitOverrideCommand, submitButton);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<RedirectToActionResult>(actionResult);

    }

    [Theory]
    [AutoMoqData]
    public async Task SubmitOverride_ShouldRedirectToView_WhenGivenStatusRejected(
     SubmitOverrideCommand submitOverrideCommand
     )
    {
        //Arrange
        Unit unit;
        string submitButton = "Reject";
 
                _mediatorMock.Setup(x => x.Send(submitOverrideCommand, CancellationToken.None)).ReturnsAsync(unit);

        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.SubmitOverride(submitOverrideCommand, submitButton);

        // Assert
        Assert.NotNull(actionResult);
        Assert.IsType<RedirectToActionResult>(actionResult);

    }


    [Theory]
    [AutoMoqData]
    public async Task SubmitOverride_ShouldRedirectToView_AfterThrowingException(
      SubmitOverrideCommand submitOverrideCommand,
      string submitButton
        )
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(submitOverrideCommand, CancellationToken.None)).Throws(new Exception());

        //Act
        //Act
        var overrideController = new OverrideController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
        var actionResult = await overrideController.SubmitOverride(submitOverrideCommand, submitButton);


        // Assert
        Assert.IsType<RedirectToActionResult>(actionResult);
        var redirectToActionResult = (RedirectToActionResult)actionResult;
        Assert.Equal("Error", redirectToActionResult.ControllerName);
        Assert.Equal("Index", redirectToActionResult.ActionName);

    }

}
