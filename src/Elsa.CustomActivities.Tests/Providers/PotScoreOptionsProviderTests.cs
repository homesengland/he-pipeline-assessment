using AutoFixture.Xunit2;
using Elsa.CustomActivities.Providers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.CustomActivities.Tests.Providers;
public class PotScoreOptionsProviderTests
{

    [Theory]
    [AutoMoqData]
    public async Task GetOptions_ShouldReturnPotScoreOptions_GivenRepositoryReturn(
        List<PotScoreOption> potScoreOptions,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        PotScoreOptionsProvider sut
        )
    {
        // Arrange
        elsaCustomRepository.Setup(x => x.GetPotScoreOptionsAsync(CancellationToken.None)).ReturnsAsync( potScoreOptions );


        var expectedPotSoreOption = JsonConvert.SerializeObject(potScoreOptions.Select(x => x.Name));

        // Act
        var result = await sut.GetOptions();


        // Assert
        Assert.NotNull( result );
        Assert.Equal(expectedPotSoreOption, result );

    }


}
