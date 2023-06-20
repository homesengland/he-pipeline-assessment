using FluentValidation;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Override;
using He.PipelineAssessment.UI.Features.Override.EditOverride;
using He.PipelineAssessment.UI.Features.Rollback;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor;
using Newtonsoft.Json;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using System.ComponentModel.DataAnnotations;
using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback
{
    public class RollbackControllerTests
    {
        private readonly Mock<ILogger<RollbackController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<AssessmentInterventionCommand>> _validatorMock;

        public RollbackControllerTests()
        {
            _loggerMock = new Mock<ILogger<RollbackController>>();
            _mediatorMock = new Mock<IMediator>();
            _validatorMock = new Mock<IValidator<AssessmentInterventionCommand>>();
        }

        [Theory]
        [AutoMoqData]
        public async Task Rollback_ShouldRedirectToView(
            AssessmentInterventionDto assessmentInterventionDto, string workflowInstanceId)
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateRollbackRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

            //Act
            var overrideController = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await overrideController.Rollback(workflowInstanceId);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ViewResult>(actionResult);
        }


        [Theory]
        [AutoMoqData]
        public async Task CreateRollback_ShouldRedirectToView_WhenGivenValidationResultIsValid(
            ValidationResult validationResult
        )
        {
            //Arrange
            validationResult.Errors = new List<ValidationFailure>();

            var assessmentInterventionDto = new AssessmentInterventionDto { ValidationResult = null };

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateRollbackCommand>(), CancellationToken.None))
                .ReturnsAsync(validationResult);

            //Act
            var overrideController = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await overrideController.CreateRollback(assessmentInterventionDto);

            //Assert
            Assert.IsType<RedirectToActionResult>(actionResult);
            var redirectToActionResult = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetailsAssessor", redirectToActionResult.ActionName);
            Assert.Null(assessmentInterventionDto.ValidationResult);

        }

        [Theory]
        [AutoMoqData]
        public async Task CreateOverride_ShouldRedirectToView_WhenGivenValidationResultIsInvalid(
            AssessmentInterventionDto assessmentInterventionDto,
            ValidationResult validationResult
        )
        {
            //Arrange

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateRollbackCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.CreateRollback(assessmentInterventionDto);

            //Assert
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("Rollback", viewResult.ViewName);
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
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.CreateRollback(assessmentInterventionDto);

            //Assert
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("Rollback", viewResult.ViewName);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollback_ShouldRedirectToView_WhenGivenStatusISPending(
            int interventionId,
            AssessmentInterventionDto assessmentInterventionDto
        )
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Pending;

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditRollbackRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollback(interventionId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ViewResult>(actionResult);
            Assert.Equal(InterventionStatus.Pending, assessmentInterventionDto.AssessmentInterventionCommand.Status);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("EditRollback", viewResult.ViewName);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollback_ShouldRedirectToView_WhenGivenStatusIsApproved(
            int interventionId,
            AssessmentInterventionDto assessmentInterventionDto
        )
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Approved;

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditRollbackRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollback(interventionId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var result = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetails", result.ActionName);

        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollback_ShouldRedirectToView_WhenGivenStatusIsDraft(
            int interventionId,
            AssessmentInterventionDto assessmentInterventionDto
        )
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Draft;

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditRollbackRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollback(interventionId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var result = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetailsAssessor", result.ActionName);

        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollback_ShouldRedirectToCheckYourDetails_GivenValidationResultIsValid(
            int interventionId,
            AssessmentInterventionDto assessmentInterventionDto,
            ValidationResult validationResult
        )
        {
            //Arrange
            validationResult.Errors = new List<ValidationFailure>();

            assessmentInterventionDto.ValidationResult = null;

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditRollbackCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditRollbackCommand>(), CancellationToken.None)).ReturnsAsync(interventionId);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollback(assessmentInterventionDto);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var redirectToActionResult = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetails", redirectToActionResult.ActionName);
            Assert.Null(assessmentInterventionDto.ValidationResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollback_ShouldRedirectToView_WhenGivenValidationResultIsInValid(
            AssessmentInterventionDto assessmentInterventionDto,
            ValidationResult validationResult
        )
        {
            //Arrange
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditRollbackCommand>(), CancellationToken.None)).ReturnsAsync(validationResult);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollback(assessmentInterventionDto);

            // Assert
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("EditRollback", viewResult.ViewName);
            Assert.Equal(validationResult, assessmentInterventionDto.ValidationResult);

        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollback_ShouldRedirectToError_GivenAssessmentInterventionCommandIsNull(
            AssessmentInterventionDto assessmentInterventionDto
        )
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand = null!;

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollback(assessmentInterventionDto);

            // Assert
            Assert.IsType<RedirectToActionResult>(actionResult);
            var redirectToActionResult = (RedirectToActionResult)actionResult;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }


        [Theory]
        [AutoMoqData]
        public async Task EditRollbackAssessor_ShouldRedirectToView_WhenGivenStatusIsDraft(
          int interventionId,
          AssessmentInterventionDto assessmentInterventionDto
      )
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Draft;

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditRollbackAssessorRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollbackAssessor(interventionId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ViewResult>(actionResult);
            var result = (ViewResult)actionResult;
            Assert.Equal("EditRollbackAssessor", result.ViewName);

        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollbackAssessor_ShouldRedirectToCheckYourDetails_WhenGivenStatusIsNotPending(
            int interventionId,
            AssessmentInterventionDto assessmentInterventionDto
        )
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Pending;

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditRollbackAssessorRequest>(), CancellationToken.None)).ReturnsAsync(assessmentInterventionDto);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollbackAssessor(interventionId);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var result = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetailsAssessor", result.ActionName);

        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollbackAssessor_ShouldRedirectToCheckYourDetails_GivenValidationResultIsValid(
            int interventionId,
            AssessmentInterventionDto assessmentInterventionDto,
            ValidationResult validationResult
        )
        {
            //Arrange
            validationResult.Errors = new List<ValidationFailure>();

            assessmentInterventionDto.ValidationResult = null;
            assessmentInterventionDto.AssessmentInterventionCommand.Status = InterventionStatus.Approved;

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditRollbackAssessorCommand>(), CancellationToken.None))
                .ReturnsAsync(validationResult);

            _mediatorMock.Setup(x => x.Send(It.IsAny<EditRollbackAssessorCommand>(), CancellationToken.None)).ReturnsAsync(interventionId);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollbackAssessor(assessmentInterventionDto);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var redirectToActionResult = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetailsAssessor", redirectToActionResult.ActionName);
            Assert.Null(assessmentInterventionDto.ValidationResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollbackAssessor_ShouldRedirectToView_WhenGivenValidationResultIsInValid(
            AssessmentInterventionDto assessmentInterventionDto,
            ValidationResult validationResult
        )
        {
            //Arrange
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<EditRollbackAssessorCommand>(), CancellationToken.None))
                .ReturnsAsync(validationResult);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollbackAssessor(assessmentInterventionDto);

            // Assert
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("EditRollbackAssessor", viewResult.ViewName);
            Assert.Equal(validationResult, assessmentInterventionDto.ValidationResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditRollbackAssessor_ShouldRedirectToError_GivenAssessmentInterventionCommandIsNull(
            AssessmentInterventionDto assessmentInterventionDto
        )
        {
            //Arrange
            assessmentInterventionDto.AssessmentInterventionCommand = null!;

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.EditRollbackAssessor(assessmentInterventionDto);

            // Assert
            Assert.IsType<RedirectToActionResult>(actionResult);
            var redirectToActionResult = (RedirectToActionResult)actionResult;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task CheckYourDetails_ShouldRedirectToView(
            int interventionId,
            ConfirmRollbackCommand command
            )
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<LoadRollbackCheckYourAnswersAssessorRequest>(), CancellationToken.None))
                .ReturnsAsync(command);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.CheckYourDetails(interventionId);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("RollbackCheckYourDetails", viewResult.ViewName);

        }

        [Theory]
        [AutoMoqData]
        public async Task CheckYourDetails_ShouldRedirectToEditRollbackAssessorView_GivenConfirmRollbackCommandIsNotNull(
            int interventionId,
            ConfirmRollbackCommand command
        )
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<LoadRollbackCheckYourAnswersAssessorRequest>(), CancellationToken.None))
                .ReturnsAsync(command);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.CheckYourDetailsAssessor(interventionId);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<ViewResult>(actionResult);
            var viewResult = (ViewResult)actionResult;
            Assert.Equal("RollbackCheckYourDetailsAssessor", viewResult.ViewName);
        }

        [Theory]
        [AutoMoqData]
        public async Task CheckYourDetails_ShouldRedirectToEditRollbackAssessorView_GivenConfirmRollbackCommandIsNull(
            int interventionId
        )
        {
            //Arrange

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.CheckYourDetailsAssessor(interventionId);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var viewResult = (RedirectToActionResult)actionResult;
            Assert.Equal("EditRollbackAssessor", viewResult.ActionName);

        }

        [Theory]
        [InlineAutoMoqData("Default")]
        [InlineAutoMoqData("Submit")]
        [InlineAutoMoqData("Reject")]
        public async Task SubmitRollback_ShouldRedirectToCheckYourDetails_GivenDefaultSubmitValue(string submitButton,
        SubmitRollbackCommand submitOverrideCommand
        )
        {
            //Arrange
            Unit unit;
            _mediatorMock.Setup(x => x.Send(submitOverrideCommand, CancellationToken.None))
                .ReturnsAsync(unit);


            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.SubmitRollback(submitOverrideCommand, submitButton);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var viewResult = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetails", viewResult.ActionName);
        }

        [Theory]
        [InlineAutoMoqData("Default")]
        [InlineAutoMoqData("Submit")]
        public async Task ConfirmRollback_ShouldRedirectToCheckYourDetailsAssessorView_GivenDefaultSubmitValue(string submitButton,
            ConfirmRollbackCommand command
        )
        {
            //Arrange
            Unit unit;
            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None))
                .ReturnsAsync(unit);

            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.ConfirmRollback(command, submitButton);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var viewResult = (RedirectToActionResult)actionResult;
            Assert.Equal("CheckYourDetailsAssessor", viewResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task ConfirmRollback_ShouldRedirectToCheckYourDetailsAssessorView_GivenCancelValue(
            ConfirmRollbackCommand command
        )
        {
            //Arrange
            Unit unit;
            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None))
                .ReturnsAsync(unit);


            //Act
            var controller = new RollbackController(_loggerMock.Object, _mediatorMock.Object, _validatorMock.Object);
            var actionResult = await controller.ConfirmRollback(command, "Cancel");

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<RedirectToActionResult>(actionResult);
            var viewResult = (RedirectToActionResult)actionResult;
            Assert.Equal("Summary", viewResult.ActionName);
            Assert.Equal("Assessment", viewResult.ControllerName);
        }
    }
}
