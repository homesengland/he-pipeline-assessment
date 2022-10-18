using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Common.Tests;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow
{
    public class ChoiceTests
    {
        [Theory]
        [InlineAutoMoqData("Test 1", true, false)]
        [InlineAutoMoqData("Test 2", false, true)]

        public void ChoiceCanBeInstantiated_GivenCorrectData(string answer, bool isSelected, bool isSingle)
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
