using AutoFixture.Xunit2;
using Elsa.CustomActivities.Describers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Activities.DataDictionaryProvider;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Newtonsoft.Json;
using System.ComponentModel;
using Xunit;

namespace Elsa.Server.Tests.Features.Activities.DataDictionaryProvider
{
    public class DataDictionaryCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsCorrectValues_GivenNoErrorsThrown(
            [Frozen] Mock<ICustomPropertyDescriber> propertyDescriber,
            [Frozen] Mock<IElsaCustomRepository> repository,
            DataDictionaryCommand command,
            List<HeActivityInputDescriptor> inputDescriptors,
            List<DataDictionaryGroup> dataDictionaryGroups,
            List<DataDictionary> dataDictionaries,
            DataDictionaryCommandHandler sut)
        {
            //Arrange
            propertyDescriber.Setup(x => x.DescribeInputProperties(typeof(Question))).Returns(inputDescriptors);
            repository.Setup(x => x.GetDataDictionaryGroupsAsync(false, CancellationToken.None)).ReturnsAsync(dataDictionaryGroups);
            repository.Setup(x => x.GetDataDictionaryListAsync(false, CancellationToken.None)).ReturnsAsync(dataDictionaries);

            var jsonData = JsonConvert.SerializeObject(dataDictionaries);
            //Act
            var result = await sut.Handle(command, CancellationToken.None);
            Dictionary<string, string>? deserialisedResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            if (deserialisedResult != null)
            {
                deserialisedResult.TryGetValue("Intellisense", out string? intellisense);
                if (intellisense != null)
                {
                    Assert.Contains(dataDictionaries[0].Group.Name + "_" + dataDictionaries[0].Name, intellisense);
                    Assert.Contains(dataDictionaries[1].Group.Name + "_" + dataDictionaries[1].Name, intellisense);
                    Assert.Contains(dataDictionaries[2].Group.Name + "_" + dataDictionaries[2].Name, intellisense);
                }
            }

            //Assert
            Assert.NotNull(result);


        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsEmptyDictionary_GivenExceptionIsThrown(
            [Frozen] Mock<ICustomPropertyDescriber> propertyDescriber,
            DataDictionaryCommand command,
            Exception e,
            DataDictionaryCommandHandler sut)
        {
            //Arrange
            propertyDescriber.Setup(x => x.DescribeInputProperties(typeof(Question))).Throws(e);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);
            var resultObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            if (resultObj != null)
            {
                var dataDictionaryObj = JsonConvert.DeserializeObject<List<DataDictionary>>(resultObj["Dictionary"]);
                if (dataDictionaryObj != null)
                {
                    //Assert
                    Assert.Empty(dataDictionaryObj);
                    Assert.Empty(resultObj["Intellisense"]);
                }
            }
        }
    }
}
