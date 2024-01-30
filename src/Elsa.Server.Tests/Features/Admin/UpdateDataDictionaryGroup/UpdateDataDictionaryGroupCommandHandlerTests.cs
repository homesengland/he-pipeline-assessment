using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryGroup;
using Elsa.Server.Models;
using He.PipelineAssessment.Tests.Common;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.Server.Tests.Features.Admin.UpdateDataDictionaryGroup
{
    public class UpdateDataDictionaryGroupCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnUpdateDataDictionaryGroupCommandResponse(
            [Frozen] Mock<IMediator> mediator,
            OperationResult<bool> clearCacheResult,
        UpdateDataDictionaryGroupCommand command,
        UpdateDataDictionaryGroupCommandHandler sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(It.IsAny<ClearDictionaryCacheCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(clearCacheResult);
            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            mediator.Verify(x => x.Send(It.IsAny<ClearDictionaryCacheCommand>(), CancellationToken.None), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_GivenGroupIsNull(
                        [Frozen] Mock<IMediator> mediator,
UpdateDataDictionaryGroupCommand command,
UpdateDataDictionaryGroupCommandHandler sut)
        {
            //Arrange
            command.group.Name = string.Empty;

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            var errorMessage = result.ErrorMessages.First();
            Assert.Equal("Data dictionary group could not be updated, becuase name was invalid.", errorMessage);
            mediator.Verify(x => x.Send(It.IsAny<ClearDictionaryCacheCommand>(), CancellationToken.None), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_GivenCallToElsaCustomRepositoryFails(
                        [Frozen] Mock<IMediator> mediator,
    [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
    UpdateDataDictionaryGroupCommand command,
    Exception exception,
    UpdateDataDictionaryGroupCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.UpdateDataDictionaryGroup(It.IsAny<DataDictionaryGroup>(), CancellationToken.None))
                .Throws(exception);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            var errorMessage = result.ErrorMessages.First();
            Assert.Equal(exception.Message, errorMessage);
            mediator.Verify(x => x.Send(It.IsAny<ClearDictionaryCacheCommand>(), CancellationToken.None), Times.Never);
        }
    }
}
