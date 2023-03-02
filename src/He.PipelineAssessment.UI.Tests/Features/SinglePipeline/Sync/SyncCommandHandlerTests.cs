using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Repository;
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
            [Frozen] Mock<ISinglePipelineProvider> singlePipelineService,
            Exception exception,
            SyncCommandHandler sut)
        {
            //Arrange
            singlePipelineService.Setup(x => x.GetSinglePipelineData()).Throws(exception);

            //Act
            var result = await sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ErrorMessages.Count);
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
            var result = await sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ErrorMessages.Count);
            Assert.Equal("Single Pipeline Response data returned null", result.ErrorMessages.First());
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

            assessmentRepository.Setup(x => x.CreateAssessments(assessmentsTobeAdded)).ReturnsAsync(It.IsAny<int>());
            syncCommandHandlerHelper.Setup(x => x.UpdateAssessments(assessments, existingAssessments, singlePipelineDataResponse)).Returns(It.IsAny<List<Models.Assessment>>());

            //Act
            var result = await sut.Handle(It.IsAny<SyncCommand>(), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorMessages.Count);
        }

    }
}
