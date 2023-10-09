using AutoFixture.Xunit2;
using MediatR;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.Server.Features.Workflow.QuestionScreenValidateAndSave;
using Elsa.Server.Models;
using FluentValidation;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;

namespace Elsa.Server.Tests.Features.Workflow.QuestionScreenValidateAndSave;

public class QuestionScreenValidateAndSaveCommandHandlerTests
{

    [Theory]
    [AutoMoqData]
    public async Task
    Handle_ShouldReturnSuccessfulOperationResultAndCallAllDependencies_WhenNoValidationErrors(
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ILogger<QuestionScreenValidateAndSaveCommand>> logger,
        [Frozen] Mock<IValidator<WorkflowActivityDataDto>> validator,
        QuestionScreenValidateAndSaveCommand validateAndSaveCommand,
        OperationResult<QuestionScreenSaveAndContinueResponse> response)
    {
        //Arrange
        
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand = validateAndSaveCommand.ToQuestionScreenSaveAndContinueCommand();
        ValidationResult validationResult = new ValidationResult();
        var errorList = validationResult.Errors.Count() > 0 ? validationResult.Errors.Select(x => x.ErrorMessage) : new List<string>();
        response.ValidationMessages = validationResult;
        validator.Setup(x => x.Validate(validateAndSaveCommand)).Returns(validationResult);
        mediator.Setup(x => x.Send(saveAndContinueCommand, CancellationToken.None)).ReturnsAsync(response);

        QuestionScreenValidateAndSaveCommandHandler sut = new QuestionScreenValidateAndSaveCommandHandler(validator.Object, mediator.Object, logger.Object);

        //Act

        var results = await sut.Handle(validateAndSaveCommand, CancellationToken.None);

        //Assert
        validator.Verify(
                x => x.Validate(validateAndSaveCommand),
                Times.Once);
        mediator.Verify(
                x => x.Send(It.IsAny<QuestionScreenSaveAndContinueCommand>(), CancellationToken.None),
                Times.Once);
        Assert.NotNull(results);
        Assert.IsType<OperationResult<QuestionScreenValidateAndSaveResponse>>(results);
        Assert.True(results.IsValid);

    }

    [Theory]
    [AutoMoqData]
    public async Task
    Handle_ShouldReturnUnsuccessfulOperationResultAndNotCallMediator_WhenValidationHasErrors(
    [Frozen] Mock<IMediator> mediator,
    [Frozen] Mock<ILogger<QuestionScreenValidateAndSaveCommand>> logger,
    [Frozen] Mock<IValidator<WorkflowActivityDataDto>> validator,
    QuestionScreenValidateAndSaveCommand validateAndSaveCommand,
    OperationResult<QuestionScreenSaveAndContinueResponse> response,
    List<ValidationFailure> validationFailures)
    {
        //Arrange

        var saveAndContinueCommand = validateAndSaveCommand.ToQuestionScreenSaveAndContinueCommand();
        ValidationResult validationResult = new ValidationResult(validationFailures);
        response.ValidationMessages = validationResult;
        validator.Setup(x => x.Validate(validateAndSaveCommand)).Returns(validationResult);
        mediator.Setup(x => x.Send(saveAndContinueCommand, CancellationToken.None)).ReturnsAsync(response);

        QuestionScreenValidateAndSaveCommandHandler sut = new QuestionScreenValidateAndSaveCommandHandler(validator.Object, mediator.Object, logger.Object);

        //Act

        var results = await sut.Handle(validateAndSaveCommand, CancellationToken.None);

        //Assert
        validator.Verify(
                x => x.Validate(validateAndSaveCommand),
                Times.Once);
        mediator.Verify(
                x => x.Send(saveAndContinueCommand, CancellationToken.None),
                Times.Never);
        Assert.NotNull(results);
        Assert.IsType<OperationResult<QuestionScreenValidateAndSaveResponse>>(results);
        Assert.Equal(validationFailures.Count(), results.ValidationMessages!.Errors.Count());
        Assert.False(results.IsValid);

    }

    [Theory]
    [AutoMoqData]
    public async Task
    Handle_ShouldReturnUnsuccessfulOperationResultCallMediator_WhenValidationHasNoErrorsButMediatorReturnsErrors(
    [Frozen] Mock<IMediator> mediator,
    [Frozen] Mock<ILogger<QuestionScreenValidateAndSaveCommand>> logger,
    [Frozen] Mock<IValidator<WorkflowActivityDataDto>> validator,
    QuestionScreenValidateAndSaveCommand validateAndSaveCommand,
    OperationResult<QuestionScreenSaveAndContinueResponse> response,
    List<ValidationFailure> validationFailures)
    {
        //Arrange
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand = validateAndSaveCommand.ToQuestionScreenSaveAndContinueCommand();
        ValidationResult validationResult = new ValidationResult();
        ValidationResult saveAndContinueValidationResult = new ValidationResult(validationFailures);
        var errorList = validationResult.Errors.Count() > 0 ? validationResult.Errors.Select(x => x.ErrorMessage) : new List<string>();

        response.ValidationMessages = saveAndContinueValidationResult;
        validator.Setup(x => x.Validate(validateAndSaveCommand)).Returns(validationResult);
        mediator.Setup(x => x.Send(It.IsAny<QuestionScreenSaveAndContinueCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        QuestionScreenValidateAndSaveCommandHandler sut = new QuestionScreenValidateAndSaveCommandHandler(validator.Object, mediator.Object, logger.Object);

        //Act

        var results = await sut.Handle(validateAndSaveCommand, CancellationToken.None);

        //Assert
        validator.Verify(
                x => x.Validate(validateAndSaveCommand),
                Times.Once);
        mediator.Verify(
                x => x.Send(It.IsAny<QuestionScreenSaveAndContinueCommand>(), It.IsAny<CancellationToken>()),
                Times.Once);
        Assert.NotNull(results);
        Assert.IsType<OperationResult<QuestionScreenValidateAndSaveResponse>>(results);
        Assert.Equal(saveAndContinueValidationResult.Errors.Count(), results.ValidationMessages!.Errors.Count());
        Assert.False(results.IsValid);

    }
}