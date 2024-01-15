using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionary.ArchiveDataDictionaryItem;
using Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryGroup;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.Server.Tests.Features.Admin.CreateDataDictionaryGroup
{
    public class CreateDataDictionaryGroupCommandHandlerTests
    {
            [Theory]
            [AutoMoqData]
            public async Task Handle_ShouldReturnCreateDataDictionaryGroupCommandResponse(
            CreateDataDictionaryGroupCommand command,
            CreateDataDictionaryGroupCommandHandler sut)
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
            CreateDataDictionaryGroupCommand command,
            Exception exception,
            CreateDataDictionaryGroupCommandHandler sut)
            {
                //Arrange
                elsaCustomRepository.Setup(x => x.CreateDataDictionaryGroup(It.IsAny<QuestionDataDictionaryGroup>(), CancellationToken.None))
                    .Throws(exception);

                //Act
                var result = await sut.Handle(command, CancellationToken.None);

                //Assert
                Assert.NotNull(result);
                Assert.False(result.IsSuccess);
                var errorMessage = result.ErrorMessages.First();
                Assert.Equal(exception.Message, errorMessage);
            }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_GivenGroupNameIsNull(
            CreateDataDictionaryGroupCommand command,
            CreateDataDictionaryGroupCommandHandler sut)
        {
            //Arrange
            command.Name = string.Empty;

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            var errorMessage = result.ErrorMessages.First();
            Assert.Equal("Data dictionary group could not be created, becuase name was invalid.", errorMessage);
        }
    }
}
