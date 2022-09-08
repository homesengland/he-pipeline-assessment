using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Newtonsoft.Json.Linq;
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
            choice.IsSelected = isSelected;
            choice.IsSingle = isSingle;
            //Assert
            Assert.NotNull(choice);
            Assert.Equal(answer, choice.Answer);
            Assert.Equal(isSelected, choice.IsSelected);
            Assert.Equal(isSingle, choice.IsSingle);
        }
    }
}
