using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.SinglePipeline.Sync
{
    public class SyncCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsErrors_GivenExceptionIsThrown(
            [Frozen] Mock<IEsriSinglePipelineClient> esriSinglePipelineClient,
            Exception exception,
            SyncCommandHandler sut)
        {
            //Arrange
            esriSinglePipelineClient.Setup(x => x.GetSinglePipelineData()).Throws(exception);

            //Act
            var result = await sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ErrorMessages.Count);
        }


        private readonly IEsriSinglePipelineClient _esriSinglePipelineClient;
        private readonly IEsriSinglePipelineDataJsonHelper _jsonHelper;
        private readonly IAssessmentRepository _assessmentRepository;

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsErrors_GivenResponseDataIsNull(
            [Frozen] Mock<IEsriSinglePipelineClient> esriSinglePipelineClient,
            SyncCommandHandler sut)
        {
            //Arrange
            esriSinglePipelineClient.Setup(x => x.GetSinglePipelineData()).ReturnsAsync((string?)null);

            //Act
            var result = await sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ErrorMessages.Count);
            Assert.Equal("Single Pipeline Response data returned null", result.ErrorMessages.First());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsErrors_GivenResponseFailsToDeserialize(
            [Frozen] Mock<IEsriSinglePipelineClient> esriSinglePipelineClient,
            [Frozen] Mock<IEsriSinglePipelineDataJsonHelper> esriSinglePipelineDataJsonHelper,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            string stringResponse,
            List<Assessment> assessments,
            SinglePipelineData singlePipelineDataResponse,
            SyncCommandHandler sut)
        {
            //Arrange
            esriSinglePipelineClient.Setup(x => x.GetSinglePipelineData()).ReturnsAsync(stringResponse);
            esriSinglePipelineDataJsonHelper.Setup(x => x.JsonToSinglePipelineDataList(stringResponse)).Returns((List<SinglePipelineData>?)null);
            //Act
            var result = await sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ErrorMessages.Count);
            Assert.Equal("Single Pipeline Response data failed to deserialize", result.ErrorMessages.First());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNoErrors_GivenResponseIsValid(
            [Frozen] Mock<IEsriSinglePipelineClient> esriSinglePipelineClient,
            [Frozen] Mock<IEsriSinglePipelineDataJsonHelper> esriSinglePipelineDataJsonHelper,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            string stringResponse,
            List<Assessment> assessments,
            List<SinglePipelineData> singlePipelineDataResponse,
            SyncCommandHandler sut)
        {
            //Arrange
            esriSinglePipelineClient.Setup(x => x.GetSinglePipelineData()).ReturnsAsync(stringResponse);
            esriSinglePipelineDataJsonHelper.Setup(x => x.JsonToSinglePipelineDataList(stringResponse)).Returns(singlePipelineDataResponse);
            assessmentRepository.Setup(x => x.GetAssessments()).ReturnsAsync(assessments);
            assessmentRepository.Setup(x => x.CreateAssessments(assessments)).ReturnsAsync(It.IsAny<int>());

            //Act
            var result = await sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorMessages.Count);
        }

    }
}
