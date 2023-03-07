using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.VFM;
using System.Text.Json;
using Xunit;


namespace He.PipelineAssessment.Data.Tests.VFM
{
    public class EsriVFMDataJsonHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ProperInputData_To_JsonToVFMCalculationData_ReturnsProperResponse(
            EsriVFMResponse inputData,
            EsriVFMDataJsonHelper helper)
        {
            //Arrange
            var strData = JsonSerializer.Serialize(inputData);

            //Act
            var result = helper.JsonToVFMCalculationData(strData);

            //Assert
            Assert.NotNull(result);
        }

        [Theory]
        [AutoMoqData]
        public void InvalidInputData_To_JsonToVFMCalculationData_ReturnsNull(
            string inputData,
            EsriVFMDataJsonHelper helper)
        {
            //Arrange
            //Act
            var result = helper.JsonToVFMCalculationData(inputData);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void ProperInputDataButNoFeatures_To_JsonToSinglePipelineData_ReturnsNull(
            EsriVFMResponse inputData,
            EsriVFMDataJsonHelper helper)
        {
            //Arrange
            inputData.features = new List<Feature>();
            var strData = JsonSerializer.Serialize(inputData);
            //Act
            var result = helper.JsonToVFMCalculationData(strData);

            //Assert
            Assert.Null(result);
        }
    }
}
