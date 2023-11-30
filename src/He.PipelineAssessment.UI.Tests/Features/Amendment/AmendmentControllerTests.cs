using FluentValidation;
using Microsoft.Extensions.Logging;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Amendment;
using He.PipelineAssessment.UI.Features.Amendment.CreateAmendment;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

using Xunit;
using FluentValidation.Results;
using He.PipelineAssessment.UI.Features.Amendment.LoadAmendmentCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Amendment.EditAmendment;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment
{
    public class AmendmentControllerTests
    {
        private readonly Mock<ILogger<AmendmentController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<AssessmentInterventionCommand>> _validatorMock;

        public AmendmentControllerTests()
        {
            _loggerMock = new Mock<ILogger<AmendmentController>>();
            _mediatorMock = new Mock<IMediator>();
            _validatorMock = new Mock<IValidator<AssessmentInterventionCommand>>();
        }

        [Theory]
        [AutoMoqData]
        public async Task Amendment_ShouldRedirectToView(
    AssessmentInterventionDto assessmentInterventionDto, string workflowInstanceId)
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateAmendmentRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

            //Act
            var overrideController = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await overrideController.Amendment(workflowInstanceId);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ViewResult>(actionResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAmendment_ShouldRedirectToView_WhenGivenValidationResultIsValid(
            ValidationResult validationResult
        )
        {
            //Arrange
            validationResult.Errors = new List<ValidationFailure>();

            var assessmentInterventionDto = new AssessmentInterventionDto { ValidationResult = null };

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAmendmentCommand>(), CancellationToken.None))
                .ReturnsAsync(validationResult);

            //Act
            var overrideController = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await overrideController.CreateAmendment(assessmentInterventionDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(actionResult);
            var redirectToActionResult = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetails", redirectToActionResult.ActionName);
            Assert.Null(assessmentInterventionDto.ValidationResult);

        }


        [Theory]
        [AutoMoqData]
        public async Task CreateAmendment_ShouldRedirectToView_WhenGivenValidationResultIsInvalid(
            AssessmentInterventionDto assessmentInterventionDto,
            ValidationResult validationResult
        )
        {
            //Arrange

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAmendmentCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.CreateAmendment(assessmentInterventionDto);

            //Assert
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("Amendment", viewResult.ViewName);
            Assert.Equal(validationResult, assessmentInterventionDto.ValidationResult);

        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAmendment_ShouldRedirectToView_WhenGivenFakeDynamicResultISNull(
    AssessmentInterventionDto assessmentInterventionDto
)
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand = null!;

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.CreateAmendment(assessmentInterventionDto);

            //Assert
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("Amendment", viewResult.ViewName);
        }

        [Theory]
        [AutoMoqData]
        public async Task CheckYourDetails_ShouldRedirectToView(
            int interventionId,
            SubmitAmendmentCommand command
            )
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<LoadAmendmentCheckYourAnswersRequest>(), CancellationToken.None))
                .ReturnsAsync(command);

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.CheckYourDetails(interventionId);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("AmendmentCheckYourDetails", viewResult.ViewName);

        }

        [Theory]
        [AutoMoqData]
        public async Task EditAmendment_ShouldRedirectToView_WhenGivenStatusIsDraft(
    int interventionId,
    AssessmentInterventionDto assessmentInterventionDto
)
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Draft;

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditAmendmentRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditAmendment(interventionId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ViewResult>(actionResult);
            Assert.Equal(InterventionStatus.Draft, assessmentInterventionDto.AssessmentInterventionCommand.Status);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("EditAmendment", viewResult.ViewName);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditAmendment_ShouldRedirectToView_WhenGivenStatusIsApproved(
            int interventionId,
            AssessmentInterventionDto assessmentInterventionDto
        )
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Approved;

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditAmendmentRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditAmendment(interventionId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var result = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetails", result.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditAmendment_ShouldRedirectToCheckYourDetails_GivenValidationResultIsValid(
            int interventionId,
            AssessmentInterventionDto assessmentInterventionDto,
            ValidationResult validationResult
        )
        {
            //Arrange
            validationResult.Errors = new List<ValidationFailure>();

            assessmentInterventionDto.ValidationResult = null;

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditAmendmentCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditAmendmentCommand>(), CancellationToken.None)).ReturnsAsync(interventionId);

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditAmendment(assessmentInterventionDto);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var redirectToActionResult = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetails", redirectToActionResult.ActionName);
            Assert.Null(assessmentInterventionDto.ValidationResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditAmendment_ShouldRedirectToView_WhenGivenValidationResultIsInValid(
            AssessmentInterventionDto assessmentInterventionDto,
            ValidationResult validationResult
        )
        {
            //Arrange
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditAmendmentCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditAmendment(assessmentInterventionDto);

            // Assert
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("EditAmendment", viewResult.ViewName);
            Assert.Equal(validationResult, assessmentInterventionDto.ValidationResult);

        }

        [Theory]
        [AutoMoqData]
        public async Task EditAmendment_ShouldRedirectToError_GivenAssessmentInterventionCommandIsNull(
            AssessmentInterventionDto assessmentInterventionDto
            )
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand = null!;

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditAmendment(assessmentInterventionDto);

            // Assert
            Assert.IsType<RedirectToActionResult>(actionResult);
            var redirectToActionResult = (RedirectToActionResult)actionResult;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [InlineAutoMoqData("Default")]
        [InlineAutoMoqData("Submit")]
        public async Task SubmitAmendment_ShouldRedirectToCheckYourDetails_GivenDefaultSubmitValue(string submitButton,
        SubmitAmendmentCommand submitOverrideCommand
)
        {
            //Arrange

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.SubmitAmendment(submitOverrideCommand, submitButton);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var result = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetails", result.ActionName);
        }


        [Theory]
        [AutoMoqData]
        public async Task SubmitAmendment_ShouldRedirectToCheckYourDetails_GivenCancelSubmitValue(string submitButton,
            SubmitAmendmentCommand submitOverrideCommand
            )
        {
            //Arrange
            submitButton = "Cancel";

            //Act
            var controller = new AmendmentController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.SubmitAmendment(submitOverrideCommand, submitButton);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var result = (RedirectToActionResult)actionResult;
            Assert.Equal("Summary", result.ActionName);
            Assert.Equal("Assessment", result.ControllerName);
        }
    }
}
