using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.RegionalFigs;
using System.Text.Json;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.RegionalFigs
{
    public class EsriRegionalIPUJsonHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ProperInputData_To_JsonToREgionalFigsData_ReturnsProperResponse(
            EsriRegionalFigsResponse inputData,
            EsriRegionalFigsJsonHelper helper)
        {
            //Arrange
            var strData = JsonSerializer.Serialize(inputData);

            //Act
            var result = helper.JsonToRegionalFigsData(strData);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(inputData.features.First().attributes.object_id, result!.object_id);
        }

        [Theory]
        [AutoMoqData]
        public void InvalidInputData_To_JsonToRegionalFigs_ReturnsNull(
            string inputData,
            EsriRegionalFigsJsonHelper helper)
        {
            //Arrange
            //Act
            var result = helper.JsonToRegionalFigsData(inputData);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void ProperInputDataButNoFeatures_To_JsonToRegionalFigsData_ReturnsNull(
            EsriRegionalFigsResponse inputData,
            EsriRegionalFigsJsonHelper helper)
        {
            //Arrange
            inputData.features = new List<Feature>();
            var strData = JsonSerializer.Serialize(inputData);
            //Act
            var result = helper.JsonToRegionalFigsData(strData);

            //Assert
            Assert.Null(result);
        }
    }
}
