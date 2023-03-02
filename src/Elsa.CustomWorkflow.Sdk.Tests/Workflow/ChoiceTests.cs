using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow
{
    public class ChoiceTests
    {
        [Theory]
        [InlineAutoMoqData("Test 1", false)]
        [InlineAutoMoqData("Test 2", true)]

        public void ChoiceCanBeInstantiated_GivenCorrectData(string answer, bool isSingle)
        {
            //Arrange
            Choice choice = new Choice();

            //Act
            choice.Answer = answer;

            choice.IsSingle = isSingle;
            //Assert
            Assert.NotNull(choice);
            Assert.Equal(isSingle, choice.IsSingle);
        }
    }
}
