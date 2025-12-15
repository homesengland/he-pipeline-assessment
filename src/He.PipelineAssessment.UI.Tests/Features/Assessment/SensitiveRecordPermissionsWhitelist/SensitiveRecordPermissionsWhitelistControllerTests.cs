using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist;
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
        private static void SetupControllerContext(SensitiveRecordPermissionsWhitelistController controller)
        {
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        #region Add Tests

        [Theory]
        [AutoMoqData]
        public async Task Add_ShouldAddPermission_WhenUserIsAdmin(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);
            var email = "test.user@homesengland.gov.uk";

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "other.user@test.com"
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);
            userProvider.Setup(x => x.GetUserName()).Returns("admin.user@test.com");
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(assessmentId))
                .ReturnsAsync(new List<SensitiveRecordWhitelist>());
            assessmentRepository.Setup(x => x.CreateSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()))
                .ReturnsAsync(1);

            // Act
            var result = await sut.Add(assessmentId, correlationId, email);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Contains($"Permission for {email} added successfully", sut.TempData["SuccessMessage"]?.ToString());
            assessmentRepository.Verify(x => x.CreateSensitiveRecordWhitelist(It.Is<SensitiveRecordWhitelist>(
                w => w.AssessmentId == assessmentId && w.Email == email)), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_ShouldAddPermission_WhenUserIsProjectManager(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);
            var email = "test.user@homesengland.gov.uk";
            var currentUsername = "project.manager@test.com";

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = currentUsername
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(false);
            userProvider.Setup(x => x.GetUserName()).Returns(currentUsername);
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(assessmentId))
                .ReturnsAsync(new List<SensitiveRecordWhitelist>());
            assessmentRepository.Setup(x => x.CreateSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()))
                .ReturnsAsync(1);

            // Act
            var result = await sut.Add(assessmentId, correlationId, email);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Contains($"Permission for {email} added successfully", sut.TempData["SuccessMessage"]?.ToString());
            assessmentRepository.Verify(x => x.CreateSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_ShouldRedirectToAccessDenied_WhenUserIsNotAuthorized(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);
            var email = "test.user@homesengland.gov.uk";

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "other.user@test.com"
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(false);
            userProvider.Setup(x => x.GetUserName()).Returns("unauthorized.user@test.com");

            // Act
            var result = await sut.Add(assessmentId, correlationId, email);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AccessDenied", redirectToActionResult.ActionName);
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            assessmentRepository.Verify(x => x.CreateSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_ShouldReturnError_WhenEmailIsEmpty(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "project.manager@test.com"
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);

            // Act
            var result = await sut.Add(assessmentId, correlationId, string.Empty);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Equal("Enter an email address", sut.TempData["ErrorMessage"]);
            Assert.Equal("Enter an email address", sut.TempData["EmailValidationError"]);
            Assert.Equal(string.Empty, sut.TempData["EmailValue"]);
            assessmentRepository.Verify(x => x.CreateSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_ShouldReturnError_WhenEmailFormatIsInvalid(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);

            var invalidEmail = "not-an-email";
            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "project.manager@test.com"
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);

            // Act
            var result = await sut.Add(assessmentId, correlationId, invalidEmail);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Equal("Invalid email address format", sut.TempData["ErrorMessage"]);
            Assert.Equal("Enter an email address in the correct format such as name@homesengland.gov.uk", sut.TempData["EmailValidationError"]);
            Assert.Equal(invalidEmail, sut.TempData["EmailValue"]);
            assessmentRepository.Verify(x => x.CreateSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_ShouldReturnError_WhenEmailAlreadyExists(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);
            var email = "existing.user@homesengland.gov.uk";

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "project.manager@test.com"
                }
            };

            var existingPermissions = new List<SensitiveRecordWhitelist>
            {
                new SensitiveRecordWhitelist { Id = 1, AssessmentId = assessmentId, Email = email }
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(assessmentId))
                .ReturnsAsync(existingPermissions);

            // Act
            var result = await sut.Add(assessmentId, correlationId, email);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Equal($"{email} is already on the permissions list for this assessment", sut.TempData["ErrorMessage"]);
            Assert.Equal($"{email} is already on the permissions list for this assessment", sut.TempData["EmailValidationError"]);
            Assert.Equal(email, sut.TempData["EmailValue"]);
            assessmentRepository.Verify(x => x.CreateSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Add_ShouldHandleException_WhenErrorOccurs(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);
            var email = "test.user@homesengland.gov.uk";

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "project.manager@test.com"
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(assessmentId))
                .ReturnsAsync(new List<SensitiveRecordWhitelist>());
            assessmentRepository.Setup(x => x.CreateSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await sut.Add(assessmentId, correlationId, email);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Contains("Failed to add permission", sut.TempData["ErrorMessage"]?.ToString());
        }

        #endregion

        #region Remove Tests

        [Theory]
        [AutoMoqData]
        public async Task Remove_ShouldRemovePermission_WhenUserIsAdmin(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int id,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "other.user@test.com"
                }
            };

            var whitelist = new SensitiveRecordWhitelist
            {
                Id = id,
                AssessmentId = assessmentId,
                Email = "test@example.com"
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);
            userProvider.Setup(x => x.GetUserName()).Returns("admin.user@test.com");
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelistById(id))
                .ReturnsAsync(whitelist);
            assessmentRepository.Setup(x => x.DeleteSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()))
                .ReturnsAsync(1);

            // Act
            var result = await sut.Remove(id, assessmentId, correlationId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Contains($"Permission for {whitelist.Email} removed successfully", sut.TempData["SuccessMessage"]?.ToString());
            assessmentRepository.Verify(x => x.DeleteSensitiveRecordWhitelist(It.Is<SensitiveRecordWhitelist>(
                w => w.Id == id)), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Remove_ShouldRemovePermission_WhenUserIsProjectManager(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int id,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);

            var currentUsername = "project.manager@test.com";
            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = currentUsername
                }
            };

            var whitelist = new SensitiveRecordWhitelist
            {
                Id = id,
                AssessmentId = assessmentId,
                Email = "test@example.com"
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(false);
            userProvider.Setup(x => x.GetUserName()).Returns(currentUsername);
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelistById(id))
                .ReturnsAsync(whitelist);
            assessmentRepository.Setup(x => x.DeleteSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()))
                .ReturnsAsync(1);

            // Act
            var result = await sut.Remove(id, assessmentId, correlationId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Contains($"Permission for {whitelist.Email} removed successfully", sut.TempData["SuccessMessage"]?.ToString());
            assessmentRepository.Verify(x => x.DeleteSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Remove_ShouldRedirectToAccessDenied_WhenUserIsNotAuthorized(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int id,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "other.user@test.com"
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(false);
            userProvider.Setup(x => x.GetUserName()).Returns("unauthorized.user@test.com");

            // Act
            var result = await sut.Remove(id, assessmentId, correlationId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AccessDenied", redirectToActionResult.ActionName);
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            assessmentRepository.Verify(x => x.DeleteSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Remove_ShouldReturnError_WhenWhitelistEntryNotFound(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int id,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "project.manager@test.com"
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelistById(id))
                .ReturnsAsync((SensitiveRecordWhitelist?)null);

            // Act
            var result = await sut.Remove(id, assessmentId, correlationId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Equal("Permission not found.", sut.TempData["ErrorMessage"]);
            assessmentRepository.Verify(x => x.DeleteSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Remove_ShouldReturnError_WhenWhitelistEntryBelongsToDifferentAssessment(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int id,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "project.manager@test.com"
                }
            };

            var whitelist = new SensitiveRecordWhitelist
            {
                Id = id,
                AssessmentId = assessmentId + 999, // Different assessment ID
                Email = "test@example.com"
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelistById(id))
                .ReturnsAsync(whitelist);

            // Act
            var result = await sut.Remove(id, assessmentId, correlationId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Equal("Invalid permission reference.", sut.TempData["ErrorMessage"]);
            assessmentRepository.Verify(x => x.DeleteSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Remove_ShouldHandleException_WhenErrorOccurs(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            SensitiveRecordPermissionsWhitelistController sut,
            int id,
            int assessmentId,
            int correlationId)
        {
            // Arrange
            SetupControllerContext(sut);

            var response = new SensitiveRecordPermissionsWhitelistResponse
            {
                AssessmentSummary = new AssessmentSummaryResponse
                {
                    ProjectManager = "project.manager@test.com"
                }
            };

            var whitelist = new SensitiveRecordWhitelist
            {
                Id = id,
                AssessmentId = assessmentId,
                Email = "test@example.com"
            };

            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(response);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelistById(id))
                .ReturnsAsync(whitelist);
            assessmentRepository.Setup(x => x.DeleteSensitiveRecordWhitelist(It.IsAny<SensitiveRecordWhitelist>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await sut.Remove(id, assessmentId, correlationId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Assessment", redirectToActionResult.ControllerName);
            Assert.Contains("Failed to remove permission", sut.TempData["ErrorMessage"]?.ToString());
        }

        #endregion
    }
}