using Castle.Core.Logging;
using Elsa.Server.Features.Admin.DataDictionary.ClearDictionaryCache;
using Elsa.Server.Features.Admin.DataDictionary.UpdateDataDictionaryItem;
using Elsa.Server.Models;
using He.PipelineAssessment.Tests.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.Server.Tests.Features.Admin.ClearDataDictionaryCache
{
    public class ClearDataDictionaryCacheCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnResultWithNoErrors(
            ILogger<ClearDictionaryCacheCommandHandler> logger,
            Mock<IConnectionMultiplexer> connection,
            Mock<IDatabase> cache,
            ClearDictionaryCacheCommand command)
        {
            //Arrange
            cache.Setup(x => x.KeyDeleteAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(true);
            cache.Setup(x => x.KeyExistsAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(true);
            connection.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(cache.Object);

            ClearDictionaryCacheCommandHandler sut = new ClearDictionaryCacheCommandHandler(connection.Object, logger);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Empty(result.ErrorMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnResultWithErrorsGivenErrorThrownByDatabase(
                ILogger<ClearDictionaryCacheCommandHandler> logger,
                Mock<IConnectionMultiplexer> connection,
                Mock<IDatabase> cache,
                ClearDictionaryCacheCommand command,
                Exception e)
        {
            //Arrange
            cache.Setup(x => x.KeyDeleteAsync(command.CacheKey, CommandFlags.None)).ThrowsAsync(e);
            cache.Setup(x => x.KeyExistsAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(false);
            connection.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(cache.Object);

            ClearDictionaryCacheCommandHandler sut = new ClearDictionaryCacheCommandHandler(connection.Object, logger);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.NotEmpty(result.ErrorMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnResultWitErrorsGivenKeyNotFoundInDatabase(
        ILogger<ClearDictionaryCacheCommandHandler> logger,
        Mock<IConnectionMultiplexer> connection,
        Mock<IDatabase> cache,
        ClearDictionaryCacheCommand command)
        {
            //Arrange
            cache.Setup(x => x.KeyDeleteAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(false);
            cache.Setup(x => x.KeyExistsAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(false);
            connection.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(cache.Object);

            ClearDictionaryCacheCommandHandler sut = new ClearDictionaryCacheCommandHandler(connection.Object, logger);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Single(result.ErrorMessages);
        }
    }
}
