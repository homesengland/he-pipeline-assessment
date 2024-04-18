using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Server.DataDictionaryAccessors;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
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
            Mock<IElsaCustomRepository> repo,
            ClearDictionaryCacheCommand command)
        {
            //Arrange
            cache.Setup(x => x.KeyDeleteAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(true);
            cache.Setup(x => x.KeyExistsAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(true);
            connection.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(cache.Object);
            CachedDataDictionaryIntellisenseAccessor accessor = new CachedDataDictionaryIntellisenseAccessor(connection.Object, repo.Object);
            ClearDictionaryCacheCommandHandler sut = new ClearDictionaryCacheCommandHandler(accessor, logger);

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
                Mock<IElsaCustomRepository> repo,
                ClearDictionaryCacheCommand command,
                Exception e)
        {
            //Arrange
            cache.Setup(x => x.KeyDeleteAsync(command.CacheKey, CommandFlags.None)).ThrowsAsync(e);
            cache.Setup(x => x.KeyExistsAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(false);
            connection.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(cache.Object);

            CachedDataDictionaryIntellisenseAccessor accessor = new CachedDataDictionaryIntellisenseAccessor(connection.Object, repo.Object);
            ClearDictionaryCacheCommandHandler sut = new ClearDictionaryCacheCommandHandler(accessor, logger);

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
        Mock<IElsaCustomRepository> repo,
        ClearDictionaryCacheCommand command)
        {
            //Arrange
            cache.Setup(x => x.KeyDeleteAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(false);
            cache.Setup(x => x.KeyExistsAsync(command.CacheKey, CommandFlags.None)).ReturnsAsync(false);
            connection.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(cache.Object);

            CachedDataDictionaryIntellisenseAccessor accessor = new CachedDataDictionaryIntellisenseAccessor(connection.Object, repo.Object);
            ClearDictionaryCacheCommandHandler sut = new ClearDictionaryCacheCommandHandler(accessor, logger);

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
