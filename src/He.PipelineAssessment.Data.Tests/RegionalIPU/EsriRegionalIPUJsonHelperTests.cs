using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.RegionalIPU;
using System.Text.Json;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.RegionalIPU
{
    public class EsriRegionalIPUJsonHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ProperInputData_To_JsonToREgionalIPUData_ReturnsProperResponse(
            EsriRegionalIPUResponse inputData,
            EsriRegionalIPUJsonHelper helper)
        {
            //Arrange
            var strData = JsonSerializer.Serialize(inputData);

            //Act
            var result = helper.JsonToRegionalIPUData(strData);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(inputData.features.First().attributes.object_id, result!.object_id);
        }

        [Theory]
        [AutoMoqData]
        public void InvalidInputData_To_JsonToRegionalIPU_ReturnsNull(
            string inputData,
            EsriRegionalIPUJsonHelper helper)
        {
            //Arrange
            //Act
            var result = helper.JsonToRegionalIPUData(inputData);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void ProperInputDataButNoFeatures_To_JsonToRegionalIPUData_ReturnsNull(
            EsriRegionalIPUResponse inputData,
            EsriRegionalIPUJsonHelper helper)
        {
            //Arrange
            inputData.features = new List<Feature>();
            var strData = JsonSerializer.Serialize(inputData);
            //Act
            var result = helper.JsonToRegionalIPUData(strData);

            //Assert
            Assert.Null(result);
        }
    }
}
