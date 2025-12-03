using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist;
using He.PipelineAssessment.UI.Features.Assessments;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.UI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using Castle.Core.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace He.PipelineAssessment.UI.Tests.Features.SinglePipeline
{
    public class AssessmentControllerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Index_ShouldRedirectToAction_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            AssessmentListRequest request,
            List<AssessmentDataViewModel> response,
            AssessmentController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);

            //Act
            var result = await sut.Index();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

        }

        [Theory]
        [AutoMoqData]
        public async Task Summary_ShouldRedirectToAction_GivenNoExceptionsThrow(
           [Frozen] Mock<IMediator> mediator,
           [Frozen] Mock<IUserProvider> userProvider,
           AssessmentSummaryResponse summaryResponse,
           AssessmentController sut,
           int correlationId,
           int assessmentId)
        {
            //Arrange
            summaryResponse.Permissions = new List<SensitiveRecordPermissionsWhitelistDto>();

            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), CancellationToken.None))
                .ReturnsAsync(summaryResponse);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(false);
            userProvider.Setup(x => x.GetUserName()).Returns("user@test.com");

            //Act
            var result = await sut.Summary(assessmentId, correlationId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

        }

        [Theory]
        [AutoMoqData]
        public async Task Summary_ShouldReturnView_WhenUserIsNotAdminOrProjectManager(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            AssessmentSummaryResponse summaryResponse,
            AssessmentController sut,
            int correlationId,
            int assessmentId)
        {
            //Arrange
            summaryResponse.ProjectManager = "project.manager@test.com";
            summaryResponse.Permissions = new List<SensitiveRecordPermissionsWhitelistDto>();

            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), CancellationToken.None))
                .ReturnsAsync(summaryResponse);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(false);
            userProvider.Setup(x => x.GetUserName()).Returns("other.user@test.com");

            //Act
            var result = await sut.Summary(assessmentId, correlationId);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Summary", viewResult.ViewName);
            var model = Assert.IsType<AssessmentSummaryResponse>(viewResult.Model);
            Assert.Empty(model.Permissions);
            mediator.Verify(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Summary_ShouldLoadPermissions_WhenUserIsAdmin(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            AssessmentSummaryResponse summaryResponse,
            SensitiveRecordPermissionsWhitelistResponse permissionsResponse,
            AssessmentController sut,
            int correlationId,
            int assessmentId)
        {
            //Arrange
            summaryResponse.ProjectManager = "project.manager@test.com";
            summaryResponse.Permissions = new List<SensitiveRecordPermissionsWhitelistDto>();

            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), CancellationToken.None))
                .ReturnsAsync(summaryResponse);
            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(permissionsResponse);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(true);
            userProvider.Setup(x => x.GetUserName()).Returns("admin.user@test.com");

            //Act
            var result = await sut.Summary(assessmentId, correlationId);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Summary", viewResult.ViewName);
            var model = Assert.IsType<AssessmentSummaryResponse>(viewResult.Model);
            Assert.Equal(permissionsResponse.Permissions, model.Permissions);
            mediator.Verify(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Summary_ShouldLoadPermissions_WhenUserIsProjectManager(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IUserProvider> userProvider,
            AssessmentSummaryResponse summaryResponse,
            SensitiveRecordPermissionsWhitelistResponse permissionsResponse,
            AssessmentController sut,
            int correlationId,
            int assessmentId)
        {
            //Arrange
            var currentUsername = "project.manager@test.com";
            summaryResponse.ProjectManager = currentUsername;
            summaryResponse.Permissions = new List<SensitiveRecordPermissionsWhitelistDto>();

            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), CancellationToken.None))
                .ReturnsAsync(summaryResponse);
            mediator.Setup(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None))
                .ReturnsAsync(permissionsResponse);
            userProvider.Setup(x => x.CheckUserRole(Constants.AppRole.PipelineAdminOperations)).Returns(false);
            userProvider.Setup(x => x.GetUserName()).Returns(currentUsername);

            //Act
            var result = await sut.Summary(assessmentId, correlationId);

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Summary", viewResult.ViewName);
            var model = Assert.IsType<AssessmentSummaryResponse>(viewResult.Model);
            Assert.Equal(permissionsResponse.Permissions, model.Permissions);
            mediator.Verify(x => x.Send(It.IsAny<SensitiveRecordPermissionsWhitelistRequest>(), CancellationToken.None), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task TestSummary_ShouldRedirectToSummary_EnableTestSummaryPageIsFalse(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IConfiguration> configuration,
            AssessmentListRequest request,
            List<AssessmentDataViewModel> response,
            AssessmentController sut,
            int correlationId,
            int assessmentId)
        {
            //Arrange
            configuration.Setup(x => x["Environment:EnableTestSummaryPage"]).Returns("false");

            mediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);



            //Act
            var result = await sut.TestSummary(assessmentId, correlationId);

            //Assert
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Summary", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task TestSummary_ShouldDirectToTestSummaryView_EnableTestSummaryPageIsTrue(
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IConfiguration> configuration,
        AssessmentListRequest request,
        List<AssessmentDataViewModel> response,
        AssessmentController sut,
        int correlationId,
        int assessmentId)
        {
            //Arrange
            configuration.Setup(x => x["Environment:EnableTestSummaryPage"]).Returns("true");
            mediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(response);

            //Act
            var result = await sut.TestSummary(assessmentId, correlationId);

            //Assert
            var viewResult = (ViewResult)result;
            Assert.Equal("TestSummary", viewResult.ViewName);
        }
    }
}
