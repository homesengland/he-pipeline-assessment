using AutoFixture.Xunit2;
using Elsa.CustomActivities.Describers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Activities.DataDictionary;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.Server.Tests.Features.Activities.DataDictionary
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
            List<QuestionDataDictionaryGroup> dataDictionaries,
            DataDictionaryCommandHandler sut)
        {
            //Arrange
            propertyDescriber.Setup(x => x.DescribeInputProperties(typeof(Question))).Returns(inputDescriptors);
            repository.Setup(x => x.GetQuestionDataDictionaryGroupsAsync(CancellationToken.None)).ReturnsAsync(dataDictionaries);

            var jsonData = JsonConvert.SerializeObject(dataDictionaries);
            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(jsonData,result);
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

            //Assert
            Assert.Empty(result);
        }
    }
}
