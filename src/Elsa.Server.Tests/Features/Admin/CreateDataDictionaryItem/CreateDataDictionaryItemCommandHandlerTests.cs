using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryGroup;
using Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryItem;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.Server.Tests.Features.Admin.CreateDataDictionaryItem
{
    public class CreateDataDictionaryItemCommandHandlerTests
    {
            [Theory]
            [AutoMoqData]
            public async Task Handle_ShouldReturnCreateDataDictionaryItemCommandResponse(
            CreateDataDictionaryItemCommand command,
            CreateDataDictionaryItemCommandHandler sut)
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
                CreateDataDictionaryItemCommand command,
                Exception exception,
                CreateDataDictionaryItemCommandHandler sut)
            {
                //Arrange
                elsaCustomRepository.Setup(x => x.CreateDataDictionaryItem(It.IsAny<QuestionDataDictionary>(), CancellationToken.None))
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
                CreateDataDictionaryItemCommand command,
                CreateDataDictionaryItemCommandHandler sut)
            {
                //Arrange
                command.Name = string.Empty;

                //Act
                var result = await sut.Handle(command, CancellationToken.None);

                //Assert
                Assert.NotNull(result);
                Assert.False(result.IsSuccess);
                var errorMessage = result.ErrorMessages.First();
                Assert.Equal("Data dictionary item could not be created, becuase name was invalid.", errorMessage);
            }
        
    }
}
