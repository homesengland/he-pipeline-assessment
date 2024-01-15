using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Server.Features.Admin.DataDictionary;
using Elsa.Server.Features.Admin.DataDictionary.ArchiveDataDictionaryItem;
using Elsa.Server.Features.Workflow.ArchiveQuestions;
using He.PipelineAssessment.Tests.Common;
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
    ArchiveDataDictionaryItemCommand command,
    ArchiveDataDictionaryItemCommandHandler sut)
        {
            //Arrange

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_GivenCallToElsaCustomRepositoryFails(
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
        }
    }
}
