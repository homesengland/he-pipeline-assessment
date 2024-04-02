using AutoFixture.Xunit2;
using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomModels;
using Elsa.Metadata;
using Elsa.Server.Features.Activities;
using Elsa.Server.Features.Activities.CustomActivityProperties;
using Elsa.Server.Features.Activities.DataDictionaryProvider;
using Elsa.Server.Models;
using He.PipelineAssessment.Tests.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.Server.Tests.Features.Activities
{
    public class ActivitiesControllerTests
    {
        [Theory]
        [AutoData]
        public async Task ActvityController_GetCustomActivityProperties_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            Dictionary<string, string> response,
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<CustomPropertyCommand>(), CancellationToken.None)).ReturnsAsync(response);

            ActivitiesController controller = new ActivitiesController(mediatorMock.Object);

            //Act
            var result = await controller.GetCustomActivityProperties();

            var firstKey = response.Keys.First();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task ActvityController_GetCustomActivityProperties_ShouldReturn500_WhenCommandHandlerThrowsException(
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<CustomPropertyCommand>(), CancellationToken.None)).ThrowsAsync(exception);

            ActivitiesController controller = new ActivitiesController(mediatorMock.Object);

            //Act
            var result = await controller.GetCustomActivityProperties();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<Exception>(objectResult.Value);

            var exceptionResult = (Exception)objectResult.Value!;

            Assert.Equal(exception.Message, exceptionResult.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetDataDictionary_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            List<DataDictionaryGroup> dataDictionaries,
            Mock<IMediator> mediatorMock)
        {
            var response = JsonConvert.SerializeObject(dataDictionaries);

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<DataDictionaryCommand>(), CancellationToken.None)).ReturnsAsync(response);

            ActivitiesController controller = new ActivitiesController(mediatorMock.Object);

            //Act
            var result = await controller.GetDataDictionary();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        
        }

        [Theory]
        [AutoData]
        public async Task GetDataDictionary_ShouldReturn500_WhenCommandHandlerThrowsException(
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<DataDictionaryCommand>(), CancellationToken.None)).ThrowsAsync(exception);

            ActivitiesController controller = new ActivitiesController(mediatorMock.Object);

            //Act
            var result = await controller.GetDataDictionary();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<Exception>(objectResult.Value);

            var exceptionResult = (Exception)objectResult.Value!;

            Assert.Equal(exception.Message, exceptionResult.Message);
        }
    }
}
