using He.PipelineAssessment.Data.ExtendedSinglePipeline;
using He.PipelineAssessment.Tests.Common;
using System.Text.Json;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.SinglePipelineExtendedExtended
{
    public class GetSinglePipelineExtendedDataJsonHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ProperInputData_To_JsonToSinglePipelineExtendedData_ReturnsProperResponse(
            EsriSinglePipelineExtendedResponse inputData,
            EsriSinglePipelineExtendedDataJsonHelper helper)
        {
            //Arrange
            var strData = JsonSerializer.Serialize(inputData);

            //Act
            var result = helper.JsonToSinglePipelineExtendedData(strData);

            //Assert
            Assert.NotNull(result);
        }

        [Theory]
        [AutoMoqData]
        public void InvalidInputData_To_JsonToSinglePipelineExtendedData_ReturnsNull(
            string inputData,
            EsriSinglePipelineExtendedDataJsonHelper helper)
        {
            //Arrange
            //Act
            var result = helper.JsonToSinglePipelineExtendedData(inputData);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void ProperInputDataButNoFeatures_To_JsonToSinglePipelineExtendedData_ReturnsNull(
            EsriSinglePipelineExtendedResponse inputData,
            EsriSinglePipelineExtendedDataJsonHelper helper)
        {
            //Arrange
            inputData.features = new List<Feature>();
            var strData = JsonSerializer.Serialize(inputData);
            //Act
            var result = helper.JsonToSinglePipelineExtendedData(strData);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void ProperInputData_To_JsonToSinglePipelineExtendedDataList_ReturnsProperResponse(
            EsriSinglePipelineExtendedResponse inputData,
            EsriSinglePipelineExtendedDataJsonHelper helper)
        {
            //Arrange
            var strData = JsonSerializer.Serialize(inputData);

            //Act
            var result = helper.JsonToSinglePipelineExtendedDataList(strData);

            //Assert
            Assert.NotNull(result);

        }

        [Theory]
        [AutoMoqData]
        public void InvalidInputData_To_JsonToSinglePipelineExtendedDataList_ReturnsNull(
            string inputData,
            EsriSinglePipelineExtendedDataJsonHelper helper)
        {
            //Arrange
            //Act
            var result = helper.JsonToSinglePipelineExtendedDataList(inputData);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void ProperInputDataButNoFeatures_To_JsonToSinglePipelineExtendedDataList_ReturnsNull(
            EsriSinglePipelineExtendedResponse inputData,
            EsriSinglePipelineExtendedDataJsonHelper helper)
        {
            //Arrange
            inputData.features = new List<Feature>();
            var strData = JsonSerializer.Serialize(inputData);
            //Act
            var result = helper.JsonToSinglePipelineExtendedDataList(strData);

            //Assert
            Assert.Null(result);
        }
    }
}
