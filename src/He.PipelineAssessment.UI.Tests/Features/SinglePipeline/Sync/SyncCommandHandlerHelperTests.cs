using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.SinglePipeline.Sync
{
    public class SyncCommandHandlerHelperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task AssessmentsToBeAdded_ShouldRetrunListofAssessments_GivenListofSourceandDestinationSpIds(
         [Frozen] Mock<IDateTimeProvider> dateTimeProvider,       
         List<Models.Assessment> assessmentList,       
         List<SinglePipelineData> singlePipelineData,
        SyncCommandHandlerHelper sut)
        {
            //Arrange
            var sourceAssessmentSpIds = new List<int>
            {
             2,6,8
            };         
           var destinationAssessmentSpIds =  assessmentList.Select(x => x.SpId).ToList();

          //Act
           var result =  sut.AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, singlePipelineData);


            //Assert
            Assert.NotSame(sourceAssessmentSpIds,result);
           
        }
        [Theory]
        [AutoMoqData]
        public async Task AssessmentsToBeAdded_ShouldRetrunSameListofAssessments_GivenListofSourceandDestinationSpIds(
        [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
        [Frozen] Mock<List<int>> assessmentList,
        List<SinglePipelineData> singlePipelineData,
         SyncCommandHandlerHelper sut)
        {
            //Arrange
            //var sourceAssessmentSpIds = new Models.Assessment()
            //{
            //    SpId = 2
            //};


            var sourceAssessmentSpIds = new List<int>
            {
                2,6,8
            };
            //singlePipelineData.First().sp_id = 7;
            //singlePipelineData.Last().sp_id = 5;
            var destinationAssessmentSpIds = new List<int>();
            destinationAssessmentSpIds.Add(2);
            destinationAssessmentSpIds.Add(4);
            destinationAssessmentSpIds.Add(8);

           // var destinationAssessmentSpIds = assessmentList.Select(x => x.SpId).ToList();

            assessmentList.Setup(x => x.AddRange(destinationAssessmentSpIds));
            //Act

            var result = sut.AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, singlePipelineData);

            
            //Assert

            Assert.Empty(result);
           // Assert.Equal(new List<int> { 2,6,8}, result.);
          //  Assert.Equal(new List<int> { 2,6, 8 }, result.Select(x => x.Id).ToList());
            // Assert.Equal(new List<int> { 5, 7 }, result.Select(x => x.Id).ToList());
        }
    }
}
