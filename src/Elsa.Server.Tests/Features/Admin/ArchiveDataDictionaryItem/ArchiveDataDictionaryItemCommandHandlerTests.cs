using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Server.Features.Admin.DataDictionaryHandler;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryItem;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using Elsa.Server.Features.Workflow.ArchiveQuestions;
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

namespace Elsa.Server.Tests.Features.Admin.ArchiveDataDictionaryItem
{
    public class ArchiveDataDictionaryItemCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnArchiveDataDictionaryCommandResponse(
         [Frozen] Mock<IMediator> mediator,
            OperationResult<bool> clearCacheResult,
            ArchiveDataDictionaryItemCommand command,
            ArchiveDataDictionaryItemCommandHandler sut)
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
    ArchiveDataDictionaryItemCommand command,
    Exception exception,
    ArchiveDataDictionaryItemCommandHandler sut)
        {
            //Arrange

            elsaCustomRepository.Setup(x => x.ArchiveDataDictionaryItem(command.Id, CancellationToken.None))
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
