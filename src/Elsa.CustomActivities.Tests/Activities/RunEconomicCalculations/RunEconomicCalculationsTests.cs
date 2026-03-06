using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.RunEconomicCalculations
{
    public class RunEconomicCalculationsTests
    {
        [Theory]
        [AutoMoqData]
        public void ProcessWorkflowOutput_ShouldReturnStringOutput_GivenNonNumericValue(
            CustomActivities.Activities.RunEconomicCalculations.RunEconomicCalculations sut)
        {
            //Arrange
            sut.Output!.WorkflowOutput = "Weak";
            sut.StringScoreOutput = null;
            sut.NumericScoreOutput = null;

            //Act
            sut.ProcessWorkflowOutput();

            //Assert
            Assert.NotNull(sut.StringScoreOutput);
            Assert.Equal("Weak", sut.StringScoreOutput);
            Assert.Null(sut.NumericScoreOutput);
        }

        [Theory]
        [AutoMoqData]
            public void ProcessWorkflowOutput_ShouldReturnStringOutputAndNumericOutput_GivenNumericValue(
            CustomActivities.Activities.RunEconomicCalculations.RunEconomicCalculations sut)
        {
            //Arrange
            sut.Output!.WorkflowOutput = "123";
            sut.StringScoreOutput = null;
            sut.NumericScoreOutput = null;

            //Act
            sut.ProcessWorkflowOutput();

            //Assert
            Assert.NotNull(sut.StringScoreOutput);
            Assert.Equal("123", sut.StringScoreOutput);
            Assert.Equal(123, sut.NumericScoreOutput);
            Assert.NotNull(sut.NumericScoreOutput);
        }

        [Theory]
        [AutoMoqData]
        public void ProcessWorkflowOutput_ShouldHandleNullOutput(
            CustomActivities.Activities.RunEconomicCalculations.RunEconomicCalculations sut)
        {
            //Arrange
            sut.Output = null;
            sut.StringScoreOutput = null;
            sut.NumericScoreOutput = null;

            //Act
            sut.ProcessWorkflowOutput();

            //Assert
            Assert.Null(sut.StringScoreOutput);
            Assert.Null(sut.NumericScoreOutput);
        }

        [Theory]
        [InlineAutoMoqData("123.45")]
        [InlineAutoMoqData("0")]
        [InlineAutoMoqData("-50")]
        public void ProcessWorkflowOutput_ShouldParseDecimalValues(
            string numericValue,
            CustomActivities.Activities.RunEconomicCalculations.RunEconomicCalculations sut)
        {
            //Arrange
            sut.Output!.WorkflowOutput = numericValue;

            //Act
            sut.ProcessWorkflowOutput();

            //Assert
            Assert.Equal(numericValue, sut.StringScoreOutput);
            Assert.Equal(decimal.Parse(numericValue), sut.NumericScoreOutput);
        }
    }
}
