using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionary.UpdateDataDictionaryGroup;
using He.PipelineAssessment.Tests.Common;
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
        UpdateDataDictionaryGroupCommand command,
        UpdateDataDictionaryGroupCommandHandler sut)
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
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_GivenCallToElsaCustomRepositoryFails(
    [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
    UpdateDataDictionaryGroupCommand command,
    Exception exception,
    UpdateDataDictionaryGroupCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.UpdateDataDictionaryGroup(It.IsAny<QuestionDataDictionaryGroup>(), CancellationToken.None))
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
