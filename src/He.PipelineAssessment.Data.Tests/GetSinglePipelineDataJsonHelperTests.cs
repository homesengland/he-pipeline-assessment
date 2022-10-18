﻿using AutoFixture.Xunit2;
using Elsa.Server.Features.Datasources.GetSinglePipelineData;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Data.SinglePipeline;
using Moq;
using System.Net;
using System.Text.Json;
using Xunit;

namespace He.PipelineAssessment.Data.Tests
{
    public class GetSinglePipelineDataJsonHelperTests
    {
        [Theory]
        [AutoMoqData]
        public void ProperInputData_To_JsonToSinglePipelineData_ReturnsProperResponse(
            EsriSinglePipelineResponse inputData,
            EsriSinglePipelineDataJsonHelper helper)
        {
            //Arrange
            var strData = JsonSerializer.Serialize(inputData);

            //Act
            var result = helper.JsonToSinglePipelineData(strData);

            //Assert
            Assert.NotNull(result);
        }

        [Theory]
        [AutoMoqData]
        public void InvalidInputData_To_JsonToSinglePipelineData_ReturnsNull(
            string inputData,
            EsriSinglePipelineDataJsonHelper helper)
        {
            //Arrange
            //Act
            var result = helper.JsonToSinglePipelineData(inputData);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void ProperInputDataButNoFeatures_To_JsonToSinglePipelineData_ReturnsNull(
            EsriSinglePipelineResponse inputData,
            EsriSinglePipelineDataJsonHelper helper)
        {
            //Arrange
            inputData.features = new List<Feature>();
            var strData = JsonSerializer.Serialize(inputData);
            //Act
            var result = helper.JsonToSinglePipelineData(strData);

            //Assert
            Assert.Null(result);
        }
    }
}
