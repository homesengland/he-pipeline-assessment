using AutoFixture;
using AutoFixture.Xunit2;
using Elsa.Server.Features.Admin.DataDictionaryHandler;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryItem;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryItem;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryItem;
using Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen;
using Elsa.Server.Features.Workflow.LoadQuestionScreen;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Models;
using Esprima.Ast;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.Server.Tests.Features.Admin
{
    public class DataDictionaryControllerTests
    {
        [Theory]
        [AutoData]
        public async Task DataDictionaryController_CreateDataDictionaryGroup_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            CreateDataDictionaryGroupCommand command,
            CreateDataDictionaryGroupCommandResponse response,
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            var createGroupResult = new OperationResult<CreateDataDictionaryGroupCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(createGroupResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.CreateDataDictionaryGroup(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_CreateDataDictionaryGroup_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            CreateDataDictionaryGroupCommand command,
            OperationResult<CreateDataDictionaryGroupCommandResponse> response,
    Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.CreateDataDictionaryGroup(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', response.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_CreateDataDictionaryGroup_ShouldReturn500_WhenCommandHandlerThrowsException(
            CreateDataDictionaryGroupCommand command,
    Exception exception,
    Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ThrowsAsync(exception);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.CreateDataDictionaryGroup(command);

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
        [AutoData]
        public async Task DataDictionaryController_UpdateDataDictionaryGroup_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            var request = SampleRequest<UpdateDataDictionaryGroupCommand>();
            var response = SampleRequest<UpdateDataDictionaryGroupCommandResponse>();
            var updateGroupResult = new OperationResult<UpdateDataDictionaryGroupCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<UpdateDataDictionaryGroupCommand>(), CancellationToken.None)).ReturnsAsync(updateGroupResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.UpdateDataDictionaryGroup(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_UpdateDataDictionaryGroup_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            var request = SampleRequest<UpdateDataDictionaryGroupCommand>();
            var response = SampleRequest<UpdateDataDictionaryGroupCommandResponse>();
            var updateGroupResult = new OperationResult<UpdateDataDictionaryGroupCommandResponse>
            {
                ErrorMessages = new List<string>() { new String("Error 1") },
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(updateGroupResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.UpdateDataDictionaryGroup(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', updateGroupResult.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_UpdateDataDictionaryGroup_ShouldReturn500_WhenCommandHandlerThrowsException(
         Exception exception,
         Mock<IMediator> mediatorMock)
        {

            //Arrange
            var request = SampleRequest<UpdateDataDictionaryGroupCommand>();
            mediatorMock.Setup(x => x.Send(request, CancellationToken.None)).ThrowsAsync(exception);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.UpdateDataDictionaryGroup(request);

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
        [AutoData]
        public async Task DataDictionaryController_CreateDataDictionaryItem_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            CreateDataDictionaryItemCommand command,
            CreateDataDictionaryItemCommandResponse response,
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            var createGroupResult = new OperationResult<CreateDataDictionaryItemCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(createGroupResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.CreateDataDictionaryItem(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_CreateDataDictionaryItem_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            CreateDataDictionaryItemCommand command,
            OperationResult<CreateDataDictionaryItemCommandResponse> response,
    Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.CreateDataDictionaryItem(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', response.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_CreateDataDictionaryItem_ShouldReturn500_WhenCommandHandlerThrowsException(
            CreateDataDictionaryItemCommand command,
    Exception exception,
    Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ThrowsAsync(exception);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.CreateDataDictionaryItem(command);

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
        [AutoData]
        public async Task DataDictionaryController_UpdateDataDictionaryItem_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            var request = SampleRequest<UpdateDataDictionaryItemCommand>();
            var response = SampleRequest<UpdateDataDictionaryItemCommandResponse>();
            var updateItemResult = new OperationResult<UpdateDataDictionaryItemCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<UpdateDataDictionaryItemCommand>(), CancellationToken.None)).ReturnsAsync(updateItemResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.UpdateDataDictionaryItem(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_UpdateDataDictionaryItem_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            var request = SampleRequest<UpdateDataDictionaryItemCommand>();
            var response = SampleRequest<UpdateDataDictionaryItemCommandResponse>();
            var updateItemResult = new OperationResult<UpdateDataDictionaryItemCommandResponse>
            {
                ErrorMessages = new List<string>() { new String("Error 1") },
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(updateItemResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.UpdateDataDictionaryItem(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', updateItemResult.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_UpdateDataDictionaryItem_ShouldReturn500_WhenCommandHandlerThrowsException(
         Exception exception,
         Mock<IMediator> mediatorMock)
        {

            //Arrange
            var request = SampleRequest<UpdateDataDictionaryItemCommand>();
            mediatorMock.Setup(x => x.Send(request, CancellationToken.None)).ThrowsAsync(exception);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.UpdateDataDictionaryItem(request);

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
        [AutoData]
        public async Task DataDictionaryController_ArchiveDataDictionaryItem_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            ArchiveDataDictionaryItemCommand command,
            ArchiveDataDictionaryItemCommandResponse response,
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            var archiveGroupResult = new OperationResult<ArchiveDataDictionaryItemCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(archiveGroupResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveDataDictionaryItem(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_ArchiveDataDictionaryItem_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            ArchiveDataDictionaryItemCommand command,
            OperationResult<ArchiveDataDictionaryItemCommandResponse> response,
    Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveDataDictionaryItem(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', response.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_ArchiveDataDictionaryItem_ShouldReturn500_WhenCommandHandlerThrowsException(
            ArchiveDataDictionaryItemCommand command,
    Exception exception,
    Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ThrowsAsync(exception);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveDataDictionaryItem(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<Exception>(objectResult.Value);

            var exceptionResult = (Exception)objectResult.Value!;

            Assert.Equal(exception.Message, exceptionResult.Message);
        }

        private T SampleRequest<T>()
        {
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => fixture.Behaviors.Remove(x));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var response = fixture.Create<T>();
            return response;
        }
    }
}
