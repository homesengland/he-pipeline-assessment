using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist;
using He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.AddPermission;
using He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.RemovePermission;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Assessment.SensitiveRecordPermissionsWhitelist
{
    public class SensitiveRecordPermissionsWhitelistControllerTests
    {
        private static void SetupTempData(SensitiveRecordPermissionsWhitelistController controller)
        {
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;
        }

        #region Add Permission Tests

        [Theory]
        [AutoMoqData]
        public async Task Add_ReturnsRedirectToAction_WhenPermissionAddedSuccessfully(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int assessmentId,
            int correlationId,
            string email,
            string username,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns(username);
            userProvider.Setup(x => x.IsAdmin()).Returns(true);

            var successResponse = new AddSensitiveRecordPermissionResponse
            {
                IsSuccess = true,
                SuccessMessage = $"Permission for {email} added successfully"
            };

            mediator.Setup(x => x.Send(
                It.IsAny<AddSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(successResponse);

            // Act
            var result = await sut.Add(assessmentId, correlationId, email);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectResult.ActionName);
            Assert.Equal("Assessment", redirectResult.ControllerName);
            Assert.Equal("permissions", redirectResult.Fragment);
            Assert.Equal(successResponse.SuccessMessage, sut.TempData["SuccessMessage"]);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_SetsValidationErrors_WhenEmailValidationFails(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int assessmentId,
            int correlationId,
            string email,
            string username,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns(username);
            userProvider.Setup(x => x.IsAdmin()).Returns(false);

            var failureResponse = new AddSensitiveRecordPermissionResponse
            {
                IsSuccess = false,
                ErrorMessage = "Invalid email address format",
                ValidationMessage = "Enter an email address in the correct format such as name@homesengland.gov.uk",
                EmailValue = email
            };

            mediator.Setup(x => x.Send(
                It.IsAny<AddSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(failureResponse);

            // Act
            var result = await sut.Add(assessmentId, correlationId, email);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(failureResponse.ErrorMessage, sut.TempData["ErrorMessage"]);
            Assert.Equal(failureResponse.ValidationMessage, sut.TempData["EmailValidationError"]);
            Assert.Equal(failureResponse.EmailValue, sut.TempData["EmailValue"]);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_SetsOnlyErrorMessage_WhenValidationMessageIsNull(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int assessmentId,
            int correlationId,
            string email,
            string username,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns(username);
            userProvider.Setup(x => x.IsAdmin()).Returns(true);

            var failureResponse = new AddSensitiveRecordPermissionResponse
            {
                IsSuccess = false,
                ErrorMessage = "Failed to add permission: Database error",
                ValidationMessage = null,
                EmailValue = null
            };

            mediator.Setup(x => x.Send(
                It.IsAny<AddSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(failureResponse);

            // Act
            var result = await sut.Add(assessmentId, correlationId, email);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(failureResponse.ErrorMessage, sut.TempData["ErrorMessage"]);
            Assert.False(sut.TempData.ContainsKey("EmailValidationError"));
            Assert.False(sut.TempData.ContainsKey("EmailValue"));
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_PassesCorrectCommandToMediator(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int assessmentId,
            int correlationId,
            string email,
            string username,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns(username);
            userProvider.Setup(x => x.IsAdmin()).Returns(false);

            var successResponse = new AddSensitiveRecordPermissionResponse
            {
                IsSuccess = true,
                SuccessMessage = "Success"
            };

            mediator.Setup(x => x.Send(
                It.IsAny<AddSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(successResponse);

            // Act
            await sut.Add(assessmentId, correlationId, email);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<AddSensitiveRecordPermissionCommand>(cmd =>
                    cmd.AssessmentId == assessmentId &&
                    cmd.Email == email &&
                    cmd.CurrentUsername == username &&
                    cmd.IsAdmin == false),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_HandlesNullEmail_ByPassingEmptyString(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int assessmentId,
            int correlationId,
            string username,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns(username);
            userProvider.Setup(x => x.IsAdmin()).Returns(true);

            var failureResponse = new AddSensitiveRecordPermissionResponse
            {
                IsSuccess = false,
                ErrorMessage = "Enter an email address"
            };

            mediator.Setup(x => x.Send(
                It.IsAny<AddSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(failureResponse);

            // Act
            await sut.Add(assessmentId, correlationId, null!);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<AddSensitiveRecordPermissionCommand>(cmd =>
                    cmd.Email == string.Empty),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_HandlesNullUsername_ByPassingEmptyString(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int assessmentId,
            int correlationId,
            string email,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns((string?)null);
            userProvider.Setup(x => x.IsAdmin()).Returns(false);

            var successResponse = new AddSensitiveRecordPermissionResponse
            {
                IsSuccess = true,
                SuccessMessage = "Success"
            };

            mediator.Setup(x => x.Send(
                It.IsAny<AddSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(successResponse);

            // Act
            await sut.Add(assessmentId, correlationId, email);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<AddSensitiveRecordPermissionCommand>(cmd =>
                    cmd.CurrentUsername == string.Empty),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Remove Permission Tests

        [Theory]
        [AutoMoqData]
        public async Task Remove_ReturnsRedirectToAction_WhenPermissionRemovedSuccessfully(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int id,
            int assessmentId,
            int correlationId,
            string username,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns(username);
            userProvider.Setup(x => x.IsAdmin()).Returns(true);

            var successResponse = new RemoveSensitiveRecordPermissionResponse
            {
                IsSuccess = true,
                SuccessMessage = "Permission removed successfully."
            };

            mediator.Setup(x => x.Send(
                It.IsAny<RemoveSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(successResponse);

            // Act
            var result = await sut.Remove(id, assessmentId, correlationId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectResult.ActionName);
            Assert.Equal("Assessment", redirectResult.ControllerName);
            Assert.Equal("permissions", redirectResult.Fragment);
            Assert.Equal(successResponse.SuccessMessage, sut.TempData["SuccessMessage"]);
        }

        [Theory]
        [AutoMoqData]
        public async Task Remove_SetsErrorMessage_WhenRemovalFails(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int id,
            int assessmentId,
            int correlationId,
            string username,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns(username);
            userProvider.Setup(x => x.IsAdmin()).Returns(false);

            var failureResponse = new RemoveSensitiveRecordPermissionResponse
            {
                IsSuccess = false,
                ErrorMessage = "Permission not found."
            };

            mediator.Setup(x => x.Send(
                It.IsAny<RemoveSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(failureResponse);

            // Act
            var result = await sut.Remove(id, assessmentId, correlationId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(failureResponse.ErrorMessage, sut.TempData["ErrorMessage"]);
            Assert.False(sut.TempData.ContainsKey("SuccessMessage"));
        }

        [Theory]
        [AutoMoqData]
        public async Task Remove_PassesCorrectCommandToMediator(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int id,
            int assessmentId,
            int correlationId,
            string username,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns(username);
            userProvider.Setup(x => x.IsAdmin()).Returns(true);

            var successResponse = new RemoveSensitiveRecordPermissionResponse
            {
                IsSuccess = true,
                SuccessMessage = "Success"
            };

            mediator.Setup(x => x.Send(
                It.IsAny<RemoveSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(successResponse);

            // Act
            await sut.Remove(id, assessmentId, correlationId);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<RemoveSensitiveRecordPermissionCommand>(cmd =>
                    cmd.Id == id &&
                    cmd.AssessmentId == assessmentId &&
                    cmd.CurrentUsername == username &&
                    cmd.IsAdmin == true),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Remove_HandlesNullUsername_ByPassingEmptyString(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            int id,
            int assessmentId,
            int correlationId,
            SensitiveRecordPermissionsWhitelistController sut)
        {
            // Arrange
            SetupTempData(sut);
            userProvider.Setup(x => x.UserName()).Returns((string?)null);
            userProvider.Setup(x => x.IsAdmin()).Returns(false);

            var successResponse = new RemoveSensitiveRecordPermissionResponse
            {
                IsSuccess = true,
                SuccessMessage = "Success"
            };

            mediator.Setup(x => x.Send(
                It.IsAny<RemoveSensitiveRecordPermissionCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(successResponse);

            // Act
            await sut.Remove(id, assessmentId, correlationId);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<RemoveSensitiveRecordPermissionCommand>(cmd =>
                    cmd.CurrentUsername == string.Empty),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}