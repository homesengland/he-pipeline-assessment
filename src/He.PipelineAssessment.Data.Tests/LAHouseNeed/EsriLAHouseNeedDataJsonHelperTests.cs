using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Tests.Common;
using System.Text.Json;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.LAHouseNeed
{
    public class EsriLAHouseNeedDataJsonHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ProperInputData_To_JsonToLAHouseNeedData_ReturnsProperResponse(
            EsriLAHouseNeedResponse inputData,
            EsriLAHouseNeedDataJsonHelper helper)
        {
            //Arrange
            var strData = JsonSerializer.Serialize(inputData);

            //Act
            var result = helper.JsonToLAHouseNeedData(strData);

            //Assert
            Assert.NotNull(result);
        }

        [Theory]
        [AutoMoqData]
        public void InvalidInputData_To_JsonToSinglePipelineData_ReturnsNull(
            string inputData,
            EsriLAHouseNeedDataJsonHelper helper)
        {
            //Arrange
            //Act
            var result = helper.JsonToLAHouseNeedData(inputData);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void ProperInputDataButNoFeatures_To_JsonToSinglePipelineData_ReturnsNull(
            EsriLAHouseNeedResponse inputData,
            EsriLAHouseNeedDataJsonHelper helper)
        {
            //Arrange
            inputData.features = new List<Feature>();
            var strData = JsonSerializer.Serialize(inputData);
            //Act
            var result = helper.JsonToLAHouseNeedData(strData);

            //Assert
            Assert.Null(result);
        }
    }
}
