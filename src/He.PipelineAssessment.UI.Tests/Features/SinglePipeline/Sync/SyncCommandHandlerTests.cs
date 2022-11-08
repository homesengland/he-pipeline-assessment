using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Repo = He.PipelineAssessment.Models;

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

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsErrors_GivenResponseDataIsNull(
            [Frozen] Mock<IEsriSinglePipelineClient> esriSinglePipelineClient,
            [Frozen] Mock<IEsriSinglePipelineDataJsonHelper> esriSinglePipelineDataJsonHelper,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            IConfiguration configuration,
            SyncCommandHandler sut)
        {
            //Arrange
            esriSinglePipelineClient.Setup(x => x.GetSinglePipelineData()).ReturnsAsync((string?)null);

            var inMemorySettings = new Dictionary<string, string> {
                {"Data:UseSeedData", "false"}};
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            sut = new SyncCommandHandler(esriSinglePipelineClient.Object, esriSinglePipelineDataJsonHelper.Object, assessmentRepository.Object, configuration, assessmentRepository.Object);

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
            IConfiguration configuration,
            string stringResponse,
            SyncCommandHandler sut)
        {
            //Arrange
            esriSinglePipelineClient.Setup(x => x.GetSinglePipelineData()).ReturnsAsync(stringResponse);
            esriSinglePipelineDataJsonHelper.Setup(x => x.JsonToSinglePipelineDataList(stringResponse)).Returns((List<SinglePipelineData>?)null);

            var inMemorySettings = new Dictionary<string, string> {
                {"Data:UseSeedData", "false"}};
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            sut = new SyncCommandHandler(esriSinglePipelineClient.Object, esriSinglePipelineDataJsonHelper.Object, assessmentRepository.Object, configuration, assessmentRepository.Object);

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
            IConfiguration configuration,
            string stringResponse,
            List<Repo.Assessment> assessments,
            List<SinglePipelineData> singlePipelineDataResponse,
            SyncCommandHandler sut)
        {
            //Arrange
            esriSinglePipelineClient.Setup(x => x.GetSinglePipelineData()).ReturnsAsync(stringResponse);
            esriSinglePipelineDataJsonHelper.Setup(x => x.JsonToSinglePipelineDataList(stringResponse)).Returns(singlePipelineDataResponse);
            assessmentRepository.Setup(x => x.GetAssessments()).ReturnsAsync(assessments);
            assessmentRepository.Setup(x => x.CreateAssessments(assessments)).ReturnsAsync(It.IsAny<int>());
            var inMemorySettings = new Dictionary<string, string> {
                {"Data:UseSeedData", "false"}};
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            sut = new SyncCommandHandler(esriSinglePipelineClient.Object, esriSinglePipelineDataJsonHelper.Object, assessmentRepository.Object, configuration, assessmentRepository.Object);

            //Act
            var result = await sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorMessages.Count);
        }

    }
}
