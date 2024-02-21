using AutoFixture;
using AutoFixture.Xunit2;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionaryHandler;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryRecord;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryRecord;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryRecord;
using Elsa.Server.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Drawing.Text;
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
        public async Task DataDictionaryController_CreateDataDictionaryRecord_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            
            CreateDataDictionaryRecordCommandResponse response,
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            CreateDataDictionaryRecordCommand command = new CreateDataDictionaryRecordCommand()
            {
                DictionaryRecord = SampleDictionary("Sample_Record")
            };
            var createGroupResult = new OperationResult<CreateDataDictionaryRecordCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(createGroupResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.CreateDataDictionaryRecord(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_CreateDataDictionaryRecord_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors( 
            OperationResult<CreateDataDictionaryRecordCommandResponse> response,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            CreateDataDictionaryRecordCommand command = new CreateDataDictionaryRecordCommand()
            {
                DictionaryRecord = SampleDictionary("Sample_Record")
            };
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.CreateDataDictionaryRecord(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', response.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_CreateDataDictionaryRecord_ShouldReturn500_WhenCommandHandlerThrowsException(
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            CreateDataDictionaryRecordCommand command = new CreateDataDictionaryRecordCommand();
            command.DictionaryRecord = SampleDictionary("Sample_Record");
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ThrowsAsync(exception);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.CreateDataDictionaryRecord(command);

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
        public async Task DataDictionaryController_UpdateDataDictionaryRecord_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            var request = SampleRequest<UpdateDataDictionaryRecordCommand>();
            var response = SampleRequest<UpdateDataDictionaryRecordCommandResponse>();
            var updateItemResult = new OperationResult<UpdateDataDictionaryRecordCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(It.IsAny<UpdateDataDictionaryRecordCommand>(), CancellationToken.None)).ReturnsAsync(updateItemResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.UpdateDataDictionaryRecord(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_UpdateDataDictionaryRecord_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            var request = SampleRequest<UpdateDataDictionaryRecordCommand>();
            var response = SampleRequest<UpdateDataDictionaryRecordCommandResponse>();
            var updateItemResult = new OperationResult<UpdateDataDictionaryRecordCommandResponse>
            {
                ErrorMessages = new List<string>() { new String("Error 1") },
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(updateItemResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.UpdateDataDictionaryRecord(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', updateItemResult.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_UpdateDataDictionaryRecord_ShouldReturn500_WhenCommandHandlerThrowsException(
         Exception exception,
         Mock<IMediator> mediatorMock)
        {

            //Arrange
            var request = SampleRequest<UpdateDataDictionaryRecordCommand>();
            mediatorMock.Setup(x => x.Send(request, CancellationToken.None)).ThrowsAsync(exception);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.UpdateDataDictionaryRecord(request);

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
        public async Task DataDictionaryController_ArchiveDataDictionaryRecord_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            ArchiveDataDictionaryRecordCommand command,
            ArchiveDataDictionaryRecordCommandResponse response,
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            var archiveGroupResult = new OperationResult<ArchiveDataDictionaryRecordCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(archiveGroupResult);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveDataDictionaryRecord(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_ArchiveDataDictionaryRecord_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            ArchiveDataDictionaryRecordCommand command,
            OperationResult<ArchiveDataDictionaryRecordCommandResponse> response,
    Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveDataDictionaryRecord(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', response.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task DataDictionaryController_ArchiveDataDictionaryRecord_ShouldReturn500_WhenCommandHandlerThrowsException(
            ArchiveDataDictionaryRecordCommand command,
    Exception exception,
    Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ThrowsAsync(exception);

            DataDictionaryController controller = new DataDictionaryController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveDataDictionaryRecord(command);

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

        private DataDictionary SampleDictionary(string name, string? legacyName = null, int id = 1, int groupId = 1)
        {
            return new DataDictionary
            {
                Id = id,
                DataDictionaryGroupId = groupId,
                Description = "Sample Description",
                Type = "Text",
                CreatedDateTime = DateTime.Now,
                LastModifiedDateTime = DateTime.Now,
                Name = name,
                LegacyName = legacyName ?? name
            };
        }
    }
}

