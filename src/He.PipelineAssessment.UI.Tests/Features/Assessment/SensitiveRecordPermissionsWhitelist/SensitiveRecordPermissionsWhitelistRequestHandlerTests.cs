using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist;
using MediatR;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Assessment.SensitiveRecordPermissionsWhitelist
{
    public class SensitiveRecordPermissionsWhitelistRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsResponseWithAssessmentSummaryAndPermissions_GivenValidRequest(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            AssessmentSummaryResponse assessmentSummaryResponse,
            List<SensitiveRecordWhitelist> whitelistRecords,
            SensitiveRecordPermissionsWhitelistRequest request,
            SensitiveRecordPermissionsWhitelistRequestHandler sut)
        {
            // Arrange
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(request.AssessmentId))
                .ReturnsAsync(whitelistRecords);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(assessmentSummaryResponse);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(assessmentSummaryResponse, result.AssessmentSummary);
            Assert.NotNull(result.Permissions);
            Assert.Equal(whitelistRecords.Count, result.Permissions.Count);
            mediator.Verify(x => x.Send(It.Is<AssessmentSummaryRequest>(r =>
                r.AssessmentId == request.AssessmentId && r.CorrelationId == 0),
                It.IsAny<CancellationToken>()), Times.Once);
            assessmentRepository.Verify(x => x.GetSensitiveRecordWhitelist(request.AssessmentId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsEmptyPermissionsList_GivenNoWhitelistRecordsExist(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            AssessmentSummaryResponse assessmentSummaryResponse,
            SensitiveRecordPermissionsWhitelistRequest request,
            SensitiveRecordPermissionsWhitelistRequestHandler sut)
        {
            // Arrange
            var emptyWhitelistRecords = new List<SensitiveRecordWhitelist>();
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(request.AssessmentId))
                .ReturnsAsync(emptyWhitelistRecords);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(assessmentSummaryResponse);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(assessmentSummaryResponse, result.AssessmentSummary);
            Assert.NotNull(result.Permissions);
            Assert.Empty(result.Permissions);
            assessmentRepository.Verify(x => x.GetSensitiveRecordWhitelist(request.AssessmentId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_MapsWhitelistRecordsToPermissionsDtos_GivenWhitelistRecordsExist(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            AssessmentSummaryResponse assessmentSummaryResponse,
            SensitiveRecordPermissionsWhitelistRequest request,
            SensitiveRecordPermissionsWhitelistRequestHandler sut)
        {
            // Arrange
            var whitelistRecords = new List<SensitiveRecordWhitelist>
            {
                new SensitiveRecordWhitelist
                {
                    Id = 1,
                    AssessmentId = request.AssessmentId,
                    Email = "user1@test.com",
                    CreatedDateTime = new DateTime(2024, 1, 15, 10, 30, 0),
                    CreatedBy = "admin@test.com"
                },
                new SensitiveRecordWhitelist
                {
                    Id = 2,
                    AssessmentId = request.AssessmentId,
                    Email = "user2@test.com",
                    CreatedDateTime = new DateTime(2024, 2, 20, 14, 45, 0),
                    CreatedBy = "manager@test.com"
                }
            };

            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(request.AssessmentId))
                .ReturnsAsync(whitelistRecords);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(assessmentSummaryResponse);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Permissions.Count);

            var firstPermission = result.Permissions.First();
            Assert.Equal(1, firstPermission.Id);
            Assert.Equal("user1@test.com", firstPermission.Email);
            Assert.Equal(new DateTime(2024, 1, 15, 10, 30, 0), firstPermission.DateAdded);
            Assert.Equal("admin@test.com", firstPermission.AddedBy);

            var secondPermission = result.Permissions.Last();
            Assert.Equal(2, secondPermission.Id);
            Assert.Equal("user2@test.com", secondPermission.Email);
            Assert.Equal(new DateTime(2024, 2, 20, 14, 45, 0), secondPermission.DateAdded);
            Assert.Equal("manager@test.com", secondPermission.AddedBy);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_PassesCorrectParametersToAssessmentSummaryRequest_GivenRequest(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            AssessmentSummaryResponse assessmentSummaryResponse,
            List<SensitiveRecordWhitelist> whitelistRecords,
            SensitiveRecordPermissionsWhitelistRequest request,
            SensitiveRecordPermissionsWhitelistRequestHandler sut)
        {
            // Arrange
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(request.AssessmentId))
                .ReturnsAsync(whitelistRecords);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(assessmentSummaryResponse);

            // Act
            await sut.Handle(request, CancellationToken.None);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<AssessmentSummaryRequest>(r =>
                    r.AssessmentId == request.AssessmentId &&
                    r.CorrelationId == 0),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenRepositoryThrowsException(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            AssessmentSummaryResponse assessmentSummaryResponse,
            SensitiveRecordPermissionsWhitelistRequest request,
            Exception exception,
            SensitiveRecordPermissionsWhitelistRequestHandler sut)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(assessmentSummaryResponse);
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(request.AssessmentId))
                .ThrowsAsync(exception);

            // Act & Assert
            var result = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));
            Assert.Equal(exception.Message, result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenMediatorThrowsException(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            List<SensitiveRecordWhitelist> whitelistRecords,
            SensitiveRecordPermissionsWhitelistRequest request,
            Exception exception,
            SensitiveRecordPermissionsWhitelistRequestHandler sut)
        {
            // Arrange
            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(request.AssessmentId))
                .ReturnsAsync(whitelistRecords);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act & Assert
            var result = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));
            Assert.Equal(exception.Message, result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CallsRepositoryBeforeMediator_GivenValidRequest(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            AssessmentSummaryResponse assessmentSummaryResponse,
            List<SensitiveRecordWhitelist> whitelistRecords,
            SensitiveRecordPermissionsWhitelistRequest request,
            SensitiveRecordPermissionsWhitelistRequestHandler sut)
        {
            // Arrange
            var callOrder = new List<string>();

            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(request.AssessmentId))
                .ReturnsAsync(whitelistRecords)
                .Callback(() => callOrder.Add("Repository"));

            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(assessmentSummaryResponse)
                .Callback(() => callOrder.Add("Mediator"));

            // Act
            await sut.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(2, callOrder.Count);
            Assert.Equal("Mediator", callOrder[0]);
            Assert.Equal("Repository", callOrder[1]);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_PreservesAllWhitelistProperties_GivenWhitelistRecordsWithAllProperties(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            AssessmentSummaryResponse assessmentSummaryResponse,
            SensitiveRecordPermissionsWhitelistRequest request,
            SensitiveRecordPermissionsWhitelistRequestHandler sut)
        {
            // Arrange
            var specificDate = new DateTime(2024, 3, 15, 9, 30, 45);
            var whitelistRecords = new List<SensitiveRecordWhitelist>
            {
                new SensitiveRecordWhitelist
                {
                    Id = 123,
                    AssessmentId = request.AssessmentId,
                    Email = "specific.user@example.com",
                    CreatedDateTime = specificDate,
                    CreatedBy = "specific.admin@example.com"
                }
            };

            assessmentRepository.Setup(x => x.GetSensitiveRecordWhitelist(request.AssessmentId))
                .ReturnsAsync(whitelistRecords);
            mediator.Setup(x => x.Send(It.IsAny<AssessmentSummaryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(assessmentSummaryResponse);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            var permission = result.Permissions.Single();
            Assert.Equal(123, permission.Id);
            Assert.Equal("specific.user@example.com", permission.Email);
            Assert.Equal(specificDate, permission.DateAdded);
            Assert.Equal("specific.admin@example.com", permission.AddedBy);
        }
    }
}