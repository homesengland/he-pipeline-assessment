using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryRecord;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using Elsa.Server.Models;
using He.PipelineAssessment.Tests.Common;
using MediatR;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Admin.ArchiveDataDictionaryRecord
{
    public class ArchiveDataDictionaryRecordCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnArchiveDataDictionaryCommandResponse(
         [Frozen] Mock<IMediator> mediator,
            OperationResult<bool> clearCacheResult,
            ArchiveDataDictionaryRecordCommand command,
            ArchiveDataDictionaryRecordCommandHandler sut)
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
        public async Task Handle_ShouldReturnErrors_GivenCallToElsaCustomRepositoryFails(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            ArchiveDataDictionaryRecordCommand command,
            Exception exception,
            ArchiveDataDictionaryRecordCommandHandler sut)
        {
            //Arrange

            elsaCustomRepository.Setup(x => x.ArchiveDataDictionaryItem(command.Id, true, CancellationToken.None))
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
