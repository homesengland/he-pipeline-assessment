using AutoFixture.Xunit2;
using Elsa.CustomActivities.Describers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Activities.CustomActivityProperties;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;
using Question = Elsa.CustomActivities.Activities.QuestionScreen.Question;

namespace Elsa.Server.Tests.Features.Activities.CustomActivityProperty;

public class CustomPropertyCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsCorrectValues_GivenNoErrorsThrown(
        [Frozen] Mock<ICustomPropertyDescriber> propertyDescriber,
        [Frozen] Mock<IElsaCustomRepository> repository,
        CustomPropertyCommand loadCustomProperties,
        List<HeActivityInputDescriptor> inputDescriptors,
        List<QuestionDataDictionaryGroup> dataDictionaries,
        CustomPropertyCommandHandler sut)
    {
        //Arrange
        propertyDescriber.Setup(x => x.DescribeInputProperties(typeof(Question))).Returns(inputDescriptors);
        repository.Setup(x => x.GetQuestionDataDictionaryGroupsAsync(CancellationToken.None)).ReturnsAsync(dataDictionaries);

        //Act
        var result = await sut.Handle(loadCustomProperties, CancellationToken.None);

        //Assert
        Assert.True(result.GetType() == typeof(Dictionary<string, string>));
        Assert.Equal(JsonConvert.SerializeObject(inputDescriptors, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }), result["QuestionProperties"]);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsEmptyDictionary_GivenExceptionIsThrown(
        [Frozen] Mock<ICustomPropertyDescriber> propertyDescriber,
        CustomPropertyCommand loadCustomProperties,
        Exception e,
        CustomPropertyCommandHandler sut)
    {
        //Arrange
        propertyDescriber.Setup(x => x.DescribeInputProperties(typeof(Question))).Throws(e);

        //Act
        var result = await sut.Handle(loadCustomProperties, CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }
}