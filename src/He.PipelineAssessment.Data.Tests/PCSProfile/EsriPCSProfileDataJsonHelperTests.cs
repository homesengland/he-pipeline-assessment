using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.PCSProfile;
using System.Text.Json;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.PCSProfile
{
    public class EsriPCSProfileDataJsonHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ProperInputData_To_JsonToSinglePipelineData_ReturnsProperResponse(
            EsriPCSProfileResponse inputData,
            EsriPCSProfileDataJsonHelper helper)
        {
            //Arrange
            var strData = JsonSerializer.Serialize(inputData);

            //Act
            var result = helper.JsonToPCSProfileData(strData);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(inputData.features.First().attributes.object_id, result!.object_id);
        }

        [Theory]
        [AutoMoqData]
        public void InvalidInputData_To_JsonToSinglePipelineData_ReturnsNull(
            string inputData,
            EsriPCSProfileDataJsonHelper helper)
        {
            //Arrange
            //Act
            var result = helper.JsonToPCSProfileData(inputData);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void ProperInputDataButNoFeatures_To_JsonToSinglePipelineData_ReturnsNull(
            EsriPCSProfileResponse inputData,
            EsriPCSProfileDataJsonHelper helper)
        {
            //Arrange
            inputData.features = new List<Feature>();
            var strData = JsonSerializer.Serialize(inputData);
            //Act
            var result = helper.JsonToPCSProfileData(strData);

            //Assert
            Assert.Null(result);
        }
    }
}
