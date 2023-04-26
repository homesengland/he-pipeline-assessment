using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Elsa.CustomActivities.OptionsProviders;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.CustomActivities.Tests.OptionsProvider
{
    public class QuestionDataDictionaryOptionsProviderTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetOptions_QuestionDataDictionaryGroups_GivenRepositoryReturn(
            List<QuestionDataDictionaryGroup> groups,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            QuestionDataDictionaryOptionsProvider sut
        )
        {
            // Arrange
            elsaCustomRepository.Setup(x => x.GetQuestionDataDictionaryGroupsAsync(CancellationToken.None)).ReturnsAsync(groups);

            var expected = JsonConvert.SerializeObject(groups);

            // Act
            var result = await sut.GetOptions();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }
    }
}
