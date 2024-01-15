using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionary.UpdateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionary.UpdateDataDictionaryItem;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.Server.Tests.Features.Admin.UpdateDataDictionaryItem
{
    public class UpdateDataDictionaryItemCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnUpdateDataDictionaryItemCommandResponse(
        UpdateDataDictionaryItemCommand command,
        UpdateDataDictionaryItemCommandHandler sut)
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
        public async Task Handle_ShouldReturnErrors_GivenGroupIsNull(
UpdateDataDictionaryItemCommand command,
UpdateDataDictionaryItemCommandHandler sut)
        {
            //Arrange
            command.Item = null;

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            var errorMessage = result.ErrorMessages.First();
            Assert.Equal("Data dictionary group could not be updated, becuase name or legacy name were invalid.", errorMessage);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_GivenCallToElsaCustomRepositoryFails(
    [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
    UpdateDataDictionaryItemCommand command,
    Exception exception,
    UpdateDataDictionaryItemCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.UpdateDataDictionaryItem(It.IsAny<QuestionDataDictionary>(), CancellationToken.None))
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
