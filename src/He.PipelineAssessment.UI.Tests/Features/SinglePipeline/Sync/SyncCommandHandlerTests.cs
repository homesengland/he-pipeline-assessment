using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using Moq;
using Xunit;
using He.PipelineAssessment.Models.ViewModels;

namespace He.PipelineAssessment.UI.Tests.Features.SinglePipeline.Sync
{
    public class SyncCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsErrors_GivenExceptionIsThrown(
            [Frozen] Mock<ISinglePipelineProvider> singlePipelineService,
            Exception exception,
            SyncCommandHandler sut)
        {
            //Arrange
            singlePipelineService.Setup(x => x.GetSinglePipelineData()).Throws(exception);

            //Act
            var exceptionThrown = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None));

            //Assert 
            Assert.Equal("Single Pipeline Data failed to sync", exceptionThrown.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsErrors_GivenResponseDataIsEmpty(
            [Frozen] Mock<ISinglePipelineProvider> singlePipelineService,           
            SyncCommandHandler sut)
        {
            //Arrange
            singlePipelineService.Setup(x => x.GetSinglePipelineData()).ReturnsAsync(new List<SinglePipelineData>());

            //Act
            var exceptionThrown = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None));

            //Assert
            Assert.Equal("Single Pipeline Data failed to sync", exceptionThrown.Message);
        }       

        [Theory]
        [AutoMoqData]
       
        public async Task Handle_CreateorUpdateAssessmentRecords_GivenSignlePipelineDataIsNotEmpty(
            
            [Frozen] Mock<ISyncCommandHandlerHelper> syncCommandHandlerHelper,
            [Frozen] Mock<ISinglePipelineProvider> singlePipelineService,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            List<Models.Assessment> assessmentsTobeAdded,           
            List<Models.Assessment> assessments,
            List<SinglePipelineData> singlePipelineDataResponse,
            SyncCommandHandler sut)
        {
            //Arrange
            singlePipelineService.Setup(x => x.GetSinglePipelineData()).ReturnsAsync(singlePipelineDataResponse);
           
            assessmentRepository.Setup(x => x.GetAssessments()).ReturnsAsync(assessments);

            var sourceAssessmentSpIds = singlePipelineDataResponse.Select(x => x.sp_id!.Value).ToList();
            var destinationAssessmentSpIds = assessments.Select(x => x.SpId).ToList();

            syncCommandHandlerHelper.Setup(x => x.AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, singlePipelineDataResponse)).Returns(assessmentsTobeAdded);

            var existingAssessments = destinationAssessmentSpIds.Intersect(sourceAssessmentSpIds).ToList();

            syncCommandHandlerHelper.Setup(x => x.UpdateAssessments(assessments, existingAssessments, singlePipelineDataResponse)).Returns(6);

            //Act
            var result = await sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x => x.CreateAssessments(assessmentsTobeAdded), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<SyncModel>(result);
            Assert.Equal(assessmentsTobeAdded.Count, result.NewAssessmentCount);
            Assert.True(result.Synced);
            Assert.Equal(6, result.UpdatedAssessmentCount);
        }

    }
}
